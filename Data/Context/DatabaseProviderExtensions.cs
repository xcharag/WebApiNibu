using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApiNibu.Data.Context;

/// <summary>
/// Extension methods for configuring database providers dynamically
/// </summary>
public static class DatabaseProviderExtensions
{
    /// <summary>
    /// Adds the appropriate DbContext based on configuration
    /// </summary>
    public static IServiceCollection AddDatabaseProvider(
        this IServiceCollection services,
        IConfiguration configuration,
        bool isDevelopment = false)
    {
        var databaseProvider = configuration.GetValue<string>("DatabaseProvider") ?? "PostgreSql";
        
        // Parse provider type
        if (!System.Enum.TryParse<DatabaseProviderType>(databaseProvider, true, out var providerType))
        {
            throw new InvalidOperationException(
                $"Invalid database provider: '{databaseProvider}'. " +
                $"Supported providers: {string.Join(", ", System.Enum.GetNames<DatabaseProviderType>())}");
        }
        
        // Get connection string based on provider
        var connectionString = GetConnectionString(configuration, providerType);
        
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException(
                $"Connection string for provider '{providerType}' not found in configuration.");
        }
        
        services.AddDbContext<CoreDbContext>(options =>
        {
            ConfigureDbContext(options, providerType, connectionString, isDevelopment);
        });

        // Register the interface for dependency injection
        services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<CoreDbContext>());

        return services;
    }

    private static string? GetConnectionString(IConfiguration configuration, DatabaseProviderType providerType)
    {
        return providerType switch
        {
            DatabaseProviderType.PostgreSql => configuration.GetConnectionString("PostgreSqlConnection"),
            DatabaseProviderType.MySql => configuration.GetConnectionString("MySqlConnection") 
                                          ?? configuration.GetConnectionString("OracleConnection"), // Fallback for legacy naming
            DatabaseProviderType.Oracle => configuration.GetConnectionString("OracleConnection"),
            DatabaseProviderType.SqlServer => configuration.GetConnectionString("SqlServerConnection"),
            _ => throw new ArgumentOutOfRangeException(nameof(providerType), providerType, "Unsupported database provider")
        };
    }

    private static void ConfigureDbContext(
        DbContextOptionsBuilder options,
        DatabaseProviderType providerType,
        string connectionString,
        bool isDevelopment)
    {
        switch (providerType)
        {
            case DatabaseProviderType.PostgreSql:
                ConfigurePostgreSql(options, connectionString);
                break;
                
            case DatabaseProviderType.MySql:
                ConfigureMySql(options, connectionString);
                break;
                
            case DatabaseProviderType.Oracle:
                ConfigureOracle(options, connectionString);
                break;
                
            case DatabaseProviderType.SqlServer:
                ConfigureSqlServer(options, connectionString);
                break;
                
            default:
                throw new ArgumentOutOfRangeException(nameof(providerType), providerType, "Unsupported database provider");
        }

        // Enable development logging
        if (isDevelopment)
        {
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        }
    }

    private static void ConfigurePostgreSql(DbContextOptionsBuilder options, string connectionString)
    {
        options.UseNpgsql(connectionString, npgsqlOptions =>
        {
            npgsqlOptions.MigrationsAssembly(typeof(CoreDbContext).Assembly.FullName);
            npgsqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorCodesToAdd: null);
        });
    }

    private static void ConfigureMySql(DbContextOptionsBuilder options, string connectionString)
    {
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), mysqlOptions =>
        {
            mysqlOptions.MigrationsAssembly(typeof(CoreDbContext).Assembly.FullName);
            mysqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null);
        });
    }

    private static void ConfigureOracle(DbContextOptionsBuilder options, string connectionString)
    {
        // Oracle requires Oracle.EntityFrameworkCore package
        // Uncomment when using Oracle:
        // options.UseOracle(connectionString, oracleOptions =>
        // {
        //     oracleOptions.MigrationsAssembly(typeof(CoreDbContext).Assembly.FullName);
        // });
        
        throw new NotSupportedException(
            "Oracle provider requires Oracle.EntityFrameworkCore package. " +
            "Install the package and uncomment the Oracle configuration in DatabaseProviderExtensions.cs");
    }

    private static void ConfigureSqlServer(DbContextOptionsBuilder options, string connectionString)
    {
        // SQL Server requires Microsoft.EntityFrameworkCore.SqlServer package
        // Uncomment when using SQL Server:
        // options.UseSqlServer(connectionString, sqlServerOptions =>
        // {
        //     sqlServerOptions.MigrationsAssembly(typeof(CoreDbContext).Assembly.FullName);
        //     sqlServerOptions.EnableRetryOnFailure(
        //         maxRetryCount: 5,
        //         maxRetryDelay: TimeSpan.FromSeconds(10),
        //         errorNumbersToAdd: null);
        // });
        
        throw new NotSupportedException(
            "SQL Server provider requires Microsoft.EntityFrameworkCore.SqlServer package. " +
            "Install the package and uncomment the SQL Server configuration in DatabaseProviderExtensions.cs");
    }
}


