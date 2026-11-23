using System.Collections.Generic;
using AcademicRiskRadar.Web.Models;

namespace AcademicRiskRadar.Web.Services;

/// <summary>
/// Unit IV: Repository Pattern with Dependency Injection
/// Unit V: SQL Server Operations Abstraction (CRUD, Joins, Views, Stored Procedures)
/// </summary>
public interface IStudentRepository
{
    // CRUD
    IEnumerable<StudentEntity> GetAllStudents();
    StudentEntity? GetStudentById(string id);
    StudentEntity? GetStudentByCode(string code);
    void AddStudent(StudentEntity student);
    void UpdateStudent(StudentEntity student);
    bool DeleteStudent(string id);

    // Filter & Search (Unit III QueryString & Unit V SQL Clauses: WHERE, ORDER BY, LIKE)
    IEnumerable<StudentEntity> QueryStudents(string? department, RiskLevel? riskLevel, string? searchTerm, string? sortBy);

    // Unit V: SQL Clauses - GROUP BY and HAVING demonstration
    IEnumerable<DepartmentRiskSummaryDto> GetDepartmentRiskSummary();

    // Unit V: SQL Joins demonstration (Inner, Left, Right, Full)
    IEnumerable<StudentCourseJoinDto> ExecuteInnerJoin();
    IEnumerable<StudentInterventionJoinDto> ExecuteLeftJoin();
    IEnumerable<MentorStudentJoinDto> ExecuteRightJoin();
    IEnumerable<FullJoinDto> ExecuteFullJoin();

    // Unit V: SQL Server Views demonstration
    IEnumerable<HighRiskStudentViewDto> GetHighRiskStudentsView();
    IEnumerable<AttendancePerformanceViewDto> GetAttendancePerformanceView();

    // Unit V: Stored Procedures demonstration
    IEnumerable<StudentEntity> ExecuteSpGetStudentsAtRisk(RiskLevel minRisk, double maxAttendance);
    SpEnrollmentResultDto ExecuteSpEnrollStudentAndRecalculateRisk(string studentId, string courseCode, string courseName, int credits);

    // Mentors
    IEnumerable<FacultyMentor> GetAllMentors();
    void AddMentor(FacultyMentor mentor);

    // Interventions
    IEnumerable<MentoringInterventionEntity> GetAllInterventions();
    void AddIntervention(MentoringInterventionEntity intervention);

    // Audit Logs (Unit IV Custom Middleware)
    IEnumerable<AuditLogEntry> GetRecentAuditLogs(int count = 20);
    void AddAuditLog(AuditLogEntry log);
}

// Data Transfer Objects for Unit V Joins & Views
public record DepartmentRiskSummaryDto(string Department, int TotalStudents, int HighRiskCount, double AvgAttendance, double AvgAssessment);
public record StudentCourseJoinDto(string StudentCode, string StudentName, string CourseCode, string CourseName, double Attendance, double Marks);
public record StudentInterventionJoinDto(string StudentCode, string StudentName, string? ActionType, string? Status, string? MentorName);
public record MentorStudentJoinDto(string MentorName, string Department, string? AssignedStudentCode, string? AssignedStudentName);
public record FullJoinDto(string? StudentName, string? Department, string? ActionType, string? Status);
public record HighRiskStudentViewDto(string StudentCode, string Name, string Department, int Semester, double RiskScore, string RiskLevel, string Mentor);
public record AttendancePerformanceViewDto(string StudentCode, string Name, double AttendanceRate, double AssessmentScore, string StatusGrade);
public record SpEnrollmentResultDto(bool Success, string Message, string StudentCode, double OldRiskScore, double NewRiskScore, int TotalCourses);
