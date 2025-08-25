using Microsoft.AspNetCore.Authorization;
using PackageManager.Core.Enums;

namespace PackageManage.API.Attributes;

public class RequiredRolesAttribute : AuthorizeAttribute
{
    public RequiredRolesAttribute(params Role[] roles)
    {
        Roles = string.Join(",", roles.Select(r => r.ToString()));
    }
}