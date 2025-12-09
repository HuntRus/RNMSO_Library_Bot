namespace RNMSO_Library_Bot;

internal static class Configuration
{
    internal static string Token { get; }

    static Configuration()
    {
        var specialFolder = Environment.SpecialFolder.LocalApplicationData;
        var folderPath = Path.Combine(Environment.GetFolderPath(specialFolder), "RNMSO_Library_Bot");
        var configPath = Path.Combine(folderPath, "config.txt");
        if (!Directory.Exists(folderPath) || !File.Exists(configPath))
        {
            Directory.CreateDirectory(folderPath);
            File.Create(configPath);
        }

        Token = File.ReadAllText(configPath);
    }
}
