namespace RNMSO_Library_Bot;

public static class Access
{
    private static readonly string _filePath =
        Path.Combine(Environment.GetEnvironmentVariable("RNMSO_LIBRARY_BOT")!, "Users\\Whitelist.txt");

    public static bool IsWhitelisted(string phoneNumber)
    {
        using StreamReader sr = File.OpenText(_filePath);

        string line;
        while ((line = sr.ReadLine()) != null)
        {
            if (line == phoneNumber)
                return true;
        }

        return false;
    }
}
