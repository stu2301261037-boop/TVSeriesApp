using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TVSeriesApp.Data;
using TVSeriesApp.Models;

namespace TVSeriesApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var topSeries = await _context.Series
                .OrderByDescending(s => s.Rating)
                .Take(6)
                .ToListAsync();

            return View(topSeries);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}