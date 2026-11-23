
using System;
using System.Collections.Generic;
using System.Linq;

namespace AcademicRiskRadar.Web.Services;

// A shared alert record.
public sealed class AcademicAlert
{
    public int Id { get; set; }

    public string StudentId { get; set; } = "";
    public string StudentName { get; set; } = "";

    public string Title { get; set; } = "";
    public string Message { get; set; } = "";

    public string Priority { get; set; } = "Moderate";
    public string Category { get; set; } = "Academic";

    public DateTime CreatedAt { get; set; }

    public bool Acknowledged { get; set; }

    public DateTime? AcknowledgedAt { get; set; }
}

// A shared mentoring intervention record.
public sealed class AcademicIntervention
{
    public int Id { get; set; }

    public string StudentId { get; set; } = "";
    public string StudentName { get; set; } = "";

    public string Mentor { get; set; } = "";

    public string Title { get; set; } = "";
    public string ActionPlan { get; set; } = "";

    public DateTime MeetingDate { get; set; }

    public string Priority { get; set; } = "Moderate";
    public string Status { get; set; } = "Scheduled";

    public string Notes { get; set; } = "";

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}

// This service is the single in-memory source of
// workflow information for one Blazor scope.
//
// Later, the same operations can use a database
// without requiring every page to maintain
// its own copy of the data.
public sealed class AcademicWorkflowService
{
    private readonly List<AcademicAlert> _alerts = new();
    private readonly List<AcademicIntervention> _interventions = new();

    private int _nextAlertId = 1;
    private int _nextInterventionId = 1;

    private static readonly HashSet<string> ValidPriorities =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "Low",
            "Moderate",
            "High"
        };

    private static readonly HashSet<string> ValidStatuses =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "Scheduled",
            "In Progress",
            "Completed",
            "Cancelled"
        };

    public AcademicWorkflowService()
    {
        SeedDemoData();
    }

    // ---------------------------------------
    // ALERT QUERIES
    // ---------------------------------------

    public IReadOnlyList<AcademicAlert> GetAllAlerts()
    {
        return _alerts
            .OrderByDescending(a => a.CreatedAt)
            .ThenByDescending(a => a.Id)
            .ToList();
    }

    public AcademicAlert? GetAlertById(int id)
    {
        return _alerts.FirstOrDefault(a => a.Id == id);
    }

    public IReadOnlyList<AcademicAlert> GetAlertsByStudent(
        string studentId)
    {
        return _alerts
            .Where(a => string.Equals(
                a.StudentId,
                studentId,
                StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(a => a.CreatedAt)
            .ToList();
    }

    public int GetTotalAlertCount()
    {
        return _alerts.Count;
    }

    public int GetPendingAlertCount()
    {
        return _alerts.Count(a => !a.Acknowledged);
    }

    public int GetAcknowledgedAlertCount()
    {
        return _alerts.Count(a => a.Acknowledged);
    }

    public int GetPendingHighPriorityAlertCount()
    {
        return _alerts.Count(a =>
            !a.Acknowledged && a.Priority == "High");
    }

    // ---------------------------------------
    // ALERT ACTIONS
    // ---------------------------------------

    public bool SetAlertAcknowledged(
        int alertId,
        bool acknowledged)
    {
        var alert = GetAlertById(alertId);

        if (alert is null)
        {
            return false;
        }

        if (alert.Acknowledged == acknowledged)
        {
            return true;
        }

        alert.Acknowledged = acknowledged;

        alert.AcknowledgedAt = acknowledged
            ? DateTime.Now
            : null;

        return true;
    }

    public AcademicAlert CreateAlert(
        string studentId,
        string studentName,
        string title,
        string message,
        string priority,
        string category)
    {
        if (string.IsNullOrWhiteSpace(studentId) ||
            string.IsNullOrWhiteSpace(studentName) ||
            string.IsNullOrWhiteSpace(title) ||
            string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException(
                "Student, title and message are required.");
        }

        if (!ValidPriorities.Contains(priority))
        {
            throw new ArgumentException(
                "Invalid alert priority.");
        }

        var alert = new AcademicAlert
        {
            Id = _nextAlertId++,
            StudentId = studentId.Trim(),
            StudentName = studentName.Trim(),
            Title = title.Trim(),
            Message = message.Trim(),
            Priority = priority,
            Category = string.IsNullOrWhiteSpace(category)
                ? "Academic"
                : category.Trim(),
            CreatedAt = DateTime.Now,
            Acknowledged = false
        };

        _alerts.Add(alert);

        return alert;
    }

    // ---------------------------------------
    // INTERVENTION QUERIES
    // ---------------------------------------

    public IReadOnlyList<AcademicIntervention>
        GetAllInterventions()
    {
        return _interventions
            .OrderByDescending(i => i.Id)
            .ToList();
    }

    public AcademicIntervention? GetInterventionById(int id)
    {
        return _interventions.FirstOrDefault(i => i.Id == id);
    }

    public IReadOnlyList<AcademicIntervention>
        GetInterventionsByStudent(string studentId)
    {
        return _interventions
            .Where(i => string.Equals(
                i.StudentId,
                studentId,
                StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(i => i.Id)
            .ToList();
    }

    public int GetTotalInterventionCount()
    {
        return _interventions.Count;
    }

    public int GetScheduledInterventionCount()
    {
        return _interventions.Count(
            i => i.Status == "Scheduled");
    }

    public int GetInProgressInterventionCount()
    {
        return _interventions.Count(
            i => i.Status == "In Progress");
    }

    public int GetCompletedInterventionCount()
    {
        return _interventions.Count(
            i => i.Status == "Completed");
    }

    public int GetActiveInterventionCount()
    {
        return _interventions.Count(i =>
            i.Status == "Scheduled" ||
            i.Status == "In Progress");
    }

    // ---------------------------------------
    // INTERVENTION ACTIONS
    // ---------------------------------------

    public AcademicIntervention CreateIntervention(
        string studentId,
        string studentName,
        string mentor,
        DateTime meetingDate,
        string priority,
        string title,
        string actionPlan)
    {
        if (string.IsNullOrWhiteSpace(studentId) ||
            string.IsNullOrWhiteSpace(studentName) ||
            string.IsNullOrWhiteSpace(mentor) ||
            string.IsNullOrWhiteSpace(title) ||
            string.IsNullOrWhiteSpace(actionPlan))
        {
            throw new ArgumentException(
                "Complete all required intervention fields.");
        }

        if (!ValidPriorities.Contains(priority))
        {
            throw new ArgumentException(
                "Invalid intervention priority.");
        }

        if (meetingDate <= DateTime.Now)
        {
            throw new ArgumentException(
                "The meeting date must be in the future.");
        }

        var intervention = new AcademicIntervention
        {
            Id = _nextInterventionId++,
            StudentId = studentId.Trim(),
            StudentName = studentName.Trim(),
            Mentor = mentor.Trim(),
            MeetingDate = meetingDate,
            Priority = priority,
            Title = title.Trim(),
            ActionPlan = actionPlan.Trim(),
            Status = "Scheduled",
            CreatedAt = DateTime.Now,
            Notes = ""
        };

        _interventions.Add(intervention);

        return intervention;
    }

    public bool UpdateInterventionStatus(
        int interventionId,
        string status)
    {
        if (!ValidStatuses.Contains(status))
        {
            return false;
        }

        var intervention =
            GetInterventionById(interventionId);

        if (intervention is null)
        {
            return false;
        }

        intervention.Status = status;
        intervention.UpdatedAt = DateTime.Now;

        return true;
    }

    public bool UpdateInterventionNotes(
        int interventionId,
        string notes)
    {
        var intervention =
            GetInterventionById(interventionId);

        if (intervention is null)
        {
            return false;
        }

        intervention.Notes = notes?.Trim() ?? "";
        intervention.UpdatedAt = DateTime.Now;

        return true;
    }

    // ---------------------------------------
    // SYNTHETIC DEMONSTRATION RECORDS
    // ---------------------------------------

    private void SeedDemoData()
    {
        AddDemoAlert(
            "STU005",
            "Aditya Singh",
            "Low Attendance and Missing Assignments",
            "Attendance is below the demonstration review " +
            "threshold, with multiple outstanding assignments.",
            "High",
            "Attendance",
            new DateTime(2026, 11, 16, 10, 30, 0),
            false
        );

        AddDemoAlert(
            "STU001",
            "Aarav Sharma",
            "Assessment Performance Decline",
            "Recent assessment history shows " +
            "deteriorating academic performance.",
            "High",
            "Assessment",
            new DateTime(2026, 11, 16, 9, 15, 0),
            false
        );

        AddDemoAlert(
            "STU008",
            "Ananya Roy",
            "Attendance Monitoring Required",
            "Attendance remains below the " +
            "demonstration review threshold.",
            "High",
            "Attendance",
            new DateTime(2026, 11, 15, 14, 0, 0),
            true
        );

        AddDemoAlert(
            "STU003",
            "Rohan Gupta",
            "Attendance Deterioration",
            "Attendance has declined during " +
            "the six-week monitoring period.",
            "Moderate",
            "Trend",
            new DateTime(2026, 11, 15, 11, 0, 0),
            false
        );

        AddDemoAlert(
            "STU010",
            "Ishita Das",
            "Assessment Trend Concern",
            "Assessment scores show a downward trend.",
            "Moderate",
            "Assessment",
            new DateTime(2026, 11, 14, 15, 0, 0),
            false
        );

        AddDemoAlert(
            "STU006",
            "Neha Kapoor",
            "Assessment Review Recommended",
            "Assessment performance should be " +
            "reviewed with the assigned mentor.",
            "Moderate",
            "Assessment",
            new DateTime(2026, 11, 14, 10, 0, 0),
            true
        );

        AddDemoAlert(
            "STU001",
            "Aarav Sharma",
            "Outstanding Assignment Review",
            "Several assignments are not recorded " +
            "as submitted.",
            "High",
            "Assignments",
            new DateTime(2026, 11, 13, 12, 0, 0),
            false
        );

        AddDemoAlert(
            "STU005",
            "Aditya Singh",
            "Low LMS Engagement",
            "Learning activity is below the " +
            "demonstration engagement threshold.",
            "High",
            "LMS",
            new DateTime(2026, 11, 12, 9, 0, 0),
            true
        );

        AddDemoIntervention(
            "STU001",
            "Aarav Sharma",
            "Dr. Rajesh Kumar",
            new DateTime(2026, 11, 20, 10, 0, 0),
            "High",
            "Attendance and Assessment Improvement",
            "Review attendance barriers, identify " +
            "difficult subjects, schedule weekly " +
            "academic support and monitor progress.",
            "Scheduled",
            ""
        );

        AddDemoIntervention(
            "STU005",
            "Aditya Singh",
            "Dr. Rajesh Kumar",
            new DateTime(2026, 11, 18, 14, 30, 0),
            "High",
            "Assignment Recovery Support",
            "Review pending assignments, agree on " +
            "achievable submission targets and " +
            "arrange academic assistance.",
            "In Progress",
            "Initial academic review completed. " +
            "Follow-up support discussion is planned."
        );

        AddDemoIntervention(
            "STU003",
            "Rohan Gupta",
            "Dr. Rajesh Kumar",
            new DateTime(2026, 11, 12, 11, 0, 0),
            "Moderate",
            "Attendance Mentoring Session",
            "Discuss attendance changes, review " +
            "the weekly class schedule and " +
            "identify support options.",
            "Completed",
            "Demonstration record: mentoring " +
            "discussion completed."
        );
    }

    private void AddDemoAlert(
        string studentId,
        string studentName,
        string title,
        string message,
        string priority,
        string category,
        DateTime createdAt,
        bool acknowledged)
    {
        _alerts.Add(new AcademicAlert
        {
            Id = _nextAlertId++,
            StudentId = studentId,
            StudentName = studentName,
            Title = title,
            Message = message,
            Priority = priority,
            Category = category,
            CreatedAt = createdAt,
            Acknowledged = acknowledged,
            AcknowledgedAt = acknowledged
                ? createdAt.AddHours(2)
                : null
        });
    }

    private void AddDemoIntervention(
        string studentId,
        string studentName,
        string mentor,
        DateTime meetingDate,
        string priority,
        string title,
        string actionPlan,
        string status,
        string notes)
    {
        _interventions.Add(new AcademicIntervention
        {
            Id = _nextInterventionId++,
            StudentId = studentId,
            StudentName = studentName,
            Mentor = mentor,
            MeetingDate = meetingDate,
            Priority = priority,
            Title = title,
            ActionPlan = actionPlan,
            Status = status,
            Notes = notes,
            CreatedAt = new DateTime(2026, 11, 12, 9, 0, 0)
        });
    }
}
