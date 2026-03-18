using RNMSO_Library_Bot.Data.Models;
using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace RNMSO_Library_Bot.Data;

/// <summary>
/// Contains config properties.
/// </summary>
public static class Config
{
    /// <summary>
    /// Options for reading and writing JSON files.
    /// </summary>
    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        WriteIndented = true
    };

    /// <inheritdoc cref="Models.ConfigModel.Token"/>
    public static string Token => Model.Token;

    /// <summary>
    /// Date format. Determines folders structure and bot's behaviour.
    /// </summary>
    public static DateTimeFormatInfo RegionalFormat => CultureInfo.GetCultureInfo(Model.RegionalFormat).DateTimeFormat;

    /// <inheritdoc cref="ConfigModel.MainFolderPath"/>
    public static string MainFolderPath => Model.MainFolderPath;

    /// <inheritdoc cref="ConfigModel.LibraryFolderPath"/>
    public static string LibraryFolderPath => Model.LibraryFolderPath;

    /// <inheritdoc cref="ConfigModel.UsersFolderPath"/>
    public static string UsersFolderPath => Model.UsersFolderPath;

    /// <inheritdoc cref="ConfigModel.GroupsFolderPath"/>
    public static string GroupsFolderPath => Model.GroupsFolderPath;

    /// <summary>
    /// <see cref="ConfigModel"/> deserialized from config file.
    /// </summary>
    private static ConfigModel Model
    {
        get
        {
            if (!File.Exists("config.json"))
                return Generate();

            var file = File.ReadAllText("config.json");
            var config = JsonSerializer.Deserialize<ConfigModel>(file);
            return config is null ? throw new NullReferenceException() : config;
        }
    }

    /// <summary>
    /// Generates default <see cref="ConfigModel"/>.
    /// </summary>
    /// <returns>Generated <see cref="ConfigModel"/>.</returns>
    private static ConfigModel Generate()
    {
        var model = new ConfigModel()
        {
            Token = string.Empty,
            RegionalFormat = "ru-ru",
            MainFolderPath = Environment.CurrentDirectory,
            LibraryFolderPath = Path.Combine(MainFolderPath, "Library"),
            UsersFolderPath = Path.Combine(MainFolderPath, "Users"),
            GroupsFolderPath = Path.Combine(MainFolderPath, "Groups")
        };

        var json = JsonSerializer.Serialize(model, JsonOptions);
        File.WriteAllText("config.json", json);

        return model;
    }
}
