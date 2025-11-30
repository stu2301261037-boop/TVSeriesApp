using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TVSeriesApp.Data;
using TVSeriesApp.Models;
using TVSeriesApp.Attributes;

namespace TVSeriesApp.Controllers
{
	[Authorize]
	public class SeriesController : Controller
	{
		private readonly ApplicationDbContext _context;

		public SeriesController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: Series
		[AllowAnonymous]
		public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? pageNumber)
		{
			ViewData["CurrentSort"] = sortOrder;
			ViewData["TitleSortParm"] = string.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
			ViewData["RatingSortParm"] = sortOrder == "rating" ? "rating_desc" : "rating";
			ViewData["DateSortParm"] = sortOrder == "date" ? "date_desc" : "date";

			if (searchString != null)
			{
				pageNumber = 1;
			}
			else
			{
				searchString = currentFilter;
			}

			ViewData["CurrentFilter"] = searchString;

			var series = from s in _context.Series
						 select s;

			if (!string.IsNullOrEmpty(searchString))
			{
				series = series.Where(s => s.Title.Contains(searchString)
										|| s.Genre.Contains(searchString)
										|| s.Description.Contains(searchString));
			}

			switch (sortOrder)
			{
				case "title_desc":
					series = series.OrderByDescending(s => s.Title);
					break;
				case "rating":
					series = series.OrderBy(s => s.Rating);
					break;
				case "rating_desc":
					series = series.OrderByDescending(s => s.Rating);
					break;
				case "date":
					series = series.OrderBy(s => s.ReleaseDate);
					break;
				case "date_desc":
					series = series.OrderByDescending(s => s.ReleaseDate);
					break;
				default:
					series = series.OrderBy(s => s.Title);
					break;
			}

			int pageSize = 5;
            return View(await PaginatedList<Series>.CreateAsync(series.AsNoTracking(), pageNumber ?? 1, pageSize));
		}
		
        // GET: Series/Details/5
        [AllowAnonymous]
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var series = await _context.Series
				.Include(s => s.Episodes)
				.FirstOrDefaultAsync(m => m.Id == id);

            if (series == null)
			{
				return NotFound();
			}

			return View(series);
		}

		// GET: Series/Create
		[AuthorizeRoles("Admin")]
		public IActionResult Create()
		{
			return View();
		}

		// POST: Series/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		[AuthorizeRoles("Admin")]
		public async Task<IActionResult> Create([Bind("Id,Title,Description,ReleaseDate,Seasons,Rating,Genre,IsCompleted")] Series series)
		{
			if (ModelState.IsValid)
			{
				_context.Add(series);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(series);
		}

		// GET: Series/Edit/5
		[AuthorizeRoles("Admin")]
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var series = await _context.Series.FindAsync(id);
			if (series == null)
			{
				return NotFound();
			}
			return View(series);
		}

		// POST: Series/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[AuthorizeRoles("Admin")]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,ReleaseDate,Seasons,Rating,Genre,IsCompleted")] Series series)
		{
			if (id != series.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(series);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!SeriesExists(series.Id))
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
			return View(series);
		}

		// GET: Series/Delete/5
		[AuthorizeRoles("Admin")]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var series = await _context.Series
				.FirstOrDefaultAsync(m => m.Id == id);
			if (series == null)
			{
				return NotFound();
			}

			return View(series);
		}

		// POST: Series/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		[AuthorizeRoles("Admin")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var series = await _context.Series.FindAsync(id);
			if (series != null)
			{
				_context.Series.Remove(series);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool SeriesExists(int id)
		{
			return _context.Series.Any(e => e.Id == id);
		}

	}
}
