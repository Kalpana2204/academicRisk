using System.ComponentModel.DataAnnotations;

namespace AcademicRiskRadar.Web.Models.ViewModels;

/// <summary>
/// Unit IV: Authentication and Authorization in Web Application
/// </summary>
public class LoginViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Remember Me")]
    public bool RememberMe { get; set; } = false;

    public string? ReturnUrl { get; set; }
}

/// <summary>
/// Unit VI: ASP.NET Web API Request Model & Validation
/// </summary>
public class RiskCalculationRequestViewModel
{
    [Range(0, 100, ErrorMessage = "Attendance must be between 0 and 100%")]
    public double AttendanceRate { get; set; }

    [Range(0, 100, ErrorMessage = "Assessment must be between 0 and 100%")]
    public double AssessmentScore { get; set; }

    [Range(0, 100, ErrorMessage = "Assignment completion must be between 0 and 100%")]
    public double AssignmentCompletionPercentage { get; set; }

    [Range(0, 500, ErrorMessage = "LMS hours must be between 0 and 500")]
    public double LmsActivityHours { get; set; }

    // Optional customized factor weights
    public double AttendanceWeight { get; set; } = 35;
    public double AssessmentWeight { get; set; } = 30;
    public double AssignmentWeight { get; set; } = 20;
    public double LmsWeight { get; set; } = 15;
}

public class RiskCalculationResponseViewModel
{
    public double RiskScore { get; set; }
    public string RiskLevel { get; set; } = "Low";
    public string PrimaryFactor { get; set; } = string.Empty;
    public string Recommendation { get; set; } = string.Empty;
    public DateTime EvaluatedAt { get; set; } = DateTime.UtcNow;
}
