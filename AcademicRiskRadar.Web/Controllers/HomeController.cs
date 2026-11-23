using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using AcademicRiskRadar.Web.Models;
using AcademicRiskRadar.Web.Services;

namespace AcademicRiskRadar.Web.Controllers;

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
        ViewData["Title"] = "Academic Risk Radar | Home";
        ViewBag.SystemVersion = "ASP.NET Core 10 (LTS Architecture)";
        ViewBag.TotalStudents = _repository.GetAllStudents().Count();
        ViewBag.HighRiskCount = _repository.GetHighRiskStudentsView().Count();
        ViewBag.TotalInterventions = _repository.GetAllInterventions().Count();

        // Role-based personalization: If authenticated as Student, load student's individual profile
        if (User.Identity?.IsAuthenticated == true && User.IsInRole("Student"))
        {
            string? userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            string? studentId = User.FindFirst("StudentId")?.Value;
            string? userName = User.Identity.Name;

            StudentEntity? student = null;
            if (!string.IsNullOrEmpty(studentId))
            {
                student = _repository.GetStudentById(studentId);
            }
            if (student == null && !string.IsNullOrEmpty(userEmail))
            {
                student = _repository.GetAllStudents().FirstOrDefault(s => s.Email.Equals(userEmail, StringComparison.OrdinalIgnoreCase));
            }
            if (student == null && !string.IsNullOrEmpty(userName))
            {
                student = _repository.GetAllStudents().FirstOrDefault(s => s.Name.Equals(userName, StringComparison.OrdinalIgnoreCase));
            }

            // Fallback: if student entity wasn't in repository, provision profile immediately
            if (student == null)
            {
                student = new StudentEntity
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
                _repository.AddStudent(student);
            }

            ViewBag.IsStudent = true;
            ViewBag.CurrentStudent = student;
        }

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
