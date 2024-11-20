using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ClimbStatisticsController : Controller
{
    private readonly AppDbContext _context;

    public ClimbStatisticsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetYears()
    {
        var years = _context.ClimbStatistics
            .Select(c => c.Year)
            .Distinct()
            .OrderBy(y => y)
            .ToList();

        return Json(years);
    }

    // Эндпойнт для получения статистики по годам
    [HttpGet]
    public async Task<IActionResult> GetClimbStatistics(int year)
    {
        var statistics = await _context.ClimbStatistics
            .Where(cs => cs.Year == year)
            .GroupBy(cs => cs.ClimberName)
            .Select(g => new
            {
                climberName = g.Key,
                climbCount = g.Sum(cs => cs.ClimbCount)
            })
            .ToListAsync();

        return Json(statistics);
    }

    // Эндпойнт для отображения страницы с диаграммами
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddClimbStatistic(int year, string climberName, int climbCount)
    {
        var climbStatistic = new ClimbStatistic { Year = year, ClimberName = climberName, ClimbCount = climbCount };
        _context.ClimbStatistics.Add(climbStatistic);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteClimbStatistic(int id)
    {
        var climbStatistic = await _context.ClimbStatistics.FindAsync(id);
        if (climbStatistic != null)
        {
            _context.ClimbStatistics.Remove(climbStatistic);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult AddClimbStatistic()
    {
        return View();
    }

    [HttpGet]
    public IActionResult DeleteClimbStatistic()
    {
        return View();
    }
}
