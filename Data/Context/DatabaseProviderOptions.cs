namespace WebApiNibu.Data.Context;

/// <summary>
/// Enum representing supported database providers
/// </summary>
public enum DatabaseProviderType
{
    PostgreSql,
    MySql,
    Oracle,
    SqlServer
}

/// <summary>
/// Configuration options for database provider
/// </summary>
public class DatabaseProviderOptions
{
    public const string SectionName = "Database";
    
    /// <summary>
    /// The database provider to use (PostgreSql, MySql, Oracle, SqlServer)
    /// </summary>
    public DatabaseProviderType Provider { get; set; } = DatabaseProviderType.PostgreSql;
    
    /// <summary>
    /// Connection string for the selected provider
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;
    
    /// <summary>
    /// Enable retry on failure (for transient errors)
    /// </summary>
    public bool EnableRetryOnFailure { get; set; } = true;
    
    /// <summary>
    /// Maximum retry count for transient failures
    /// </summary>
    public int MaxRetryCount { get; set; } = 5;
    
    /// <summary>
    /// Maximum delay between retries in seconds
    /// </summary>
    public int MaxRetryDelaySeconds { get; set; } = 10;
}

