using Microsoft.AspNetCore.Authorization;

namespace WebApiNibu.Authorization;
public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}

