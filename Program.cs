using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Abstraction;
using WebApiNibu.Services.Contract.CopaUpsa;
using WebApiNibu.Services.Contract.Feed.Events;
using WebApiNibu.Services.Contract.Feed.News;
using WebApiNibu.Services.Contract.Feed.Polls;
using WebApiNibu.Services.Contract.Person;
using WebApiNibu.Services.Contract.School;
using WebApiNibu.Services.Implementation.CopaUpsa;
using WebApiNibu.Services.Implementation.Feed.Events;
using WebApiNibu.Services.Implementation.Feed.News;
using WebApiNibu.Services.Implementation.Feed.Polls;
using WebApiNibu.Services.Implementation.Person;
using WebApiNibu.Services.Implementation.School;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Enable endpoint discovery for OpenAPI
builder.Services.AddEndpointsApiExplorer();

// Configure CORS from appsettings
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        if (allowedOrigins.Length > 0)
            policy.WithOrigins(allowedOrigins);
        else
            policy.AllowAnyOrigin();

        policy.AllowAnyHeader()
              .AllowAnyMethod();
    });
});

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
builder.Services.AddScoped<IStatisticEvent, StatisticEventImpl>();

// Configure DbContext with dynamic database provider
builder.Services.AddDatabaseProvider(builder.Configuration, builder.Environment.IsDevelopment());

// Built-in OpenAPI document (optional alongside Swashbuckle)
builder.Services.AddOpenApi();

// Swashbuckle: Swagger generator
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Auto-apply pending migrations
await ApplyMigrationsAsync(app);

// Expose the OpenAPI spec regardless of environment (minimal API doc)
app.MapOpenApi();

// Swashbuckle: Enable Swagger middleware and UI
app.UseSwagger(c =>
{
    // Serve at /swagger/v1/swagger.json by default
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiNibu v1");
    c.RoutePrefix = "swagger"; // UI at /swagger
});

// Optional: redirect root to Swagger UI for quick discovery
app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

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
