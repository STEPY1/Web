using AspnetIdentityRoleBasedTutorial.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace AspnetIdentityRoleBasedTutorial.Controllers
{
    [Authorize(Roles = "Staff")]
    public class StaffController : Controller
    {      
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public StaffController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;

        }
        public IActionResult Index(string searchString, int? searchInt)
        {
            var users = _userManager.Users.ToList();

            // Filter the users by name if searchString is not null or empty
            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.Name.Contains(searchString)).ToList();
            }

            // Filter the users by age if searchInt is not null
            if (searchInt.HasValue)
            {
                users = users.Where(u => u.Age == searchInt).ToList();
            }

            var roles = _roleManager.Roles.ToList();
            ViewBag.Users = users;
            ViewBag.Roles = roles;

            return View(users);
        }
        // Action method for displaying the create user form
        public IActionResult Create()
        {
            return View();
        }
        // Action method for processing user creation
        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Name = model.Name,
                    UserName = model.UserName,
                    Email = model.Email,                  
                    Birthday = model.Birthday,
                    EducationLevel = model.EducationLevel,
                    Programming = model.Programming,
                    ToeicScore = model.ToeicScore,
                    Experience = model.Experience,
                    PasswordHash = model.PasswordHash,
                    Address = model.Address
                };

                var result = await _userManager.CreateAsync(user, model.SecurityStamp);

                if (result.Succeeded)
                {
                    // Gán vai trò "User" cho người dùng mới
                    await _userManager.AddToRoleAsync(user, "User");

                    // Redirect to index or any other page after successful user creation
                    return RedirectToAction("Index", "Staff");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
        // Action method for displaying the create user form
        public IActionResult CreateTrainer()
        {

            return View();
        }
        // Action method for processing user creation
        [HttpPost]
        public async Task<IActionResult> CreateTrainer(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Name = model.Name,
                    UserName = model.Email,                                     
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.SecurityStamp);

                if (result.Succeeded)
                {
                    // Gán vai trò "User" cho người dùng mới
                    await _userManager.AddToRoleAsync(user, "Trainer");

                    // Redirect to index or any other page after successful user creation
                    return RedirectToAction("Index", "Staff");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUser model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id);
                    if (user == null)
                    {
                        return NotFound();
                    }

                    // Update user properties
                    user.Name = model.Name;
                    user.UserName = model.UserName;
                    user.Email=model.Email;
                    user.Birthday = model.Birthday;
                    user.EducationLevel = model.EducationLevel;
                    user.Programming = model.Programming;
                    user.ToeicScore = model.ToeicScore;
                    user.Experience = model.Experience;
                    user.Address = model.Address;

                    // Update user in the database
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(model);
        }


        public async Task<IActionResult> EditTrainer(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTrainer(string id, ApplicationUser model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id);
                    if (user == null)
                    {
                        return NotFound();
                    }

                    user.Name = model.Name;
                    user.UserName = model.UserName;
                    user.Address = model.Address;
                    user.PhoneNumber = model.PhoneNumber;                                      
                    user.Email = model.Email;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(model);
        }
        private bool UserExists(string id)
        {
            return _userManager.Users.Any(e => e.Id == id);
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
                return RedirectToAction("Index", "Staff");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(user);
        }

        // Hiển thị xác nhận xóa người dùng
        public async Task<IActionResult> DeleteTrainer(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // Xử lý xóa người dùng
        [HttpPost, ActionName("DeleteTrainer")]
        public async Task<IActionResult> DeleteTrainerConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Staff");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(user);
        }
    }
}
