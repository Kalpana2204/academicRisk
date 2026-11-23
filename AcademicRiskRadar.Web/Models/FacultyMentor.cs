using System;

namespace AcademicRiskRadar.Web.Models;

/// <summary>
/// Unit II: Inheritance & Polymorphism
/// FacultyMentor inherits from Person and overrides abstract and virtual methods.
/// </summary>
public class FacultyMentor : Person
{
    public string Department { get; set; } = string.Empty;
    public string Designation { get; set; } = "Assistant Professor";
    public string OfficeRoom { get; set; } = string.Empty;

    // Unit II: Constructor chaining with base keyword
    public FacultyMentor() : base()
    {
    }

    public FacultyMentor(string id, string name, string email, string department, string designation)
        : base(id, name, email)
    {
        Department = department;
        Designation = designation;
    }

    // Unit II: Implementing abstract method (Polymorphism)
    public override string GetRoleDescription() => $"Faculty Mentor - {Designation}";

    public override string GetSummary()
    {
        return $"{base.GetSummary()} | Dept: {Department} | Office: {OfficeRoom}";
    }
}
