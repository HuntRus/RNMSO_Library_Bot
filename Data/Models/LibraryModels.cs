namespace RNMSO_Library_Bot.Data.Models;

public class Concert(string filePath) : LibraryItem(filePath)
{
    public List<Composition>? Compositions
    {
        get
        {
            var compositions = new List<Composition>();

            var files = Directory.GetDirectories(FullPath);
            foreach (var file in files)
            {
                var composition = new Composition(file) { Date = Date };
                compositions.Add(composition);
            }

            return compositions;
        }
    }
}

public class Composition(string filePath) : LibraryItem(filePath)
{
    public List<Part>? Parts
    {
        get
        {
            var parts = new List<Part>();

            var files = Directory.GetFiles(FullPath);
            foreach (var file in files)
            {
                var part = new Part(file) { Date = Date };
                parts.Add(part);
            }

            return parts;
        }
    }
}

public class Part(string filePath) : LibraryItem(filePath)
{
    public string? Group
    {
        get
        {
            var filename = Path.GetFileNameWithoutExtension(FileName).
                                Replace(" ", "").
                                Replace(".", "").
                                Replace("_", "");
            
            int firstLength = 0;
            for (; !char.IsLetter(filename[firstLength]) && firstLength < filename.Length - 1; firstLength++) { }

            filename = filename[firstLength..];
            
            var group = Data.Group.FindByAlias(filename);
            return group?.Name;
        }
    }
}

/// <summary>
/// Base class for library items.
/// </summary>
/// <param name="filePath">Full path to file</param>
public abstract class LibraryItem(string filePath)
{
    public string FileName { get; set; } = Path.GetFileName(filePath);
    public required DateOnly Date { get; set; }
    public string FullPath { get; set; } = filePath;
}