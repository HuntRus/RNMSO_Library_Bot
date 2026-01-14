namespace RNMSO_Library_Bot.Data.Models;

public class GroupModel
{
    public required string Name { get; set; }
    public List<string>? Aliases { get; set; }
    public List<string>? Members { get; set; }
}