using Microsoft.AspNetCore.Authorization;

namespace FDMS_API.Configurations.CustomAuthorize.CanEdit
{
    public class EditRequirement:IAuthorizationRequirement
    {
        public EditRequirement()
        {
            
        }
    }
}
