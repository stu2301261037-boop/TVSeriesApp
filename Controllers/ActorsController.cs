using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TVSeriesApp.Data;
using TVSeriesApp.Models;
using TVSeriesApp.Attributes;

namespace TVSeriesApp.Controllers
{
    [Authorize]
    public class ActorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["BirthDateSortParm"] = sortOrder == "birthdate" ? "birthdate_desc" : "birthdate";
            ViewData["SalarySortParm"] = sortOrder == "salary" ? "salary_desc" : "salary";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var actors = from a in _context.Actors select a;

            if (!string.IsNullOrEmpty(searchString))
            {
                actors = actors.Where(a => a.FirstName.Contains(searchString) ||
                                         a.LastName.Contains(searchString) ||
                                         a.Nationality.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    actors = actors.OrderByDescending(a => a.LastName);
                    break;
                case "birthdate":
                    actors = actors.OrderBy(a => a.BirthDate);
                    break;
                case "birthdate_desc":
                    actors = actors.OrderByDescending(a => a.BirthDate);
                    break;
                case "salary":
                    actors = actors.OrderBy(a => a.Salary);
                    break;
                case "salary_desc":
                    actors = actors.OrderByDescending(a => a.Salary);
                    break;
                default:
                    actors = actors.OrderBy(a => a.LastName);
                    break;
            }

            int pageSize = 5;
            return View(await PaginatedList<Actor>.CreateAsync(actors.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var actor = await _context.Actors.FirstOrDefaultAsync(m => m.Id == id);
            if (actor == null) return NotFound();
            return View(actor);
        }

        [AuthorizeRoles("Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles("Admin")]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,BirthDate,Nationality,Salary,IsAwardWinner")] Actor actor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actor);
        }

        [AuthorizeRoles("Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null) return NotFound();
            return View(actor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles("Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,BirthDate,Nationality,Salary,IsAwardWinner")] Actor actor)
        {
            if (id != actor.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorExists(actor.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(actor);
        }

        [AuthorizeRoles("Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var actor = await _context.Actors.FirstOrDefaultAsync(m => m.Id == id);
            if (actor == null) return NotFound();
            return View(actor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles("Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor != null) _context.Actors.Remove(actor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActorExists(int id)
        {
            return _context.Actors.Any(e => e.Id == id);
        }
    }
}
