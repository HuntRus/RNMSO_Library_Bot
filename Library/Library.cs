namespace RNMSO_Library_Bot.Library;

public static class Library
{
    public static List<Concert>? Concerts
    {
        get
        {
            var concerts = new List<Concert>();

            var files = Directory.GetDirectories(Configuration.LibraryFolderPath);
            foreach (var file in files)
            {
                var concert = new Concert(file) { Date = DateOnly.Parse(Path.GetFileName(file), Configuration.RegionalFormat) };
                concerts.Add(concert);
            }

            return concerts;
        }
    }
}