using System.Text.Json;
using WebApiNibu.Data.Dto.Common;

namespace WebApiNibu.Extensions;

/// <summary>
/// Service for communicating with the authentication microservice.
/// Use this to validate tokens, fetch user details, or check permissions remotely.
/// </summary>
public interface IAuthMicroserviceClient
{
    Task<bool> ValidateTokenAsync(string token);
    Task<UserInfo?> GetCurrentUserAsync(string token);
    Task<ApiResponseDto<RoleDto>?> GetRoleByName(string token, string roleName, int companyId);
    Task<IEnumerable<string>> GetUserPermissionsAsync(string token);
    Task<bool> VerifyPermissionAsync(string token, string module, string controller, string action, int typePermission = 0);
    // Returns (Success, ResponseBodyOrError) to help debugging when assignment fails
    Task<(bool Success, string? Response)> AssignRolePermissionAsync(string token, int roleId, int permissionId, int companyId, bool read = true, bool write = true, bool update = true, bool delete = true, DateTimeOffset? expirationDate = null);
}

public class AuthMicroserviceClient(HttpClient httpClient, ILogger<AuthMicroserviceClient> logger)
    : IAuthMicroserviceClient
{
    public async Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/auth/validate");
            request.Headers.Add("Cookie", $"accessToken={token}");
            
            var response = await httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error validating token with auth microservice");
            return false;
        }
    }

    public async Task<UserInfo?> GetCurrentUserAsync(string token)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/auth/me");
            request.Headers.Add("Cookie", $"accessToken={token}");
            
            var response = await httpClient.SendAsync(request);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<UserInfo>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching user info from auth microservice");
            return null;
        }
    }
    
    public async Task<ApiResponseDto<RoleDto>?> GetRoleByName(string token, string roleName, int companyId)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/Role/name/{roleName}/{companyId}");
            request.Headers.Add("Cookie", $"accessToken={token}");
            
            var response = await httpClient.SendAsync(request);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApiResponseDto<RoleDto>?>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new ApiResponseDto<RoleDto>();
            }
            
            return new ApiResponseDto<RoleDto>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching role by name from auth microservice");
            return new ApiResponseDto<RoleDto>();
        }
    }

    public async Task<IEnumerable<string>> GetUserPermissionsAsync(string token)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/auth/permissions");
            request.Headers.Add("Cookie", $"accessToken={token}");
            
            var response = await httpClient.SendAsync(request);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<string>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? Enumerable.Empty<string>();
            }
            
            return Enumerable.Empty<string>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching user permissions from auth microservice");
            return Enumerable.Empty<string>();
        }
    }

    /// <summary>
    /// Verifies if the current user has the specified permission by calling the auth microservice.
    /// This is the recommended approach to avoid JWT token bloat.
    /// </summary>
    /// <param name="token">JWT bearer token</param>
    /// <param name="module">Module name (e.g., "SISCON", "SISAPI")</param>
    /// <param name="controller">Controller name (e.g., "User", "Company")</param>
    /// <param name="action">Action name (Read, Write, Update, Delete)</param>
    /// <param name="typePermission">Type of permission: 0=ControllerAction, 1=MenuOption, 2=UserAction, 3=ProjectView</param>
    /// <returns>True if the user has the permission, false otherwise</returns>
    public async Task<bool> VerifyPermissionAsync(string token, string module, string controller, string action, int typePermission = 0)
    {
        try
        {
            var url = $"/api/Auth/verify-permission?module={module}&controller={controller}&action={action}&typePermission={typePermission}";
            
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Cookie", $"accessToken={token}");
            
            logger.LogDebug("Verifying permission: {Url}", url);
            
            var response = await httpClient.SendAsync(request);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<PermissionVerificationResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                return result?.Success == true && result.Data;
            }
            
            logger.LogWarning("Permission verification failed with status code: {StatusCode}", response.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error verifying permission with auth microservice - Module: {Module}, Controller: {Controller}, Action: {Action}", 
                module, controller, action);
            return false;
        }
    }

    public async Task<(bool Success, string? Response)> AssignRolePermissionAsync(string token, int roleId, int permissionId, int companyId, bool read = true, bool write = true, bool update = true, bool delete = true, DateTimeOffset? expirationDate = null)
    {
        try
        {
            var url = $"/api/RolePermission/assign?companyId={companyId}";
            var payload = new
            {
                roleId = roleId,
                permissionId = permissionId,
                read = read,
                write = write,
                update = update,
                delete = delete,
                expirationDate = expirationDate?.UtcDateTime,
                companyId = companyId
            };

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(payload)
            };

            if (!string.IsNullOrWhiteSpace(token))
            {
                // Ensure we send both Authorization header and cookie for compatibility
                var authHeader = token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) ? token : $"Bearer {token}";
                request.Headers.TryAddWithoutValidation("Authorization", authHeader);
                // send cookie without Bearer prefix
                var cookieValue = token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) ? token.Substring(7).Trim() : token;
                request.Headers.Add("Cookie", $"accessToken={cookieValue}");
            }

            var response = await httpClient.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return (true, body);
            logger.LogWarning("AssignRolePermission returned non-success {Status} Body: {Body}", response.StatusCode, body);
            return (false, body);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error assigning role permission to role {RoleId} for permission {PermissionId}", roleId, permissionId);
            return (false, ex.Message);
        }
    }
}

/// <summary>
/// User information from the auth microservice
/// </summary>
public class UserInfo
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
    public IEnumerable<string> Permissions { get; set; } = Enumerable.Empty<string>();
}

/// <summary>
/// Response from the verify-permission endpoint
/// </summary>
public class PermissionVerificationResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool Data { get; set; }
    public List<string>? Errors { get; set; }
}

public class RoleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool Active { get; set; }
    public int? CompanyId { get; set; }
    public string? CompanyName { get; set; }
}