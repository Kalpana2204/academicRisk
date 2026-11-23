-- =========================================================================================
-- ACADEMIC RISK RADAR - COMPLETE SQL SERVER ENTERPRISE DATABASE SCHEMA & SCRIPTS
-- UNITS COVERED:
-- Unit V: SQL Server For Backend Development (Databases, Tables, Constraints, Clauses, 
--         Operators, Joins, Views, Stored Procedures)
-- Unit IV: Developing Application with Code First & Database First Approaches
-- =========================================================================================

-- -----------------------------------------------------------------------------------------
-- 1. DATABASE CREATION
-- -----------------------------------------------------------------------------------------
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'AcademicRiskRadarDB')
BEGIN
    CREATE DATABASE AcademicRiskRadarDB;
END
GO

USE AcademicRiskRadarDB;
GO

-- -----------------------------------------------------------------------------------------
-- 2. DDL: TABLE CREATION WITH CONSTRAINTS (PK, FK, CHECK, UNIQUE, NOT NULL, DEFAULT)
-- -----------------------------------------------------------------------------------------

-- Drop dependent tables in reverse order of foreign keys
IF OBJECT_ID('dbo.MentoringInterventions', 'U') IS NOT NULL DROP TABLE dbo.MentoringInterventions;
IF OBJECT_ID('dbo.RiskAssessments', 'U') IS NOT NULL DROP TABLE dbo.RiskAssessments;
IF OBJECT_ID('dbo.StudentEnrollments', 'U') IS NOT NULL DROP TABLE dbo.StudentEnrollments;
IF OBJECT_ID('dbo.Courses', 'U') IS NOT NULL DROP TABLE dbo.Courses;
IF OBJECT_ID('dbo.Students', 'U') IS NOT NULL DROP TABLE dbo.Students;
IF OBJECT_ID('dbo.FacultyMentors', 'U') IS NOT NULL DROP TABLE dbo.FacultyMentors;
IF OBJECT_ID('dbo.SystemAuditLogs', 'U') IS NOT NULL DROP TABLE dbo.SystemAuditLogs;
GO

-- Table 1: Faculty Mentors
CREATE TABLE dbo.FacultyMentors (
    MentorId NVARCHAR(20) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(120) NOT NULL,
    Department NVARCHAR(50) NOT NULL,
    Designation NVARCHAR(80) NOT NULL DEFAULT 'Assistant Professor',
    OfficeRoom NVARCHAR(30) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    -- Constraints
    CONSTRAINT PK_FacultyMentors PRIMARY KEY CLUSTERED (MentorId),
    CONSTRAINT UQ_FacultyMentors_Email UNIQUE (Email)
);
GO

-- Table 2: Students
CREATE TABLE dbo.Students (
    StudentId NVARCHAR(20) NOT NULL,
    StudentCode NVARCHAR(25) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(120) NOT NULL,
    Department NVARCHAR(50) NOT NULL,
    Program NVARCHAR(100) NOT NULL,
    Semester INT NOT NULL,
    Section NVARCHAR(5) NOT NULL DEFAULT 'A',
    MentorId NVARCHAR(20) NULL,
    AttendanceRate DECIMAL(5, 2) NOT NULL DEFAULT 85.00,
    AssessmentScore DECIMAL(5, 2) NOT NULL DEFAULT 75.00,
    SubmittedAssignments INT NOT NULL DEFAULT 0,
    TotalAssignments INT NOT NULL DEFAULT 10,
    LmsActivityHours DECIMAL(6, 2) NOT NULL DEFAULT 20.00,
    RiskLevel NVARCHAR(20) NOT NULL DEFAULT 'Low',
    ComputedRiskScore DECIMAL(5, 2) NOT NULL DEFAULT 15.00,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    -- Constraints
    CONSTRAINT PK_Students PRIMARY KEY CLUSTERED (StudentId),
    CONSTRAINT UQ_Students_StudentCode UNIQUE (StudentCode),
    CONSTRAINT UQ_Students_Email UNIQUE (Email),
    CONSTRAINT FK_Students_Mentor FOREIGN KEY (MentorId) 
        REFERENCES dbo.FacultyMentors (MentorId) ON DELETE SET NULL,
    CONSTRAINT CK_Students_Semester CHECK (Semester BETWEEN 1 AND 8),
    CONSTRAINT CK_Students_Attendance CHECK (AttendanceRate BETWEEN 0.00 AND 100.00),
    CONSTRAINT CK_Students_Assessment CHECK (AssessmentScore BETWEEN 0.00 AND 100.00),
    CONSTRAINT CK_Students_Assignments CHECK (SubmittedAssignments <= TotalAssignments),
    CONSTRAINT CK_Students_RiskLevel CHECK (RiskLevel IN ('Low', 'Moderate', 'High', 'Critical'))
);
GO

-- Table 3: Courses
CREATE TABLE dbo.Courses (
    CourseCode NVARCHAR(20) NOT NULL,
    CourseName NVARCHAR(150) NOT NULL,
    Department NVARCHAR(50) NOT NULL,
    Credits INT NOT NULL DEFAULT 3,

    CONSTRAINT PK_Courses PRIMARY KEY CLUSTERED (CourseCode),
    CONSTRAINT CK_Courses_Credits CHECK (Credits BETWEEN 1 AND 6)
);
GO

-- Table 4: StudentEnrollments (Composite Primary Key & Junction Table)
CREATE TABLE dbo.StudentEnrollments (
    StudentId NVARCHAR(20) NOT NULL,
    CourseCode NVARCHAR(20) NOT NULL,
    AttendanceRate DECIMAL(5, 2) NOT NULL DEFAULT 80.00,
    MarksObtained DECIMAL(5, 2) NOT NULL DEFAULT 70.00,
    GradeLetter NVARCHAR(2) NULL,
    EnrolledOn DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT PK_StudentEnrollments PRIMARY KEY CLUSTERED (StudentId, CourseCode),
    CONSTRAINT FK_Enrollments_Student FOREIGN KEY (StudentId) 
        REFERENCES dbo.Students (StudentId) ON DELETE CASCADE,
    CONSTRAINT FK_Enrollments_Course FOREIGN KEY (CourseCode) 
        REFERENCES dbo.Courses (CourseCode) ON DELETE CASCADE,
    CONSTRAINT CK_Enrollments_Attendance CHECK (AttendanceRate BETWEEN 0.00 AND 100.00),
    CONSTRAINT CK_Enrollments_Marks CHECK (MarksObtained BETWEEN 0.00 AND 100.00)
);
GO

-- Table 5: MentoringInterventions
CREATE TABLE dbo.MentoringInterventions (
    InterventionId INT IDENTITY(1,1) NOT NULL,
    StudentId NVARCHAR(20) NOT NULL,
    ActionType NVARCHAR(60) NOT NULL,
    Status NVARCHAR(30) NOT NULL DEFAULT 'Scheduled',
    Notes NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CompletedAt DATETIME2 NULL,

    CONSTRAINT PK_MentoringInterventions PRIMARY KEY CLUSTERED (InterventionId),
    CONSTRAINT FK_Interventions_Student FOREIGN KEY (StudentId) 
        REFERENCES dbo.Students (StudentId) ON DELETE CASCADE,
    CONSTRAINT CK_Interventions_Status CHECK (Status IN ('Scheduled', 'In Progress', 'Completed', 'Follow-up Required'))
);
GO

-- Table 6: SystemAuditLogs (Unit IV Security & Middleware Logging)
CREATE TABLE dbo.SystemAuditLogs (
    LogId INT IDENTITY(1,1) NOT NULL,
    HttpMethod NVARCHAR(10) NOT NULL,
    Path NVARCHAR(250) NOT NULL,
    StatusCode INT NOT NULL,
    ElapsedMilliseconds BIGINT NOT NULL,
    ClientIp NVARCHAR(50) NOT NULL,
    UserIdentity NVARCHAR(120) NOT NULL,
    Role NVARCHAR(40) NOT NULL,
    Timestamp DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT PK_SystemAuditLogs PRIMARY KEY CLUSTERED (LogId)
);
GO

-- -----------------------------------------------------------------------------------------
-- 3. DML: SAMPLE DATA SEEDING
-- -----------------------------------------------------------------------------------------

-- Seed Mentors
INSERT INTO dbo.FacultyMentors (MentorId, FullName, Email, Department, Designation, OfficeRoom) VALUES
('MNT-01', N'Dr. Rajesh Kumar', N'rajesh.kumar@academicradar.edu', N'CSE', N'Professor & HOD', N'CS-301'),
('MNT-02', N'Prof. Ananya Iyer', N'ananya.iyer@academicradar.edu', N'ECE', N'Associate Professor', N'EC-204'),
('MNT-03', N'Dr. Vikram Singh', N'vikram.singh@academicradar.edu', N'MECH', N'Assistant Professor', N'ME-108'),
('MNT-04', N'Dr. Sneha Patil', N'sneha.patil@academicradar.edu', N'IT', N'Assistant Professor', N'IT-402'),
('MNT-05', N'Dr. Aris Thorne', N'aris.thorne@academicradar.edu', N'CSE', N'Visiting Fellow', N'CS-310');

-- Seed Courses
INSERT INTO dbo.Courses (CourseCode, CourseName, Department, Credits) VALUES
('CS501', N'Database Management Systems', N'CSE', 4),
('CS502', N'Operating Systems', N'CSE', 4),
('CS503', N'Computer Networks', N'CSE', 3),
('EC301', N'Signals and Systems', N'ECE', 4),
('EC302', N'Digital Electronics', N'ECE', 4),
('ME601', N'Thermodynamics II', N'MECH', 4),
('IT401', N'Web Technologies', N'IT', 3);

-- Seed Students
INSERT INTO dbo.Students (StudentId, StudentCode, FullName, Email, Department, Program, Semester, Section, MentorId, AttendanceRate, AssessmentScore, SubmittedAssignments, TotalAssignments, LmsActivityHours, RiskLevel, ComputedRiskScore) VALUES
('STU-001', 'CSE-2024-001', N'Aarav Sharma', N'aarav.sharma@academicradar.edu', N'CSE', N'B.Tech Computer Science', 5, 'A', 'MNT-01', 58.50, 52.00, 4, 10, 12.00, 'High', 67.50),
('STU-002', 'CSE-2024-002', N'Priya Verma', N'priya.verma@academicradar.edu', N'CSE', N'B.Tech Computer Science', 5, 'A', 'MNT-01', 92.00, 88.50, 10, 10, 45.00, 'Low', 11.20),
('STU-003', 'ECE-2024-015', N'Rohan Mehta', N'rohan.mehta@academicradar.edu', N'ECE', N'B.Tech Electronics', 3, 'B', 'MNT-02', 48.00, 42.00, 3, 8, 8.50, 'Critical', 79.40),
('STU-004', 'IT-2024-009', N'Neha Kulkarni', N'neha.kulkarni@academicradar.edu', N'IT', N'B.Tech Information Technology', 4, 'A', 'MNT-04', 72.50, 68.00, 7, 10, 22.00, 'Moderate', 38.60),
('STU-005', 'MECH-2024-022', N'Karan Singhania', N'karan.singhania@academicradar.edu', N'MECH', N'B.Tech Mechanical', 6, 'C', 'MNT-03', 63.00, 58.00, 5, 9, 14.00, 'High', 56.80),
('STU-006', 'CSE-2024-033', N'Simran Kaur', N'simran.kaur@academicradar.edu', N'CSE', N'B.Tech Computer Science', 5, 'B', 'MNT-01', 88.00, 82.00, 9, 10, 38.00, 'Low', 18.50);

-- Seed Enrollments
INSERT INTO dbo.StudentEnrollments (StudentId, CourseCode, AttendanceRate, MarksObtained) VALUES
('STU-001', 'CS501', 56.00, 48.00),
('STU-001', 'CS502', 60.00, 54.00),
('STU-002', 'CS501', 94.00, 90.00),
('STU-002', 'CS502', 91.00, 86.00),
('STU-003', 'EC301', 45.00, 40.00),
('STU-003', 'EC302', 51.00, 44.00),
('STU-004', 'IT401', 75.00, 72.00),
('STU-005', 'ME601', 62.00, 55.00);

-- Seed Interventions
INSERT INTO dbo.MentoringInterventions (StudentId, ActionType, Status, Notes) VALUES
('STU-001', N'Counseling Session', N'In Progress', N'Discussed attendance lag in CS501 and assigned peer study circle.'),
('STU-003', N'Parent Notification', N'Scheduled', N'Official academic warning dispatched for < 50% attendance.'),
('STU-005', N'Remedial Classes', N'Completed', N'Completed supplementary numerical modules for Thermodynamics.');
GO

-- -----------------------------------------------------------------------------------------
-- 4. SQL CLAUSES & OPERATORS DEMONSTRATION
-- -----------------------------------------------------------------------------------------

-- A. WHERE Clause with Comparison & Logical Operators (AND, OR, NOT)
SELECT StudentCode, FullName, Department, AttendanceRate, AssessmentScore, RiskLevel
FROM dbo.Students
WHERE (AttendanceRate < 75.00 OR AssessmentScore < 60.00)
  AND NOT (RiskLevel = 'Low')
ORDER BY ComputedRiskScore DESC;

-- B. Special Operators: IN, BETWEEN, LIKE
SELECT StudentCode, FullName, Department, ComputedRiskScore
FROM dbo.Students
WHERE Department IN ('CSE', 'IT')
  AND ComputedRiskScore BETWEEN 30.00 AND 80.00
  AND FullName LIKE '%a%'
ORDER BY FullName ASC;

-- C. GROUP BY & HAVING Clauses
SELECT 
    Department,
    COUNT(*) AS TotalMonitoredStudents,
    SUM(CASE WHEN RiskLevel IN ('High', 'Critical') THEN 1 ELSE 0 END) AS HighRiskStudents,
    CAST(AVG(AttendanceRate) AS DECIMAL(5,2)) AS AverageAttendance,
    CAST(AVG(AssessmentScore) AS DECIMAL(5,2)) AS AverageAssessment
FROM dbo.Students
GROUP BY Department
HAVING COUNT(*) >= 1
ORDER BY HighRiskStudents DESC;
GO

-- -----------------------------------------------------------------------------------------
-- 5. SQL JOINS (INNER, LEFT, RIGHT, FULL OUTER)
-- -----------------------------------------------------------------------------------------

-- 1. INNER JOIN (Students matching Course Enrollments)
SELECT 
    s.StudentCode, 
    s.FullName AS StudentName, 
    c.CourseCode, 
    c.CourseName, 
    e.AttendanceRate, 
    e.MarksObtained
FROM dbo.Students s
INNER JOIN dbo.StudentEnrollments e ON s.StudentId = e.StudentId
INNER JOIN dbo.Courses c ON e.CourseCode = c.CourseCode;

-- 2. LEFT OUTER JOIN (All Students and their Interventions if any)
SELECT 
    s.StudentCode, 
    s.FullName, 
    s.RiskLevel, 
    i.ActionType, 
    i.Status AS InterventionStatus, 
    i.Notes
FROM dbo.Students s
LEFT OUTER JOIN dbo.MentoringInterventions i ON s.StudentId = i.StudentId;

-- 3. RIGHT OUTER JOIN (All Mentors whether they have assigned students or not)
SELECT 
    m.FullName AS MentorName, 
    m.Department, 
    m.Designation, 
    s.StudentCode, 
    s.FullName AS StudentName
FROM dbo.Students s
RIGHT OUTER JOIN dbo.FacultyMentors m ON s.MentorId = m.MentorId;

-- 4. FULL OUTER JOIN (Comprehensive match of students and interventions)
SELECT 
    s.StudentCode,
    s.FullName AS StudentName,
    i.InterventionId,
    i.ActionType,
    i.Status
FROM dbo.Students s
FULL OUTER JOIN dbo.MentoringInterventions i ON s.StudentId = i.StudentId;
GO

-- -----------------------------------------------------------------------------------------
-- 6. SQL SERVER VIEWS (User Defined Views)
-- -----------------------------------------------------------------------------------------

IF OBJECT_ID('dbo.vw_HighRiskStudentsOverview', 'V') IS NOT NULL DROP VIEW dbo.vw_HighRiskStudentsOverview;
GO
CREATE VIEW dbo.vw_HighRiskStudentsOverview
AS
SELECT 
    s.StudentCode,
    s.FullName,
    s.Department,
    s.Semester,
    s.AttendanceRate,
    s.AssessmentScore,
    s.ComputedRiskScore,
    s.RiskLevel,
    ISNULL(m.FullName, 'Unassigned') AS MentorName
FROM dbo.Students s
LEFT JOIN dbo.FacultyMentors m ON s.MentorId = m.MentorId
WHERE s.RiskLevel IN ('High', 'Critical');
GO

IF OBJECT_ID('dbo.vw_StudentAttendancePerformance', 'V') IS NOT NULL DROP VIEW dbo.vw_StudentAttendancePerformance;
GO
CREATE VIEW dbo.vw_StudentAttendancePerformance
AS
SELECT 
    s.StudentCode,
    s.FullName,
    s.AttendanceRate,
    s.AssessmentScore,
    CASE 
        WHEN s.AttendanceRate >= 85.00 THEN 'A (Exemplary Attendance)'
        WHEN s.AttendanceRate >= 75.00 THEN 'B (Satisfactory Standard)'
        WHEN s.AttendanceRate >= 65.00 THEN 'C (Borderline Alert)'
        ELSE 'F (Immediate Remedial Action Required)'
    END AS AttendanceGrade
FROM dbo.Students s;
GO

-- -----------------------------------------------------------------------------------------
-- 7. STORED PROCEDURES (User Defined Stored Procedures)
-- -----------------------------------------------------------------------------------------

IF OBJECT_ID('dbo.sp_GetStudentsAtRisk', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_GetStudentsAtRisk;
GO
CREATE PROCEDURE dbo.sp_GetStudentsAtRisk
    @MinRiskLevel NVARCHAR(20) = 'High',
    @MaxAttendance DECIMAL(5,2) = 65.00
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        s.StudentId,
        s.StudentCode,
        s.FullName,
        s.Department,
        s.AttendanceRate,
        s.AssessmentScore,
        s.ComputedRiskScore,
        s.RiskLevel,
        ISNULL(m.FullName, 'Unassigned') AS MentorName
    FROM dbo.Students s
    LEFT JOIN dbo.FacultyMentors m ON s.MentorId = m.MentorId
    WHERE (s.RiskLevel = @MinRiskLevel OR s.RiskLevel = 'Critical')
       OR s.AttendanceRate <= @MaxAttendance
    ORDER BY s.ComputedRiskScore DESC;
END;
GO

IF OBJECT_ID('dbo.sp_EnrollStudentAndRecalculateRisk', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_EnrollStudentAndRecalculateRisk;
GO
CREATE PROCEDURE dbo.sp_EnrollStudentAndRecalculateRisk
    @StudentId NVARCHAR(20),
    @CourseCode NVARCHAR(20),
    @InitialAttendance DECIMAL(5,2) = 75.00,
    @InitialMarks DECIMAL(5,2) = 65.00,
    @NewRiskScore DECIMAL(5,2) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Check if student and course exist
        IF NOT EXISTS (SELECT 1 FROM dbo.Students WHERE StudentId = @StudentId)
        BEGIN
            RAISERROR('Student identifier does not exist.', 16, 1);
        END

        IF NOT EXISTS (SELECT 1 FROM dbo.Courses WHERE CourseCode = @CourseCode)
        BEGIN
            RAISERROR('Course code does not exist.', 16, 1);
        END

        -- Insert enrollment
        INSERT INTO dbo.StudentEnrollments (StudentId, CourseCode, AttendanceRate, MarksObtained)
        VALUES (@StudentId, @CourseCode, @InitialAttendance, @InitialMarks);

        -- Recalculate average student scores
        DECLARE @AvgAtt DECIMAL(5,2), @AvgMarks DECIMAL(5,2);
        SELECT 
            @AvgAtt = AVG(AttendanceRate),
            @AvgMarks = AVG(MarksObtained)
        FROM dbo.StudentEnrollments
        WHERE StudentId = @StudentId;

        -- Calculate new risk score
        SET @NewRiskScore = ((100.00 - @AvgAtt) * 0.40) + ((100.00 - @AvgMarks) * 0.35) + 10.00;

        DECLARE @NewRiskLevel NVARCHAR(20) = 
            CASE 
                WHEN @NewRiskScore >= 70 THEN 'Critical'
                WHEN @NewRiskScore >= 50 THEN 'High'
                WHEN @NewRiskScore >= 30 THEN 'Moderate'
                ELSE 'Low'
            END;

        -- Update Student Master Record
        UPDATE dbo.Students
        SET AttendanceRate = @AvgAtt,
            AssessmentScore = @AvgMarks,
            ComputedRiskScore = @NewRiskScore,
            RiskLevel = @NewRiskLevel,
            UpdatedAt = GETUTCDATE()
        WHERE StudentId = @StudentId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO
