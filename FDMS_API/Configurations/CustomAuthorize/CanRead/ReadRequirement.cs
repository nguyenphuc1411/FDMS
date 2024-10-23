using Microsoft.AspNetCore.Authorization;

namespace FDMS_API.Configurations.CustomAuthorize.CanRead
{
    public class ReadRequirement:IAuthorizationRequirement
    {
        public ReadRequirement()
        {
            
        }
    }
}
