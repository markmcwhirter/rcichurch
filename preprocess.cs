#!/usr/bin/env dotnet

using System.Text;

var baseDirectory = Directory.GetCurrentDirectory();

var inputDirectory = Path.Combine(baseDirectory, "input");
var outputDirectory = Path.Combine(baseDirectory, "output");

var headerPath = Path.Combine(baseDirectory, "header.html");
var footerPath = Path.Combine(baseDirectory, "footer.html");

if (!Directory.Exists(inputDirectory))
{
    Console.WriteLine($"Input directory does not exist: {inputDirectory}");
    return;
}

if (!File.Exists(headerPath))
{
    Console.WriteLine($"Header file does not exist: {headerPath}");
    return;
}

if (!File.Exists(footerPath))
{
    Console.WriteLine($"Footer file does not exist: {footerPath}");
    return;
}

Directory.CreateDirectory(outputDirectory);

var header = await File.ReadAllTextAsync(headerPath, Encoding.UTF8);
var footer = await File.ReadAllTextAsync(footerPath, Encoding.UTF8);

var files = Directory.GetFiles(inputDirectory, "*.*", SearchOption.TopDirectoryOnly);

foreach (var inputFilePath in files)
{
    var fileName = Path.GetFileName(inputFilePath);

    if (fileName.Equals("header.html", StringComparison.OrdinalIgnoreCase) ||
        fileName.Equals("footer.html", StringComparison.OrdinalIgnoreCase))
    {
        continue;
    }

    var pageContent = await File.ReadAllTextAsync(inputFilePath, Encoding.UTF8);

    var outputFilePath = Path.Combine(outputDirectory, fileName);

    var finalContent =
        header +
        Environment.NewLine +
        pageContent +
        Environment.NewLine +
        footer;

    await File.WriteAllTextAsync(outputFilePath, finalContent, Encoding.UTF8);

    Console.WriteLine($"Created: {outputFilePath}");
}

Console.WriteLine("Processing complete.");