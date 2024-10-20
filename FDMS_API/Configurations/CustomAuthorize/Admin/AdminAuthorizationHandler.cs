using FDMS_API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FDMS_API.Configurations.CustomAuthorize.Admin
{
    public class AdminAuthorizationHandler : AuthorizationHandler<AdminRequirement>
    {
        private readonly AppDbContext _context;
        public AdminAuthorizationHandler(AppDbContext context)
        {
            _context = context;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRequirement requirement)
        {
            var userID = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(userID == null)
            {
                context.Fail();
                return;
            }
            bool isAdmin = await _context.Users.AnyAsync(x => x.UserID.ToString() == userID && x.Role == "ADMIN");
            
            if(isAdmin)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}
