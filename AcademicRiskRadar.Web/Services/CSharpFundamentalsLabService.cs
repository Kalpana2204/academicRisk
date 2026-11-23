using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcademicRiskRadar.Web.Models;

namespace AcademicRiskRadar.Web.Services;

/// <summary>
/// Unit II: C# Programming Fundamentals - Executable Educational Engine
/// Covers: Variables, Datatypes, Operators & Precedence, Conditionals, Loops,
/// Jump Statements, Arrays & Types, Strings & Methods, OOP Concepts, Exceptions.
/// </summary>
public class CSharpFundamentalsLabService
{
    // 1. Variable & Datatype Demo
    public (Dictionary<string, string> Types, string Explanation) RunDataTypesDemo()
    {
        // Value Types
        byte byteVal = 255;
        int intVal = 1042;
        long longVal = 9876543210L;
        float floatVal = 78.5f;
        double doubleVal = 89.95;
        decimal decimalVal = 14500.50m;
        bool boolVal = true;
        char charVal = 'A';

        // Nullable Value Type
        int? nullableInt = null;

        // Reference Types
        string stringVal = "Academic Risk Radar";
        object objVal = 42;
        int[] arrayVal = new int[] { 10, 20, 30 };

        var map = new Dictionary<string, string>
        {
            { "byte (Value Type - 8 bit)", $"{byteVal} (Range: 0 to 255)" },
            { "int (Value Type - 32 bit)", $"{intVal} (Signed 32-bit integer)" },
            { "long (Value Type - 64 bit)", $"{longVal} (Large integer with 'L' suffix)" },
            { "float (Value Type - 32 bit)", $"{floatVal}f (Single precision with 'f')" },
            { "double (Value Type - 64 bit)", $"{doubleVal} (Double precision standard)" },
            { "decimal (Value Type - 128 bit)", $"{decimalVal:C} (High precision financial/academic)" },
            { "bool (Value Type - 1 bit logic)", $"{boolVal}" },
            { "char (Value Type - 16 bit Unicode)", $"'{charVal}'" },
            { "int? (Nullable Value Type)", nullableInt == null ? "null (HasValue: False)" : nullableInt.ToString()! },
            { "string (Reference Type - Immutability)", $"\"{stringVal}\" (Length: {stringVal.Length})" },
            { "object (Reference Type - Root)", $"{objVal} (Type: {objVal.GetType().Name})" },
            { "int[] (Reference Type - Array)", $"[{string.Join(", ", arrayVal)}]" }
        };

        return (map, "Value types are stored in the execution Stack and contain their data directly. Reference types store a pointer on the Stack referencing the object instance allocated on the Managed Heap.");
    }

    // 2. Operators & Precedence Demo
    public (List<string> Results, string Explanation) RunOperatorsDemo(int a, int b)
    {
        var logs = new List<string>();

        // Arithmetic
        logs.Add($"Arithmetic: a + b = {a + b}, a - b = {a - b}, a * b = {a * b}, a / b = {(b != 0 ? (a / (double)b).ToString("F2") : "Undefined")}, a % b = {(b != 0 ? (a % b).ToString() : "Undefined")}");

        // Relational
        logs.Add($"Relational: a == b: {a == b}, a != b: {a != b}, a > b: {a > b}, a < b: {a < b}, a >= b: {a >= b}, a <= b: {a <= b}");

        // Logical
        bool cond1 = a > 50;
        bool cond2 = b < 50;
        logs.Add($"Logical: (a > 50 && b < 50) is {cond1 && cond2}, (a > 50 || b < 50) is {cond1 || cond2}, !(a > 50) is {!cond1}");

        // Bitwise
        logs.Add($"Bitwise: a & b = {a & b}, a | b = {a | b}, a ^ b = {a ^ b}, ~a = {~a}, a << 1 = {a << 1}, a >> 1 = {a >> 1}");

        // Ternary & Null-coalescing
        string grade = a >= 75 ? "Distinction" : "Pass/Remedial";
        string? nullString = null;
        string defaultStr = nullString ?? "Fallback Academic Note";
        logs.Add($"Ternary Operator (a >= 75): \"{grade}\"");
        logs.Add($"Null-Coalescing Operator (nullString ?? fallback): \"{defaultStr}\"");

        // Operator Precedence Demo
        // Expression: a + b * 2 evaluated as a + (b * 2) due to multiplicative precedence over additive
        int precedenceResult = a + b * 2;
        logs.Add($"Precedence Hierarchy: {a} + {b} * 2 = {precedenceResult} (Multiplication binds tighter than Addition)");

        return (logs, "C# operator precedence rules ensure: 1. Primary/Postfix -> 2. Unary -> 3. Multiplicative (*, /, %) -> 4. Additive (+, -) -> 5. Relational -> 6. Equality -> 7. Logical AND -> 8. Logical OR -> 9. Ternary -> 10. Assignment.");
    }

    // 3. Conditional Statements Demo
    public (string BranchTaken, List<string> ExecutionSteps) RunConditionalsDemo(double attendance, double assessment)
    {
        var steps = new List<string>();
        string riskVerdict;

        steps.Add($"Evaluating conditions for Attendance = {attendance}%, Assessment = {assessment}%");

        // if-else-if ladder
        if (attendance < 60.0 && assessment < 50.0)
        {
            steps.Add("Condition: (attendance < 60 && assessment < 50) evaluated to TRUE");
            riskVerdict = "CRITICAL RISK: Immediate counseling and Dean notification required.";
        }
        else if (attendance < 75.0 || assessment < 60.0)
        {
            steps.Add("Condition: (attendance < 75 || assessment < 60) evaluated to TRUE");
            riskVerdict = "HIGH RISK: Assigning mentor intervention and remedial sessions.";
        }
        else if (attendance >= 85.0 && assessment >= 75.0)
        {
            steps.Add("Condition: (attendance >= 85 && assessment >= 75) evaluated to TRUE");
            riskVerdict = "LOW RISK / MERIT: Student is performing in top academic quadrant.";
        }
        else
        {
            steps.Add("Fallback ELSE branch evaluated");
            riskVerdict = "MODERATE RISK: Routine monitoring recommended.";
        }

        // Switch with pattern matching (Modern C# feature)
        string tier = (int)attendance switch
        {
            >= 90 => "Tier A+ (Honor Roll)",
            >= 75 => "Tier B (Eligible for Examinations)",
            >= 60 => "Tier C (Conditional Attendance)",
            _ => "Tier D (Debarred / Below Minimum Standard)"
        };
        steps.Add($"Switch Expression Pattern Matching Result: '{tier}'");

        return (riskVerdict, steps);
    }

    // 4. Loops & Jump Statements Demo
    public (List<string> LoopTrace, string Summary) RunLoopsAndJumpDemo(int iterations = 6)
    {
        var trace = new List<string>();

        // 1. For Loop with Break & Continue
        trace.Add("--- 1. FOR LOOP WITH JUMP STATEMENTS (break, continue) ---");
        for (int i = 1; i <= iterations; i++)
        {
            if (i == 2)
            {
                trace.Add($"Iteration {i}: Encountered 'continue' -> Skipped processing for week {i}.");
                continue;
            }
            if (i == 5)
            {
                trace.Add($"Iteration {i}: Encountered 'break' -> Terminated loop early at week {i}.");
                break;
            }
            trace.Add($"Iteration {i}: Processed student weekly attendance log #{i}.");
        }

        // 2. While Loop
        trace.Add("--- 2. WHILE LOOP (Condition checked before entry) ---");
        int count = 1;
        while (count <= 3)
        {
            trace.Add($"While loop pass {count}: Verified LMS login status.");
            count++;
        }

        // 3. Do-While Loop (Executes at least once)
        trace.Add("--- 3. DO-WHILE LOOP (Executes at least once) ---");
        int doCount = 1;
        do
        {
            trace.Add($"Do-While pass {doCount}: Calculated baseline attendance quotient.");
            doCount++;
        } while (doCount <= 2);

        // 4. Foreach Loop
        trace.Add("--- 4. FOREACH LOOP (Iterating over collection) ---");
        string[] modules = { "Algorithms", "Databases", "Web Architecture" };
        foreach (var mod in modules)
        {
            trace.Add($"Foreach module: Scanning risk parameters for '{mod}'.");
        }

        return (trace, "Loops provide iteration control. 'break' exits the innermost loop immediately, 'continue' skips to the next iteration, 'return' exits the containing method, and 'goto' transfers control to a labeled statement.");
    }

    // 5. Arrays & Types of Arrays & Manipulating Arrays
    public (Dictionary<string, object> ArrayDemos, string CodeSample) RunArraysDemo()
    {
        // 1. Single-Dimensional Array
        int[] singleArr = { 88, 62, 74, 95, 51, 69 };
        int[] sortedSingle = (int[])singleArr.Clone();
        Array.Sort(sortedSingle);
        int[] reversedSingle = (int[])sortedSingle.Clone();
        Array.Reverse(reversedSingle);
        int foundIndex = Array.IndexOf(singleArr, 95);
        int highestPassing = Array.Find(sortedSingle, score => score >= 75);

        // 2. Multidimensional 2D Rectangular Array [Students, Semesters]
        int[,] rectangularArr = new int[2, 3]
        {
            { 85, 78, 92 }, // Student 1: Sem 1, 2, 3
            { 60, 58, 64 }  // Student 2: Sem 1, 2, 3
        };

        // 3. Jagged Array (Array of arrays with variable lengths)
        int[][] jaggedArr = new int[3][]
        {
            new int[] { 85, 90 },             // 2 assignments
            new int[] { 70, 75, 80, 85, 90 }, // 5 assignments
            new int[] { 60 }                  // 1 assignment
        };

        var dict = new Dictionary<string, object>
        {
            { "Single-Dimensional Original", $"[{string.Join(", ", singleArr)}]" },
            { "Array.Sort()", $"[{string.Join(", ", sortedSingle)}]" },
            { "Array.Reverse()", $"[{string.Join(", ", reversedSingle)}]" },
            { "Array.IndexOf(..., 95)", $"Score 95 found at index: {foundIndex}" },
            { "Array.Find(>= 75)", $"First score meeting threshold: {highestPassing}" },
            { "2D Rectangular Array (2x3)", $"Dimensions: [{rectangularArr.GetLength(0)}, {rectangularArr.GetLength(1)}], [0,0]={rectangularArr[0,0]}, [1,2]={rectangularArr[1,2]}" },
            { "Jagged Array (Array of Arrays)", $"Row 0 length: {jaggedArr[0].Length}, Row 1 length: {jaggedArr[1].Length}, Row 2 length: {jaggedArr[2].Length}" }
        };

        string code = @"// 1D Array
int[] marks = { 88, 62, 74, 95 };
Array.Sort(marks);

// 2D Rectangular Array
int[,] matrix = new int[2, 3] { { 85, 78, 92 }, { 60, 58, 64 } };

// Jagged Array (Variable Column Dimensions)
int[][] jagged = new int[3][];
jagged[0] = new int[] { 85, 90 };
jagged[1] = new int[] { 70, 75, 80 };";

        return (dict, code);
    }

    // 6. Strings & String Methods & Manipulating Strings
    public (Dictionary<string, string> MethodsOutput, string StringBuilderResult) RunStringsDemo(string input = "  Academic Risk Radar: Early Warning System v2.0  ")
    {
        var methods = new Dictionary<string, string>
        {
            { "Original Raw String", $"\"{input}\"" },
            { "Length Property", input.Length.ToString() },
            { "Trim()", $"\"{input.Trim()}\"" },
            { "ToUpper()", input.Trim().ToUpper() },
            { "ToLower()", input.Trim().ToLower() },
            { "Substring(0, 19)", input.Trim().Substring(0, 19) },
            { "Contains(\"Warning\")", input.Contains("Warning").ToString() },
            { "StartsWith(\"Academic\")", input.Trim().StartsWith("Academic").ToString() },
            { "EndsWith(\"v2.0\")", input.Trim().EndsWith("v2.0").ToString() },
            { "Replace(\"Radar\", \"Engine\")", input.Trim().Replace("Radar", "Engine") },
            { "IndexOf(':')", input.IndexOf(':').ToString() }
        };

        // Split & Join
        string[] tokens = input.Trim().Split(new[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        methods.Add("Split() & string.Join(' | ')", string.Join(" | ", tokens));

        // StringBuilder for high-performance string manipulation
        var sb = new StringBuilder();
        sb.Append("[STUDENT AUDIT] ");
        sb.AppendFormat("Timestamp: {0:yyyy-MM-dd HH:mm:ss} | ", DateTime.UtcNow);
        sb.Append("Risk status calculated successfully.");
        sb.Replace("calculated", "verified & saved");

        return (methods, sb.ToString());
    }

    // 7. Exception Handling Demo (try, catch, finally, custom exception)
    public (bool Success, string Message, string Details, string FlowSummary) RunExceptionDemo(string studentId, double proposedScore)
    {
        var log = new StringBuilder();
        try
        {
            log.AppendLine("1. ENTERED 'try' block.");
            if (string.IsNullOrWhiteSpace(studentId))
            {
                throw new ArgumentNullException(nameof(studentId), "Student identifier cannot be null or whitespace.");
            }

            if (proposedScore > 80.0)
            {
                log.AppendLine("2. Condition triggered -> Throwing AcademicRiskThresholdExceededException!");
                throw new AcademicRiskThresholdExceededException(studentId, proposedScore, 80.0);
            }

            log.AppendLine($"2. Score {proposedScore} is safe. No exceptions thrown.");
            return (true, "Execution Succeeded without error.", $"Risk score {proposedScore:F1} within standard limits.", log.ToString());
        }
        catch (AcademicRiskThresholdExceededException ex)
        {
            log.AppendLine($"3. CAUGHT AcademicRiskThresholdExceededException: {ex.Message}");
            return (false, "Triggered Custom Academic Exception!", ex.Message, log.ToString());
        }
        catch (ArgumentNullException ex)
        {
            log.AppendLine($"3. CAUGHT ArgumentNullException: {ex.ParamName} missing.");
            return (false, "Input Validation Exception!", ex.Message, log.ToString());
        }
        catch (Exception ex)
        {
            log.AppendLine($"3. CAUGHT General System.Exception: {ex.Message}");
            return (false, "General Error!", ex.Message, log.ToString());
        }
        finally
        {
            log.AppendLine("4. ENTERED 'finally' block: Cleared memory buffers & closed diagnostic stream.");
        }
    }
}
