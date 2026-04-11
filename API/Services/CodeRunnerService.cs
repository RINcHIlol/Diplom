namespace diplom.Services;

using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

public class CodeRunnerService
{
    private const int TimeoutMs = 3000;

    public async Task<string> RunAsync(string userCode)
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "code_runner", Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);

        try
        {
            // csproj
            var csprojPath = Path.Combine(tempDir, "Program.csproj");
            await File.WriteAllTextAsync(csprojPath, GetCsproj());

            // ❌ ВАЖНО: без WrapCode
            var programPath = Path.Combine(tempDir, "Program.cs");
            await File.WriteAllTextAsync(programPath, userCode);

            return await RunProcess(tempDir);
        }
        finally
        {
            try { Directory.Delete(tempDir, true); } catch { }
        }
    }

    private string GetCsproj()
    {
        return @"<Project Sdk=""Microsoft.NET.Sdk"">
<PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
</PropertyGroup>
</Project>";
    }

    private async Task<string> RunProcess(string workingDir)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = "run --no-restore",
            WorkingDirectory = workingDir,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };

        using var process = new Process { StartInfo = psi };

        process.Start();

        var outputTask = process.StandardOutput.ReadToEndAsync();
        var errorTask = process.StandardError.ReadToEndAsync();

        var timeoutTask = Task.Delay(TimeoutMs);
        var completed = await Task.WhenAny(process.WaitForExitAsync(), timeoutTask);

        if (completed == timeoutTask)
        {
            try { process.Kill(); } catch { }
            return "TIMEOUT";
        }

        var output = await outputTask;
        var error = await errorTask;

        if (!string.IsNullOrWhiteSpace(error))
            return $"COMPILATION_ERROR:\n{error}";

        return ExtractLastLine(output);
    }
    
    private string ExtractLastLine(string output)
    {
        var lines = output
            .Replace("\r", "")
            .Split('\n', StringSplitOptions.RemoveEmptyEntries);

        return lines.LastOrDefault()?.Trim() ?? "";
    }
}