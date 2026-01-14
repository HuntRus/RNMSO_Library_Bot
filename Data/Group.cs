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
    /// Finds corresponding <see cref="Models.GroupModel"/> by name.
    /// </summary>
    /// <param name="name">Group name.</param>
    /// <returns>A <see cref="Models.GroupModel"/> if group was found or <c>null</c> if wasn't.</returns>
    public static GroupModel? FindByName(string name)
    {
        var list = GetAll();
        if (list is null)
            return null;

        foreach (var group in list)
        {
            if (group.Name == name)
                return group;
        }

        return null;
    }

    /// <summary>
    /// Determines group of file by <paramref name="alias"/>.
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
            if (group.Aliases is null)
                continue;
            else if (group.Aliases.Contains(alias))
                return group;
        }

        return null;
    }
}