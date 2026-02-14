using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WebApiNibu.Data.Entity.CopaUpsa;
using WebApiNibu.Data.Entity.FatherTable;
using WebApiNibu.Data.Entity.Feed.Events;
using WebApiNibu.Data.Entity.Feed.News;
using WebApiNibu.Data.Entity.Feed.Polls;
using WebApiNibu.Data.Entity.Person;
using WebApiNibu.Data.Entity.School;
using WebApiNibu.Data.Entity.Tags;
using WebApiNibu.Data.Entity.UsersAndAccess;

namespace WebApiNibu.Data.Context;

/// <summary>
/// Database context abstraction for multi-provider support.
/// Allows switching between different database providers (PostgreSQL, MySQL, Oracle, etc.)
/// </summary>
public interface IAppDbContext
{
    // Copa Upsa
    DbSet<Match> Matches { get; set; }
    DbSet<MatchStatus> MatchStatuses { get; set; }
    DbSet<Participation> Participations { get; set; }
    DbSet<PhaseType> PhaseTypes { get; set; }
    DbSet<Position> Positions { get; set; }
    DbSet<Roster> Rosters { get; set; }
    DbSet<Sport> Sports { get; set; }
    DbSet<Statistic> Statistics { get; set; }
    DbSet<StatisticEvent> StatisticEvents { get; set; }
    DbSet<Tournament> Tournaments { get; set; }
    DbSet<TournamentParent> TournamentParents { get; set; }

    // Feed - Events
    DbSet<Event> Events { get; set; }
    DbSet<EventDetail> EventDetails { get; set; }
    DbSet<EventInteraction> EventInteractions { get; set; }

    // Feed - News
    DbSet<News> News { get; set; }
    DbSet<NewsDetail> NewsDetails { get; set; }
    DbSet<NewsReaction> NewsReactions { get; set; }

    // Feed - Polls
    DbSet<Poll> Polls { get; set; }
    DbSet<Option> Options { get; set; }
    DbSet<SelectedOption> SelectedOptions { get; set; }

    // Person
    DbSet<AcademicPreference> AcademicPreferences { get; set; }
    DbSet<Adult> Adults { get; set; }
    DbSet<AdultType> AdultTypes { get; set; }
    DbSet<Carreer> Carreers { get; set; }
    DbSet<Country> Countries { get; set; }
    DbSet<DocumentType> DocumentTypes { get; set; }
    DbSet<InterestActivity> InterestActivities { get; set; }
    DbSet<Merch> Merchs { get; set; }
    DbSet<MerchObtention> MerchObtentions { get; set; }
    DbSet<MerchType> MerchTypes { get; set; }
    DbSet<PreferencesStudent> PreferencesStudents { get; set; }
    DbSet<Role> Roles { get; set; }
    DbSet<SchoolStudent> SchoolStudents { get; set; }
    DbSet<StudentInterest> StudentInterests { get; set; }
    DbSet<University> Universities { get; set; }
    DbSet<Worker> Workers { get; set; }

    // School
    DbSet<Contact> Contacts { get; set; }
    DbSet<SchoolTable> Schools { get; set; }

    // Tags
    DbSet<EventTag> EventTags { get; set; }
    DbSet<NewsTag> NewsTags { get; set; }
    DbSet<NotifyTag> NotifyTags { get; set; }
    DbSet<PollTag> PollTags { get; set; }
    DbSet<Tag> Tags { get; set; }

    // Users and Access
    DbSet<QrAccess> QrAccesses { get; set; }
    DbSet<Users> Users { get; set; }

    // DbContext methods
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    DatabaseFacade Database { get; }
}

