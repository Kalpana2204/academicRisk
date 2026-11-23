
using System;
using System.Collections.Generic;
using System.Linq;

namespace AcademicRiskRadar.Web.Services;

public sealed record RiskWeights(
    double Attendance,
    double Assessment,
    double Assignments,
    double Lms
)
{
    public double Total =>
        Attendance + Assessment + Assignments + Lms;

    public static RiskWeights Default =>
        new(35, 30, 20, 15);
}

public sealed record RiskContribution(
    string Name,
    double IndicatorRisk,
    double WeightShare,
    double Contribution
);

public sealed record RiskScoreResult(
    double Score,
    string Band,
    string ModelVersion,
    List<RiskContribution> Contributions
);

public static class RiskScoringService
{
    public const string ModelVersion = "demo-0.2";

    public const double ModerateThreshold = 30;
    public const double HighThreshold = 60;

    public static RiskScoreResult Calculate(
        Student student,
        RiskWeights weights)
    {
        if (student is null)
        {
            throw new ArgumentNullException(nameof(student));
        }

        if (weights is null || !AreWeightsValid(weights))
        {
            throw new ArgumentException(
                "At least one non-negative weight must be positive.",
                nameof(weights)
            );
        }

        var indicators = new List<(
            string Name,
            double Risk,
            double Weight
        )>
        {
            (
                "Attendance",
                100 - ClampPercent(student.Attendance),
                weights.Attendance
            ),
            (
                "Assessment Performance",
                100 - ClampPercent(student.Assessment),
                weights.Assessment
            ),
            (
                "LMS Engagement",
                100 - ClampPercent(student.LmsActivity),
                weights.Lms
            )
        };

        // Missing assignment totals are not treated
        // as perfect assignment completion.
        if (student.TotalAssignments > 0)
        {
            double completion =
                100.0 *
                Math.Clamp(
                    student.SubmittedAssignments,
                    0,
                    student.TotalAssignments
                ) /
                student.TotalAssignments;

            indicators.Add((
                "Assignment Completion",
                100 - completion,
                weights.Assignments
            ));
        }

        // Normalize over indicators that have usable data.
        double applicableWeight =
            indicators.Sum(x => x.Weight);

        if (applicableWeight <= 0)
        {
            throw new InvalidOperationException(
                "No weighted academic indicators are available."
            );
        }

        var contributions = indicators
            .Select(indicator =>
            {
                double weightShare =
                    100.0 * indicator.Weight / applicableWeight;

                double contribution =
                    indicator.Risk *
                    indicator.Weight /
                    applicableWeight;

                return new RiskContribution(
                    indicator.Name,
                    indicator.Risk,
                    weightShare,
                    contribution
                );
            })
            .OrderByDescending(x => x.Contribution)
            .ToList();

        double score = Math.Clamp(
            contributions.Sum(x => x.Contribution),
            0,
            100
        );

        return new RiskScoreResult(
            score,
            GetBand(score),
            ModelVersion,
            contributions
        );
    }

    public static string GetBand(double score)
    {
        if (score >= HighThreshold)
        {
            return "High";
        }

        if (score >= ModerateThreshold)
        {
            return "Moderate";
        }

        return "Low";
    }

    public static bool AreWeightsValid(RiskWeights weights)
    {
        return weights.Attendance >= 0 &&
               weights.Assessment >= 0 &&
               weights.Assignments >= 0 &&
               weights.Lms >= 0 &&
               double.IsFinite(weights.Total) &&
               weights.Total > 0;
    }

    private static double ClampPercent(double value)
    {
        return Math.Clamp(value, 0, 100);
    }
}
