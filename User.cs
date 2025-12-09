using System.Text.Json;

namespace RNMSO_Library_Bot;

public static class User
{
    private static readonly string _folderPath = 
        Path.Combine(Environment.GetEnvironmentVariable("RNMSO_LIBRARY_BOT")!, "Users");

    public static Model? GetUser(string phoneNumber)
    {
        var filePath = Path.Combine(_folderPath, $"{phoneNumber}.json");
        var file = File.ReadAllText(filePath);

        var user = JsonSerializer.Deserialize<Model>(file);

        return user;
    }

    public static Model AddUser(string phoneNumber, string group)
    {
        var filePath = Path.Combine(_folderPath, $"{phoneNumber}.json");
        var user = new Model()
        {
            PhoneNumber = phoneNumber,
            Group = group
        };

        File.WriteAllText(filePath, JsonSerializer.Serialize(user));

        return user;
    }

    public static void RemoveUser(string phoneNumber)
        => File.Delete(Path.Combine(_folderPath, $"{phoneNumber}.json"));

    public class Model
    {
        public required string PhoneNumber { get; set; }
        public required string Group { get; set; }
        public bool Superuser { get; set; } = false;
    }
}
