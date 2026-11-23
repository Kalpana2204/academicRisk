using System;
using System.Collections.Generic;
using AcademicRiskRadar.Web.Models;

namespace AcademicRiskRadar.Web.Database;

/// <summary>
/// Unit IV: Developing Application with Code First Approach & Database First Approach
/// 
/// ARCHITECTURAL COMPARISON:
/// 1. CODE-FIRST APPROACH:
///    - You write C# domain model classes first.
///    - You define DbContext and relationships via Data Annotations ([Key], [Required], [ForeignKey]) 
///      and Fluent API (modelBuilder.Entity<T>()).
///    - EF Core CLI generates and manages database migrations:
///         dotnet ef migrations add InitialCreate
///         dotnet ef database update
///    - Gives full type safety, version-controlled database schema changes, and clean domain design.
///
/// 2. DATABASE-FIRST APPROACH:
///    - The SQL Server database and tables already exist in production.
///    - You reverse-engineer C# POCO entity classes and DbContext from the existing schema using EF Core CLI:
///         dotnet ef dbcontext scaffold "Server=(localdb)\mssqllocaldb;Database=AcademicRiskRadarDB;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models/Scaffolded
///    - Ideal when integrating with legacy corporate databases or DBA-maintained schemas.
/// </summary>
public class CodeFirstArchitectureGuide
{
    public const string CodeFirstCommandAddMigration = "dotnet ef migrations add InitialCreate";
    public const string CodeFirstCommandUpdateDb = "dotnet ef database update";
    public const string DbFirstCommandScaffold = "dotnet ef dbcontext scaffold \"Server=localhost;Database=AcademicRiskRadarDB;Trusted_Connection=True;TrustServerCertificate=True;\" Microsoft.EntityFrameworkCore.SqlServer -o Models/DbFirst";

    public static Dictionary<string, string> GetComparisonTable() => new()
    {
        { "Starting Point", "Code-First: C# POCO Classes | Database-First: Existing SQL Server DB" },
        { "Schema Changes", "Code-First: In C# via Migrations | Database-First: Alter scripts executed in SSMS" },
        { "Tooling", "Code-First: dotnet ef migrations | Database-First: dotnet ef dbcontext scaffold" },
        { "Best For", "Code-First: New agile projects, DDD | Database-First: Legacy DBs, strict DBA governance" },
        { "Control", "Code-First: Developer controls code | Database-First: DBA controls table schema" }
    };
}
