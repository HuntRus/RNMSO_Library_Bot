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

    public static string GetGroupFromFilename(string filename)
    {
        var text = File.ReadAllText(Path.Combine(Environment.GetEnvironmentVariable("RNMSO_LIBRARY_BOT"), "groups.json"));
        var groups = JsonSerializer.Deserialize<GroupMoudel>(text);

        if (groups.FirstViolin.Contains(filename))
            return "FirstViolin";
        else if (groups.SecondViolin.Contains(filename))
            return "SecondViolin";
        else if (groups.Viola.Contains(filename))
            return "Viola";
        else if (groups.Cello.Contains(filename))
            return "Cello";
        else if (groups.Contrabass.Contains(filename))
            return "Contrabass";
        else if (groups.Flute.Contains(filename))
            return "Flute";
        else if (groups.Oboe.Contains(filename))
            return "Oboe";
        else if (groups.Clarinet.Contains(filename))
            return "Clarinet";
        else if (groups.Bassoon.Contains(filename))
            return "Bassoon";
        else if (groups.Horn.Contains(filename))
            return "Horn";
        else if (groups.Trumpet.Contains(filename))
            return "Trumpet";
        else if (groups.TromboneAndTuba.Contains(filename))
            return "TromboneAndTuba";
        else if (groups.Percussion.Contains(filename))
            return "Percussion";
        else if (groups.HarpAndKeyboard.Contains(filename))
            return "HarpAndKeyboard";
        else if (groups.OtherInstruments.Contains(filename))
            return "OtherInstruments";
        else if (groups.Librarian.Contains(filename))
            return "Librarian";

        return "Unauthorized";
    }

    private static Model Save(Model user)
    {
        var jsonData = JsonSerializer.Serialize(user);
        File.WriteAllText($"{_folderPath}\\{user.PhoneNumber}.json", jsonData);
        return user;
    }

    public static Model? Get(string phoneNumber)
        => UsersList!.Find(x => x.PhoneNumber == phoneNumber);

    public static Model? Get(long id)
        => UsersList!.Find(x => x.Id == id);

    public static Model Add(string phoneNumber, string group)
        => Save(new Model() { PhoneNumber = phoneNumber, Group = group })!;

    public static Model? EditId(string phoneNumber, long id)
    {
        var user = Get(phoneNumber);
        user.Id = id;

        return Save(user);
    }

    public static Model? EditGroup(string phoneNumber, string group)
    {
        var user = Get(phoneNumber);
        user.Group = group;

        return Save(user);
    }

    public static void Remove(string phoneNumber)
    {
        var user = Get(phoneNumber);
        if (user != null)
            File.Delete($"{_folderPath}\\{phoneNumber}.json");
    }

    public class GroupMoudel
    {
        public List<string> FirstViolin { get; set; }
        public List<string> SecondViolin { get; set; }
        public List<string> Viola { get; set; }
        public List<string> Cello { get; set; }
        public List<string> Contrabass { get; set; }
        public List<string> Flute { get; set; }
        public List<string> Oboe{ get; set; }
        public List<string> Clarinet { get; set; }
        public List<string> Bassoon{ get; set; }
        public List<string> Horn{ get; set; }
        public List<string> Trumpet { get; set; }
        public List<string> TromboneAndTuba{ get; set; }
        public List<string> Percussion { get; set; }
        public List<string> HarpAndKeyboard { get; set; }
        public List<string> OtherInstruments { get; set; }
        public List<string> Librarian { get; set; }
}

    public class Model
    {
        public long Id { get; set; } = default;
        public required string PhoneNumber { get; set; }
        public required string Group { get; set; }
    }
}
