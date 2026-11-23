using System;
using System.Collections.Generic;
using System.Linq;
using AcademicRiskRadar.Web.Models;

namespace AcademicRiskRadar.Web.Services;

/// <summary>
/// Unit IV & V: Repository Pattern implementing SQL Server simulation and CRUD operations.
/// Registered as Scoped Service in Dependency Injection container.
/// </summary>
public class StudentRepository : IStudentRepository
{
    private static readonly List<StudentEntity> _students = new();
    private static readonly List<FacultyMentor> _mentors = new();
    private static readonly List<MentoringInterventionEntity> _interventions = new();
    private static readonly List<AuditLogEntry> _auditLogs = new();
    private static readonly object _lock = new();

    static StudentRepository()
    {
        SeedInitialData();
    }

    private static void SeedInitialData()
    {
        lock (_lock)
        {
            if (_students.Any()) return;

            // Seed Mentors
            _mentors.AddRange(new[]
            {
                new FacultyMentor("MNT-01", "Dr. Rajesh Kumar", "rajesh.kumar@academicradar.edu", "CSE", "Professor & HOD") { OfficeRoom = "CS-301" },
                new FacultyMentor("MNT-02", "Prof. Ananya Iyer", "ananya.iyer@academicradar.edu", "ECE", "Associate Professor") { OfficeRoom = "EC-204" },
                new FacultyMentor("MNT-03", "Dr. Vikram Singh", "vikram.singh@academicradar.edu", "MECH", "Assistant Professor") { OfficeRoom = "ME-108" },
                new FacultyMentor("MNT-04", "Dr. Sneha Patil", "sneha.patil@academicradar.edu", "IT", "Assistant Professor") { OfficeRoom = "IT-402" },
                new FacultyMentor("MNT-05", "Dr. Aris Thorne", "aris.thorne@academicradar.edu", "CSE", "Adjunct Fellow") { OfficeRoom = "CS-310" } // Unassigned mentor for Right Join demo
            });

            // Seed Students
            var s1 = new StudentEntity("STU-001", "CSE-2024-001", "Aarav Sharma", "aarav.sharma@academicradar.edu", "CSE", "B.Tech Computer Science", 5)
            {
                Section = "A",
                MentorId = "MNT-01",
                MentorName = "Dr. Rajesh Kumar",
                AttendanceRate = 58.5,
                AssessmentScore = 52.0,
                SubmittedAssignments = 4,
                TotalAssignments = 10,
                LmsActivityHours = 12.0,
                Risk = RiskLevel.High,
                ComputedRiskScore = 67.5,
                AttendanceHistory = new[] { 85, 78, 70, 64, 58 },
                AssessmentHistory = new[] { 78, 70, 62, 55, 52 },
                Courses = new()
                {
                    new() { CourseCode = "CS501", CourseName = "Database Management Systems", AttendanceRate = 56, MarksObtained = 48, Credits = 4 },
                    new() { CourseCode = "CS502", CourseName = "Operating Systems", AttendanceRate = 60, MarksObtained = 54, Credits = 4 },
                    new() { CourseCode = "CS503", CourseName = "Computer Networks", AttendanceRate = 59, MarksObtained = 55, Credits = 3 }
                }
            };

            var s2 = new StudentEntity("STU-002", "CSE-2024-002", "Priya Verma", "priya.verma@academicradar.edu", "CSE", "B.Tech Computer Science", 5)
            {
                Section = "A",
                MentorId = "MNT-01",
                MentorName = "Dr. Rajesh Kumar",
                AttendanceRate = 92.0,
                AssessmentScore = 88.5,
                SubmittedAssignments = 10,
                TotalAssignments = 10,
                LmsActivityHours = 45.0,
                Risk = RiskLevel.Low,
                ComputedRiskScore = 11.2,
                AttendanceHistory = new[] { 90, 91, 93, 92, 92 },
                AssessmentHistory = new[] { 85, 87, 88, 90, 88 },
                Courses = new()
                {
                    new() { CourseCode = "CS501", CourseName = "Database Management Systems", AttendanceRate = 94, MarksObtained = 90, Credits = 4 },
                    new() { CourseCode = "CS502", CourseName = "Operating Systems", AttendanceRate = 91, MarksObtained = 86, Credits = 4 }
                }
            };

            var s3 = new StudentEntity("STU-003", "ECE-2024-015", "Rohan Mehta", "rohan.mehta@academicradar.edu", "ECE", "B.Tech Electronics", 3)
            {
                Section = "B",
                MentorId = "MNT-02",
                MentorName = "Prof. Ananya Iyer",
                AttendanceRate = 48.0,
                AssessmentScore = 42.0,
                SubmittedAssignments = 3,
                TotalAssignments = 8,
                LmsActivityHours = 8.5,
                Risk = RiskLevel.Critical,
                ComputedRiskScore = 79.4,
                AttendanceHistory = new[] { 72, 65, 58, 52, 48 },
                AssessmentHistory = new[] { 68, 55, 49, 44, 42 },
                Courses = new()
                {
                    new() { CourseCode = "EC301", CourseName = "Signals and Systems", AttendanceRate = 45, MarksObtained = 40, Credits = 4 },
                    new() { CourseCode = "EC302", CourseName = "Digital Electronics", AttendanceRate = 51, MarksObtained = 44, Credits = 4 }
                }
            };

            var s4 = new StudentEntity("STU-004", "IT-2024-009", "Neha Kulkarni", "neha.kulkarni@academicradar.edu", "IT", "B.Tech Information Technology", 4)
            {
                Section = "A",
                MentorId = "MNT-04",
                MentorName = "Dr. Sneha Patil",
                AttendanceRate = 72.5,
                AssessmentScore = 68.0,
                SubmittedAssignments = 7,
                TotalAssignments = 10,
                LmsActivityHours = 22.0,
                Risk = RiskLevel.Moderate,
                ComputedRiskScore = 38.6,
                AttendanceHistory = new[] { 78, 75, 74, 71, 72 },
                AssessmentHistory = new[] { 70, 69, 68, 67, 68 },
                Courses = new()
                {
                    new() { CourseCode = "IT401", CourseName = "Web Technologies", AttendanceRate = 75, MarksObtained = 72, Credits = 3 },
                    new() { CourseCode = "IT402", CourseName = "Software Engineering", AttendanceRate = 70, MarksObtained = 64, Credits = 3 }
                }
            };

            var s5 = new StudentEntity("STU-005", "MECH-2024-022", "Karan Singhania", "karan.singhania@academicradar.edu", "MECH", "B.Tech Mechanical", 6)
            {
                Section = "C",
                MentorId = "MNT-03",
                MentorName = "Dr. Vikram Singh",
                AttendanceRate = 63.0,
                AssessmentScore = 58.0,
                SubmittedAssignments = 5,
                TotalAssignments = 9,
                LmsActivityHours = 14.0,
                Risk = RiskLevel.High,
                ComputedRiskScore = 56.8,
                AttendanceHistory = new[] { 79, 74, 70, 66, 63 },
                AssessmentHistory = new[] { 72, 68, 64, 60, 58 },
                Courses = new()
                {
                    new() { CourseCode = "ME601", CourseName = "Thermodynamics II", AttendanceRate = 62, MarksObtained = 55, Credits = 4 },
                    new() { CourseCode = "ME602", CourseName = "Heat Transfer", AttendanceRate = 64, MarksObtained = 61, Credits = 4 }
                }
            };

            var s6 = new StudentEntity("STU-006", "CSE-2024-033", "Simran Kaur", "simran.kaur@academicradar.edu", "CSE", "B.Tech Computer Science", 5)
            {
                Section = "B",
                MentorId = "MNT-01",
                MentorName = "Dr. Rajesh Kumar",
                AttendanceRate = 88.0,
                AssessmentScore = 82.0,
                SubmittedAssignments = 9,
                TotalAssignments = 10,
                LmsActivityHours = 38.0,
                Risk = RiskLevel.Low,
                ComputedRiskScore = 18.5,
                AttendanceHistory = new[] { 86, 87, 88, 88, 88 },
                AssessmentHistory = new[] { 80, 81, 82, 83, 82 },
                Courses = new()
                {
                    new() { CourseCode = "CS501", CourseName = "Database Management Systems", AttendanceRate = 89, MarksObtained = 84, Credits = 4 },
                    new() { CourseCode = "CS502", CourseName = "Operating Systems", AttendanceRate = 87, MarksObtained = 80, Credits = 4 }
                }
            };

            _students.AddRange(new[] { s1, s2, s3, s4, s5, s6 });

            // Seed Interventions
            _interventions.AddRange(new[]
            {
                new MentoringInterventionEntity
                {
                    Id = 1,
                    StudentId = "STU-001",
                    StudentName = "Aarav Sharma",
                    MentorName = "Dr. Rajesh Kumar",
                    ActionType = "Counseling Session",
                    Status = "In Progress",
                    Notes = "Discussed attendance deficit in CS501 and CS502. Assigned peer tutor.",
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new MentoringInterventionEntity
                {
                    Id = 2,
                    StudentId = "STU-003",
                    StudentName = "Rohan Mehta",
                    MentorName = "Prof. Ananya Iyer",
                    ActionType = "Parent Notification",
                    Status = "Scheduled",
                    Notes = "Formal warning letter dispatched regarding 48% attendance and internal test score of 42%.",
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new MentoringInterventionEntity
                {
                    Id = 3,
                    StudentId = "STU-005",
                    StudentName = "Karan Singhania",
                    MentorName = "Dr. Vikram Singh",
                    ActionType = "Remedial Classes",
                    Status = "Completed",
                    Notes = "Completed 8-hour remedial workshop on Thermodynamics numericals.",
                    CreatedAt = DateTime.UtcNow.AddDays(-12),
                    CompletedAt = DateTime.UtcNow.AddDays(-2)
                }
            });
        }
    }

    // CRUD
    public IEnumerable<StudentEntity> GetAllStudents()
    {
        lock (_lock)
        {
            return _students.ToList();
        }
    }

    public StudentEntity? GetStudentById(string id)
    {
        lock (_lock)
        {
            return _students.FirstOrDefault(s => s.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
        }
    }

    public StudentEntity? GetStudentByCode(string code)
    {
        lock (_lock)
        {
            return _students.FirstOrDefault(s => s.StudentCode.Equals(code, StringComparison.OrdinalIgnoreCase));
        }
    }

    public void AddStudent(StudentEntity student)
    {
        lock (_lock)
        {
            if (string.IsNullOrWhiteSpace(student.Id))
            {
                student.Id = $"STU-{_students.Count + 1:D3}";
            }
            _students.Add(student);
        }
    }

    public void UpdateStudent(StudentEntity student)
    {
        lock (_lock)
        {
            var existingIndex = _students.FindIndex(s => s.Id.Equals(student.Id, StringComparison.OrdinalIgnoreCase));
            if (existingIndex >= 0)
            {
                _students[existingIndex] = student;
            }
        }
    }

    public bool DeleteStudent(string id)
    {
        lock (_lock)
        {
            var student = _students.FirstOrDefault(s => s.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
            if (student != null)
            {
                return _students.Remove(student);
            }
            return false;
        }
    }

    // Unit III QueryString & Unit V SQL Clauses (WHERE, LIKE, ORDER BY)
    public IEnumerable<StudentEntity> QueryStudents(string? department, RiskLevel? riskLevel, string? searchTerm, string? sortBy)
    {
        lock (_lock)
        {
            var query = _students.AsQueryable();

            // SQL WHERE Clauses & Operators
            if (!string.IsNullOrWhiteSpace(department))
            {
                query = query.Where(s => s.Department.Equals(department, StringComparison.OrdinalIgnoreCase));
            }

            if (riskLevel.HasValue)
            {
                query = query.Where(s => s.Risk == riskLevel.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                // SQL LIKE '%search%'
                query = query.Where(s => s.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                         s.StudentCode.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                         s.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            // SQL ORDER BY
            query = sortBy?.ToLowerInvariant() switch
            {
                "risk_desc" => query.OrderByDescending(s => s.ComputedRiskScore),
                "risk_asc" => query.OrderBy(s => s.ComputedRiskScore),
                "attendance_asc" => query.OrderBy(s => s.AttendanceRate),
                "attendance_desc" => query.OrderByDescending(s => s.AttendanceRate),
                "name" => query.OrderBy(s => s.Name),
                _ => query.OrderByDescending(s => s.ComputedRiskScore)
            };

            return query.ToList();
        }
    }

    // Unit V: SQL GROUP BY & HAVING Clauses
    public IEnumerable<DepartmentRiskSummaryDto> GetDepartmentRiskSummary()
    {
        lock (_lock)
        {
            // Translates to:
            // SELECT Department, COUNT(*) AS TotalStudents,
            //        SUM(CASE WHEN Risk IN (2, 3) THEN 1 ELSE 0 END) AS HighRiskCount,
            //        AVG(AttendanceRate) AS AvgAttendance,
            //        AVG(AssessmentScore) AS AvgAssessment
            // FROM Students
            // GROUP BY Department
            // HAVING COUNT(*) > 0
            return _students
                .GroupBy(s => s.Department)
                .Where(g => g.Any()) // HAVING COUNT(*) > 0
                .Select(g => new DepartmentRiskSummaryDto(
                    Department: g.Key,
                    TotalStudents: g.Count(),
                    HighRiskCount: g.Count(s => s.Risk == RiskLevel.High || s.Risk == RiskLevel.Critical),
                    AvgAttendance: Math.Round(g.Average(s => s.AttendanceRate), 1),
                    AvgAssessment: Math.Round(g.Average(s => s.AssessmentScore), 1)
                ))
                .OrderByDescending(x => x.HighRiskCount)
                .ToList();
        }
    }

    // Unit V: SQL Joins - INNER JOIN
    public IEnumerable<StudentCourseJoinDto> ExecuteInnerJoin()
    {
        lock (_lock)
        {
            // SELECT s.StudentCode, s.Name, c.CourseCode, c.CourseName, c.AttendanceRate, c.MarksObtained
            // FROM Students s
            // INNER JOIN StudentCourses c ON s.Id = c.StudentId
            return _students
                .SelectMany(s => s.Courses, (s, c) => new StudentCourseJoinDto(
                    s.StudentCode,
                    s.Name,
                    c.CourseCode,
                    c.CourseName,
                    c.AttendanceRate,
                    c.MarksObtained
                ))
                .ToList();
        }
    }

    // Unit V: SQL Joins - LEFT OUTER JOIN
    public IEnumerable<StudentInterventionJoinDto> ExecuteLeftJoin()
    {
        lock (_lock)
        {
            // SELECT s.StudentCode, s.Name, i.ActionType, i.Status, i.MentorName
            // FROM Students s
            // LEFT JOIN Interventions i ON s.Id = i.StudentId
            return from s in _students
                   join i in _interventions on s.Id equals i.StudentId into siGroup
                   from subI in siGroup.DefaultIfEmpty()
                   select new StudentInterventionJoinDto(
                       s.StudentCode,
                       s.Name,
                       subI?.ActionType,
                       subI?.Status,
                       subI?.MentorName
                   );
        }
    }

    // Unit V: SQL Joins - RIGHT OUTER JOIN
    public IEnumerable<MentorStudentJoinDto> ExecuteRightJoin()
    {
        lock (_lock)
        {
            // SELECT m.Name AS MentorName, m.Department, s.StudentCode, s.Name AS StudentName
            // FROM Students s
            // RIGHT JOIN Mentors m ON s.MentorId = m.Id
            return from m in _mentors
                   join s in _students on m.Id equals s.MentorId into msGroup
                   from subS in msGroup.DefaultIfEmpty()
                   select new MentorStudentJoinDto(
                       m.Name,
                       m.Department,
                       subS?.StudentCode,
                       subS?.Name
                   );
        }
    }

    // Unit V: SQL Joins - FULL OUTER JOIN
    public IEnumerable<FullJoinDto> ExecuteFullJoin()
    {
        lock (_lock)
        {
            var left = from s in _students
                       join i in _interventions on s.Id equals i.StudentId into gj
                       from subI in gj.DefaultIfEmpty()
                       select new FullJoinDto(s.Name, s.Department, subI?.ActionType, subI?.Status);

            var right = from i in _interventions
                        join s in _students on i.StudentId equals s.Id into gj
                        from subS in gj.DefaultIfEmpty()
                        where subS == null
                        select new FullJoinDto(i.StudentName, "N/A", i.ActionType, i.Status);

            return left.Concat(right).ToList();
        }
    }

    // Unit V: SQL Server Views - vw_HighRiskStudentsOverview
    public IEnumerable<HighRiskStudentViewDto> GetHighRiskStudentsView()
    {
        lock (_lock)
        {
            // CREATE VIEW vw_HighRiskStudentsOverview AS
            // SELECT StudentCode, Name, Department, Semester, ComputedRiskScore, Risk, MentorName
            // FROM Students WHERE Risk IN ('High', 'Critical');
            return _students
                .Where(s => s.Risk == RiskLevel.High || s.Risk == RiskLevel.Critical)
                .Select(s => new HighRiskStudentViewDto(
                    s.StudentCode,
                    s.Name,
                    s.Department,
                    s.Semester,
                    s.ComputedRiskScore,
                    s.Risk.ToString(),
                    s.MentorName
                ))
                .OrderByDescending(v => v.RiskScore)
                .ToList();
        }
    }

    // Unit V: SQL Server Views - vw_StudentAttendancePerformance
    public IEnumerable<AttendancePerformanceViewDto> GetAttendancePerformanceView()
    {
        lock (_lock)
        {
            return _students.Select(s => new AttendancePerformanceViewDto(
                s.StudentCode,
                s.Name,
                s.AttendanceRate,
                s.AssessmentScore,
                s.AttendanceRate switch
                {
                    >= 85 => "A (Excellent Attendance)",
                    >= 75 => "B (Satisfactory)",
                    >= 65 => "C (Warning Zone)",
                    _ => "F (Critically Low)"
                }
            )).ToList();
        }
    }

    // Unit V: Stored Procedure - sp_GetStudentsAtRisk
    public IEnumerable<StudentEntity> ExecuteSpGetStudentsAtRisk(RiskLevel minRisk, double maxAttendance)
    {
        lock (_lock)
        {
            // CREATE PROCEDURE sp_GetStudentsAtRisk
            //    @MinRisk INT,
            //    @MaxAttendance FLOAT
            // AS
            // BEGIN
            //    SELECT * FROM Students
            //    WHERE Risk >= @MinRisk OR AttendanceRate <= @MaxAttendance;
            // END
            return _students
                .Where(s => (int)s.Risk >= (int)minRisk || s.AttendanceRate <= maxAttendance)
                .OrderByDescending(s => s.ComputedRiskScore)
                .ToList();
        }
    }

    // Unit V: Stored Procedure - sp_EnrollStudentAndRecalculateRisk
    public SpEnrollmentResultDto ExecuteSpEnrollStudentAndRecalculateRisk(string studentId, string courseCode, string courseName, int credits)
    {
        lock (_lock)
        {
            var student = _students.FirstOrDefault(s => s.Id.Equals(studentId, StringComparison.OrdinalIgnoreCase) ||
                                                        s.StudentCode.Equals(studentId, StringComparison.OrdinalIgnoreCase));
            if (student == null)
            {
                return new SpEnrollmentResultDto(false, $"Student with ID/Code '{studentId}' not found.", "", 0, 0, 0);
            }

            if (student.Courses.Any(c => c.CourseCode.Equals(courseCode, StringComparison.OrdinalIgnoreCase)))
            {
                return new SpEnrollmentResultDto(false, $"Student is already enrolled in '{courseCode}'.", student.StudentCode, student.ComputedRiskScore, student.ComputedRiskScore, student.Courses.Count);
            }

            double oldRisk = student.ComputedRiskScore;

            // Enroll new course with default baseline
            student.Courses.Add(new StudentCoursePerformance
            {
                CourseCode = courseCode,
                CourseName = courseName,
                Credits = credits,
                AttendanceRate = student.AttendanceRate,
                MarksObtained = student.AssessmentScore
            });

            // Recalculate risk
            student.ComputedRiskScore = student.CalculateSimpleRiskScore(35, 35, 30);
            student.Risk = student.ComputedRiskScore switch
            {
                >= 70 => RiskLevel.Critical,
                >= 50 => RiskLevel.High,
                >= 30 => RiskLevel.Moderate,
                _ => RiskLevel.Low
            };

            return new SpEnrollmentResultDto(
                true,
                $"Successfully enrolled in '{courseName}' ({courseCode}). Risk score updated from {oldRisk:F1} to {student.ComputedRiskScore:F1}.",
                student.StudentCode,
                oldRisk,
                student.ComputedRiskScore,
                student.Courses.Count
            );
        }
    }

    // Mentors
    public IEnumerable<FacultyMentor> GetAllMentors()
    {
        lock (_lock)
        {
            return _mentors.ToList();
        }
    }

    public void AddMentor(FacultyMentor mentor)
    {
        lock (_lock)
        {
            if (string.IsNullOrWhiteSpace(mentor.Id))
            {
                mentor.Id = $"MNT-{_mentors.Count + 1:D2}";
            }
            _mentors.Add(mentor);
        }
    }

    // Interventions
    public IEnumerable<MentoringInterventionEntity> GetAllInterventions()
    {
        lock (_lock)
        {
            return _interventions.OrderByDescending(i => i.CreatedAt).ToList();
        }
    }

    public void AddIntervention(MentoringInterventionEntity intervention)
    {
        lock (_lock)
        {
            intervention.Id = _interventions.Count + 1;
            intervention.CreatedAt = DateTime.UtcNow;
            _interventions.Add(intervention);
        }
    }

    // Unit IV: Middleware Audit Logs
    public IEnumerable<AuditLogEntry> GetRecentAuditLogs(int count = 20)
    {
        lock (_lock)
        {
            return _auditLogs.TakeLast(count).Reverse().ToList();
        }
    }

    public void AddAuditLog(AuditLogEntry log)
    {
        lock (_lock)
        {
            log.Id = _auditLogs.Count + 1;
            _auditLogs.Add(log);
            if (_auditLogs.Count > 500)
            {
                _auditLogs.RemoveAt(0);
            }
        }
    }
}
