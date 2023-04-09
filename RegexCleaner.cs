using System.Text.RegularExpressions;

namespace RegexCleaner;

public class RegexCleaner
{
    private readonly string _filePath;
    private readonly string _pattern;
    
    private const string DefaultRegex = @"<target\b[^>]*>([\s\S]*?)<\/target>";

    public RegexCleaner(string[] args)
    {
        if (args.Length == 0)
        {
            PromptUserForArgs(out _filePath, out _pattern);
        }
        else
        {
            _filePath = args[0];
            _pattern = args[1];
        }

        if (!File.Exists(_filePath))
        {
            Console.WriteLine("Файл не найден.");
            Environment.Exit(0);
        }
    }

    public void Run()
    {
        string text = File.ReadAllText(_filePath);
        MatchCollection matches = GetMatches(text, _pattern);
        string result = GetMatchedText(matches);

        Console.WriteLine("Вот результат фильтрации текста по регулярному выражению:\n");
        Console.WriteLine(result);
        Console.WriteLine("Хотите ли вы записать результат в файл? (y/n)");

        string answer = Console.ReadLine()?.ToLower();

        const string positiveAnswer = "y";

        if (answer == positiveAnswer)
        {
            WriteResultToFile(_filePath, result);
        }
    }

    private void PromptUserForArgs(out string filePath, out string pattern)
    {
        Console.WriteLine("Пожалуйста, укажите регулярное выражение и файл в аргументах запуска.");
        Console.Write("Введите путь к файлу:");
        filePath = Console.ReadLine();
        Console.Write($"Введите регулярное выражение, либо нажмите Enter, чтобы оставить по умолчанию ({DefaultRegex})");
        string userInput = Console.ReadLine();

        if (string.IsNullOrEmpty(userInput))
        {
            pattern = DefaultRegex;
        }
        else
        {
            pattern = userInput;
        }
    }

    private MatchCollection GetMatches(string text, string pattern)
    {
        Regex regex = new Regex(pattern);
        MatchCollection matches = regex.Matches(text);
        return matches;
    }

    private string GetMatchedText(MatchCollection matches)
    {
        string result = "";

        foreach (Match match in matches)
        {
            result += match.Groups[1].Value + "\n";
        }

        return result;
    }

    private void WriteResultToFile(string filePath, string result)
    {
        string newFileName = Path.GetFileNameWithoutExtension(filePath) + "_regexApplied";
        string extension = Path.GetExtension(filePath);
        string newFilePath = Path.Combine(Path.GetDirectoryName(filePath), newFileName + extension);
        File.WriteAllText(newFilePath, result);
        Console.WriteLine("Файл сохранен.");
    }
}