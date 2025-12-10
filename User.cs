using System.Text.Json;

namespace RNMSO_Library_Bot;

public static class User
{
    private static readonly string _folderPath = 
        Path.Combine(Environment.GetEnvironmentVariable("RNMSO_LIBRARY_BOT")!, "Users");

    public static bool IsExists(long id)
        => File.Exists(Path.Combine(_folderPath, $"{id}.json"));

    public static Model? GetUser(string phoneNumber)
    {
        var filePath = Path.Combine(_folderPath, $"{phoneNumber}.json");
        var file = File.ReadAllText(filePath);

        var user = JsonSerializer.Deserialize<Model>(file);

        return user;
    }

    public static Model AddUser(long id, string phoneNumber, string group)
    {
        var filePath = Path.Combine(_folderPath, $"{id}.json");
        var user = new Model()
        {
            Id = id,
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
        public required long Id { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Group { get; set; }
    }
}
