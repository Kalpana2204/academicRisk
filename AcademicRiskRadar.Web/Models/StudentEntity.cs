using System;
using System.Collections.Generic;
using System.Linq;

namespace AcademicRiskRadar.Web.Models;

/// <summary>
/// Unit II: OOP Concepts - Inheritance, Polymorphism, Encapsulation, Properties, Constructors.
/// StudentEntity extends Person.
/// </summary>
public class StudentEntity : Person
{
    public string StudentCode { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Program { get; set; } = string.Empty;
    public int Semester { get; set; } = 1;
    public string Section { get; set; } = "A";

    public string MentorId { get; set; } = string.Empty;
    public string MentorName { get; set; } = string.Empty;

    // Academic performance metrics
    public double AttendanceRate { get; set; } = 85.0; // 0 to 100
    public double AssessmentScore { get; set; } = 75.0; // 0 to 100
    public int SubmittedAssignments { get; set; } = 8;
    public int TotalAssignments { get; set; } = 10;
    public double LmsActivityHours { get; set; } = 25.0;

    public RiskLevel Risk { get; set; } = RiskLevel.Low;
    public double ComputedRiskScore { get; set; } = 15.0;

    // Unit II: Arrays and Collections
    public int[] AttendanceHistory { get; set; } = Array.Empty<int>();
    public int[] AssessmentHistory { get; set; } = Array.Empty<int>();

    // Navigation collections (Unit IV Code First & Relationships)
    public List<StudentCoursePerformance> Courses { get; set; } = new();
    public List<MentoringInterventionEntity> Interventions { get; set; } = new();

    // Unit II: Computed Read-only Property
    public double AssignmentCompletionPercentage =>
        TotalAssignments > 0 ? Math.Round((double)SubmittedAssignments / TotalAssignments * 100.0, 1) : 0.0;

    // Constructors
    public StudentEntity() : base()
    {
    }

    // Unit II: Parameterized constructor chaining with base(...)
    public StudentEntity(string id, string code, string name, string email, string dept, string program, int sem)
        : base(id, name, email)
    {
        StudentCode = code;
        Department = dept;
        Program = program;
        Semester = sem;
    }

    // Unit II: Polymorphic method implementation
    public override string GetRoleDescription() => $"Student ({Program}, Sem {Semester})";

    public override string GetSummary()
    {
        return $"[{StudentCode}] {Name} | Dept: {Department} | Risk: {Risk} ({ComputedRiskScore:F1}%)";
    }

    // Unit II: Method Overloading (Compile-time Polymorphism)
    public double CalculateSimpleRiskScore()
    {
        // Simple equal-weight risk formula
        double attendanceRisk = Math.Max(0, 100.0 - AttendanceRate);
        double assessmentRisk = Math.Max(0, 100.0 - AssessmentScore);
        double assignmentRisk = Math.Max(0, 100.0 - AssignmentCompletionPercentage);
        return Math.Round((attendanceRisk + assessmentRisk + assignmentRisk) / 3.0, 1);
    }

    public double CalculateSimpleRiskScore(double attendanceWeight, double assessmentWeight, double assignmentWeight)
    {
        double totalWeight = attendanceWeight + assessmentWeight + assignmentWeight;
        if (totalWeight <= 0) totalWeight = 1.0;

        double attendanceRisk = Math.Max(0, 100.0 - AttendanceRate);
        double assessmentRisk = Math.Max(0, 100.0 - AssessmentScore);
        double assignmentRisk = Math.Max(0, 100.0 - AssignmentCompletionPercentage);

        double score = (attendanceRisk * attendanceWeight +
                        assessmentRisk * assessmentWeight +
                        assignmentRisk * assignmentWeight) / totalWeight;

        return Math.Round(score, 1);
    }
}

public class StudentCoursePerformance
{
    public string CourseCode { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;
    public int Credits { get; set; } = 3;
    public double AttendanceRate { get; set; } = 80.0;
    public double MarksObtained { get; set; } = 70.0;
}
