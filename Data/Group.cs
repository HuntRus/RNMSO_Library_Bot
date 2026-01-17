using RNMSO_Library_Bot.Data.Models;
using System.Text.Json;

namespace RNMSO_Library_Bot.Data;

/// <summary>
/// Contains methods for groups management.
/// </summary>
public static class Group
{
    /// <summary>
    /// Searches for existing groups and adds them to a list.
    /// </summary>
    /// <returns>List of <see cref="Models.GroupModel"/> if exists or <c>null</c> if doesn't.</returns>
    public static List<GroupModel>? GetAll()
    {
        var path = Config.GroupsFolderPath;
        var list = new List<GroupModel>();

        var source = Directory.GetFiles(path);
        foreach (var file in source)
        {
            var data = File.ReadAllText(file);
            var group = JsonSerializer.Deserialize<GroupModel>(data);
            if (group is null)
                continue;

            list.Add(group);
        }

        return list;
    }

    /// <summary>
    /// Finds corresponding <see cref="Models.GroupModel"/> by <paramref name="name"/>.
    /// </summary>
    /// <param name="name">Group name.</param>
    /// <returns>A <see cref="Models.GroupModel"/> if group was found or <c>null</c> if wasn't.</returns>
    public static GroupModel? FindByName(string name)
        => GetAll()?.Find(x => x.Name == name);

    /// <summary>
    /// Determines <see cref="Models.GroupModel"/> of file by <paramref name="alias"/>.
    /// </summary>
    /// <param name="alias">File name.</param>
    /// <returns>A <see cref="Models.GroupModel"/> if group was determined or <c>null</c> if wasn't.</returns>
    public static GroupModel? FindByAlias(string alias)
    {
        var list = GetAll();
        if (list is null)
            return null;

        foreach (var group in list)
        {
            if (group.Aliases.Count is 0)
                continue;
            else if (group.Aliases.Contains(alias))
                return group;
        }

        return null;
    }

    /// <summary>
    /// Adds group aliases to <see cref="GroupModel.Aliases"/>.
    /// </summary>
    /// <param name="groupName">Name of group.</param>
    /// <param name="aliases">Array of aliases.</param>
    /// <returns>A <see cref="GroupModel"/> if aliases were added or <c>null</c> if weren't.</returns>
    public static GroupModel? AddAliases(string groupName, string[] aliases)
    {
        var group = FindByName(groupName);
        if (group is null)
            return null;

        foreach (var alias in aliases)
            group.Aliases.Add(alias);

        return Save(group);
    }

    /// <summary>
    /// Removes group aliases from <see cref="GroupModel.Aliases"/>.
    /// </summary>
    /// <param name="groupName">Name of group.</param>
    /// <param name="aliases">Array of aliases.</param>
    /// <returns>A <see cref="GroupModel"/> if aliases were removed or <c>null</c> if weren't.</returns>
    public static GroupModel? RemoveAliases(string groupName, string[] aliases)
    {
        var group = FindByName(groupName);
        if (group is null)
            return null;

        foreach (var alias in aliases)
            group.Aliases.Remove(alias);

        return Save(group);
    }

    /// <summary>
    /// Adds users phone numbers to <see cref="GroupModel.Members"/>.
    /// </summary>
    /// <param name="groupName">Name of group.</param>
    /// <param name="members">Array of users phone numbers.</param>
    /// <returns>A <see cref="GroupModel"/> if members were added or <c>null</c> if weren't.</returns>
    public static GroupModel? AddMembers(string groupName, string[] members)
    {
        var group = FindByName(groupName);
        if (group is null)
            return null;

        foreach (var member in members)
            group.Members.Add(member);

        return Save(group);
    }

    /// <summary>
    /// Removes users phone numbers from <see cref="GroupModel.Members"/>.
    /// </summary>
    /// <param name="groupName">Name of group.</param>
    /// <param name="members">Array of users phone numbers.</param>
    /// <returns>A <see cref="GroupModel"/> if members were removed or <c>null</c> if weren't.</returns>
    public static GroupModel? RemoveMembers(string groupName, string[] members)
    {
        var group = FindByName(groupName);
        if (group is null)
            return null;

        foreach (var member in members)
            group.Members.Remove(member);

        return Save(group);
    }

    /// <summary>
    /// Saves new or updated <see cref="Models.GroupModel"/>.
    /// </summary>
    /// <param name="group">New or updated <see cref="Models.GroupModel"/>.</param>
    /// <returns>New or updated <see cref="Models.GroupModel"/>.</returns>
    private static GroupModel Save(GroupModel group)
    {
        var jsonData = JsonSerializer.Serialize(group);
        File.WriteAllText($"{Config.GroupsFolderPath}\\{group.Name}.json", jsonData);
        return group;
    }
}