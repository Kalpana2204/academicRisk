using System.ComponentModel.DataAnnotations;
using AcademicRiskRadar.Web.Models;

namespace AcademicRiskRadar.Web.Models.ViewModels;

/// <summary>
/// Unit III: ASP.NET Core Forms, Model Binding, Server-side & Client-side Validation.
/// Contains comprehensive Data Annotations.
/// </summary>
public class StudentViewModel
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "Student ID/Code is required")]
    [StringLength(12, MinimumLength = 3, ErrorMessage = "Student Code must be between 3 and 12 characters")]
    [RegularExpression(@"^[A-Z0-9\-]+$", ErrorMessage = "Code must contain uppercase letters, numbers, and hyphens only (e.g. STU-101)")]
    [Display(Name = "Student Code")]
    public string StudentCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Full Name is required")]
    [StringLength(80, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 80 characters")]
    [Display(Name = "Student Full Name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email Address is required")]
    [EmailAddress(ErrorMessage = "Please provide a valid institutional email address")]
    [Display(Name = "Institutional Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Department is required")]
    [Display(Name = "Academic Department")]
    public string Department { get; set; } = "CSE";

    [Required(ErrorMessage = "Program is required")]
    [Display(Name = "Degree Program")]
    public string Program { get; set; } = "B.Tech Computer Science";

    [Required(ErrorMessage = "Semester is required")]
    [Range(1, 8, ErrorMessage = "Semester must be between 1 and 8")]
    [Display(Name = "Current Semester")]
    public int Semester { get; set; } = 1;

    [Required(ErrorMessage = "Section is required")]
    [StringLength(2, ErrorMessage = "Section code cannot exceed 2 characters")]
    [Display(Name = "Class Section")]
    public string Section { get; set; } = "A";

    [Display(Name = "Faculty Mentor")]
    public string MentorName { get; set; } = "Dr. Rajesh Kumar";

    [Required(ErrorMessage = "Attendance rate is required")]
    [Range(0, 100, ErrorMessage = "Attendance must be between 0% and 100%")]
    [Display(Name = "Current Attendance (%)")]
    public double AttendanceRate { get; set; } = 75.0;

    [Required(ErrorMessage = "Assessment score is required")]
    [Range(0, 100, ErrorMessage = "Assessment score must be between 0 and 100 marks")]
    [Display(Name = "Internal Assessment Score (%)")]
    public double AssessmentScore { get; set; } = 70.0;

    [Required(ErrorMessage = "Submitted assignments count is required")]
    [Range(0, 50, ErrorMessage = "Assignments submitted must be between 0 and 50")]
    [Display(Name = "Submitted Assignments")]
    public int SubmittedAssignments { get; set; } = 8;

    [Required(ErrorMessage = "Total assignments count is required")]
    [Range(1, 50, ErrorMessage = "Total assignments must be between 1 and 50")]
    [Display(Name = "Total Assignments Due")]
    public int TotalAssignments { get; set; } = 10;

    [Range(0, 500, ErrorMessage = "LMS hours must be non-negative")]
    [Display(Name = "LMS Activity (Hours)")]
    public double LmsActivityHours { get; set; } = 20.0;

    [Display(Name = "Assigned Risk Level")]
    public RiskLevel Risk { get; set; } = RiskLevel.Low;
}
