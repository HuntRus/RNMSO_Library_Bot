using RNMSO_Library_Bot.Data.Models;

namespace RNMSO_Library_Bot.Data;

public static class Library
{
    public static List<Concert>? Concerts
    {
        get
        {
            var concerts = new List<Concert>();

            var files = Directory.GetDirectories(Config.LibraryFolderPath);
            foreach (var file in files)
            {
                var concert = new Concert(file) { Date = DateOnly.Parse(Path.GetFileName(file), Config.RegionalFormat) };
                concerts.Add(concert);
            }

            return concerts;
        }
    }
}