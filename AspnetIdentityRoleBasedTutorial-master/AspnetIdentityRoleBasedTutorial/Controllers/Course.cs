using AspnetIdentityRoleBasedTutorial.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetIdentityRoleBasedTutorial.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CourseController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            // Lấy danh sách người dùng từ UserManager
            var users = await _userManager.Users.ToListAsync();

            // Gán danh sách người dùng vào ViewBag.Users
            ViewBag.Users = users;

            // Lấy danh sách các khóa học và người đào tạo
            var courses = await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Trainer)
                .ToListAsync();

            // Lấy danh sách người dùng có vai trò là "Trainer"
            var trainers = await _userManager.GetUsersInRoleAsync("Trainer");

            // Truyền danh sách người dùng vào ViewBag.Trainers
            ViewBag.Trainers = trainers;

            return View(courses);
        }



        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Course/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoryCourse = _context.Categories.ToList();
            return View(course);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = _context.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }

            var categories = _context.Categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            ViewBag.Categories = categories;

            return View(course);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = _context.Categories.ToList();
            return View(course);
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }

        // GET: Course/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Trainer)
                .FirstOrDefaultAsync(m => m.CourseId == id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: Course/AddTrainer/5
        public async Task<IActionResult> AddTrainer(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            // Lấy dữ liệu Name của Course và truyền vào ViewBag
            ViewBag.CourseName = course.Name;

            // Lấy danh sách người dùng có vai trò là "Trainer"
            var trainers = await _userManager.GetUsersInRoleAsync("Trainer");

            // Truyền danh sách người dùng vào ViewBag.Trainers
            ViewBag.Trainers = new SelectList(trainers, "Id", "UserName"); // Chỉnh sửa ở đây để sử dụng UserName hoặc Name tùy theo yêu cầu

            return View();
        }

        // POST: Course/AddTrainer/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTrainer(int id, string trainerId)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            var trainer = await _userManager.FindByIdAsync(trainerId);
            if (trainer == null)
            {
                return NotFound();
            }

            course.Trainer = trainer;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


    }
}
