using Microsoft.AspNetCore.Authorization;

namespace FDMS_API.Configurations.CustomAuthorize.Admin
{
    public class AdminRequirement:IAuthorizationRequirement
    {
        public AdminRequirement()
        {
        }
    }
}
