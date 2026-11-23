using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AcademicRiskRadar.Web.Models;
using AcademicRiskRadar.Web.Services;

namespace AcademicRiskRadar.Web.Controllers.Api;

/// <summary>
/// Unit VI: ASP.NET Web API - Interventions Controller
/// </summary>
[ApiController]
[Route("api/interventions")]
[Produces("application/json")]
public class InterventionsApiController : ControllerBase
{
    private readonly IStudentRepository _repository;

    public InterventionsApiController(IStudentRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_repository.GetAllInterventions());
    }

    [HttpPost]
    public IActionResult Create([FromBody] MentoringInterventionEntity intervention)
    {
        if (string.IsNullOrWhiteSpace(intervention.StudentId) || string.IsNullOrWhiteSpace(intervention.Notes))
        {
            return BadRequest(new { message = "Student ID and notes are required fields." });
        }

        var student = _repository.GetStudentById(intervention.StudentId);
        if (student != null)
        {
            intervention.StudentName = student.Name;
            if (string.IsNullOrWhiteSpace(intervention.MentorName))
            {
                intervention.MentorName = student.MentorName;
            }
        }

        _repository.AddIntervention(intervention);
        return CreatedAtAction(nameof(GetAll), new { id = intervention.Id }, intervention);
    }
}

/// <summary>
/// Unit VI: Routing - Convention Routing Example Controller
/// Mapped via app.MapControllerRoute(name: "api_convention", pattern: "api/convention/{controller}/{action}/{id?}")
/// Demonstrates the difference between Convention-Based and Attribute-Based Routing.
/// </summary>
public class ConventionApiController : Controller
{
    private readonly IStudentRepository _repository;

    public ConventionApiController(IStudentRepository repository)
    {
        _repository = repository;
    }

    // Accessible via: /api/convention/ConventionApi/GetStudentSummary?id=STU-001
    // (Notice how the URL path matches the folder/controller/action naming convention)
    [HttpGet]
    public IActionResult GetStudentSummary(string? id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return Json(new
            {
                routingType = "Convention-Based Routing",
                routePattern = "api/convention/{controller}/{action}/{id?}",
                status = "Missing parameter 'id'. Example: ?id=STU-001"
            });
        }

        var student = _repository.GetStudentById(id) ?? _repository.GetStudentByCode(id);
        if (student == null)
        {
            return Json(new { routingType = "Convention-Based Routing", error = "Student not found" });
        }

        return Json(new
        {
            routingType = "Convention-Based Routing",
            controllerName = ControllerContext.ActionDescriptor.ControllerName,
            actionName = ControllerContext.ActionDescriptor.ActionName,
            student = new
            {
                student.Id,
                student.StudentCode,
                student.Name,
                student.Department,
                student.Risk
            }
        });
    }

    // Accessible via: /api/convention/ConventionApi/EchoServerStatus
    [HttpGet]
    public IActionResult EchoServerStatus()
    {
        return Json(new
        {
            routingType = "Convention-Based Routing",
            status = "Operational",
            timestamp = DateTime.UtcNow,
            serverEnvironment = Environment.OSVersion.VersionString
        });
    }
}
