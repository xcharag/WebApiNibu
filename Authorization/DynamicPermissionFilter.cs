using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiNibu.Data.Enum;
using WebApiNibu.Extensions;

namespace WebApiNibu.Authorization;

public class DynamicPermissionFilter(Module module = Module.NIBU, int typePermission = 0) : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (context.HttpContext.User.Identity?.IsAuthenticated != true)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        
        var authClient = context.HttpContext.RequestServices.GetRequiredService<IAuthMicroserviceClient>();
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<DynamicPermissionFilter>>();
        
        var token = context.HttpContext.Request.Cookies["accessToken"];
        
        if (string.IsNullOrEmpty(token))
        {
            var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                token = authHeader.Substring("Bearer ".Length).Trim();
            }
        }
        
        if (string.IsNullOrEmpty(token))
        {
            logger.LogWarning("No accessToken cookie or Authorization header found in request");
            context.Result = new UnauthorizedResult();
            return;
        }
        
        var controllerName = context.RouteData.Values["controller"]?.ToString() ?? string.Empty;
        
        var httpMethod = context.HttpContext.Request.Method.ToUpper();
        var action = httpMethod switch
        {
            "GET" => "Read",
            "POST" => "Write",
            "PUT" => "Update",
            "PATCH" => "Update",
            "DELETE" => "Delete",
            _ => "Read"
        };
        
        logger.LogDebug("Checking permission: {Module}-{Controller}:{Action} (TypePermission: {TypePermission})", 
            module, controllerName, action, typePermission);
        
        var hasPermission = await authClient.VerifyPermissionAsync(
            token, 
            module.ToString(), 
            controllerName, 
            action,
            typePermission
        );

        if (!hasPermission)
        {
            logger.LogWarning("Permission denied for: {Module}-{Controller}:{Action}", 
                module, controllerName, action);
            context.Result = new ForbidResult();
            return;
        }

        logger.LogInformation("Permission granted for: {Module}-{Controller}:{Action}", 
            module, controllerName, action);
    }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class DynamicPermissionAttribute : TypeFilterAttribute
{
    public DynamicPermissionAttribute(Module module = Module.NIBU, int typePermission = 0) 
        : base(typeof(DynamicPermissionFilter))
    {
        Arguments = [module, typePermission];
    }
}

