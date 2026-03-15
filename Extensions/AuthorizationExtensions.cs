using Microsoft.AspNetCore.Authorization;
using WebApiNibu.Authorization;

namespace WebApiNibu.Extensions;

public static class AuthorizationExtensions
{
    /// <summary>
    /// Adds custom authorization with dynamic permission policies.
    /// Uses the same authorization system as the auth microservice (sisapi-saas-microservice).
    /// 
    /// Permission format: Module-Controller:Action
    /// Example: SISCON-Accounts:Read, SISCON-Transactions:Write
    /// 
    /// This enables:
    /// 1. [RequirePermission("SISCON", "Accounts", "Read")] - Explicit permission check
    /// 2. [DynamicPermission] - Automatic permission check based on HTTP method
    /// </summary>
    public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
    {
        // Register the custom policy provider for dynamic permission policies
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        
        // Register the permission handler
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        
        // Add authorization services
        services.AddAuthorization();

        return services;
    }
}

