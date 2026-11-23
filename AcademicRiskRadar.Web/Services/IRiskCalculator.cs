using System;
using AcademicRiskRadar.Web.Models;

namespace AcademicRiskRadar.Web.Services;

/// <summary>
/// Unit II: Abstraction - Interface Contract
/// </summary>
public interface IRiskCalculator
{
    string StrategyName { get; }
    (double Score, RiskLevel Level, string Reason) Calculate(StudentEntity student);
}

/// <summary>
/// Unit II: Abstract Classes & Inheritance
/// Base class providing template method and common scoring utilities.
/// </summary>
public abstract class RiskCalculatorBase : IRiskCalculator
{
    public abstract string StrategyName { get; }

    // Unit II: Virtual method with default implementation
    public virtual (double Score, RiskLevel Level, string Reason) Calculate(StudentEntity student)
    {
        if (student == null) throw new ArgumentNullException(nameof(student));

        double rawScore = ComputeRawScore(student);
        RiskLevel level = DetermineLevel(rawScore);
        string reason = ExplainScore(student, rawScore);

        return (Math.Round(rawScore, 1), level, reason);
    }

    // Unit II: Abstract method that derived classes MUST implement
    protected abstract double ComputeRawScore(StudentEntity student);

    // Unit II: Virtual method that derived classes CAN override
    protected virtual RiskLevel DetermineLevel(double score)
    {
        return score switch
        {
            >= 70.0 => RiskLevel.Critical,
            >= 50.0 => RiskLevel.High,
            >= 30.0 => RiskLevel.Moderate,
            _ => RiskLevel.Low
        };
    }

    protected virtual string ExplainScore(StudentEntity student, double score)
    {
        if (student.AttendanceRate < 65.0)
            return $"Critical attendance deficit ({student.AttendanceRate:F0}%) is driving risk.";
        if (student.AssessmentScore < 50.0)
            return $"Severe internal assessment lag ({student.AssessmentScore:F0}%) requires remedial tutoring.";
        if (student.AssignmentCompletionPercentage < 60.0)
            return $"Incomplete coursework submissions ({student.AssignmentCompletionPercentage:F0}%).";
        return "Student performance is within standard academic tolerance.";
    }
}
