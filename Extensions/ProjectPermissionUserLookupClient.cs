using Microsoft.Extensions.Options;

namespace WebApiNibu.Extensions;

public interface IProjectPermissionUserClient
{
    Task<IReadOnlyDictionary<int, int>> GetUserPermissionsAsync(int userId, CancellationToken cancellationToken = default);
    // Creates a permission in the ProjectPermission service and returns the created Permission Id (or null if creation failed / id not returned)
    Task<int?> CreateProjectPermissionAsync(int projectId, string name, CancellationToken cancellationToken = default);
}

public class ProjectPermissionUserClient : IProjectPermissionUserClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProjectPermissionUserClient> _logger;
    private readonly ProjectPermissionSettings _settings;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProjectPermissionUserClient(HttpClient httpClient, ILogger<ProjectPermissionUserClient> logger, IOptions<ProjectPermissionSettings> options, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _logger = logger;
        _settings = options.Value;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IReadOnlyDictionary<int, int>> GetUserPermissionsAsync(int userId, CancellationToken cancellationToken = default)
    {
        var requestUrl = $"{_settings.Endpoint}?module={_settings.Module}&userId={userId}";
        var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
        var authValue = ResolveAuthorizationValue();
        if (!string.IsNullOrWhiteSpace(authValue))
        {
            request.Headers.TryAddWithoutValidation("Authorization", authValue);
        }
        try
        {
            var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var apiResponse = await response.Content.ReadFromJsonAsync<ProjectPermissionApiResponse>(cancellationToken: cancellationToken);
            if (apiResponse?.Success != true || apiResponse.Data == null)
            {
                _logger.LogWarning("Respuesta inválida para permisos de usuario {UserId}", userId);
                return new Dictionary<int, int>();
            }

            var result = new Dictionary<int, int>();
            foreach (var dto in apiResponse.Data)
            {
                if (int.TryParse(dto.Code, out var projectId) && !result.ContainsKey(projectId))
                {
                    result[projectId] = dto.Id;
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error consultando permisos de usuario {UserId}", userId);
            return new Dictionary<int, int>();
        }
    }

    public async Task<int?> CreateProjectPermissionAsync(int projectId, string name, CancellationToken cancellationToken = default)
    {
        var requestUrl = _settings.Endpoint;
        var payload = new
        {
            Code = projectId.ToString(),
            Module = _settings.Module,
            Name = name,
            Description = name,
            Active = true
        };

        var request = new HttpRequestMessage(HttpMethod.Post, requestUrl)
        {
            Content = JsonContent.Create(payload)
        };

        var authValue = ResolveAuthorizationValue();
        if (!string.IsNullOrWhiteSpace(authValue))
            request.Headers.TryAddWithoutValidation("Authorization", authValue);

        try
        {
            var response = await _httpClient.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("ProjectPermission service returned {Status} when creating permission for project {ProjectId}", response.StatusCode, projectId);
                return null;
            }

            // Try read a generic API response if present
            try
            {
                var body = await response.Content.ReadAsStringAsync(cancellationToken);
                if (string.IsNullOrWhiteSpace(body))
                {
                    // Try Location header
                    if (response.Headers.Location != null)
                    {
                        var segs = response.Headers.Location.Segments;
                        var last = segs.LastOrDefault()?.TrimEnd('/');
                        if (int.TryParse(last, out var parsedId)) return parsedId;
                    }
                    return null;
                }

                try
                {
                    using var doc = System.Text.Json.JsonDocument.Parse(body);
                    if (doc.RootElement.TryGetProperty("data", out var dataElem))
                    {
                        if (dataElem.ValueKind == System.Text.Json.JsonValueKind.Object && dataElem.TryGetProperty("id", out var idProp))
                        {
                            if (idProp.TryGetInt32(out var id)) return id;
                        }
                        else if (dataElem.ValueKind == System.Text.Json.JsonValueKind.Array && dataElem.GetArrayLength() > 0)
                        {
                            var first = dataElem[0];
                            if (first.ValueKind == System.Text.Json.JsonValueKind.Object && first.TryGetProperty("id", out var id2))
                            {
                                if (id2.TryGetInt32(out var idv)) return idv;
                            }
                        }
                    }
                    // fallback: try parsing root as object with id
                    if (doc.RootElement.ValueKind == System.Text.Json.JsonValueKind.Object && doc.RootElement.TryGetProperty("id", out var rootId))
                    {
                        if (rootId.TryGetInt32(out var rid)) return rid;
                    }
                }
                catch (System.Text.Json.JsonException ex)
                {
                    _logger.LogWarning(ex, "Failed parsing ProjectPermission create response body");
                }

                // Try Location header as last resort
                if (response.Headers.Location != null)
                {
                    var segs = response.Headers.Location.Segments;
                    var last = segs.LastOrDefault()?.TrimEnd('/');
                    if (int.TryParse(last, out var parsedId)) return parsedId;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating project permission for project {ProjectId}", projectId);
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating project permission for project {ProjectId}", projectId);
            return null;
        }
    }

    private string? ResolveAuthorizationValue()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
        {
            return null;
        }

        if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            var headerValue = authHeader.ToString();
            if (!string.IsNullOrWhiteSpace(headerValue))
            {
                return headerValue;
            }
        }

        if (context.Request.Cookies.TryGetValue("accessToken", out var cookieToken))
        {
            var token = cookieToken.Trim();
            if (!string.IsNullOrWhiteSpace(token))
            {
                return token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
                    ? token
                    : $"Bearer {token}";
            }
        }

        return null;
    }
}

internal sealed record ProjectPermissionApiResponse
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public List<ProjectPermissionItem>? Data { get; init; }
}

internal sealed record ProjectPermissionItem
{
    public int Id { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Module { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string TypePermission { get; init; } = string.Empty;
    public bool Active { get; init; }
}
