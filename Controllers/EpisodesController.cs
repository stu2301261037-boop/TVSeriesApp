using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TVSeriesApp.Data;
using TVSeriesApp.Models;
using TVSeriesApp.Attributes;

namespace TVSeriesApp.Controllers
{
    [Authorize]
    public class EpisodesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EpisodesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = string.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["RatingSortParm"] = sortOrder == "rating" ? "rating_desc" : "rating";
            ViewData["DateSortParm"] = sortOrder == "date" ? "date_desc" : "date";

            if (searchString != null) pageNumber = 1;
            else searchString = currentFilter;

            ViewData["CurrentFilter"] = searchString;

            IQueryable<Episode> episodes = _context.Episodes.Include(e => e.Series);

            if (!string.IsNullOrEmpty(searchString))
            {
                episodes = episodes.Where(e => e.Title.Contains(searchString) || e.Series.Title.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "title_desc": episodes = episodes.OrderByDescending(e => e.Title); break;
                case "rating": episodes = episodes.OrderBy(e => e.Rating); break;
                case "rating_desc": episodes = episodes.OrderByDescending(e => e.Rating); break;
                case "date": episodes = episodes.OrderBy(e => e.AirDate); break;
                case "date_desc": episodes = episodes.OrderByDescending(e => e.AirDate); break;
                default: episodes = episodes.OrderBy(e => e.Title); break;
            }

            int pageSize = 5;
            return View(await PaginatedList<Episode>.CreateAsync(episodes.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var episode = await _context.Episodes
                .Include(e => e.Series)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (episode == null) return NotFound();
            return View(episode);
        }

        [AuthorizeRoles("Admin")]
        public IActionResult Create()
        {
            ViewData["SeriesId"] = new SelectList(_context.Series, "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles("Admin")]
        public async Task<IActionResult> Create([Bind("Id,Title,SeasonNumber,EpisodeNumber,AirDate,Duration,Rating,SeriesId")] Episode episode)
        {
            if (ModelState.IsValid)
            {
                _context.Add(episode);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SeriesId"] = new SelectList(_context.Series, "Id", "Title", episode.SeriesId);
            return View(episode);
        }

        [AuthorizeRoles("Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var episode = await _context.Episodes.FindAsync(id);
            if (episode == null) return NotFound();
            ViewData["SeriesId"] = new SelectList(_context.Series, "Id", "Title", episode.SeriesId);
            return View(episode);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles("Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,SeasonNumber,EpisodeNumber,AirDate,Duration,Rating,SeriesId")] Episode episode)
        {
            if (id != episode.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(episode);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EpisodeExists(episode.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SeriesId"] = new SelectList(_context.Series, "Id", "Title", episode.SeriesId);
            return View(episode);
        }

        [AuthorizeRoles("Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var episode = await _context.Episodes
                .Include(e => e.Series)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (episode == null) return NotFound();
            return View(episode);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles("Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var episode = await _context.Episodes.FindAsync(id);
            if (episode != null) _context.Episodes.Remove(episode);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EpisodeExists(int id)
        {
            return _context.Episodes.Any(e => e.Id == id);
        }
    }
}
