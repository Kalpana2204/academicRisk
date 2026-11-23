using System;

namespace AcademicRiskRadar.Web.Models;

/// <summary>
/// Unit II: Object-Oriented Programming Concepts - Encapsulation & Abstraction
/// Abstract base class defining common person properties and abstract contract.
/// </summary>
public abstract class Person
{
    // Encapsulation: Private fields with public properties
    private string _id = string.Empty;
    private string _name = string.Empty;
    private string _email = string.Empty;

    public string Id
    {
        get => _id;
        set => _id = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("ID cannot be empty", nameof(value)) : value.Trim();
    }

    public string Name
    {
        get => _name;
        set => _name = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Name cannot be empty", nameof(value)) : value.Trim();
    }

    public string Email
    {
        get => _email;
        set => _email = string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim().ToLowerInvariant();
    }

    public string PhoneNumber { get; set; } = string.Empty;

    // Unit II: Constructors (Default and Parameterized)
    protected Person()
    {
    }

    protected Person(string id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;
    }

    // Unit II: Abstract Method (Must be implemented by derived classes)
    public abstract string GetRoleDescription();

    // Unit II: Virtual Method (Can be overridden by derived classes - Polymorphism)
    public virtual string GetSummary()
    {
        return $"{Name} ({GetRoleDescription()}) - {Email}";
    }
}
