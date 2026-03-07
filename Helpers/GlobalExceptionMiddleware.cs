using System.Net;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace WebApiNibu.Helpers;

/// <summary>
/// Global exception handler middleware that catches unhandled exceptions
/// and returns structured JSON error responses instead of raw stack traces.
/// </summary>
public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        logger.LogError(exception, "Unhandled exception for {Method} {Path}", context.Request.Method, context.Request.Path);

        var (statusCode, errors) = exception switch
        {
            DbUpdateException dbEx => HandleDbUpdateException(dbEx),
            InvalidOperationException invalidOp => (HttpStatusCode.BadRequest, new List<string> { invalidOp.Message }),
            ArgumentException argEx => (HttpStatusCode.BadRequest, new List<string> { argEx.Message }),
            _ => (HttpStatusCode.InternalServerError, new List<string> { "An unexpected error occurred. Please try again later." })
        };

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse
        {
            StatusCode = (int)statusCode,
            Errors = errors
        };

        await context.Response.WriteAsJsonAsync(response);
    }

    private static (HttpStatusCode, List<string>) HandleDbUpdateException(DbUpdateException dbEx)
    {
        // Check for PostgreSQL-specific exception
        if (dbEx.InnerException is PostgresException pgEx)
        {
            return pgEx.SqlState switch
            {
                // Foreign key violation
                PostgresErrorCodes.ForeignKeyViolation => (HttpStatusCode.Conflict, new List<string>
                {
                    FormatForeignKeyError(pgEx)
                }),

                // Unique constraint violation
                PostgresErrorCodes.UniqueViolation => (HttpStatusCode.Conflict, new List<string>
                {
                    FormatUniqueViolationError(pgEx)
                }),

                // Not-null violation
                PostgresErrorCodes.NotNullViolation => (HttpStatusCode.BadRequest, new List<string>
                {
                    $"A required field is missing: '{pgEx.ColumnName ?? "unknown"}'."
                }),

                // Check constraint violation
                PostgresErrorCodes.CheckViolation => (HttpStatusCode.BadRequest, new List<string>
                {
                    $"A validation constraint was violated: '{pgEx.ConstraintName ?? "unknown"}'."
                }),

                // String data too long
                PostgresErrorCodes.StringDataRightTruncation => (HttpStatusCode.BadRequest, new List<string>
                {
                    "One or more text fields exceed the maximum allowed length."
                }),

                _ => (HttpStatusCode.InternalServerError, new List<string>
                {
                    "A database error occurred. Please try again later."
                })
            };
        }

        return (HttpStatusCode.InternalServerError, new List<string>
        {
            "A database error occurred. Please try again later."
        });
    }

    private static string FormatForeignKeyError(PostgresException pgEx)
    {
        var constraintName = pgEx.ConstraintName ?? string.Empty;
        var tableName = pgEx.TableName ?? "unknown";

        // Constraint names follow the pattern: FK_ChildTable_ParentTable_ColumnName
        // Try to extract a human-readable message
        if (constraintName.StartsWith("FK_"))
        {
            var parts = constraintName.Split('_');
            if (parts.Length >= 4)
            {
                var parentTable = parts[2];
                return $"Cannot complete this operation because '{tableName}' has a dependency on '{parentTable}'. " +
                       $"Remove or reassign the related records first.";
            }
        }

        return $"Cannot complete this operation on '{tableName}' because it is referenced by other records. " +
               "Remove or reassign the related records first.";
    }

    private static string FormatUniqueViolationError(PostgresException pgEx)
    {
        var constraintName = pgEx.ConstraintName ?? string.Empty;
        var tableName = pgEx.TableName ?? "unknown";

        return $"A duplicate record already exists in '{tableName}' (constraint: '{constraintName}'). " +
               "Please use a unique value.";
    }
}

/// <summary>
/// Standard error response returned by the API on unhandled exceptions.
/// </summary>
public class ErrorResponse
{
    public int StatusCode { get; set; }
    public List<string> Errors { get; set; } = [];
}

