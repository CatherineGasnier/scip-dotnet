using Microsoft.Build.Exceptions;
using Microsoft.Build.Execution;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Extensions.Logging;
using RoslynProject = Microsoft.CodeAnalysis.Project;

namespace ScipDotnet;

/// <summary>
/// Decides which projects are test projects, using the MSBuild properties that the .NET SDK uses.
/// </summary>
/// <remarks>
/// <c>dotnet test</c> runs VSTest projects that set <c>IsTestProject</c> to true (Microsoft.NET.Test.Sdk sets it),
/// and Microsoft.Testing.Platform projects that set <c>IsTestingPlatformApplication</c> to true
/// (MSTest.Sdk and xunit.v3 set only this one). Test frameworks set these properties in imported
/// .props and .targets files, so they are read from an MSBuild evaluation of the project file.
/// </remarks>
public sealed class TestProjectClassifier
{
    // The condition that the .NET 9 SDK's _GetTestsProject target uses to select test projects.
    private const string IsTestProjectCondition =
        "'$(IsTestProject)' == 'true' or '$(IsTestingPlatformApplication)' == 'true'";

    private readonly MSBuildWorkspace _workspace;
    private readonly ILogger<TestProjectClassifier> _logger;
    private readonly Dictionary<string, bool> _isTestProject = new();

    public TestProjectClassifier(MSBuildWorkspace workspace, ILogger<TestProjectClassifier> logger)
    {
        _workspace = workspace;
        _logger = logger;
    }

    /// <summary>
    /// Returns true when MSBuild marks the project file as a test project.
    /// Returns false, and logs a warning, when MSBuild cannot evaluate the project file.
    /// </summary>
    public bool IsTestProject(RoslynProject project)
    {
        if (project.FilePath == null)
        {
            return false;
        }

        if (!_isTestProject.TryGetValue(project.FilePath, out var isTestProject))
        {
            isTestProject = EvaluateIsTestProject(project.FilePath);
            _isTestProject[project.FilePath] = isTestProject;
            if (isTestProject)
            {
                _logger.LogDebug("{ProjectFile} is a test project", project.FilePath);
            }
        }

        return isTestProject;
    }

    private bool EvaluateIsTestProject(string projectFile)
    {
        try
        {
            var project = Evaluate(projectFile, targetFramework: null);
            // A multi-targeted project's outer build does not import the build/ files of its packages,
            // where Microsoft.NET.Test.Sdk sets IsTestProject. Its per-framework inner builds do,
            // and `dotnet test` runs those.
            if (project.EvaluateCondition("'$(IsCrossTargetingBuild)' == 'true'")
                && FirstTargetFramework(project) is { } targetFramework)
            {
                project = Evaluate(projectFile, targetFramework);
            }

            return project.EvaluateCondition(IsTestProjectCondition);
        }
        catch (InvalidProjectFileException e)
        {
            _logger.LogWarning(
                "Treating {ProjectFile} as a non-test project because MSBuild could not evaluate it: {Message}",
                projectFile, e.Message);
            return false;
        }
    }

    /// <summary>
    /// Evaluates a project file, optionally for one target framework, with the global properties that
    /// MSBuildWorkspace used to load it. For example, those include SolutionDir when a solution was opened.
    /// </summary>
    private ProjectInstance Evaluate(string projectFile, string? targetFramework)
    {
        var globalProperties = new Dictionary<string, string>(_workspace.Properties);
        if (targetFramework != null)
        {
            globalProperties["TargetFramework"] = targetFramework;
        }

        return new ProjectInstance(projectFile, globalProperties, toolsVersion: null);
    }

    private static string? FirstTargetFramework(ProjectInstance project) =>
        project.GetPropertyValue("TargetFrameworks")
            .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .FirstOrDefault();
}
