using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using WebApiNibu.Data.Context;
using WebApiNibu.Abstraction;
using WebApiNibu.Authorization;
using WebApiNibu.Extensions;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.CopaUpsa;
using WebApiNibu.Services.Contract.Feed.Events;
using WebApiNibu.Services.Contract.Feed.News;
using WebApiNibu.Services.Contract.Feed.Polls;
using WebApiNibu.Services.Contract.Person;
using WebApiNibu.Services.Contract.School;
using WebApiNibu.Services.Contract.UsersAndAccess;
using WebApiNibu.Services.Implementation.CopaUpsa;
using WebApiNibu.Services.Implementation.Feed.Events;
using WebApiNibu.Services.Implementation.Feed.News;
using WebApiNibu.Services.Implementation.Feed.Polls;
using WebApiNibu.Services.Implementation.Person;
using WebApiNibu.Services.Implementation.School;
using WebApiNibu.Services.Implementation.UsersAndAccess;

var builder = WebApplication.CreateBuilder(args);

// Configure CORS from appsettings
var allowedOrigins = builder.Configuration["AllowedOrigins"]?.Split(',', StringSplitOptions.RemoveEmptyEntries) 
                     ?? new[] { "https://localhost:5173" };

Console.WriteLine("=".PadRight(60, '='));
Console.WriteLine("CORS Configuration:");
Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");
Console.WriteLine($"Allowed Origins ({allowedOrigins.Length}):");
foreach (var origin in allowedOrigins)
{
    Console.WriteLine($"  - {origin}");
}

Console.WriteLine("=".PadRight(60, '='));
Console.WriteLine("JasperServer Configuration:");
Console.WriteLine($"  URL: {builder.Configuration["JasperServer:Url"]}");
Console.WriteLine($"  Username: {builder.Configuration["JasperServer:Username"]}");
Console.WriteLine("=".PadRight(60, '='));

builder.Services.AddCors(options =>
{
    options.AddPolicy("ProductionPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
    
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins("https://localhost:5174", "http://localhost:5174")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddHttpClient<IAuthMicroserviceClient, AuthMicroserviceClient>(client =>
    {
        var authServiceUrl = builder.Configuration["AuthMicroserviceUrl"] ?? 
                             throw new InvalidOperationException("AuthMicroserviceUrl is not configured");
        client.BaseAddress = new Uri(authServiceUrl);
        client.Timeout = TimeSpan.FromSeconds(120);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    })
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        var handler = new HttpClientHandler();
        
        if (builder.Environment.IsDevelopment())
        {
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        }
        
        return handler;
    });

builder.Services.Configure<ProjectPermissionSettings>(builder.Configuration.GetSection("ProjectPermission"));
builder.Services.AddHttpClient<IProjectPermissionUserClient, ProjectPermissionUserClient>((sp, client) =>
    {
        var settings = sp.GetRequiredService<IOptions<ProjectPermissionSettings>>().Value;
        client.BaseAddress = new Uri(settings.BaseUrl);
        client.Timeout = TimeSpan.FromSeconds(60);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    })
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        var handler = new HttpClientHandler();
        if (builder.Environment.IsDevelopment())
        {
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        }
        return handler;
    });

builder.Services.AddAuthorization();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

builder.Services.AddHttpContextAccessor();

// Register generic CRUD service for all entities
builder.Services.AddScoped(typeof(IBaseCrud<>), typeof(BaseCrudImplementation<>));

// Register Contract/Implementation services
builder.Services.AddScoped<IEvent, EventImpl>();
builder.Services.AddScoped<IEventDetail, EventDetailImpl>();
builder.Services.AddScoped<IEventInteraction, EventInteractionImpl>();
builder.Services.AddScoped<IPoll, PollImpl>();
builder.Services.AddScoped<IOption, OptionImpl>();
builder.Services.AddScoped<ISelectedOption, SelectedOptionImpl>();
builder.Services.AddScoped<INews, NewsImpl>();
builder.Services.AddScoped<INewsDetail, NewsDetailImpl>();
builder.Services.AddScoped<INewsReaction, NewsReactionImpl>();

builder.Services.AddScoped<IAcademicPreference, AcademicPreferenceImpl>();
builder.Services.AddScoped<IAdult, AdultImpl>();
builder.Services.AddScoped<IAdultType, AdultTypeImpl>();
builder.Services.AddScoped<ICarreer, CarreerImpl>();
builder.Services.AddScoped<ICountry, CountryImpl>();
builder.Services.AddScoped<IDocumentType, DocumentTypeImpl>();
builder.Services.AddScoped<IInterestActivity, InterestActivityImpl>();
builder.Services.AddScoped<IMerch, MerchImpl>();
builder.Services.AddScoped<IMerchType, MerchTypeImpl>();
builder.Services.AddScoped<IMerchObtention, MerchObtentionImpl>();
builder.Services.AddScoped<IPreferencesStudent, PreferencesStudentImpl>();
builder.Services.AddScoped<IRole, RoleImpl>();
builder.Services.AddScoped<ISchoolStudent, SchoolStudentImpl>();
builder.Services.AddScoped<IStudentInterest, StudentInterestImpl>();
builder.Services.AddScoped<IUniversity, UniversityImpl>();
builder.Services.AddScoped<IWorker, WorkerImpl>();

builder.Services.AddScoped<ISchool, SchoolImpl>();
builder.Services.AddScoped<ISchoolsContacts, SchoolsContactsImpl>();
builder.Services.AddScoped<IQrAccess, QrAccessImpl>();

// Copa Upsa
builder.Services.AddScoped<ISport, SportImpl>();
builder.Services.AddScoped<ITournamentParent, TournamentParentImpl>();
builder.Services.AddScoped<IPhaseType, PhaseTypeImpl>();
builder.Services.AddScoped<IMatchStatus, MatchStatusImpl>();
builder.Services.AddScoped<IPosition, PositionImpl>();
builder.Services.AddScoped<ITournament, TournamentImpl>();
builder.Services.AddScoped<IStatistic, StatisticImpl>();
builder.Services.AddScoped<IParticipation, ParticipationImpl>();
builder.Services.AddScoped<IMatch, MatchImpl>();
builder.Services.AddScoped<IRoster, RosterImpl>();
builder.Services.AddScoped<ITournamentRoster, TournamentRosterImpl>();
builder.Services.AddScoped<IStatisticEvent, StatisticEventImpl>();

// Configure DbContext with dynamic database provider
builder.Services.AddDatabaseProvider(builder.Configuration, builder.Environment.IsDevelopment());

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCustomAuthorization();

// Built-in OpenAPI document (optional alongside Swashbuckle)
builder.Services.AddOpenApi();
builder.Services.AddControllers();

// Swashbuckle: Swagger generator
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiNibu", Version = "v1" });

    // Support Bearer token authentication for Swagger testing
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token from the auth microservice. Example: 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...'"
    });
    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

var corsPolicy = app.Environment.IsDevelopment() ? "FrontendPolicy" : "ProductionPolicy";
Console.WriteLine("=".PadRight(60, '='));
Console.WriteLine($"Active CORS Policy: {corsPolicy}");
Console.WriteLine("=".PadRight(60, '='));
Console.WriteLine(builder.Configuration.GetConnectionString("CoreConnection"));
app.UseCors(corsPolicy);

app.UseMiddleware<GlobalExceptionMiddleware>();

// Auto-apply pending migrations
await ApplyMigrationsAsync(app);

app.MapOpenApi();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiNibu v1");
    c.RoutePrefix = "swagger"; // UI at /swagger
});

// 4. Health check endpoint (after CORS is configured)
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }))
    .AllowAnonymous();

// 5. Debug endpoint to check CORS configuration
app.MapGet("/debug/cors", (HttpContext context) =>
{
    var origin = context.Request.Headers["Origin"].ToString();
    return Results.Ok(new
    {
        requestOrigin = origin,
        configuredOrigins = allowedOrigins,
        policy = corsPolicy,
        environment = app.Environment.EnvironmentName,
        timestamp = DateTime.UtcNow
    });
}).AllowAnonymous();

// Optional: redirect root to Swagger UI for quick discovery
app.MapGet("/", () => Results.Redirect("/swagger"));

// 6. Only redirect to HTTPS in non-Docker environments (same as SISAPI)
if (!app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

// 7. Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

// 8. Static files
app.UseStaticFiles();

// 9. Map controllers
app.MapControllers();

app.Run();

// Function to auto-apply pending migrations
static async Task ApplyMigrationsAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<CoreDbContext>();
        var logger = services.GetRequiredService<ILogger<Program>>();

        logger.LogInformation("Checking for pending migrations...");

        var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

        var migrations = pendingMigrations as string[] ?? pendingMigrations.ToArray();
        if (migrations.Length != 0)
        {
            logger.LogInformation("Found {Count} pending migration(s). Applying...", migrations.Count());
            await context.Database.MigrateAsync();
            logger.LogInformation("Migrations applied successfully.");
        }
        else
        {
            logger.LogInformation("Database is up to date. No pending migrations.");
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while applying migrations.");
        throw;
    }
}
