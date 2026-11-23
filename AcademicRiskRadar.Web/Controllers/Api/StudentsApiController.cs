using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AcademicRiskRadar.Web.Models;
using AcademicRiskRadar.Web.Models.ViewModels;
using AcademicRiskRadar.Web.Services;

namespace AcademicRiskRadar.Web.Controllers.Api;

/// <summary>
/// Unit VI: ASP.NET Core Web API with Attribute Routing & CRUD Operations
/// Follows REST principles: stateless, HTTP verbs, standard status codes.
/// </summary>
[ApiController]
[Route("api/students")]
[Produces("application/json")]
public class StudentsApiController : ControllerBase
{
    private readonly IStudentRepository _repository;
    private readonly IRiskCalculator _riskCalculator;

    public StudentsApiController(IStudentRepository repository, IRiskCalculator riskCalculator)
    {
        _repository = repository;
        _riskCalculator = riskCalculator;
    }

    // GET: /api/students
    // Supports query filtering: ?department=CSE&risk=High&search=Aarav
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<StudentEntity>), 200)]
    public IActionResult GetAll([FromQuery] string? department, [FromQuery] RiskLevel? risk, [FromQuery] string? search, [FromQuery] string? sortBy)
    {
        var students = _repository.QueryStudents(department, risk, search, sortBy);
        return Ok(students);
    }

    // GET: /api/students/{id}
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(StudentEntity), 200)]
    [ProducesResponseType(404)]
    public IActionResult GetById(string id)
    {
        var student = _repository.GetStudentById(id) ?? _repository.GetStudentByCode(id);
        if (student == null)
        {
            return NotFound(new { message = $"Student with ID or Code '{id}' was not found.", timestamp = DateTime.UtcNow });
        }
        return Ok(student);
    }

    // POST: /api/students
    [HttpPost]
    [ProducesResponseType(typeof(StudentEntity), 201)]
    [ProducesResponseType(400)]
    public IActionResult Create([FromBody] StudentViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (_repository.GetStudentByCode(model.StudentCode) != null)
        {
            return BadRequest(new { message = $"A student with code '{model.StudentCode}' already exists." });
        }

        var student = new StudentEntity(
            id: Guid.NewGuid().ToString("N")[..8].ToUpperInvariant(),
            code: model.StudentCode.ToUpperInvariant(),
            name: model.Name,
            email: model.Email,
            dept: model.Department,
            program: model.Program,
            sem: model.Semester
        )
        {
            Section = model.Section,
            MentorName = model.MentorName,
            AttendanceRate = model.AttendanceRate,
            AssessmentScore = model.AssessmentScore,
            SubmittedAssignments = model.SubmittedAssignments,
            TotalAssignments = model.TotalAssignments,
            LmsActivityHours = model.LmsActivityHours
        };

        var (score, level, _) = _riskCalculator.Calculate(student);
        student.ComputedRiskScore = score;
        student.Risk = level;

        _repository.AddStudent(student);

        // 201 Created with Location header pointing to GET /api/students/{id}
        return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
    }

    // PUT: /api/students/{id}
    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult Update(string id, [FromBody] StudentViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existing = _repository.GetStudentById(id);
        if (existing == null)
        {
            return NotFound(new { message = $"Student with ID '{id}' was not found." });
        }

        existing.Name = model.Name;
        existing.Email = model.Email;
        existing.Department = model.Department;
        existing.Program = model.Program;
        existing.Semester = model.Semester;
        existing.Section = model.Section;
        existing.MentorName = model.MentorName;
        existing.AttendanceRate = model.AttendanceRate;
        existing.AssessmentScore = model.AssessmentScore;
        existing.SubmittedAssignments = model.SubmittedAssignments;
        existing.TotalAssignments = model.TotalAssignments;
        existing.LmsActivityHours = model.LmsActivityHours;

        var (score, level, _) = _riskCalculator.Calculate(existing);
        existing.ComputedRiskScore = score;
        existing.Risk = level;

        _repository.UpdateStudent(existing);

        return NoContent(); // 204 No Content for successful PUT
    }

    // DELETE: /api/students/{id}
    [HttpDelete("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public IActionResult Delete(string id)
    {
        bool success = _repository.DeleteStudent(id);
        if (!success)
        {
            return NotFound(new { message = $"Student with ID '{id}' was not found." });
        }
        return Ok(new { message = $"Student with ID '{id}' successfully deleted.", timestamp = DateTime.UtcNow });
    }
}
