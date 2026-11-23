using System;

namespace AcademicRiskRadar.Web.Services;

/// <summary>
/// Unit IV: Dependency Injection - Service Lifetime Contracts
/// Transient: New instance created every time requested
/// Scoped: Created once per client request (HTTP request)
/// Singleton: Created once on first request and shared application-wide
/// </summary>
public interface IOperation
{
    Guid OperationId { get; }
    string LifetimeName { get; }
    DateTime CreatedAt { get; }
}

public interface IOperationTransient : IOperation { }
public interface IOperationScoped : IOperation { }
public interface IOperationSingleton : IOperation { }

public class OperationInstance : IOperationTransient, IOperationScoped, IOperationSingleton
{
    public Guid OperationId { get; } = Guid.NewGuid();
    public string LifetimeName { get; }
    public DateTime CreatedAt { get; } = DateTime.UtcNow;

    public OperationInstance(string lifetimeName)
    {
        LifetimeName = lifetimeName;
    }
}

public class OperationTransient : OperationInstance, IOperationTransient
{
    public OperationTransient() : base("Transient (New per injection)") { }
}

public class OperationScoped : OperationInstance, IOperationScoped
{
    public OperationScoped() : base("Scoped (One per HTTP Request)") { }
}

public class OperationSingleton : OperationInstance, IOperationSingleton
{
    public OperationSingleton() : base("Singleton (Shared Application-Wide)") { }
}

/// <summary>
/// Service that depends on all 3 lifetimes to demonstrate resolution behavior
/// </summary>
public interface IOperationService
{
    IOperationTransient TransientOp { get; }
    IOperationScoped ScopedOp { get; }
    IOperationSingleton SingletonOp { get; }
}

public class OperationService : IOperationService
{
    public IOperationTransient TransientOp { get; }
    public IOperationScoped ScopedOp { get; }
    public IOperationSingleton SingletonOp { get; }

    public OperationService(
        IOperationTransient transientOp,
        IOperationScoped scopedOp,
        IOperationSingleton singletonOp)
    {
        TransientOp = transientOp;
        ScopedOp = scopedOp;
        SingletonOp = singletonOp;
    }
}
