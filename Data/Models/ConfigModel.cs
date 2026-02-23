namespace RNMSO_Library_Bot.Data.Models;

/// <summary>
/// Represents <c>Config</c> object. Primarily used for <c>JSON</c> (de-)serialization.
/// </summary>
public class ConfigModel
{
    /// <summary>
    /// Bot token.
    /// </summary>
    public required string Token { get; set; }

    /// <summary>
    /// Date format name.
    /// </summary>
    public required string RegionalFormat { get; set; }

    /// <summary>
    /// Path to main folder.
    /// </summary>
    public required string MainFolderPath { get; set; }

    /// <summary>
    /// Path to notes folder.
    /// </summary>
    public required string LibraryFolderPath { get; set; }

    /// <summary>
    /// Path to users folder.
    /// </summary>
    public required string UsersFolderPath { get; set; }

    /// <summary>
    /// Path to groups folder.
    /// </summary>
    public required string GroupsFolderPath { get; set; }
}