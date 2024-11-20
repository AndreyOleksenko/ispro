using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ClimbingClub.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // Этот метод будет возвращать представление (HTML)
        public IActionResult Index()
        {
            return View();
        }

        // Этот метод будет возвращать данные для графиков
        [HttpGet]
        public async Task<IActionResult> GetClimbStatistics(int year)
        {
            var statistics = await _context.ClimbStatistics
                .Where(s => s.Year == year)
                .GroupBy(s => s.ClimberName)
                .Select(g => new
                {
                    climberName = g.Key,
                    climbCount = g.Sum(s => s.ClimbCount)
                })
                .ToListAsync();

            return Json(statistics);
        }
    }
}
