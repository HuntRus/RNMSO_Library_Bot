namespace RNMSO_Library_Bot.Data.Models;

/// <summary>
/// Represents <c>User</c> object. Primarily used for <c>JSON</c> (de-)serialization.
/// </summary>
public class UserModel
{
    /// <summary>
    /// User's id in Telegram.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// User's phone number.
    /// </summary>
    public required string PhoneNumber { get; set; }

    /// <summary>
    /// User's group name.
    /// </summary>
    public required string Group { get; set; }

    /// <summary>
    /// User's last message.
    /// </summary>
    public string LastMessage { get; set; } = string.Empty;
}