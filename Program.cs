namespace RegexCleaner
{
    public class Program
    {
        static void Main(string[] args)
        {
            RegexCleaner regexCleaner = new RegexCleaner(args);
            regexCleaner.Run();
        }
    }
}
