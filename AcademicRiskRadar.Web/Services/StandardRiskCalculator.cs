using System;
using AcademicRiskRadar.Web.Models;

namespace AcademicRiskRadar.Web.Services;

/// <summary>
/// Unit II: Polymorphism - Concrete Derived Class 1
/// Standard linear multi-factor scoring model.
/// </summary>
public class StandardRiskCalculator : RiskCalculatorBase
{
    public override string StrategyName => "Standard Multi-Factor Model";

    protected override double ComputeRawScore(StudentEntity student)
    {
        // 35% Attendance, 30% Assessment, 20% Assignments, 15% LMS
        double attendanceRisk = Math.Max(0, 100.0 - student.AttendanceRate);
        double assessmentRisk = Math.Max(0, 100.0 - student.AssessmentScore);
        double assignmentRisk = Math.Max(0, 100.0 - student.AssignmentCompletionPercentage);
        
        // LMS activity risk (benchmarked at 30 hours)
        double lmsRisk = Math.Max(0, 100.0 - Math.Min(100.0, (student.LmsActivityHours / 30.0) * 100.0));

        double weightedScore = (attendanceRisk * 0.35) +
                               (assessmentRisk * 0.30) +
                               (assignmentRisk * 0.20) +
                               (lmsRisk * 0.15);

        return Math.Clamp(weightedScore, 0.0, 100.0);
    }
}

/// <summary>
/// Unit II: Polymorphism - Concrete Derived Class 2
/// Trend-sensitive scoring model that prioritizes downward momentum in attendance history.
/// </summary>
public class WeightedTrendRiskCalculator : RiskCalculatorBase
{
    public override string StrategyName => "Trend & Momentum Sensitive Model";

    protected override double ComputeRawScore(StudentEntity student)
    {
        double baseScore = (100.0 - student.AttendanceRate) * 0.40 +
                           (100.0 - student.AssessmentScore) * 0.35 +
                           (100.0 - student.AssignmentCompletionPercentage) * 0.25;

        // Unit II: Arrays & Manipulating Arrays - Trend detection
        if (student.AttendanceHistory != null && student.AttendanceHistory.Length >= 2)
        {
            int first = student.AttendanceHistory[0];
            int last = student.AttendanceHistory[^1]; // Modern C# index from end
            int delta = first - last; // Positive means attendance dropped over time

            if (delta > 10)
            {
                // Penalize downward trend
                baseScore += 12.5;
            }
        }

        return Math.Clamp(baseScore, 0.0, 100.0);
    }

    // Overriding determine level to be stricter
    protected override RiskLevel DetermineLevel(double score)
    {
        return score switch
        {
            >= 65.0 => RiskLevel.Critical,
            >= 45.0 => RiskLevel.High,
            >= 25.0 => RiskLevel.Moderate,
            _ => RiskLevel.Low
        };
    }
}
