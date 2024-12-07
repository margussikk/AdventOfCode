using Microsoft.Extensions.Configuration;
using System.Net;
using System.Reflection;

namespace AdventOfCode.Framework.Puzzle;

public sealed class PuzzleInputProvider
{
    public static PuzzleInputProvider Instance { get; } = new();

    private readonly HttpClient _httpClient;

    private PuzzleInputProvider()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .AddUserSecrets(typeof(Program).Assembly)
            .Build();
        var session = configuration["session"];

#pragma warning disable S1075 // URIs should not be hardcoded
        var baseAddress = new Uri("https://adventofcode.com");
#pragma warning restore S1075 // URIs should not be hardcoded
        var cookieContainer = new CookieContainer();
        cookieContainer.Add(baseAddress, new Cookie("session", session));

        _httpClient = new HttpClient(
            new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                AutomaticDecompression = DecompressionMethods.All
            })
        {
            BaseAddress = baseAddress,
            DefaultRequestHeaders =
            {
                { "User-Agent", ".NET/8.0 (https://github.com/margussikk/AdventOfCode)" }
            }
        };
    }

    public string[] GetInputLines(Type solverType)
    {
        var puzzleAttribute = solverType.GetCustomAttribute<PuzzleAttribute>() ?? throw new InvalidOperationException("Puzzle attribute not found");

        var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var inputFile = Path.Combine(assemblyPath!, "Inputs", $"{puzzleAttribute.Year}", $"Day{puzzleAttribute.Day:00}.txt");
        Directory.CreateDirectory(Path.GetDirectoryName(inputFile)!);
        if (File.Exists(inputFile)) return File.ReadAllLines(inputFile);

        try
        {
            var response = _httpClient.GetAsync($"{puzzleAttribute.Year}/day/{puzzleAttribute.Day}/input")
                .GetAwaiter()
                .GetResult();

            var text = response
                .EnsureSuccessStatusCode()
                .Content.ReadAsStringAsync()
                .GetAwaiter()
                .GetResult();
            File.WriteAllText(inputFile, text);
        }
        catch
        {
            Console.WriteLine("Failed to download puzzle input, make sure you have set the correct session cookie value");
            throw;
        }

        return File.ReadAllLines(inputFile);
    }
}
