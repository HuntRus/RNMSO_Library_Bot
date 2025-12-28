using System.Globalization;
using System.Text.Json;

namespace RNMSO_Library_Bot;

public static class Configuration
{
    private static readonly string _filePath =
        Path.Combine(Environment.GetEnvironmentVariable("RNMSO_LIBRARY_BOT"), "config.json");

    public static DateTimeFormatInfo RegionalFormat => CultureInfo.GetCultureInfo("ru-ru").DateTimeFormat;
    public static string LibraryFolderPath => Path.Combine(Environment.GetEnvironmentVariable("RNMSO_LIBRARY_BOT")!, "Library");

    public static string Token { get; }

    static Configuration()
    {
        var file = File.ReadAllText(_filePath);
        var configuration = JsonSerializer.Deserialize<Model>(file);

        Token = configuration!.Token;
    }

    public class Model
    {
        public required string Token { get; set; }
    }
}
