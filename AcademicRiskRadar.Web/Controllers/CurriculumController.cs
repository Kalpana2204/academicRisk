using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AcademicRiskRadar.Web.Models;
using AcademicRiskRadar.Web.Services;

namespace AcademicRiskRadar.Web.Controllers;

/// <summary>
/// Interactive Curriculum & Syllabus Lab Controller
/// Houses interactive demonstrations and explanations for Units I through VI.
/// </summary>
public class CurriculumController : Controller
{
    private readonly CSharpFundamentalsLabService _csharpLab;
    private readonly IOperationService _operationService;
    private readonly IOperationTransient _transientOp;
    private readonly IOperationScoped _scopedOp;
    private readonly IOperationSingleton _singletonOp;
    private readonly IStudentRepository _repository;

    public CurriculumController(
        CSharpFundamentalsLabService csharpLab,
        IOperationService operationService,
        IOperationTransient transientOp,
        IOperationScoped scopedOp,
        IOperationSingleton singletonOp,
        IStudentRepository repository)
    {
        _csharpLab = csharpLab;
        _operationService = operationService;
        _transientOp = transientOp;
        _scopedOp = scopedOp;
        _singletonOp = singletonOp;
        _repository = repository;
    }

    // Hub
    [HttpGet]
    public IActionResult Index()
    {
        ViewData["Title"] = "Comprehensive .NET Curriculum & Architecture Hub";
        return View();
    }

    // Unit I: Introduction To .Net And C#
    [HttpGet]
    public IActionResult Unit1()
    {
        ViewData["Title"] = "Unit I: Introduction To .Net And C#";
        ViewBag.NetCoreVersion = Environment.Version.ToString();
        ViewBag.OSVersion = Environment.OSVersion.ToString();
        ViewBag.RuntimeDirectory = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
        return View();
    }

    // Unit II: C# Programming Fundamentals
    [HttpGet]
    public IActionResult Unit2(string demo = "datatypes", int a = 85, int b = 60, double att = 58.0, double test = 52.0)
    {
        ViewData["Title"] = "Unit II: C# Programming Fundamentals";
        ViewBag.ActiveDemo = demo.ToLowerInvariant();

        // 1. Datatypes Demo
        var (types, typeExpl) = _csharpLab.RunDataTypesDemo();
        ViewBag.DataTypes = types;
        ViewBag.DataTypesExplanation = typeExpl;

        // 2. Operators Demo
        var (opResults, opExpl) = _csharpLab.RunOperatorsDemo(a, b);
        ViewBag.OperatorResults = opResults;
        ViewBag.OperatorExplanation = opExpl;
        ViewBag.ParamA = a;
        ViewBag.ParamB = b;

        // 3. Conditionals Demo
        var (verdict, condSteps) = _csharpLab.RunConditionalsDemo(att, test);
        ViewBag.ConditionVerdict = verdict;
        ViewBag.ConditionSteps = condSteps;
        ViewBag.AttParam = att;
        ViewBag.TestParam = test;

        // 4. Loops & Jump Statements Demo
        var (loopTrace, loopSummary) = _csharpLab.RunLoopsAndJumpDemo();
        ViewBag.LoopTrace = loopTrace;
        ViewBag.LoopSummary = loopSummary;

        // 5. Arrays Demo
        var (arrayDict, arrayCode) = _csharpLab.RunArraysDemo();
        ViewBag.ArrayData = arrayDict;
        ViewBag.ArrayCode = arrayCode;

        // 6. Strings Demo
        var (strMethods, sbResult) = _csharpLab.RunStringsDemo();
        ViewBag.StringMethods = strMethods;
        ViewBag.StringBuilderResult = sbResult;

        // 7. Exception Handling Demo
        var (exSuccess, exMsg, exDetails, exFlow) = _csharpLab.RunExceptionDemo("STU-001", 85.0);
        ViewBag.ExSuccess = exSuccess;
        ViewBag.ExMsg = exMsg;
        ViewBag.ExDetails = exDetails;
        ViewBag.ExFlow = exFlow;

        return View();
    }

    // Unit III: Backend Development Using Asp.Net
    [HttpGet]
    public IActionResult Unit3(string? sampleQuery = "ComputerScience", int? priority = 1)
    {
        ViewData["Title"] = "Unit III: Backend Development Using Asp.Net Core";

        // Unit III: Live Data Passing Showcase
        // 1. ViewData
        ViewData["ViewDataSample"] = "Stored in ViewData dictionary at " + DateTime.Now.ToString("T");

        // 2. ViewBag
        ViewBag.ViewBagSample = "Stored in ViewBag dynamic property bag at " + DateTime.Now.ToString("T");

        // 3. TempData (Read or set)
        if (TempData["TempDataDemo"] == null)
        {
            TempData["TempDataDemo"] = "Flash message initialized in TempData for next redirect!";
        }

        // 4. Session
        string currentSession = HttpContext.Session.GetString("DemoSessionKey") ?? "";
        if (string.IsNullOrEmpty(currentSession))
        {
            currentSession = "Active Session Token [" + Guid.NewGuid().ToString("N")[..6].ToUpperInvariant() + "]";
            HttpContext.Session.SetString("DemoSessionKey", currentSession);
        }
        ViewBag.SessionValue = currentSession;

        // 5. QueryString parameters
        ViewBag.SampleQuery = sampleQuery;
        ViewBag.Priority = priority;

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult TriggerTempDataRedirect()
    {
        TempData["TempDataDemo"] = "SUCCESS! TempData survived this redirect across HTTP requests! (Retrieved at " + DateTime.Now.ToString("T") + ")";
        return RedirectToAction(nameof(Unit3));
    }

    // Unit IV: Asp.Net MVC and Security
    [HttpGet]
    public IActionResult Unit4()
    {
        ViewData["Title"] = "Unit IV: Asp.Net MVC and Security";

        // Unit IV: Dependency Injection Lifetimes Visualizer
        // Direct injection vs Service-injected operations
        ViewBag.DirectTransient = _transientOp.OperationId;
        ViewBag.ServiceTransient = _operationService.TransientOp.OperationId;

        ViewBag.DirectScoped = _scopedOp.OperationId;
        ViewBag.ServiceScoped = _operationService.ScopedOp.OperationId;

        ViewBag.DirectSingleton = _singletonOp.OperationId;
        ViewBag.ServiceSingleton = _operationService.SingletonOp.OperationId;

        // Recent Audit Logs captured by Custom Middleware
        ViewBag.AuditLogs = _repository.GetRecentAuditLogs(10);

        return View();
    }

    // Unit V: SQL Server For Backend Development
    [HttpGet]
    public IActionResult Unit5()
    {
        ViewData["Title"] = "Unit V: SQL Server For Backend Development";
        return View();
    }

    // Unit VI: Asp.Net With Web API
    [HttpGet]
    public IActionResult Unit6()
    {
        ViewData["Title"] = "Unit VI: Asp.Net With Web API";
        return View();
    }
}
