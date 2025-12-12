using System.Text.Json;

namespace RNMSO_Library_Bot;

public static class User
{
    private static readonly string _folderPath =
        Path.Combine(Environment.GetEnvironmentVariable("RNMSO_LIBRARY_BOT")!, "Users");

    private static List<Model>? UsersList
    {
        get
        {
            var users = new List<Model>();

            var files = Directory.GetFiles(_folderPath);
            foreach (var file in files)
            {
                var jsonData = File.ReadAllText(file);
                var userData = JsonSerializer.Deserialize<Model>(jsonData);
                users.Add(userData!);
            }

            return users;
        }
    }

    public static Model? Get(string phoneNumber)
    {
        var user = UsersList.Find(x => x.PhoneNumber == phoneNumber);
        return user;
    }

    public static Model? Get(long id)
    {
        var user = UsersList.Find(x => x.Id == id);
        return user;
    }

    public static Model? Add(string phoneNumber, string group)
    {
        var user = new Model() { PhoneNumber = phoneNumber, Group = group };

        var jsonData = JsonSerializer.Serialize(user);
        File.WriteAllText($"{_folderPath}\\{phoneNumber}.json", jsonData);
        return user;
    }

    public static Model? EditId(string phoneNumber, long id)
    {
        var user = Get(phoneNumber);
        user.Id = id;

        var jsonData = JsonSerializer.Serialize(user);
        File.WriteAllText($"{_folderPath}\\{phoneNumber}.json", jsonData);
        return user;
    }

    public static Model? EditGroup(string phoneNumber, string group)
    {
        var user = Get(phoneNumber);
        user.Group = group;

        var jsonData = JsonSerializer.Serialize(user);
        File.WriteAllText($"{_folderPath}\\{phoneNumber}.json", jsonData);
        return user;
    }

    public static void Remove(string phoneNumber)
    {
        var user = Get(phoneNumber);
        if (user != null)
            File.Delete($"{_folderPath}\\{phoneNumber}.json");
    }

    public class Model
    {
        public long Id { get; set; } = default;
        public required string PhoneNumber { get; set; }
        public required string Group { get; set; }
    }
}
