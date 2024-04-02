using AspnetIdentityRoleBasedTutorial.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetIdentityRoleBasedTutorial.Controllers
{
    public class TopicController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TopicController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Topics
        public async Task<IActionResult> Index(string searchString)
        {
            ViewBag.CurrentFilter = searchString;

            // Check if searchString is empty or null
            if (string.IsNullOrEmpty(searchString))
            {
                // If searchString is empty or null, return all topics with their course and student included
                return View(await _context.Topics
                                            .Include(t => t.Course)
                                            .Include(t => t.Student)
                                            .ToListAsync());
            }
            else
            {
                // If searchString is provided, filter topics by searchString and return the filtered topics with their course and student included
                return View(await _context.Topics
                                            .Include(t => t.Course)
                                            .Include(t => t.Student)
                                            .Where(t => t.TopicName.Contains(searchString))
                                            .ToListAsync());
            }
        }

        // GET: Topics/Create
        public IActionResult Create()
        {
            ViewBag.Courses = new SelectList(_context.Courses, "CourseId", "Name");
            return View();
        }

        // POST: Topics/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TopicName,Description,CourseId")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                _context.Add(topic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(topic);
        }

        // GET: Topics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _context.Topics.FindAsync(id);
            if (topic == null)
            {
                return NotFound();
            }
            ViewBag.Courses = new SelectList(_context.Courses, "CourseId", "Name", topic.CourseId);
            return View(topic);
        }

        // POST: Topics/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TopicId,TopicName,Description,CourseId")] Topic topic)
        {
            if (id != topic.TopicId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(topic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TopicExists(topic.TopicId))
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
            return View(topic);
        }

        // GET: Topics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _context.Topics
                .Include(t => t.Course)
                .FirstOrDefaultAsync(m => m.TopicId == id);
            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

        // POST: Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var topic = await _context.Topics.FindAsync(id);
            _context.Topics.Remove(topic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TopicExists(int id)
        {
            return _context.Topics.Any(e => e.TopicId == id);
        }

        // GET: Topics/AssignStudent/5
        public async Task<IActionResult> AssignStudent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _context.Topics.FindAsync(id);
            if (topic == null)
            {
                return NotFound();
            }

            // Retrieve the list of students with the "Student" role
            var students = await _userManager.GetUsersInRoleAsync("User");

            // Pass the list of students to the view
            ViewBag.Students = new SelectList(students, "Id", "UserName");

            return View(topic);
        }

        // POST: Topics/AssignStudent/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignStudent(int id, string studentId)
        {
            var topic = await _context.Topics.FindAsync(id);
            if (topic == null)
            {
                return NotFound();
            }

            var student = await _userManager.FindByIdAsync(studentId);
            if (student == null)
            {
                return NotFound();
            }

            topic.StudentId = studentId;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
