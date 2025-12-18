using System.Globalization;

namespace RNMSO_Library_Bot;

public static class Library
{
    private static readonly DateTimeFormatInfo _dtfi = CultureInfo.GetCultureInfo("ru-ru").DateTimeFormat;
    private static readonly string _folderPath = Path.Combine(Environment.GetEnvironmentVariable("RNMSO_LIBRARY_BOT")!, "Library");

    public static List<Concert> Concerts
    {
        get
        {
            var concerts = new List<Concert>();

            var files = Directory.GetDirectories(_folderPath);
            foreach (var file in files)
            {
                var date = DateTime.Parse(Path.GetFileName(file), _dtfi);
                var concert = new Concert() { FolderPath = file, Date = date };
                concerts.Add(concert);
            }

            return concerts;
        }
    }

    public class Concert
    {
        public required string FolderPath { get; set; }
        public required DateTime Date { get; set; }
        public List<Track>? Tracks
        {
            get
            {
                var tracks = new List<Track>();

                var files = Directory.GetDirectories(FolderPath);
                foreach (var file in files)
                {
                    var track = new Track() { FolderPath = file };
                    tracks.Add(track);
                }

                return tracks;
            }
        }
    }

    public class Track
    {
        public required string FolderPath { get; set; }
        public string Title => Path.GetFileName(FolderPath);
        public List<Part>? Parts
        {
            get
            {
                var parts = new List<Part>();

                var files = Directory.GetDirectories(FolderPath);
                foreach (var file in files)
                {
                    var part = new Part() { FilePath = file };
                    parts.Add(part);
                }

                return parts;
            }
        }
    }

    public class Part
    {
        public required string FilePath { get; set; }
        public string Title => Path.GetFileName(FilePath);
        public string Group
        {
            get
            {
                var group = "OtherInstruments";
                foreach (var position in User.GroupList)
                {
                    if (Title.Contains(position))
                        return position;
                }

                return group;
            }
        }
    }
}
