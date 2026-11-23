using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AcademicRiskRadar.Web.Services;

namespace AcademicRiskRadar.Web.Controllers;

/// <summary>
/// Unit III: MVC Pattern - HomeController
/// </summary>
public class HomeController : Controller
{
    private readonly IStudentRepository _repository;

    public HomeController(IStudentRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IActionResult Index()
    {
        // Unit III: Data Passing Techniques - ViewData and ViewBag
        ViewData["Title"] = "Academic Risk Radar | Home";
        ViewBag.SystemVersion = "ASP.NET Core 10 (LTS Architecture)";
        ViewBag.TotalStudents = _repository.GetAllStudents().Count();
        ViewBag.HighRiskCount = _repository.GetHighRiskStudentsView().Count();
        ViewBag.TotalInterventions = _repository.GetAllInterventions().Count();

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
