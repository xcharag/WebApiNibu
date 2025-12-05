using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Entity.CopaUpsa;
using WebApiNibu.Data.Entity.FatherTable;
using WebApiNibu.Data.Entity.Feed.Events;
using WebApiNibu.Data.Entity.Feed.News;
using WebApiNibu.Data.Entity.Feed.Polls;
using WebApiNibu.Data.Entity.Person;
using WebApiNibu.Data.Entity.School;
using WebApiNibu.Data.Entity.Tags;
using WebApiNibu.Data.Entity.UsersAndAccess;

namespace WebApiNibu.Data.Context.Oracle;

public class OracleDbContext : DbContext
{
    public OracleDbContext(DbContextOptions<OracleDbContext> options) : base(options)
    {
    }

    // Copa Upsa
    public DbSet<Match> Matches { get; set; }
    public DbSet<MatchStatus> MatchStatuses { get; set; }
    public DbSet<Participation> Participations { get; set; }
    public DbSet<PhaseType> PhaseTypes { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<Roster> Rosters { get; set; }
    public DbSet<Sport> Sports { get; set; }
    public DbSet<Statistic> Statistics { get; set; }
    public DbSet<StatisticEvent> StatisticEvents { get; set; }
    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<TournamentParent> TournamentParents { get; set; }

    // Feed - Events
    public DbSet<Event> Events { get; set; }
    public DbSet<EventDetail> EventDetails { get; set; }
    public DbSet<EventInteraction> EventInteractions { get; set; }

    // Feed - News
    public DbSet<News> News { get; set; }
    public DbSet<NewsDetail> NewsDetails { get; set; }
    public DbSet<NewsReaction> NewsReactions { get; set; }

    // Feed - Polls
    public DbSet<Poll> Polls { get; set; }
    public DbSet<Option> Options { get; set; }
    public DbSet<SelectedOption> SelectedOptions { get; set; }

    // Person
    public DbSet<AcademicPreference> AcademicPreferences { get; set; }
    public DbSet<Adult> Adults { get; set; }
    public DbSet<AdultType> AdultTypes { get; set; }
    public DbSet<Carreer> Carreers { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<DocumentType> DocumentTypes { get; set; }
    public DbSet<InterestActivity> InterestActivities { get; set; }
    public DbSet<Merch> Merchs { get; set; }
    public DbSet<MerchObtention> MerchObtentions { get; set; }
    public DbSet<MerchType> MerchTypes { get; set; }
    public DbSet<PreferencesStudent> PreferencesStudents { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<SchoolStudent> SchoolStudents { get; set; }
    public DbSet<StudentInterest> StudentInterests { get; set; }
    public DbSet<University> Universities { get; set; }
    public DbSet<Worker> Workers { get; set; }

    // School
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<SchoolTable> Schools { get; set; }

    // Tags
    public DbSet<EventTag> EventTags { get; set; }
    public DbSet<NewsTag> NewsTags { get; set; }
    public DbSet<NotifyTag> NotifyTags { get; set; }
    public DbSet<PollTag> PollTags { get; set; }
    public DbSet<Tag> Tags { get; set; }

    // Users and Access
    public DbSet<QrAccess> QrAccesses { get; set; }
    public DbSet<Users> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure TPH (Table Per Hierarchy) for PersonTable inheritance
        modelBuilder.Entity<PersonTable>()
            .HasDiscriminator<string>("PersonType")
            .HasValue<Adult>("Adult")
            .HasValue<Worker>("Worker")
            .HasValue<SchoolStudent>("SchoolStudent");

        // === INDEXES FOR PERFORMANCE ===

        // Users indexes
        modelBuilder.Entity<Users>()
            .HasIndex(u => u.Name)
            .HasDatabaseName("IX_Users_Name");

        modelBuilder.Entity<Users>()
            .HasIndex(u => u.IdPerson)
            .HasDatabaseName("IX_Users_IdPerson");

        // PersonTable indexes
        modelBuilder.Entity<PersonTable>()
            .HasIndex(p => p.Email)
            .IsUnique()
            .HasDatabaseName("IX_PersonTable_Email");

        modelBuilder.Entity<PersonTable>()
            .HasIndex(p => p.DocumentNumber)
            .HasDatabaseName("IX_PersonTable_DocumentNumber");

        // Event indexes
        modelBuilder.Entity<Event>()
            .HasIndex(e => e.StartDate)
            .HasDatabaseName("IX_Event_StartDate");

        modelBuilder.Entity<Event>()
            .HasIndex(e => e.IsFeatured)
            .HasDatabaseName("IX_Event_IsFeatured");

        // News indexes
        modelBuilder.Entity<News>()
            .HasIndex(n => n.CreatedAt)
            .HasDatabaseName("IX_News_PublicationDate");

        // Tournament indexes
        modelBuilder.Entity<Tournament>()
            .HasIndex(t => new { t.TournamentParentId, t.SportId })
            .HasDatabaseName("IX_Tournament_ParentId_SportId");

        // Match indexes
        modelBuilder.Entity<Match>()
            .HasIndex(m => new { m.ParticipationId, m.StartDate })
            .HasDatabaseName("IX_Match_ParticipationId_StartDate");

        // Tag indexes
        modelBuilder.Entity<Tag>()
            .HasIndex(t => t.Group)
            .HasDatabaseName("IX_Tag_Group");

        // === RELATIONSHIPS CONFIGURATION ===

        // Users -> PersonTable (One-to-One)
        modelBuilder.Entity<Users>()
            .HasOne(u => u.PersonTable)
            .WithOne(p => p.User)
            .HasForeignKey<Users>(u => u.IdPerson)
            .OnDelete(DeleteBehavior.Restrict);

        // Tournament relationships
        modelBuilder.Entity<Tournament>()
            .HasOne(t => t.TournamentParent)
            .WithMany(tp => tp.Tournaments)
            .HasForeignKey(t => t.TournamentParentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Tournament>()
            .HasOne(t => t.Sport)
            .WithMany(s => s.Tournaments)
            .HasForeignKey(t => t.SportId)
            .OnDelete(DeleteBehavior.Restrict);

        // Match relationships
        modelBuilder.Entity<Match>()
            .HasOne(m => m.Participation)
            .WithMany(p => p.Matches)
            .HasForeignKey(m => m.ParticipationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Match>()
            .HasOne(m => m.MatchStatus)
            .WithMany(ms => ms.Matches)
            .HasForeignKey(m => m.MatchStatusId)
            .OnDelete(DeleteBehavior.Restrict);

        // Roster -> Match
        modelBuilder.Entity<Roster>()
            .HasOne(r => r.Match)
            .WithMany(m => m.Rosters)
            .HasForeignKey(r => r.MatchId)
            .OnDelete(DeleteBehavior.Restrict);

        // Participation relationships
        modelBuilder.Entity<Participation>()
            .HasOne(p => p.Tournament)
            .WithMany(t => t.Participations)
            .HasForeignKey(p => p.TournamentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Participation>()
            .HasOne(p => p.PhaseType)
            .WithMany(pt => pt.Participations)
            .HasForeignKey(p => p.PhaseTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Statistic relationships
        modelBuilder.Entity<StatisticEvent>()
            .HasOne(s => s.Statistic)
            .WithMany(se => se.StatisticEvents)
            .HasForeignKey(s => s.StatisticId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<StatisticEvent>()
            .HasOne(s => s.Roster)
            .WithMany(se => se.StatisticEvents)
            .HasForeignKey(s => s.RosterId)
            .OnDelete(DeleteBehavior.Restrict);

        // Position -> Roster
        modelBuilder.Entity<Roster>()
            .HasOne(r => r.Position)
            .WithMany(p => p.Rosters)
            .HasForeignKey(r => r.PositionId)
            .OnDelete(DeleteBehavior.Restrict);

        // Event relationships
        modelBuilder.Entity<EventInteraction>()
            .HasOne(ei => ei.Event)
            .WithMany(e => e.EventInteractions)
            .HasForeignKey(ei => ei.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EventInteraction>()
            .HasOne(ei => ei.User)
            .WithMany(u => u.EventInteracts)
            .HasForeignKey(ei => ei.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EventDetail>()
            .HasOne(ed => ed.Event)
            .WithMany(e => e.EventDetails)
            .HasForeignKey(ed => ed.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        // News relationships
        modelBuilder.Entity<NewsReaction>()
            .HasOne(nr => nr.News)
            .WithMany(n => n.NewsReactions)
            .HasForeignKey(nr => nr.NewsId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<NewsReaction>()
            .HasOne(nr => nr.User)
            .WithMany(u => u.NewsReactions)
            .HasForeignKey(nr => nr.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<NewsDetail>()
            .HasOne(nd => nd.News)
            .WithMany(n => n.NewsDetails)
            .HasForeignKey(nd => nd.NewsId)
            .OnDelete(DeleteBehavior.Cascade);

        // Poll relationships
        modelBuilder.Entity<Option>()
            .HasOne(o => o.Poll)
            .WithMany(p => p.Options)
            .HasForeignKey(o => o.PollId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SelectedOption>()
            .HasOne(so => so.Option)
            .WithMany(o => o.SelectedOptions)
            .HasForeignKey(so => so.OptionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SelectedOption>()
            .HasOne(so => so.User)
            .WithMany(u => u.SelectedOptions)
            .HasForeignKey(so => so.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Tag relationships (Many-to-Many via junction tables)
        modelBuilder.Entity<EventTag>()
            .HasOne(et => et.Tag)
            .WithMany(t => t.EventTags)
            .HasForeignKey(et => et.TagId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EventTag>()
            .HasOne(et => et.News)
            .WithMany(e => e.EventTags)
            .HasForeignKey(et => et.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<NewsTag>()
            .HasOne(nt => nt.Tag)
            .WithMany(t => t.NewsTags)
            .HasForeignKey(nt => nt.TagId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<NewsTag>()
            .HasOne(nt => nt.News)
            .WithMany(n => n.NewsTags)
            .HasForeignKey(nt => nt.NewsId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PollTag>()
            .HasOne(pt => pt.Tag)
            .WithMany(t => t.PollTags)
            .HasForeignKey(pt => pt.TagId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PollTag>()
            .HasOne(pt => pt.Poll)
            .WithMany(p => p.PollTags)
            .HasForeignKey(pt => pt.PollId)
            .OnDelete(DeleteBehavior.Cascade);

        // Person relationships
        modelBuilder.Entity<PersonTable>()
            .HasOne(p => p.Country)
            .WithMany(c => c.People)
            .HasForeignKey(p => p.IdCountry)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PersonTable>()
            .HasOne(p => p.DoucmentType)
            .WithMany(dt => dt.People)
            .HasForeignKey(p => p.IdDocumentType)
            .OnDelete(DeleteBehavior.Restrict);

        // Adult relationships
        modelBuilder.Entity<Adult>()
            .HasOne(a => a.AdultType)
            .WithMany(at => at.Adults)
            .HasForeignKey(a => a.IdAdultType)
            .OnDelete(DeleteBehavior.Restrict);
        

        // === CONSTRAINTS & VALIDATIONS ===

        // Ensure decimal precision for Match scores
        modelBuilder.Entity<Match>()
            .Property(m => m.ScoreA)
            .HasPrecision(18, 4);

        modelBuilder.Entity<Match>()
            .Property(m => m.ScoreB)
            .HasPrecision(18, 4);

        modelBuilder.Entity<Match>()
            .Property(m => m.DetailPointA)
            .HasPrecision(18, 4);

        modelBuilder.Entity<Match>()
            .Property(m => m.DetailPointB)
            .HasPrecision(18, 4);

        // BaseEntity default values
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property<DateTime>("CreatedAt")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                modelBuilder.Entity(entityType.ClrType)
                    .Property<DateTime>("UpdatedAt")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");

                modelBuilder.Entity(entityType.ClrType)
                    .Property<bool>("Active")
                    .HasDefaultValue(true);
            }
        }
    }
}