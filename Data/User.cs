using RNMSO_Library_Bot.Data.Models;
using System.Text.Json;

namespace RNMSO_Library_Bot.Data;

/// <summary>
/// Contains methods for users management.
/// </summary>
public static class User
{
    /// <summary>
    /// Searches for existing users and adds them to a list.
    /// </summary>
    /// <returns>List of <see cref="Models.UserModel"/> if exists or <c>null</c> if doesn't.</returns>
    public static List<UserModel>? GetAll()
    {
        var path = Config.UsersFolderPath;
        var list = new List<UserModel>();

        var source = Directory.GetFiles(path);
        foreach (var file in source)
        {
            var data = File.ReadAllText(file);
            var user = JsonSerializer.Deserialize<UserModel>(data);
            if (user is null)
                continue;

            list.Add(user);
        }

        return list;
    }

    /// <summary>
    /// Finds corresponding <see cref="Models.UserModel"/> by <paramref name="id"/>.
    /// </summary>
    /// <param name="id">User ID in Telegram.</param>
    /// <returns>A <see cref="Models.UserModel"/> if user was found or <c>null</c> if wasn't.</returns>
    public static UserModel? Find(long id)
        => GetAll()?.Find(x => x.Id == id);

    /// <summary>
    /// Finds corresponding <see cref="Models.UserModel"/> by <paramref name="phoneNumber"/>.
    /// </summary>
    /// <param name="id">User ID in Telegram.</param>
    /// <returns>A <see cref="Models.UserModel"/> if user was found or <c>null</c> if wasn't.</returns>
    public static UserModel? Find(string phoneNumber)
        => GetAll()?.Find(x => x.PhoneNumber == phoneNumber);

    /// <summary>
    /// Creates new <see cref="Models.UserModel"/>.
    /// </summary>
    /// <param name="phoneNumber">User's phone number.</param>
    /// <param name="group">User's group name.</param>
    /// <param name="id">User's id in Telegram.</param>
    /// <returns>Created <see cref="Models.UserModel"/>.</returns>
    public static UserModel Create(string phoneNumber, string group, long id = 0)
    {
        Group.AddMembers(group, [phoneNumber]);
        var user = new UserModel() { Id = id, PhoneNumber = phoneNumber, Group = group };
        return Save(user);
    }

    /// <summary>
    /// Changes user's id in <see cref="Models.UserModel"/>.
    /// </summary>
    /// <param name="phoneNumber">User's phone number.</param>
    /// <param name="newId">User's new id.</param>
    /// <returns>Updated <see cref="Models.UserModel"/> if id was changed or <c>null</c> if wasn't.</returns>
    public static UserModel? EditId(string phoneNumber, long newId)
    {
        var user = Find(phoneNumber);
        if (user is null)
            return null;

        user.Id = newId;
        return Save(user);
    }

    /// <summary>
    /// Changes user's phone number in <see cref="Models.UserModel"/>.
    /// </summary>
    /// <param name="id">User's id in Telegram.</param>
    /// <param name="newPhoneNumber">User's new phone number.</param>
    /// <returns>Updated <see cref="Models.UserModel"/> if phone number was changed or <c>null</c> if wasn't.</returns>
    public static UserModel? EditPhoneNumber(long id, string newPhoneNumber)
    {
        var user = Find(id);
        if (user is null)
            return null;

        Group.RemoveMembers(user.Group, [user.PhoneNumber]);
        Remove(user.PhoneNumber);
        user.PhoneNumber = newPhoneNumber;
        Group.AddMembers(user.Group, [user.PhoneNumber]);
        return Save(user);
    }

    /// <summary>
    /// Changes user's group in <see cref="Models.UserModel"/>.
    /// </summary>
    /// <param name="phoneNumber">User's phone number.</param>
    /// <param name="newGroup">User's new group name.</param>
    /// <returns>Updated <see cref="Models.UserModel"/> if group was changed or <c>null</c> if wasn't.</returns>
    public static UserModel? EditGroup(string phoneNumber, string newGroup)
    {
        var user = Find(phoneNumber);
        if (user is null)
            return null;

        Group.RemoveMembers(user.Group, [user.PhoneNumber]);
        Group.AddMembers(newGroup, [user.PhoneNumber]);
        user.Group = newGroup;
        return Save(user);
    }

    /// <summary>
    /// Removes <see cref="Models.UserModel"/> with corresponding <paramref name="id"/>.
    /// </summary>
    /// <param name="id">User's id in Telegram.</param>
    /// <returns><c>True</c> if was removed or <c>false</c> if wasn't.</returns>
    public static bool Remove(long id)
    {
        var user = Find(id);
        if (user is null)
            return false;

        Group.RemoveMembers(user.Group, [user.PhoneNumber]);
        File.Delete($"{Config.UsersFolderPath}\\{user.PhoneNumber}.json");
        return true;
    }

    /// <summary>
    /// Removes <see cref="Models.UserModel"/> with corresponding <paramref name="phoneNumber"/>.
    /// </summary>
    /// <param name="phoneNumber">User's phone number.</param>
    /// <returns><c>True</c> if was removed or <c>false</c> if wasn't.</returns>
    public static bool Remove(string phoneNumber)
    {
        var user = Find(phoneNumber);
        if (user is null)
            return false;

        Group.RemoveMembers(user.Group, [user.PhoneNumber]);
        File.Delete($"{Config.UsersFolderPath}\\{phoneNumber}.json");
        return true;
    }

    /// <summary>
    /// Saves new or updated <see cref="Models.UserModel"/>.
    /// </summary>
    /// <param name="user">New or updated <see cref="Models.UserModel"/>.</param>
    /// <returns>New or updated <see cref="Models.UserModel"/>.</returns>
    private static UserModel Save(UserModel user)
    {
        var jsonData = JsonSerializer.Serialize(user);
        File.WriteAllText($"{Config.UsersFolderPath}\\{user.PhoneNumber}.json", jsonData);
        return user;
    }
}