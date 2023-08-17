using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

Console.WriteLine("Enter path to a directory:");
var directoryPath = Console.ReadLine();

Console.WriteLine("Enter path to a word file:");
var wordsFilePath = Console.ReadLine();

Console.WriteLine("Case sensitive (Y/N)");
var caseSensitive = Console.ReadLine().Equals("Y", StringComparison.OrdinalIgnoreCase);

var fileWordCount = new Dictionary<string, Dictionary<string, int>>();

try
{
    var words = File.ReadAllLines(wordsFilePath);

    foreach (var filePath in Directory.GetFiles(directoryPath))
    {
        if (filePath == wordsFilePath)
            continue;

        var fileContent = File.ReadAllText(filePath);
        var wordCount = new Dictionary<string, int>();

        foreach (var word in words)
        {
            var pattern = caseSensitive ? $@"\b{word}\b" : $@"\b{word}\b";
            var count = Regex.Matches(fileContent, pattern, RegexOptions.IgnoreCase).Count;

            if (count == 0)
                continue;

            if (wordCount.ContainsKey(word))
                wordCount[word] += count;
            else
                wordCount[word] = count;
        }

        fileWordCount[filePath] = wordCount;
    }

    Console.WriteLine("Statistics:");

    foreach (var (filePath, wordCount) in fileWordCount)
    {
        if(wordCount.Count <= 0) 
            continue;
        Console.WriteLine($"File: {filePath}");

        foreach (var (word, count) in wordCount)
        {
            Console.WriteLine($"Word: {word}, Count: {count}");
        }

        Console.WriteLine();
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error occured: {ex.Message}");
}

Console.ReadLine();
