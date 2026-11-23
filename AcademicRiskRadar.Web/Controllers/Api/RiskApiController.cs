using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AcademicRiskRadar.Web.Models.ViewModels;
using AcademicRiskRadar.Web.Services;

namespace AcademicRiskRadar.Web.Controllers.Api;

/// <summary>
/// Unit VI: ASP.NET Web API - Risk Evaluation Endpoint
/// </summary>
[ApiController]
[Route("api/risk")]
[Produces("application/json")]
public class RiskApiController : ControllerBase
{
    private readonly IStudentRepository _repository;

    public RiskApiController(IStudentRepository repository)
    {
        _repository = repository;
    }

    // POST: /api/risk/calculate
    [HttpPost("calculate")]
    [ProducesResponseType(typeof(RiskCalculationResponseViewModel), 200)]
    [ProducesResponseType(400)]
    public IActionResult CalculateRisk([FromBody] RiskCalculationRequestViewModel req)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        double totalWeight = req.AttendanceWeight + req.AssessmentWeight + req.AssignmentWeight + req.LmsWeight;
        if (totalWeight <= 0) totalWeight = 100.0;

        double attRisk = Math.Max(0, 100.0 - req.AttendanceRate);
        double testRisk = Math.Max(0, 100.0 - req.AssessmentScore);
        double assignRisk = Math.Max(0, 100.0 - req.AssignmentCompletionPercentage);
        double lmsRisk = Math.Max(0, 100.0 - Math.Min(100.0, (req.LmsActivityHours / 30.0) * 100.0));

        double score = (attRisk * req.AttendanceWeight +
                        testRisk * req.AssessmentWeight +
                        assignRisk * req.AssignmentWeight +
                        lmsRisk * req.LmsWeight) / totalWeight;

        score = Math.Clamp(score, 0.0, 100.0);

        string level = score switch
        {
            >= 70 => "Critical",
            >= 50 => "High",
            >= 30 => "Moderate",
            _ => "Low"
        };

        string primary = attRisk >= testRisk && attRisk >= assignRisk ? "Low Class Attendance"
                       : testRisk >= assignRisk ? "Lagging Internal Assessments"
                       : "Coursework & Assignment Backlog";

        string rec = level switch
        {
            "Critical" => "Immediate Parent Conference & Dean Notification required.",
            "High" => "Mandatory 1-on-1 counseling with assigned Faculty Mentor.",
            "Moderate" => "Targeted peer tutoring for specific struggling subject modules.",
            _ => "Standard academic tracking. Student is performing within safe margins."
        };

        return Ok(new RiskCalculationResponseViewModel
        {
            RiskScore = Math.Round(score, 1),
            RiskLevel = level,
            PrimaryFactor = primary,
            Recommendation = rec,
            EvaluatedAt = DateTime.UtcNow
        });
    }

    // GET: /api/risk/stats
    [HttpGet("stats")]
    public IActionResult GetRiskStats()
    {
        var students = _repository.GetAllStudents().ToList();
        var summary = _repository.GetDepartmentRiskSummary();

        return Ok(new
        {
            totalMonitored = students.Count,
            criticalCount = students.Count(s => s.Risk == Models.RiskLevel.Critical),
            highRiskCount = students.Count(s => s.Risk == Models.RiskLevel.High),
            moderateRiskCount = students.Count(s => s.Risk == Models.RiskLevel.Moderate),
            lowRiskCount = students.Count(s => s.Risk == Models.RiskLevel.Low),
            departmentBreakdown = summary,
            generatedAt = DateTime.UtcNow
        });
    }
}
