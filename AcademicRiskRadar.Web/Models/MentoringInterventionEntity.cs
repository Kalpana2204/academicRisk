using System;

namespace AcademicRiskRadar.Web.Models;

/// <summary>
/// Unit II & IV: Mentoring Intervention Model
/// </summary>
public class MentoringInterventionEntity
{
    public int Id { get; set; }
    public string StudentId { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public string MentorName { get; set; } = string.Empty;
    public string ActionType { get; set; } = "Counseling Session"; // Counseling Session, Remedial Classes, Parent Notification, Attendance Warning
    public string Status { get; set; } = "Scheduled"; // Scheduled, In Progress, Completed, Follow-up Required
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
}

/// <summary>
/// Unit IV: Security & Audit Log Model for Custom Middleware
/// </summary>
public class AuditLogEntry
{
    public int Id { get; set; }
    public string HttpMethod { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public long ElapsedMilliseconds { get; set; }
    public string ClientIp { get; set; } = string.Empty;
    public string UserIdentity { get; set; } = "Anonymous";
    public string Role { get; set; } = "Guest";
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
