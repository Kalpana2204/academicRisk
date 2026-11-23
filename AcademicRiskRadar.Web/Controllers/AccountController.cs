using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using AcademicRiskRadar.Web.Models;
using AcademicRiskRadar.Web.Models.ViewModels;
using AcademicRiskRadar.Web.Services;

namespace AcademicRiskRadar.Web.Controllers;

public class UserAccountInfo
{
    public string Password { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = "Student"; // "Admin", "Faculty", "Student"
    public string Department { get; set; } = "CSE";
    public string? StudentId { get; set; }
}

public class AccountController : Controller
{
    private readonly IStudentRepository _studentRepository;

    // Registered user accounts store
    private static readonly ConcurrentDictionary<string, UserAccountInfo> _users = new(StringComparer.OrdinalIgnoreCase)
    {
        ["admin@academicradar.edu"] = new UserAccountInfo
        {
            Password = "Admin@123",
            Name = "Dean Robert Vance",
            Role = "Admin",
            Department = "Administration"
        },
        ["faculty@academicradar.edu"] = new UserAccountInfo
        {
            Password = "Faculty@123",
            Name = "Dr. Rajesh Kumar",
            Role = "Faculty",
            Department = "CSE"
        },
        ["student@academicradar.edu"] = new UserAccountInfo
        {
            Password = "Student@123",
            Name = "Aarav Sharma",
            Role = "Student",
            Department = "CSE",
            StudentId = "STU-001"
        }
    };

    public AccountController(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        string emailLower = model.Email.Trim().ToLowerInvariant();

        if (_users.TryGetValue(emailLower, out var userData) && userData.Password == model.Password)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, emailLower),
                new Claim(ClaimTypes.Name, userData.Name),
                new Claim(ClaimTypes.Email, emailLower),
                new Claim(ClaimTypes.Role, userData.Role),
                new Claim("Department", userData.Department)
            };

            if (!string.IsNullOrEmpty(userData.StudentId))
            {
                claims.Add(new Claim("StudentId", userData.StudentId));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );

            TempData["AlertMessage"] = $"Welcome back, {userData.Name}! Signed in as {userData.Role}.";
            TempData["AlertType"] = "success";

            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, "Invalid email address or password. If you don't have an account, please register.");
        return View(model);
    }

    [HttpGet]
    public IActionResult Register(string? role = null, string? returnUrl = null)
    {
        string selectedRole = role?.Trim().ToLowerInvariant() switch
        {
            "admin" => "Admin",
            "faculty" => "Faculty",
            _ => "Student"
        };

        var model = new RegisterViewModel
        {
            Role = selectedRole,
            ReturnUrl = returnUrl
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        string emailLower = model.Email.Trim().ToLowerInvariant();

        if (_users.ContainsKey(emailLower))
        {
            ModelState.AddModelError("Email", "An account with this email address already exists. Please sign in or use another email.");
            return View(model);
        }

        string normalizedRole = model.Role.Trim().ToLowerInvariant() switch
        {
            "admin" => "Admin",
            "faculty" => "Faculty",
            _ => "Student"
        };

        string? createdStudentId = null;

        // Role-specific entity provisioning
        if (normalizedRole == "Student")
        {
            string studentCode = string.IsNullOrWhiteSpace(model.StudentCode)
                ? $"{model.Department.ToUpper()}-{DateTime.Now.Year}-{new Random().Next(100, 999)}"
                : model.StudentCode.Trim().ToUpperInvariant();

            createdStudentId = $"STU-{Guid.NewGuid().ToString()[..6].ToUpper()}";

            var newStudent = new StudentEntity
            {
                Id = createdStudentId,
                StudentCode = studentCode,
                Name = model.FullName.Trim(),
                Email = emailLower,
                Department = string.IsNullOrWhiteSpace(model.Department) ? "CSE" : model.Department.Trim().ToUpperInvariant(),
                Program = string.IsNullOrWhiteSpace(model.Program) ? "B.Tech Computer Science" : model.Program.Trim(),
                Semester = model.Semester >= 1 && model.Semester <= 8 ? model.Semester : 1,
                Section = "A",
                AttendanceRate = 88.5,
                AssessmentScore = 82.0,
                SubmittedAssignments = 8,
                TotalAssignments = 10,
                LmsActivityHours = 28.0,
                Risk = RiskLevel.Low,
                ComputedRiskScore = 15.0,
                AttendanceHistory = new[] { 85, 86, 88, 90, 88 },
                AssessmentHistory = new[] { 78, 80, 82, 85, 82 },
                Courses = new()
                {
                    new() { CourseCode = $"{model.Department.ToUpper()}101", CourseName = "Foundations of Computing", AttendanceRate = 90, MarksObtained = 85, Credits = 4 },
                    new() { CourseCode = $"{model.Department.ToUpper()}102", CourseName = "Object-Oriented Programming", AttendanceRate = 87, MarksObtained = 80, Credits = 4 }
                }
            };

            _studentRepository.AddStudent(newStudent);
        }
        else if (normalizedRole == "Faculty")
        {
            var newMentor = new FacultyMentor(
                id: $"MNT-{Guid.NewGuid().ToString()[..6].ToUpper()}",
                name: model.FullName.Trim(),
                email: emailLower,
                department: string.IsNullOrWhiteSpace(model.Department) ? "CSE" : model.Department.Trim().ToUpperInvariant(),
                designation: string.IsNullOrWhiteSpace(model.Designation) ? "Assistant Professor" : model.Designation.Trim()
            )
            {
                OfficeRoom = string.IsNullOrWhiteSpace(model.OfficeRoom) ? $"{model.Department}-201" : model.OfficeRoom.Trim()
            };

            _studentRepository.AddMentor(newMentor);
        }

        // Store user in credentials registry
        var newUser = new UserAccountInfo
        {
            Password = model.Password,
            Name = model.FullName.Trim(),
            Role = normalizedRole,
            Department = model.Department,
            StudentId = createdStudentId
        };
        _users[emailLower] = newUser;

        // Auto sign-in new user
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, emailLower),
            new Claim(ClaimTypes.Name, newUser.Name),
            new Claim(ClaimTypes.Email, emailLower),
            new Claim(ClaimTypes.Role, newUser.Role),
            new Claim("Department", newUser.Department)
        };

        if (!string.IsNullOrEmpty(newUser.StudentId))
        {
            claims.Add(new Claim("StudentId", newUser.StudentId));
        }

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties
        );

        TempData["AlertMessage"] = $"Registration successful! Welcome to Academic Risk Radar, {newUser.Name} ({newUser.Role}).";
        TempData["AlertType"] = "success";

        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
        {
            return Redirect(model.ReturnUrl);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        TempData["AlertMessage"] = "You have been logged out successfully.";
        TempData["AlertType"] = "info";
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        ViewData["Title"] = "Access Denied";
        return View();
    }

    // Quick demo login helper
    [HttpGet]
    public async Task<IActionResult> QuickLogin(string role, string? returnUrl = null)
    {
        string email = role?.ToLowerInvariant() switch
        {
            "admin" => "admin@academicradar.edu",
            "faculty" => "faculty@academicradar.edu",
            "student" => "student@academicradar.edu",
            _ => "admin@academicradar.edu"
        };

        if (_users.TryGetValue(email, out var userData))
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, email),
                new Claim(ClaimTypes.Name, userData.Name),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, userData.Role),
                new Claim("Department", userData.Department)
            };

            if (!string.IsNullOrEmpty(userData.StudentId))
            {
                claims.Add(new Claim("StudentId", userData.StudentId));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            TempData["AlertMessage"] = $"Switched role to: {userData.Role} ({userData.Name})";
            TempData["AlertType"] = "success";
        }

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index", "Home");
    }
}
