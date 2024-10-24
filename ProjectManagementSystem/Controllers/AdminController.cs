using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> identityRole)
        {
            _userManager = userManager;
            _roleManager = identityRole;
        }

        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var userList = new List<Dictionary<string, object>>();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                userList.Add(new Dictionary<string, object>
                {
                    { "UserId", user.Id },
                    { "UserName", user.UserName },
                    { "Roles", userRoles }
                });
            }
            ViewBag.Users = userList;
            return View();

        }
        public async Task<IActionResult> AssignRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = _roleManager.Roles.ToList();
            var userRoles = await _userManager.GetRolesAsync(user);

            ViewBag.UserId = user.Id;
            ViewBag.UserName = user.UserName;
            ViewBag.Roles = roles;
            ViewBag.UserRoles = userRoles;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, List<string> selectedRoles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRolesAsync(user, selectedRoles);
            

            return RedirectToAction("Index");
        }
    }
}
