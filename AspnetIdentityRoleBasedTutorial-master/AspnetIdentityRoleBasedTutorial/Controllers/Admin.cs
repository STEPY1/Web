using AspnetIdentityRoleBasedTutorial.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspnetIdentityRoleBasedTutorial.Controllers
{
    [Authorize(Roles ="Admin")]
    public class Admin : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        

        public Admin(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
           
        }
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            var roles = _roleManager.Roles.ToList();
            ViewBag.Users = users;
            ViewBag.Roles = roles;
            return View();
        }
        // Thêm hành động để hiển thị form tạo vai trò mới
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                ModelState.AddModelError("", "Role name cannot be empty.");
                return View();
            }

            if (!IsValidRoleName(Name))
            {
                ModelState.AddModelError("", "Invalid role name.");
                return View();
            }

            var roleExist = await _roleManager.RoleExistsAsync(Name);
            if (roleExist)
            {
                ModelState.AddModelError("", "Role already exists.");
                return View();
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(Name));
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Admin");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View();
        }

        private bool IsValidRoleName(string Name)
        {
            // Kiểm tra logic kiểm tra tên vai trò ở đây, ví dụ: không chứa ký tự đặc biệt...
            return true;
        }

        // Hiển thị form thêm người dùng vào vai trò
        public IActionResult AddUserToRole()
        {
            ViewBag.Users = _userManager.Users.ToList();
            ViewBag.Roles = _roleManager.Roles.ToList();
            return View();
        }

        // Xử lý thêm người dùng vào vai trò
        [HttpPost]
        public async Task<IActionResult> AddUserToRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var role = await _roleManager.FindByNameAsync(roleName);

            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                return View();
            }

            if (role == null)
            {
                ModelState.AddModelError("", "Role not found.");
                return View();
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Admin");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View();
        }



        // Action method for displaying the create user form
        public IActionResult Create()
        {
            ViewBag.Roles = _roleManager.Roles.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUser model, string roleName)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Name = model.Name,
                    UserName = model.UserName,
                    Email = model.Email,
                    PasswordHash = model.PasswordHash
                };

                var result = await _userManager.CreateAsync(user, model.SecurityStamp);

                if (result.Succeeded)
                {
                    // Thêm người dùng vào vai trò nếu roleName được chọn
                    if (!string.IsNullOrEmpty(roleName))
                    {
                        var roleExists = await _roleManager.RoleExistsAsync(roleName);
                        if (roleExists)
                        {
                            await _userManager.AddToRoleAsync(user, roleName);
                        }
                    }

                    // Redirect to index or any other page after successful user creation
                    return RedirectToAction("Index", "Admin");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
        // Hiển thị xác nhận xóa người dùng
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // Xử lý xóa người dùng
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Admin");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(user);
        }
        // Hiển thị xác nhận xóa vai trò
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        // Xử lý xác nhận xóa vai trò
        [HttpPost, ActionName("DeleteRole")]
        public async Task<IActionResult> DeleteRoleConfirmed(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Admin");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(role);
        }
    }

}
