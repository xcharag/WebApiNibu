namespace WebApiNibu.Data.Enum;

/// <summary>
/// Module identifiers that must match across all microservices.
/// Synchronized with auth microservice (sisapi-saas-microservice).
/// </summary>
public enum Module
{
    SISAPI = 0, //Auth and Authorization module (API Gateway)
    NIBU = 1,   //NIBU ADMIN DASHBOARD
}

