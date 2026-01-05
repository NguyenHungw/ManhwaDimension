//using ManhwaDimension.Constants;
//using ManhwaDimension.Repository;
//using Microsoft.AspNetCore.Authorization;

//namespace ManhwaDimension.Util.Extentions
//{
//    public class RoleAuthorizationHandler : AuthorizationHandler<RoleRequirement>
//    {
//        private readonly IRoleMenuRepository _roleMenuRepository;

//        public RoleAuthorizationHandler(IRoleMenuRepository roleMenuRepository)
//        {
//            _roleMenuRepository = roleMenuRepository;
//        }

//        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
//        {
//            var roleClaim = context.User.FindFirst(ClaimNames.ROLE_ID);
//            if (roleClaim == null || !int.TryParse(roleClaim.Value, out int roleId))
//            {
//                return;
//            }

//            var allowedMenus = await _roleMenuRepository.ListByRole(roleId);
//            if (allowedMenus.Any(m => m.Menu != null && m.Menu.Url.Equals(requirement.RequiredUrl, StringComparison.OrdinalIgnoreCase)))
//            {
//                context.Succeed(requirement);
//            }
//        }
//    }
//    public class RoleRequirement : IAuthorizationRequirement
//    {
//        public string RequiredUrl { get; }

//        public RoleRequirement(string requiredUrl)
//        {
//            RequiredUrl = requiredUrl;
//        }
//    }
//}
