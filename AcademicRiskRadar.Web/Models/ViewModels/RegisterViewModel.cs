using System.ComponentModel.DataAnnotations;

namespace AcademicRiskRadar.Web.Models.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Full Name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Institutional email address is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please confirm your password")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please select an account role")]
    [Display(Name = "Account Role")]
    public string Role { get; set; } = "Student"; // "Student", "Faculty", or "Admin"

    // Role-specific fields
    [Display(Name = "Academic Department")]
    public string Department { get; set; } = "CSE";

    [Display(Name = "Student Roll / ID Number")]
    public string? StudentCode { get; set; }

    [Display(Name = "Semester")]
    [Range(1, 8, ErrorMessage = "Semester must be between 1 and 8")]
    public int Semester { get; set; } = 1;

    [Display(Name = "Degree Program")]
    public string Program { get; set; } = "B.Tech Computer Science";

    [Display(Name = "Academic Designation")]
    public string Designation { get; set; } = "Assistant Professor";

    [Display(Name = "Office / Cabin Room")]
    public string? OfficeRoom { get; set; }

    public string? ReturnUrl { get; set; }
}
