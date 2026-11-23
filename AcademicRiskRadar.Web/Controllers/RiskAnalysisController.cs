using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AcademicRiskRadar.Web.Models;
using AcademicRiskRadar.Web.Services;

namespace AcademicRiskRadar.Web.Controllers;

/// <summary>
/// Unit II: Polymorphism & Strategy Execution Controller
/// Unit III: MVC Controller
/// </summary>
public class RiskAnalysisController : Controller
{
    private readonly IStudentRepository _repository;
    private readonly StandardRiskCalculator _standardCalculator;
    private readonly WeightedTrendRiskCalculator _trendCalculator;

    public RiskAnalysisController(
        IStudentRepository repository,
        StandardRiskCalculator standardCalculator,
        WeightedTrendRiskCalculator trendCalculator)
    {
        _repository = repository;
        _standardCalculator = standardCalculator;
        _trendCalculator = trendCalculator;
    }

    [HttpGet]
    public IActionResult Index(string? strategy)
    {
        ViewData["Title"] = "Academic Risk Engine & Analysis";
        ViewBag.SelectedStrategy = strategy ?? "Standard";

        var students = _repository.GetAllStudents().ToList();

        // Unit II: Polymorphism Demonstration - Choose strategy at runtime
        IRiskCalculator activeCalculator = (strategy?.ToLowerInvariant() == "trend")
            ? _trendCalculator
            : _standardCalculator;

        ViewBag.ActiveStrategyName = activeCalculator.StrategyName;

        // Recalculate dynamically to show polymorphism in action
        var analysisResults = students.Select(s =>
        {
            var (score, level, reason) = activeCalculator.Calculate(s);
            return new
            {
                Student = s,
                EvaluatedScore = score,
                EvaluatedLevel = level,
                Reason = reason
            };
        }).ToList();

        return View(analysisResults);
    }

    [HttpGet]
    public IActionResult LogIntervention(string studentId)
    {
        var student = _repository.GetStudentById(studentId);
        if (student == null) return NotFound();

        ViewBag.Student = student;
        return View(new MentoringInterventionEntity
        {
            StudentId = student.Id,
            StudentName = student.Name,
            MentorName = student.MentorName
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult LogIntervention(MentoringInterventionEntity intervention)
    {
        if (string.IsNullOrWhiteSpace(intervention.Notes))
        {
            ModelState.AddModelError("Notes", "Intervention notes cannot be blank.");
            var student = _repository.GetStudentById(intervention.StudentId);
            ViewBag.Student = student;
            return View(intervention);
        }

        _repository.AddIntervention(intervention);
        TempData["AlertMessage"] = $"Mentoring intervention for {intervention.StudentName} logged successfully!";
        TempData["AlertType"] = "success";

        return RedirectToAction("Details", "Students", new { id = intervention.StudentId });
    }
}
