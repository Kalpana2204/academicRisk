using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AcademicRiskRadar.Web.Models;
using AcademicRiskRadar.Web.Models.ViewModels;
using AcademicRiskRadar.Web.Services;

namespace AcademicRiskRadar.Web.Controllers;

/// <summary>
/// Unit III: Backend Development Using ASP.NET Core MVC
/// Unit IV: Authentication and Authorization
/// Demonstrates: MVC Pattern, Model Binding, Server/Client Validations,
/// ViewBag, ViewData, TempData, Sessions, QueryString, Action Results.
/// </summary>
public class StudentsController : Controller
{
    private readonly IStudentRepository _repository;
    private readonly IRiskCalculator _riskCalculator;

    public StudentsController(IStudentRepository repository, IRiskCalculator riskCalculator)
    {
        _repository = repository;
        _riskCalculator = riskCalculator;
    }

    // GET: /Students
    // Demonstrates: QueryString parameters, ViewBag, ViewData, TempData retrieval, Session
    [HttpGet]
    public IActionResult Index(
        [FromQuery] string? department,
        [FromQuery] RiskLevel? risk,
        [FromQuery] string? search,
        [FromQuery] string? sortBy)
    {
        // Role-based privacy: If authenticated as Student, restrict visibility to their own record
        if (User.Identity?.IsAuthenticated == true && User.IsInRole("Student"))
        {
            string? userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            string? studentId = User.FindFirst("StudentId")?.Value;
            string? userName = User.Identity.Name;

            StudentEntity? self = null;
            if (!string.IsNullOrEmpty(studentId))
            {
                self = _repository.GetStudentById(studentId);
            }
            if (self == null && !string.IsNullOrEmpty(userEmail))
            {
                self = _repository.GetAllStudents().FirstOrDefault(s => s.Email.Equals(userEmail, StringComparison.OrdinalIgnoreCase));
            }
            if (self == null && !string.IsNullOrEmpty(userName))
            {
                self = _repository.GetAllStudents().FirstOrDefault(s => s.Name.Equals(userName, StringComparison.OrdinalIgnoreCase));
            }

            if (self == null)
            {
                self = new StudentEntity
                {
                    Id = $"STU-{Math.Abs((userEmail ?? "SELF").GetHashCode()):X}",
                    StudentCode = "CSE-2026-101",
                    Name = userName ?? "Student",
                    Email = userEmail ?? "student@academicradar.edu",
                    Department = "CSE",
                    Program = "B.Tech Computer Science",
                    Semester = 1,
                    Section = "A",
                    AttendanceRate = 88.5,
                    AssessmentScore = 82.0,
                    SubmittedAssignments = 8,
                    TotalAssignments = 10,
                    LmsActivityHours = 28.0,
                    Risk = RiskLevel.Low,
                    ComputedRiskScore = 15.0,
                    Courses = new()
                    {
                        new() { CourseCode = "CS101", CourseName = "Foundations of Computing", AttendanceRate = 90, MarksObtained = 85, Credits = 4 },
                        new() { CourseCode = "CS102", CourseName = "Object-Oriented Programming", AttendanceRate = 87, MarksObtained = 80, Credits = 4 }
                    }
                };
                _repository.AddStudent(self);
            }

            ViewData["Title"] = "My Student Academic Record";
            ViewData["CurrentTerm"] = "Fall Semester 2026";
            ViewBag.IsStudentRole = true;
            ViewBag.TotalCount = 1;
            ViewBag.Departments = new[] { "CSE", "ECE", "MECH", "IT", "CIVIL" };
            ViewBag.LastViewedStudent = self.Name;

            return View(new List<StudentEntity> { self });
        }

        // Unit III: Data Passing Technique 1 - ViewData (Strongly-typed/Dictionary)
        ViewData["Title"] = "Student Records & Risk Monitoring";
        ViewData["CurrentTerm"] = "Fall Semester 2026";

        // Unit III: Data Passing Technique 2 - ViewBag (Dynamic Object)
        ViewBag.SelectedDepartment = department;
        ViewBag.SelectedRisk = risk;
        ViewBag.SearchTerm = search;
        ViewBag.SortBy = sortBy ?? "risk_desc";
        ViewBag.TotalCount = _repository.GetAllStudents().Count();
        ViewBag.Departments = new[] { "CSE", "ECE", "MECH", "IT", "CIVIL" };

        // Unit III: State Management - Reading from HttpContext.Session
        ViewBag.LastViewedStudent = HttpContext.Session.GetString("LastViewedStudentName") ?? "None";

        // Unit V: SQL Clauses simulation (WHERE, LIKE, ORDER BY)
        var students = _repository.QueryStudents(department, risk, search, sortBy);

        return View(students);
    }

    // GET: /Students/Details/{id}
    // Demonstrates: Route Parameter handling, Session storage
    [HttpGet]
    public IActionResult Details(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest("Student identifier must be provided.");
        }

        var student = _repository.GetStudentById(id) ?? _repository.GetStudentByCode(id);
        if (student == null)
        {
            return NotFound($"Student with ID '{id}' was not found.");
        }

        // Unit III: State Management - Storing in Session
        HttpContext.Session.SetString("LastViewedStudentId", student.Id);
        HttpContext.Session.SetString("LastViewedStudentName", student.Name);
        HttpContext.Session.SetString("LastViewedStudentCode", student.StudentCode);

        // Calculate risk with strategy
        var (score, level, reason) = _riskCalculator.Calculate(student);
        ViewBag.EvaluationReason = reason;

        // Privacy Protection: Students can only view their own scorecard
        if (User.Identity?.IsAuthenticated == true && User.IsInRole("Student"))
        {
            string? userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            string? studentId = User.FindFirst("StudentId")?.Value;
            string? userName = User.Identity.Name;

            bool isSelf = (studentId != null && student.Id.Equals(studentId, StringComparison.OrdinalIgnoreCase)) ||
                          (userEmail != null && student.Email.Equals(userEmail, StringComparison.OrdinalIgnoreCase)) ||
                          (!string.IsNullOrEmpty(userName) && student.Name.Equals(userName, StringComparison.OrdinalIgnoreCase));

            if (!isSelf)
            {
                TempData["AlertMessage"] = "Privacy Notice: You may only view your personal academic record.";
                TempData["AlertType"] = "warning";
                return RedirectToAction("Index");
            }
        }

        return View(student);
    }

    // GET: /Students/Create
    // Open for student self-registration and faculty/admin onboarding
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Create()
    {
        ViewData["Title"] = "Register New Student";
        ViewBag.Departments = new[] { "CSE", "ECE", "MECH", "IT", "CIVIL" };
        var model = new StudentViewModel();
        return View(model);
    }

    // POST: /Students/Create
    // Demonstrates: ASP.NET Core Forms, Model Binding, Server-side Validation
    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public IActionResult Create(StudentViewModel model)
    {
        ViewBag.Departments = new[] { "CSE", "ECE", "MECH", "IT", "CIVIL" };

        // Unit III: Server-side validation check
        if (!ModelState.IsValid)
        {
            // Server-side validation failed -> Re-render form with validation errors
            return View(model);
        }

        // Custom validation logic check
        if (_repository.GetStudentByCode(model.StudentCode) != null)
        {
            ModelState.AddModelError("StudentCode", "A student with this code already exists in the institutional database.");
            return View(model);
        }

        if (model.SubmittedAssignments > model.TotalAssignments)
        {
            ModelState.AddModelError("SubmittedAssignments", "Submitted assignments cannot exceed total assignments assigned.");
            return View(model);
        }

        // Map ViewModel to Domain Entity (Unit II OOP)
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
            LmsActivityHours = model.LmsActivityHours,
            AttendanceHistory = new[] { (int)model.AttendanceRate },
            AssessmentHistory = new[] { (int)model.AssessmentScore }
        };

        // Recalculate risk score
        var (score, level, _) = _riskCalculator.Calculate(student);
        student.ComputedRiskScore = score;
        student.Risk = level;

        _repository.AddStudent(student);

        // Unit III: Data Passing Technique 3 - TempData (Persists through Redirect)
        TempData["AlertMessage"] = $"Student '{student.Name}' [{student.StudentCode}] was registered successfully!";
        TempData["AlertType"] = "success";

        return RedirectToAction(nameof(Index));
    }

    // GET: /Students/Edit/{id}
    [HttpGet]
    [Authorize(Roles = "Admin,Faculty")]
    public IActionResult Edit(string id)
    {
        var student = _repository.GetStudentById(id);
        if (student == null)
        {
            return NotFound();
        }

        ViewData["Title"] = $"Edit Student: {student.Name}";
        ViewBag.Departments = new[] { "CSE", "ECE", "MECH", "IT", "CIVIL" };

        var vm = new StudentViewModel
        {
            Id = student.Id,
            StudentCode = student.StudentCode,
            Name = student.Name,
            Email = student.Email,
            Department = student.Department,
            Program = student.Program,
            Semester = student.Semester,
            Section = student.Section,
            MentorName = student.MentorName,
            AttendanceRate = student.AttendanceRate,
            AssessmentScore = student.AssessmentScore,
            SubmittedAssignments = student.SubmittedAssignments,
            TotalAssignments = student.TotalAssignments,
            LmsActivityHours = student.LmsActivityHours,
            Risk = student.Risk
        };

        return View(vm);
    }

    // POST: /Students/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Faculty")]
    public IActionResult Edit(string id, StudentViewModel model)
    {
        ViewBag.Departments = new[] { "CSE", "ECE", "MECH", "IT", "CIVIL" };

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var existing = _repository.GetStudentById(id);
        if (existing == null)
        {
            return NotFound();
        }

        // Update fields
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

        // Recalculate risk
        var (score, level, _) = _riskCalculator.Calculate(existing);
        existing.ComputedRiskScore = score;
        existing.Risk = level;

        _repository.UpdateStudent(existing);

        TempData["AlertMessage"] = $"Student '{existing.Name}' records updated successfully.";
        TempData["AlertType"] = "info";

        return RedirectToAction(nameof(Details), new { id = existing.Id });
    }

    // GET: /Students/Delete/{id}
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(string id)
    {
        var student = _repository.GetStudentById(id);
        if (student == null)
        {
            return NotFound();
        }

        return View(student);
    }

    // POST: /Students/Delete/{id}
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteConfirmed(string id)
    {
        var student = _repository.GetStudentById(id);
        string studentName = student?.Name ?? id;

        bool deleted = _repository.DeleteStudent(id);

        if (deleted)
        {
            TempData["AlertMessage"] = $"Student '{studentName}' was successfully deleted.";
            TempData["AlertType"] = "warning";
        }
        else
        {
            TempData["AlertMessage"] = $"Could not delete student '{studentName}'.";
            TempData["AlertType"] = "danger";
        }

        return RedirectToAction(nameof(Index));
    }
}
