using FDMS_API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FDMS_API.Configurations.CustomAuthorize.CanRead
{
    public class ReadAuthorizationHandler : AuthorizationHandler<ReadRequirement>
    {
        private readonly AppDbContext _dbContext;

        public ReadAuthorizationHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ReadRequirement requirement)
        {
            // Lấy userID từ claims
            var userID = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userID == null)
            {
                context.Fail();
                return;
            }

            // Lấy HttpContext
            var httpContext = context.Resource as HttpContext;
            if (httpContext == null)
            {
                context.Fail();
                return;
            }

            // Lấy documentID từ RouteValues
            if (!httpContext.Request.RouteValues.TryGetValue("documentID", out var documentID))
            {
                context.Fail();
                return;
            }

            if (!string.IsNullOrEmpty(documentID?.ToString()))
            {
                var documentId = int.Parse(documentID.ToString());

                // Lấy danh sách các nhóm mà người dùng tham gia
                var userInGroups = await _dbContext.GroupUsers
                    .Where(x => x.UserID.ToString() == userID)
                    .Select(x => x.GroupID)
                    .ToListAsync();

                // Lấy danh sách các nhóm có quyền truy cập tài liệu
                var groupCanAccessDocuments = await _dbContext.DocumentPermissions
                    .Where(x => x.DocumentID == documentId)
                    .Select(x => x.GroupID)
                    .ToListAsync();

                // Kiểm tra quyền truy cập
                foreach (var group in userInGroups)
                {
                    if (groupCanAccessDocuments.Contains(group))
                    {
                        var permission = await _dbContext.Permissions.FirstOrDefaultAsync(x => x.GroupID == group);
                        if (permission != null && permission.CanRead)
                        {
                            context.Succeed(requirement);
                            return;
                        }
                    }
                }
            }

            // Nếu không có quyền nào phù hợp, thì gọi Fail
            context.Fail();
        }

    }
}
