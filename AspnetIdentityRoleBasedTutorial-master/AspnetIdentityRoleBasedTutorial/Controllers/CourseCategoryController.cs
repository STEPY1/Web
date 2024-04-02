using AspnetIdentityRoleBasedTutorial.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetIdentityRoleBasedTutorial.Controllers
{
    [Authorize(Roles = "Staff")]
    public class CourseCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseCategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CourseCategory
        public async Task<IActionResult> Index(string searchString)
        {
            // Lấy tất cả các categories từ database
            var categories = await _context.Categories.ToListAsync();

            // Kiểm tra xem có chuỗi tìm kiếm được cung cấp không
            if (!string.IsNullOrEmpty(searchString))
            {
                // Nếu có, lọc danh sách categories để chỉ chứa các category thỏa mãn điều kiện tìm kiếm
                categories = categories.Where(c => c.Name.Contains(searchString) || c.Description.Contains(searchString)).ToList();
            }

            // Trả về view Index với danh sách categories đã được lọc (nếu có)
            return View(categories);
        }

        // GET: CourseCategory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CourseCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: CourseCategory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: CourseCategory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        // GET: CourseCategory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: CourseCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
