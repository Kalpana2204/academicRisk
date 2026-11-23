
using System;
using System.Collections.Generic;
using System.Linq;

namespace AcademicRiskRadar.Web.Services;

// Shared student data model
public class Student
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Department { get; set; } = "";
    public string Program { get; set; } = "";

    public int Semester { get; set; }
    public string Section { get; set; } = "";
    public string Mentor { get; set; } = "";

    public int Attendance { get; set; }
    public int Assessment { get; set; }

    public int SubmittedAssignments { get; set; }
    public int TotalAssignments { get; set; }

    public int LmsActivity { get; set; }

    // Temporary demo classification.
    // Later this will come from the shared scoring engine.
    public string Risk { get; set; } = "Low";

    public List<CoursePerformance> Courses { get; set; } = new();

    public List<int> AttendanceHistory { get; set; } = new();
    public List<int> AssessmentHistory { get; set; } = new();

    public double AssignmentCompletion =>
        TotalAssignments > 0
            ? 100.0 * SubmittedAssignments / TotalAssignments
            : 0;
}

public class CoursePerformance
{
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public int Attendance { get; set; }
    public int Marks { get; set; }
}

public class AcademicDataService
{
    private readonly List<Student> _students = new()
    {
        new Student
        {
            Id = "STU001",
            Name = "Aarav Sharma",
            Department = "CSE",
            Program = "B.Tech Computer Science",
            Semester = 5,
            Section = "A",
            Mentor = "Dr. Rajesh Kumar",
            Attendance = 61,
            Assessment = 54,
            SubmittedAssignments = 5,
            TotalAssignments = 9,
            LmsActivity = 42,
            Risk = "High",

            AttendanceHistory = new() { 82, 78, 74, 69, 65, 61 },
            AssessmentHistory = new() { 76, 72, 67, 62, 58, 54 },

            Courses = new()
            {
                new()
                {
                    Code = "CSE301",
                    Name = "Database Management Systems",
                    Attendance = 58,
                    Marks = 51
                },
                new()
                {
                    Code = "CSE302",
                    Name = "Operating Systems",
                    Attendance = 63,
                    Marks = 55
                },
                new()
                {
                    Code = "CSE303",
                    Name = "Computer Networks",
                    Attendance = 60,
                    Marks = 56
                }
            }
        },

        new Student
        {
            Id = "STU002",
            Name = "Priya Verma",
            Department = "ECE",
            Program = "B.Tech Electronics and Communication",
            Semester = 5,
            Section = "B",
            Mentor = "Dr. Meera Singh",
            Attendance = 88,
            Assessment = 84,
            SubmittedAssignments = 9,
            TotalAssignments = 10,
            LmsActivity = 91,
            Risk = "Low",
            AttendanceHistory = new() { 85, 86, 87, 88, 88, 88 },
            AssessmentHistory = new() { 79, 80, 82, 82, 83, 84 }
        },

        new Student
        {
            Id = "STU003",
            Name = "Rohan Gupta",
            Department = "CSE",
            Program = "B.Tech Computer Science",
            Semester = 5,
            Section = "A",
            Mentor = "Dr. Rajesh Kumar",
            Attendance = 72,
            Assessment = 62,
            SubmittedAssignments = 7,
            TotalAssignments = 10,
            LmsActivity = 65,
            Risk = "Moderate",
            AttendanceHistory = new() { 84, 82, 80, 77, 74, 72 },
            AssessmentHistory = new() { 70, 69, 66, 65, 63, 62 }
        },

        new Student
        {
            Id = "STU004",
            Name = "Sneha Patel",
            Department = "ME",
            Program = "B.Tech Mechanical Engineering",
            Semester = 5,
            Section = "A",
            Mentor = "Dr. Amit Sharma",
            Attendance = 93,
            Assessment = 89,
            SubmittedAssignments = 10,
            TotalAssignments = 10,
            LmsActivity = 95,
            Risk = "Low",
            AttendanceHistory = new() { 89, 90, 91, 91, 92, 93 },
            AssessmentHistory = new() { 84, 85, 86, 87, 88, 89 }
        },

        new Student
        {
            Id = "STU005",
            Name = "Aditya Singh",
            Department = "CSE",
            Program = "B.Tech Computer Science",
            Semester = 5,
            Section = "B",
            Mentor = "Dr. Rajesh Kumar",
            Attendance = 49,
            Assessment = 43,
            SubmittedAssignments = 3,
            TotalAssignments = 10,
            LmsActivity = 28,
            Risk = "High",
            AttendanceHistory = new() { 75, 68, 61, 57, 53, 49 },
            AssessmentHistory = new() { 65, 60, 54, 50, 46, 43 }
        },

        new Student
        {
            Id = "STU006",
            Name = "Neha Kapoor",
            Department = "BBA",
            Program = "Bachelor of Business Administration",
            Semester = 5,
            Section = "A",
            Mentor = "Dr. Pooja Mehta",
            Attendance = 79,
            Assessment = 69,
            SubmittedAssignments = 8,
            TotalAssignments = 10,
            LmsActivity = 70,
            Risk = "Moderate",
            AttendanceHistory = new() { 82, 81, 80, 80, 79, 79 },
            AssessmentHistory = new() { 79, 77, 75, 73, 71, 69 }
        },

        new Student
        {
            Id = "STU007",
            Name = "Karan Mehta",
            Department = "ECE",
            Program = "B.Tech Electronics and Communication",
            Semester = 5,
            Section = "A",
            Mentor = "Dr. Meera Singh",
            Attendance = 91,
            Assessment = 86,
            SubmittedAssignments = 10,
            TotalAssignments = 10,
            LmsActivity = 89,
            Risk = "Low",
            AttendanceHistory = new() { 87, 88, 89, 89, 90, 91 },
            AssessmentHistory = new() { 82, 83, 84, 85, 85, 86 }
        },

        new Student
        {
            Id = "STU008",
            Name = "Ananya Roy",
            Department = "CSE",
            Program = "B.Tech Computer Science",
            Semester = 5,
            Section = "B",
            Mentor = "Dr. Rajesh Kumar",
            Attendance = 57,
            Assessment = 51,
            SubmittedAssignments = 4,
            TotalAssignments = 9,
            LmsActivity = 38,
            Risk = "High",
            AttendanceHistory = new() { 79, 74, 69, 65, 60, 57 },
            AssessmentHistory = new() { 72, 68, 63, 59, 55, 51 }
        },

        new Student
        {
            Id = "STU009",
            Name = "Vivek Kumar",
            Department = "ME",
            Program = "B.Tech Mechanical Engineering",
            Semester = 5,
            Section = "B",
            Mentor = "Dr. Amit Sharma",
            Attendance = 83,
            Assessment = 77,
            SubmittedAssignments = 9,
            TotalAssignments = 10,
            LmsActivity = 83,
            Risk = "Low",
            AttendanceHistory = new() { 80, 81, 82, 82, 83, 83 },
            AssessmentHistory = new() { 73, 74, 75, 76, 76, 77 }
        },

        new Student
        {
            Id = "STU010",
            Name = "Ishita Das",
            Department = "CSE",
            Program = "B.Tech Computer Science",
            Semester = 5,
            Section = "A",
            Mentor = "Dr. Rajesh Kumar",
            Attendance = 76,
            Assessment = 65,
            SubmittedAssignments = 7,
            TotalAssignments = 10,
            LmsActivity = 62,
            Risk = "Moderate",
            AttendanceHistory = new() { 83, 81, 79, 78, 77, 76 },
            AssessmentHistory = new() { 74, 72, 70, 68, 67, 65 }
        }
    };

    // Return a read-only collection to prevent callers
    // from directly adding or removing students.
    public IReadOnlyList<Student> GetAllStudents()
    {
        return _students.AsReadOnly();
    }

    public Student? GetStudentById(string id)
    {
        return _students.FirstOrDefault(
            student => student.Id.Equals(
                id,
                StringComparison.OrdinalIgnoreCase
            )
        );
    }

    public IReadOnlyList<Student> GetStudentsByDepartment(
        string department)
    {
        return _students
            .Where(student =>
                student.Department.Equals(
                    department,
                    StringComparison.OrdinalIgnoreCase
                )
            )
            .ToList()
            .AsReadOnly();
    }

    public IReadOnlyList<Student> SearchStudents(string search)
    {
        if (string.IsNullOrWhiteSpace(search))
        {
            return GetAllStudents();
        }

        return _students
            .Where(student =>
                student.Name.Contains(
                    search,
                    StringComparison.OrdinalIgnoreCase
                ) ||
                student.Id.Contains(
                    search,
                    StringComparison.OrdinalIgnoreCase
                ) ||
                student.Department.Contains(
                    search,
                    StringComparison.OrdinalIgnoreCase
                )
            )
            .ToList()
            .AsReadOnly();
    }

    public int GetTotalStudents()
    {
        return _students.Count;
    }

    public int GetHighRiskCount()
    {
        return _students.Count(
            student => student.Risk == "High"
        );
    }

    public int GetModerateRiskCount()
    {
        return _students.Count(
            student => student.Risk == "Moderate"
        );
    }

    public int GetLowRiskCount()
    {
        return _students.Count(
            student => student.Risk == "Low"
        );
    }

    public double GetAverageAttendance()
    {
        return _students.Count == 0
            ? 0
            : _students.Average(student => student.Attendance);
    }

    public double GetAverageAssessment()
    {
        return _students.Count == 0
            ? 0
            : _students.Average(student => student.Assessment);
    }
}
