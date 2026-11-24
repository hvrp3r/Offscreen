using Offscreen.Infrastructure.Configuration;

namespace Offscreen.Infrastructure.Tests.Configuration;

public sealed class DotEnvTests
{
    [Fact]
    public void Parse_ReadsKeysAndValues()
    {
        var values = DotEnv.Parse(["OFFSCREEN_SERVER__PORT=9000", "  OFFSCREEN_NAME = Living room  "]);

        Assert.Equal("9000", values["OFFSCREEN_SERVER__PORT"]);
        Assert.Equal("Living room", values["OFFSCREEN_NAME"]);
    }

    [Fact]
    public void Parse_SkipsBlankLinesAndComments()
    {
        var values = DotEnv.Parse(["", "# comment", "   ", "KEY=value"]);

        Assert.Single(values);
    }

    [Theory]
    [InlineData("KEY=\"quoted value\"", "quoted value")]
    [InlineData("KEY='quoted value'", "quoted value")]
    [InlineData("KEY=\"mismatched'", "\"mismatched'")]
    [InlineData("KEY=a=b", "a=b")]
    [InlineData("KEY=", "")]
    public void Parse_HandlesValueFormats(string line, string expected)
    {
        Assert.Equal(expected, DotEnv.Parse([line])["KEY"]);
    }

    [Fact]
    public void Parse_ReportsTheMalformedLine()
    {
        var error = Assert.Throws<FormatException>(() => DotEnv.Parse(["KEY=value", "", "not a pair"]));

        Assert.Contains("line 3", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Load_KeepsExistingEnvironmentVariables()
    {
        var existing = $"OFFSCREEN_TEST_{Guid.NewGuid():N}";
        var added = $"OFFSCREEN_TEST_{Guid.NewGuid():N}";
        var path = Path.GetTempFileName();
        try
        {
            Environment.SetEnvironmentVariable(existing, "from environment");
            File.WriteAllLines(path, [$"{existing}=from file", $"{added}=from file"]);

            DotEnv.Load(path);

            Assert.Equal("from environment", Environment.GetEnvironmentVariable(existing));
            Assert.Equal("from file", Environment.GetEnvironmentVariable(added));
        }
        finally
        {
            Environment.SetEnvironmentVariable(existing, null);
            Environment.SetEnvironmentVariable(added, null);
            File.Delete(path);
        }
    }

    [Fact]
    public void Find_ReturnsTheClosestFileInParentDirectories()
    {
        var root = Directory.CreateTempSubdirectory("offscreen-");
        try
        {
            var nested = root.CreateSubdirectory("core").CreateSubdirectory("src");
            var expected = Path.Combine(root.FullName, "test.env");
            File.WriteAllText(expected, "KEY=value");

            Assert.Equal(expected, DotEnv.Find(nested.FullName, "test.env"));
        }
        finally
        {
            root.Delete(recursive: true);
        }
    }

    [Fact]
    public void Find_ReturnsNullWhenNoFileExists()
    {
        Assert.Null(DotEnv.Find(Path.GetTempPath(), $"{Guid.NewGuid():N}.env"));
    }

    [Fact]
    public void Load_IgnoresMissingFile()
    {
        DotEnv.Load(Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.env"));
    }
}
