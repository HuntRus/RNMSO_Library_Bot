namespace RNMSO_Library_Bot.Data.Models;

/// <summary>
/// Represents <c>Group</c> object. Primarily used for <c>JSON</c> (de-)serialization.
/// </summary>
public class GroupModel
{
    /// <summary>
    /// Name of group.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// List of synonyms for file searching.
    /// </summary>
    public List<string> Aliases { get; set; } = [];

    /// <summary>
    /// List of users phone numbers in group.
    /// </summary>
    public List<string> Members { get; set; } = [];
}