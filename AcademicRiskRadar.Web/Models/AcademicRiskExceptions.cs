using System;

namespace AcademicRiskRadar.Web.Models;

/// <summary>
/// Unit II: Exception Handling - Custom Exception Hierarchy
/// </summary>
public class AcademicRiskException : Exception
{
    public AcademicRiskException() : base("An error occurred during academic risk processing.") { }
    public AcademicRiskException(string message) : base(message) { }
    public AcademicRiskException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// Unit II: Custom Exception for threshold breaches
/// </summary>
public class AcademicRiskThresholdExceededException : AcademicRiskException
{
    public string StudentId { get; }
    public double CalculatedRiskScore { get; }
    public double Threshold { get; }

    public AcademicRiskThresholdExceededException(string studentId, double riskScore, double threshold)
        : base($"Student '{studentId}' exceeded critical risk threshold! Score: {riskScore:F1} (Threshold: {threshold:F1})")
    {
        StudentId = studentId;
        CalculatedRiskScore = riskScore;
        Threshold = threshold;
    }
}
