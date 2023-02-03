using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using eSchool.Data;
using eSchool.Models;

namespace eSchool.Controllers
{
    public class ClassesController : Controller
    {
        private readonly SchoolDbContext _context;

        public ClassesController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: Classes
        public async Task<IActionResult> Index()
        {
            var schoolDbContext = _context.Classes.Include(q => q.Course).Include(q => q.Lecturer);
            return View(await schoolDbContext.ToListAsync());
        }

        // GET: Classes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Classes == null)
            {
                return NotFound();
            }

            var @class = await _context.Classes
                .Include(q => q.Course)
                .Include(q => q.Lecturer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@class == null)
            {
                return NotFound();
            }

            return View(@class);
        }

        private void CreateSelectList()
        {
            var course = _context.Courses.Select(q => new
            {
                CName = $"{q.Codes} - {q.Name}",
                q.Id
            });

            ViewData["CourseId"] = new SelectList(course, "Id", "CName");
            var lecturer = _context.Lecturers.Select(q => new
            {
                Name = $"{q.FirstName} {q.Surname}",
                q.Id
            });
            ViewData["LecturerId"] = new SelectList(lecturer, "Id", "Name");
        }
        // GET: Classes/Create
        public IActionResult Create()
        {
           CreateSelectList();
            return View();
        }

        // POST: Classes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LecturerId,CourseId,CourseTime")] Class @class)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@class);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            CreateSelectList();
            return View(@class);
        }

        // GET: Classes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Classes == null)
            {
                return NotFound();
            }

            var @class = await _context.Classes.FindAsync(id);
            if (@class == null)
            {
                return NotFound();
            }
            CreateSelectList();
            return View(@class);
        }

        // POST: Classes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LecturerId,CourseId,CourseTime")] Class @class)
        {
            if (id != @class.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@class);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassExists(@class.Id))
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
            CreateSelectList();
            return View(@class);
        }

        // GET: Classes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Classes == null)
            {
                return NotFound();
            }

            var @class = await _context.Classes
                .Include(q => q.Course)
                .Include(q => q.Lecturer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@class == null)
            {
                return NotFound();
            }

            return View(@class);
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Classes == null)
            {
                return Problem("Entity set 'SchoolDbContext.Classes'  is null.");
            }
            var @class = await _context.Classes.FindAsync(id);
            if (@class != null)
            {
                _context.Classes.Remove(@class);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> ManageEnrollments(int id)
        {
            var @class = await _context.Classes
                .Include(q => q.Course)
                .Include(q => q.Lecturer)
                .Include(q=>q.Enrollments).ThenInclude(q=>q.Student)
                .FirstOrDefaultAsync(m => m.Id == id);

            var students = await _context.Students.ToListAsync();
            var model = new ClassEnrollmentViewModel();
            model.Class = new ClassViewModel
            {
                Id = @class.Id,
                CourseName = $"{@class.Course.Codes} - {@class.Course.Name}",
                LecturerName = $"{@class.Lecturer.FirstName} {@class.Lecturer.Surname}",
                Time = @class.CourseTime.ToString(),
            };
            foreach (var stud in students)
            {
                model.Students.Add( new StudentEnrollmentViewModel
                {
                    Id=stud.Id,
                    FirstName = stud.FirstName,
                    Surname = stud.Surname,
                    IsEnrolled = (@class?.Enrollments?.Any(q=>q.StudentId == stud.Id)).GetValueOrDefault()
                });
                
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnrollStudent(int classId, int studentId, bool shouldEnroll)
        {
            var enrollment = new Enrollment();
            if (shouldEnroll)
            {
                enrollment.ClassId =classId;
                enrollment.StudentId = studentId;
                await _context.AddAsync(enrollment);
            }
            else
            {
                enrollment = await _context.Enrollments.FirstOrDefaultAsync(
                    q => q.StudentId == studentId && q.ClassId == classId);
                if (enrollment != null)
                {

                    _context.Remove(enrollment);
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManageEnrollments), new {id=classId});
           // return View();
        }
        private bool ClassExists(int id)
        {
          return (_context.Classes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
