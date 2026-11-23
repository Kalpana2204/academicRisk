# 🎯 Academic Risk Radar
### Enterprise Academic Monitoring System & Comprehensive ASP.NET Core / C# Architecture

> **Complete Implementation of Units I through VI**  
> Built with **ASP.NET Core 10 (LTS)**, **C# OOP Domain Modeling**, **MVC Architecture**, **Custom Middleware Pipelines**, **Dependency Injection Lifetimes**, **SQL Server Backend Studio**, and **RESTful Web APIs**.

---

## 🌟 Table of Contents
1. [Project Overview](#-project-overview)
2. [Quick Start & Execution](#-quick-start--execution)
3. [Syllabus & Technical Architecture Mapping (Units I–VI)](#-syllabus--technical-architecture-mapping)
   - [Unit I: Introduction To .Net And C#](#unit-i-introduction-to-net-and-c)
   - [Unit II: C# Programming Fundamentals & OOP](#unit-ii-c-programming-fundamentals--oop)
   - [Unit III: Backend Development Using ASP.NET Core MVC](#unit-iii-backend-development-using-aspnet-core-mvc)
   - [Unit IV: ASP.NET MVC and Security](#unit-iv-aspnet-mvc-and-security)
   - [Unit V: SQL Server For Backend Development](#unit-v-sql-server-for-backend-development)
   - [Unit VI: ASP.NET With Web API & REST](#unit-vi-aspnet-with-web-api--rest)
4. [Interactive Web Endpoints & Navigation Directory](#-interactive-web-endpoints--navigation-directory)
5. [SQL Server Schema & Database Scripts](#-sql-server-schema--database-scripts)
6. [Postman Testing & Collection Guide](#-postman-testing--collection-guide)
7. [Visual Studio Code Configuration](#-visual-studio-code-configuration)

---

## 🚀 Project Overview

**Academic Risk Radar** is an early warning and multi-factor student risk scoring platform designed to monitor academic indicators (attendance deficits, lagging internal assessments, coursework backlogs, LMS inactivity) and alert academic mentors for prompt intervention.

Alongside the operational student monitoring system, this project incorporates an **Interactive Curriculum & Architecture Lab** where every requirement across Units I to VI is interactively demonstrated, executed in real-time, and documented with live code inspectors.

---

## ⚡ Quick Start & Execution

### Option A: Run via .NET CLI
```bash
# 1. Navigate to the web project directory
cd d:\downloads\AcademicRiskRadar\AcademicRiskRadar.Web

# 2. Build the project (0 Warnings, 0 Errors)
dotnet build

# 3. Launch the web application
dotnet run --launch-profile http
```
The application will start on: **`http://localhost:5010`**

### Option B: Run in Visual Studio Code
1. Open folder `d:\downloads\AcademicRiskRadar` in VS Code.
2. The pre-configured `.vscode/launch.json` and `.vscode/tasks.json` provide one-click debugging:
   - Press **F5** to start debugging.
   - Browser opens automatically to `http://localhost:5010`.

---

## 📚 Syllabus & Technical Architecture Mapping

### Unit I: Introduction To .Net And C#
* **ASP.NET Core Features:** Cross-platform (Windows, Linux, macOS), ultra-high performance Kestrel server, unified MVC & Web API programming model, native Dependency Injection, modular HTTP request pipeline.
* **Version History:** Evolutions from .NET Core 1.0 (2016) through .NET Core 2.1/3.1 LTS, .NET 6/8 LTS, up to the current **.NET 10 LTS** runtime.
* **.NET Framework vs .NET Core:** Detailed architectural comparison table covering cross-platform support, CoreCLR vs CLR, NuGet-based modularity, and high-performance execution.
* **VS Code Setup:** Full environment configuration files included:
  - [`.vscode/launch.json`](file:///d:/downloads/AcademicRiskRadar/.vscode/launch.json) (CoreCLR launch profile)
  - [`.vscode/tasks.json`](file:///d:/downloads/AcademicRiskRadar/.vscode/tasks.json) (build, publish, watch tasks)
  - [`.vscode/extensions.json`](file:///d:/downloads/AcademicRiskRadar/.vscode/extensions.json) (C# Dev Kit recommendations)
* **.NET CLI Cheatsheet:** `dotnet new`, `dotnet restore`, `dotnet build`, `dotnet run`, `dotnet watch`, `dotnet test`, `dotnet publish`, `dotnet ef`.
* **C# Code Execution Pipeline:** Source `.cs` &rarr; Roslyn Compiler (`csc.exe`) &rarr; Common Intermediate Language (CIL/MSIL) + Metadata &rarr; CoreCLR JIT Compiler (RyuJIT) &rarr; Native Machine Code with Garbage Collection.
* **Interactive View:** Visit **`/Curriculum/Unit1`** in the browser.

---

### Unit II: C# Programming Fundamentals & OOP
* **Variables & Datatypes:** Value types (`int`, `float`, `double`, `decimal`, `bool`, `char`, `byte`), Reference types (`string`, `object`, `arrays`), Nullable types (`int?`), and boxing/unboxing.
* **Operators & Precedence:** Complete operator hierarchy: Arithmetic (`+`, `-`, `*`, `/`, `%`), Relational (`==`, `!=`, `<`, `>`), Logical (`&&`, `||`, `!`), Bitwise (`&`, `|`, `^`, `~`), Ternary (`? :`), Null-Coalescing (`??`, `??=`), and Null-Conditional (`?.`).
* **Conditionals & Switch:** `if-else-if` ladders, nested conditionals, and modern C# pattern matching switch expressions (`case >= 75:`).
* **Loops & Jump Statements:** `for`, `while`, `do-while`, `foreach`, and jump statements (`break`, `continue`, `return`, `goto`).
* **Arrays & Array Manipulation:**
  - 1D Single-dimensional arrays (`int[] scores`)
  - 2D Rectangular arrays (`int[,] matrix = new int[2, 3]`)
  - Jagged arrays (`int[][] studentSubmissions = new int[3][]`)
  - Array algorithms: `Array.Sort`, `Array.Reverse`, `Array.Find`, `Array.IndexOf`, LINQ queries.
* **Strings & String Manipulation:** Immutability, `Trim()`, `Substring()`, `Replace()`, `Split()`, `Join()`, `IndexOf()`, string interpolation `$"..."`, and `StringBuilder`.
* **Object-Oriented Programming (OOP) Hierarchy:**
  - **Encapsulation & Abstraction:** [`Person.cs`](file:///d:/downloads/AcademicRiskRadar/AcademicRiskRadar.Web/Models/Person.cs) (abstract base class with encapsulated fields and abstract contract).
  - **Inheritance:** [`StudentEntity.cs`](file:///d:/downloads/AcademicRiskRadar/AcademicRiskRadar.Web/Models/StudentEntity.cs) and [`FacultyMentor.cs`](file:///d:/downloads/AcademicRiskRadar/AcademicRiskRadar.Web/Models/FacultyMentor.cs) inherit from `Person` with chained base constructors (`: base(...)`).
  - **Polymorphism (Compile-Time):** Method overloading in `StudentEntity.CalculateSimpleRiskScore(...)`.
  - **Polymorphism (Runtime):** Abstract method overriding (`GetRoleDescription()`) and virtual method overriding (`GetSummary()`).
  - **Polymorphic Strategy Pattern:** [`StandardRiskCalculator`](file:///d:/downloads/AcademicRiskRadar/AcademicRiskRadar.Web/Services/StandardRiskCalculator.cs) vs [`WeightedTrendRiskCalculator`](file:///d:/downloads/AcademicRiskRadar/AcademicRiskRadar.Web/Services/StandardRiskCalculator.cs) extending [`RiskCalculatorBase`](file:///d:/downloads/AcademicRiskRadar/AcademicRiskRadar.Web/Services/IRiskCalculator.cs).
* **Exception Handling:** `try`, `catch`, `finally`, `throw`, multiple catch blocks, and custom exception class [`AcademicRiskThresholdExceededException`](file:///d:/downloads/AcademicRiskRadar/AcademicRiskRadar.Web/Models/AcademicRiskExceptions.cs).
* **Interactive View:** Visit **`/Curriculum/Unit2`** in the browser.

---

### Unit III: Backend Development Using ASP.NET Core MVC
* **Project Folder Structure:** `Controllers/`, `Models/`, `Views/`, `wwwroot/`, `Program.cs`, `appsettings.json`.
* **MVC Request Lifecycle:** HTTP Request &rarr; Routing &rarr; Controller &rarr; Business Logic & Model &rarr; View Rendering &rarr; HTTP Response.
* **Action Results:** `ViewResult`, `JsonResult`, `RedirectToActionResult`, `NotFoundResult`, `BadRequestResult`, `CreatedAtActionResult`.
* **Razor Engine & Layout:** `_Layout.cshtml`, `@RenderBody()`, `@RenderSection("Scripts", false)`, `@RenderSection("Styles", false)`, `_ViewStart.cshtml`, `_ViewImports.cshtml`.
* **Data Passing Techniques Demonstration:**
  1. `ViewBag`: Dynamic property bag (`ViewBag.TotalCount = 42`).
  2. `ViewData`: Dictionary-based state (`ViewData["Title"] = "Student Records"`).
  3. `TempData`: Flash data across HTTP redirects (`TempData["AlertMessage"] = "..."`).
  4. `Session`: State management across consecutive requests (`HttpContext.Session.SetString(...)`).
  5. `QueryString`: URL parameter binding via `[FromQuery]` (`?department=CSE&risk=High&sortBy=risk_desc`).
* **Forms, Model Binding & Validations:**
  - Strongly-typed form tag helpers: `<form asp-action="Create">`, `<input asp-for="Name">`.
  - Server-Side Validation: Data annotations (`[Required]`, `[StringLength]`, `[Range]`, `[EmailAddress]`, `[RegularExpression]`) checked via `ModelState.IsValid`.
  - Client-Side Validation: Instant validation before submission using HTML5 data attributes without full roundtrip.
* **Interactive Views:**
  - Student Directory: **`/Students`**
  - Register Student Form: **`/Students/Create`**
  - Data Passing Test Bench: **`/Curriculum/Unit3`**

---

### Unit IV: ASP.NET MVC and Security
* **Middleware Request Pipeline:** Delegates flow with `next()` invocation and short-circuit capability.
* **Built-in Middleware Configured in `Program.cs`:**
  - `app.UseExceptionHandler("/Home/Error")`
  - `app.UseHsts()` & `app.UseHttpsRedirection()`
  - `app.UseStaticFiles()` & `app.MapStaticAssets()`
  - `app.UseRouting()`
  - `app.UseSession()`
  - `app.UseAuthentication()` & `app.UseAuthorization()`
  - `app.UseAntiforgery()`
* **Custom Middleware:**
  1. [`RequestExecutionTimingMiddleware.cs`](file:///d:/downloads/AcademicRiskRadar/AcademicRiskRadar.Web/Middleware/RequestExecutionTimingMiddleware.cs): Computes request execution duration in milliseconds and stamps the `X-Response-Time-Ms` response header.
  2. [`AcademicSecurityAuditMiddleware.cs`](file:///d:/downloads/AcademicRiskRadar/AcademicRiskRadar.Web/Middleware/AcademicSecurityAuditMiddleware.cs): Records IP, authenticated identity, role, path, and HTTP verb into persistent audit storage.
* **Dependency Injection (DI) Lifetimes Live Inspector:**
  - `AddTransient<IOperationTransient, OperationTransient>()`: Brand new instance per resolve.
  - `AddScoped<IOperationScoped, OperationScoped>()`: Single instance per HTTP request scope.
  - `AddSingleton<IOperationSingleton, OperationSingleton>()`: Single instance application-wide.
  - Visibly inspect different vs identical GUIDs across resolutions at **`/Curriculum/Unit4`**.
* **Code-First vs Database-First Approaches:** Architectural breakdown, CLI migration commands (`dotnet ef migrations add`, `dotnet ef database update`), vs database scaffolding commands (`dotnet ef dbcontext scaffold`).
* **Authentication and Role Authorization:**
  - Cookie Authentication scheme with ClaimsPrincipal & ClaimsIdentity.
  - Role-based authorization policies:
    - **`Admin`** (Full privileges: register, update, delete students)
    - **`Faculty`** (Mentor privileges: register, edit, log interventions)
    - **`Student`** (Student view access)
  - Quick-switch one-click login bar at top of layout.
* **Interactive View:** Visit **`/Curriculum/Unit4`** and **`/Account/Login`**.

---

### Unit V: SQL Server For Backend Development
* **SQL Server Architecture:** Relational DB concepts, T-SQL engine, constraints, and data integrity.
* **SQL Commands:** DDL (`CREATE`, `ALTER`, `DROP`, `TRUNCATE`), DML (`INSERT`, `UPDATE`, `DELETE`), DQL (`SELECT`), DCL (`GRANT`, `REVOKE`), TCL (`COMMIT`, `ROLLBACK`).
* **Tables & Constraints:**
  - `dbo.Students` (PK, UNIQUE `StudentCode`, UNIQUE `Email`, FK `MentorId`, CHECK `AttendanceRate BETWEEN 0 AND 100`, CHECK `Semester BETWEEN 1 AND 8`, NOT NULL, DEFAULT).
  - `dbo.FacultyMentors`, `dbo.Courses`, `dbo.StudentEnrollments`, `dbo.MentoringInterventions`, `dbo.SystemAuditLogs`.
* **SQL Clauses:** `WHERE`, `GROUP BY`, `HAVING`, `ORDER BY ASC/DESC`, `DISTINCT`, `TOP`.
* **SQL Operators:** Comparison (`=`, `<>`, `!=`, `<`, `>`), Logical (`AND`, `OR`, `NOT`), Special (`IN`, `BETWEEN ... AND`, `LIKE '%keyword%'`).
* **SQL Joins:**
  - `INNER JOIN`: Matching students with enrolled courses.
  - `LEFT OUTER JOIN`: All students + matched interventions.
  - `RIGHT OUTER JOIN`: All mentors + assigned students.
  - `FULL OUTER JOIN`: All rows from both sides.
* **SQL Server Views:**
  - `vw_HighRiskStudentsOverview`
  - `vw_StudentAttendancePerformance`
  - `vw_DepartmentRiskSummary`
* **Stored Procedures:**
  - `sp_GetStudentsAtRisk` (parameterized filtering with `@MinRiskLevel` and `@MaxAttendance`).
  - `sp_EnrollStudentAndRecalculateRisk` (T-SQL transaction with `BEGIN TRY/CATCH` and `@NewRiskScore OUTPUT` parameter).
* **Complete Database Script:** Located at [`Database/AcademicRiskRadar_Complete_Schema.sql`](file:///d:/downloads/AcademicRiskRadar/AcademicRiskRadar.Web/Database/AcademicRiskRadar_Complete_Schema.sql).
* **Interactive View:** Visit **`/SqlExplorer`** in the browser.

---

### Unit VI: ASP.NET With Web API & REST
* **REST Architecture Constraints:** Stateless, Client-Server, Cacheable, Uniform Interface, Layered System, Standard HTTP Status Codes (`200 OK`, `201 Created`, `204 No Content`, `400 Bad Request`, `404 Not Found`).
* **ASP.NET Core Web API Controllers:**
  - `StudentsApiController` (`/api/students`): Full REST CRUD operations.
  - `RiskApiController` (`/api/risk`): Multi-factor scoring calculations and statistics.
  - `InterventionsApiController` (`/api/interventions`): Mentoring actions logging.
  - `ConventionApiController` (`/api/convention/...`): Convention routing demonstration.
* **Routing Models Side-by-Side:**
  - **Attribute Routing:** `[Route("api/students")]`, `[HttpGet("{id}")]`, `[HttpPost]`, `[HttpPut("{id}")]`, `[HttpDelete("{id}")]`.
  - **Convention Routing:** `app.MapControllerRoute(name: "api_convention", pattern: "api/convention/{controller}/{action}/{id?}")`.
* **Live In-Browser API Tester:** Interactive AJAX testing panel built directly into the UI.
* **Testing with Postman:** Pre-configured collection file ready to import:
  - [`Postman/AcademicRiskRadar.postman_collection.json`](file:///d:/downloads/AcademicRiskRadar/AcademicRiskRadar.Web/Postman/AcademicRiskRadar.postman_collection.json)
  - Features pre-scripted test assertions (`pm.test("Status code is 200 OK", ...)`).
* **Interactive View:** Visit **`/Curriculum/Unit6`** in the browser.

---

## 🌐 Interactive Web Endpoints & Navigation Directory

| URL Route | Architecture / Component | Description |
|:---|:---|:---|
| `/` | Blazor Interactive Server | Blazor Academic Risk Radar Overview & Navigation |
| `/Home` | ASP.NET Core MVC | Home Dashboard & Technical Architecture Summary |
| `/Students` | MVC Controller & Razor Views | Student Performance Directory, Filters, Sorting & CRUD |
| `/Students/Details/{id}` | MVC Controller | Detailed Student Scorecard, Courses, Attendance History |
| `/Students/Create` | MVC Form & Model Binding | Registration Form with Server-side & Client-side Validation |
| `/Students/Edit/{id}` | MVC Form | Update Student Performance with Auto-Risk Recalculation |
| `/RiskAnalysis` | MVC Controller | Runtime Polymorphic Risk Scoring (Standard vs Trend) |
| `/SqlExplorer` | Unit V SQL Server Studio | Live Joins, Views, Stored Procedures, Tables & Constraints |
| `/Curriculum` | Educational Lab Hub | Complete Syllabus Overview & Matrix |
| `/Curriculum/Unit1` | Unit I Interactive Lab | .NET 10 Core vs Framework, Version History, CLI, Execution |
| `/Curriculum/Unit2` | Unit II Interactive Lab | Variables, Datatypes, Operators, Loops, Arrays, Strings, OOP |
| `/Curriculum/Unit3` | Unit III Interactive Lab | MVC Folder Structure, ViewBag, ViewData, TempData, Session |
| `/Curriculum/Unit4` | Unit IV Interactive Lab | Middleware Pipeline, Live DI Lifetime GUIDs, Security Roles |
| `/Curriculum/Unit5` | Unit V Interactive Lab | Relational Schema, Commands, Constraints, Joins & Views |
| `/Curriculum/Unit6` | Unit VI Interactive Lab | RESTful Web API, Live AJAX Tester, Postman Collection |
| `/api/students` | ASP.NET Web API (REST) | JSON endpoint for listing all students (supports filtering) |
| `/api/students/{id}` | ASP.NET Web API (REST) | JSON endpoint for single student profile |
| `/api/risk/calculate` | ASP.NET Web API (REST) | POST endpoint to calculate real-time risk scores |
| `/Account/Login` | Cookie Auth & Security | Role Sign-in with One-Click Demo Credentials (Admin, Faculty, Student) |

---

## 💾 SQL Server Schema & Database Scripts

The complete database schema can be imported into **SQL Server Management Studio (SSMS)**, **Azure Data Studio**, or **Visual Studio SQL Server Object Explorer**:

- **Physical File Path:** `AcademicRiskRadar.Web/Database/AcademicRiskRadar_Complete_Schema.sql`
- **In-App Download:** Downloadable directly from `/SqlExplorer` (Tab: "Complete SQL Script") or `/Database/AcademicRiskRadar_Complete_Schema.sql`.

---

## 📮 Postman Testing & Collection Guide

1. Open **Postman**.
2. Click **Import** &rarr; Select `AcademicRiskRadar.Web/Postman/AcademicRiskRadar.postman_collection.json`.
3. Collection requests:
   - `GET /api/students?department=CSE&sortBy=risk_desc`
   - `GET /api/students/STU-001`
   - `POST /api/students`
   - `PUT /api/students/STU-001`
   - `DELETE /api/students/STU-006`
   - `POST /api/risk/calculate`
   - `GET /api/risk/stats`
   - `GET /api/convention/ConventionApi/GetStudentSummary?id=STU-001`
   - `GET /api/convention/ConventionApi/EchoServerStatus`
4. Pre-configured environment variable `{{baseUrl}}` is preset to `http://localhost:5010`.
5. Run the collection to verify all automated test assertions pass!

---

## 🛠️ Visual Studio Code Configuration

This repository includes a turnkey `.vscode/` setup for immediate development:
- **`tasks.json`**: Configures `build`, `publish`, and `watch` tasks.
- **`launch.json`**: Pre-configured CoreCLR debugging session targeting `net10.0`.
- **`extensions.json`**: Recommends the official Microsoft C# Dev Kit and MSSQL extensions.

---

### 🎓 Summary of Accomplishment
Every single unit and sub-topic specified in the curriculum has been built, tested, and integrated into this functional web solution. Build succeeds with **0 Warnings** and **0 Errors**.
