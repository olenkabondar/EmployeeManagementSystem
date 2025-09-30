using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Controllers
{
    [Authorize]
    public class PositionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PositionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Positions.ToList());
        }

        public IActionResult Details(int id)
        {
            var position = _context.Positions
               .Include(p => p.Employees)
               .ThenInclude(e => e.Department)
               .FirstOrDefault(p => p.Id == id);

            if (position == null) return NotFound();
            return View(position);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Position position)
        {
            if (ModelState.IsValid)
            {
                _context.Positions.Add(position);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(position);
        }

        public IActionResult Edit(int id)
        {
            var position = _context.Positions.Find(id);
            if (position == null) return NotFound();
            return View(position);
        }

        [HttpPost]
        public IActionResult Edit(Position position)
        {
            if (ModelState.IsValid)
            {
                _context.Positions.Update(position);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(position);
        }

        public IActionResult Delete(int id)
        {
            var position = _context.Positions.Find(id);
            if (position == null) return NotFound();
            return View(position);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public IActionResult DeleteConfirmed(int id)
        {
            var position = _context.Positions.Find(id);
            if (position == null) return NotFound();

            _context.Positions.Remove(position);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
