using FluentAssertions;
using TranslationManagement.Dal.Model;
using Xunit;

namespace TranslationManagement.Api.Tests.Entity;

/// <summary>
/// The EF core requires parameterless ctors, but for testing purposes is the best use custom constructors,
/// to ensure nothing get's messed up in any entity run this test
/// </summary>
public class EntityTests
{
    [Fact]
    public void AllEntitiesHaveParameterlessConstructor()
    {
        var entitiesNamespace = typeof(JobStatus).Namespace;
        var entities = typeof(JobStatus)
            .Assembly
            .GetTypes()
            .Where(t => t.IsClass && t.Namespace?.StartsWith(entitiesNamespace) == true)
            .ToList();

        entities.All(t => t
                .GetConstructors()
                .Any(c =>
                    c.IsPublic && c.GetParameters()?.Any() != true)
            )
            .Should()
            .BeTrue();
    }
}