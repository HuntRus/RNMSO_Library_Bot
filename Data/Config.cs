using RNMSO_Library_Bot.Data.Models;
using System.Globalization;
using System.Text.Json;

namespace RNMSO_Library_Bot.Data;

public static class Config
{
    private static readonly string _filePath =
        Path.Combine(Environment.GetEnvironmentVariable("RNMSO_LIBRARY_BOT"), "config.json");

    public static string LibraryFolderPath => Path.Combine(Environment.GetEnvironmentVariable("RNMSO_LIBRARY_BOT"), "Library");
    public static string UsersFolderPath => Path.Combine(Environment.GetEnvironmentVariable("RNMSO_LIBRARY_BOT"), "Users");
    public static string GroupsFolderPath => Path.Combine(Environment.GetEnvironmentVariable("RNMSO_LIBRARY_BOT"), "Groups");

    public static DateTimeFormatInfo RegionalFormat => CultureInfo.GetCultureInfo("ru-ru").DateTimeFormat;

    public static string Token { get; }

    static Config()
    {
        var file = File.ReadAllText(_filePath);
        var configuration = JsonSerializer.Deserialize<ConfigModel>(file);

        Token = configuration!.Token;
    }
}
