using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AcademicRiskRadar.Web.Models;
using AcademicRiskRadar.Web.Services;

namespace AcademicRiskRadar.Web.Controllers;

/// <summary>
/// Unit V: SQL Server For Backend Development
/// Interactive SQL Server Explorer Controller demonstrating:
/// Tables, Constraints, Clauses, Operators, Joins, Views, and Stored Procedures.
/// </summary>
public class SqlExplorerController : Controller
{
    private readonly IStudentRepository _repository;

    public SqlExplorerController(IStudentRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IActionResult Index(string tab = "joins")
    {
        ViewData["Title"] = "SQL Server Backend Studio & Explorer";
        ViewBag.ActiveTab = tab.ToLowerInvariant();

        // 1. Joins data
        ViewBag.InnerJoinData = _repository.ExecuteInnerJoin();
        ViewBag.LeftJoinData = _repository.ExecuteLeftJoin();
        ViewBag.RightJoinData = _repository.ExecuteRightJoin();
        ViewBag.FullJoinData = _repository.ExecuteFullJoin();

        // 2. Views data
        ViewBag.HighRiskView = _repository.GetHighRiskStudentsView();
        ViewBag.AttendanceView = _repository.GetAttendancePerformanceView();
        ViewBag.DeptSummaryView = _repository.GetDepartmentRiskSummary();

        // 3. Stored Procedure demo baseline
        ViewBag.SpRiskData = _repository.ExecuteSpGetStudentsAtRisk(RiskLevel.High, 65.0);

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RunStoredProcedure(RiskLevel minRisk, double maxAttendance)
    {
        var results = _repository.ExecuteSpGetStudentsAtRisk(minRisk, maxAttendance);
        TempData["SpMessage"] = $"Executed sp_GetStudentsAtRisk (MinRisk: {minRisk}, MaxAttendance: {maxAttendance}%). Returned {results.Count()} records.";
        TempData["SpMinRisk"] = (int)minRisk;
        TempData["SpMaxAtt"] = maxAttendance;
        return RedirectToAction(nameof(Index), new { tab = "procs" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RunEnrollmentProcedure(string studentId, string courseCode, string courseName, int credits)
    {
        var result = _repository.ExecuteSpEnrollStudentAndRecalculateRisk(studentId, courseCode, courseName, credits);
        TempData["EnrollMessage"] = result.Message;
        TempData["EnrollSuccess"] = result.Success;
        return RedirectToAction(nameof(Index), new { tab = "procs" });
    }
}
