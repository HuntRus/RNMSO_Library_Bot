using RNMSO_Library_Bot.Data.Models;

namespace RNMSO_Library_Bot.Data;

public static class Library
{
    public static List<Concert>? GetConcerts()
    {
        var path = Config.LibraryFolderPath;
        var list = new List<Concert>();

        var source = Directory.GetDirectories(path);
        foreach (var dir in source)
        {
            var name = Path.GetFileName(dir);
            var format = Config.RegionalFormat;
            if (!DateOnly.TryParse(name, format, out DateOnly date))
                continue;
            
            var concert = new Concert(dir) { Date = date };
            list.Add(concert);
        }

        return list;
    }
}