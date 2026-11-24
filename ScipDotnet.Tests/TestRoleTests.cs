using Scip;
using Document = Scip.Document;
using Index = Scip.Index;

namespace ScipDotnet.Tests;

/// <summary>
/// Checks that SymbolRole.Test marks exactly the occurrences in files that only test projects compile.
/// Most tests use the snapshots/input/test-projects solution. It has a library, a multi-targeted C# project
/// that uses Microsoft.NET.Test.Sdk (which sets IsTestProject only in its per-framework inner builds),
/// and a Visual Basic project that sets IsTestingPlatformApplication.
/// </summary>
[TestFixture]
public class TestRoleTests
{
    private const int DefinitionRole = (int)SymbolRole.Definition;
    private const int TestRole = (int)SymbolRole.Test;

    private Index _index = null!;

    [OneTimeSetUp]
    public void IndexTestProjects()
    {
        var directory = Path.Join(SnapshotTests.RootDirectory(), "snapshots", "input", "test-projects");
        _index = Index.Parser.ParseFrom(File.ReadAllBytes(SnapshotTests.IndexDirectory(directory)));
    }

    [Test]
    public void TestMethodAndHelperDefinitionsHaveTestRole()
    {
        Assert.That(Definition("Library.Tests/CalculatorTests.cs", "/CalculatorTests#AddsTwoNumbers().").SymbolRoles,
            Is.EqualTo(DefinitionRole | TestRole));
        Assert.That(Definition("Library.Tests/CalculatorTests.cs", "/CalculatorTests#AssertEqual().").SymbolRoles,
            Is.EqualTo(DefinitionRole | TestRole));
        Assert.That(Definition("Library.Tests/TestData.cs", "/TestData#Three.").SymbolRoles,
            Is.EqualTo(DefinitionRole | TestRole));
        Assert.That(Definition("Library.VBTests/CalculatorChecks.vb", "/CalculatorChecks#AddsTwoNumbers().").SymbolRoles,
            Is.EqualTo(DefinitionRole | TestRole));
    }

    [TestCase("Library.Tests/CalculatorTests.cs")]
    [TestCase("Library.VBTests/CalculatorChecks.vb")]
    public void ReferenceFromTestCodeToLibraryHasTestRole(string path)
    {
        var references = Occurrences(path).Where(occurrence => occurrence.Symbol.EndsWith("/Calculator#Add().")).ToList();
        Assert.That(references.Select(occurrence => occurrence.SymbolRoles), Is.EqualTo(new[] { TestRole }));
    }

    [TestCase("Library.Tests/CalculatorTests.cs")]
    [TestCase("Library.Tests/TestData.cs")]
    [TestCase("Library.VBTests/CalculatorChecks.vb")]
    public void EveryOccurrenceInTestProjectHasTestRole(string path)
    {
        Assert.That(Occurrences(path), Is.Not.Empty);
        Assert.That(Occurrences(path).Select(occurrence => occurrence.SymbolRoles & TestRole), Has.All.EqualTo(TestRole));
    }

    [Test]
    public void LibraryCodeHasNoTestRole()
    {
        Assert.That(Definition("Library/Calculator.cs", "/Calculator#Add().").SymbolRoles, Is.EqualTo(DefinitionRole));
        foreach (var path in new[] { "Library/Calculator.cs", "Library/Guard.cs" })
        {
            Assert.That(Occurrences(path), Is.Not.Empty, path);
            Assert.That(Occurrences(path).Select(occurrence => occurrence.SymbolRoles & TestRole), Has.All.Zero, path);
        }

        // Library.Tests links Guard.cs, so the file is indexed once per project. Neither copy is test code,
        // because a file is test code only when every project that compiles it is a test project.
        Assert.That(Documents("Library/Guard.cs"), Has.Count.EqualTo(2));
    }

    // MSBuild defines $(SolutionDir) only when it loads a solution, and some projects import files through it.
    // Here the imported file makes the project a test project.
    [Test]
    public void ProjectThatImportsThroughSolutionDirIsTestProject()
    {
        var directory = Path.Join(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(Path.Join(directory, "Checks"));
        try
        {
            File.WriteAllText(Path.Join(directory, "TestProject.props"), """
                <Project>
                  <PropertyGroup>
                    <IsTestProject>true</IsTestProject>
                  </PropertyGroup>
                </Project>
                """);
            File.WriteAllText(Path.Join(directory, "Checks", "Checks.csproj"), $"""
                <Project Sdk="Microsoft.NET.Sdk">
                  <Import Project="$(SolutionDir)TestProject.props" />
                  <PropertyGroup>
                    <TargetFramework>net{Environment.Version.Major}.0</TargetFramework>
                  </PropertyGroup>
                </Project>
                """);
            File.WriteAllText(Path.Join(directory, "Checks", "Check.cs"), "public class Check { public void Run() { } }");
            File.WriteAllText(Path.Join(directory, "Checks.slnx"), """<Solution><Project Path="Checks/Checks.csproj" /></Solution>""");

            var index = Index.Parser.ParseFrom(File.ReadAllBytes(SnapshotTests.IndexDirectory(directory)));
            var occurrences = index.Documents.Single(document => document.RelativePath.Replace('\\', '/') == "Checks/Check.cs").Occurrences;
            Assert.That(occurrences, Is.Not.Empty);
            Assert.That(occurrences.Select(occurrence => occurrence.SymbolRoles & TestRole), Has.All.EqualTo(TestRole));
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    private List<Document> Documents(string path) =>
        _index.Documents.Where(document => document.RelativePath.Replace('\\', '/') == path).ToList();

    private List<Occurrence> Occurrences(string path) =>
        Documents(path).SelectMany(document => document.Occurrences).ToList();

    private Occurrence Definition(string path, string symbolSuffix) =>
        Occurrences(path).Single(occurrence => occurrence.Symbol.EndsWith(symbolSuffix)
                                               && (occurrence.SymbolRoles & DefinitionRole) != 0);
}
