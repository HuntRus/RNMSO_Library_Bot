using RNMSO_Library_Bot.Data.Models;
using System.Globalization;
using System.Text.Json;

namespace RNMSO_Library_Bot.Data;

public static class Config
{
    public static string MainFolderPath => Environment.CurrentDirectory;
    public static string LibraryFolderPath => Path.Combine(MainFolderPath, "Library");
    public static string UsersFolderPath => Path.Combine(MainFolderPath, "Users");
    public static string GroupsFolderPath => Path.Combine(MainFolderPath, "Groups");
 
    private static readonly string _filePath = Path.Combine(MainFolderPath, "config.json");

    public static DateTimeFormatInfo RegionalFormat => CultureInfo.GetCultureInfo("ru-ru").DateTimeFormat;

    public static string Token { get; }

    static Config()
    {
        var file = File.ReadAllText(_filePath);
        var configuration = JsonSerializer.Deserialize<ConfigModel>(file);

        Token = configuration!.Token;
    }
}
