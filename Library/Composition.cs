namespace RNMSO_Library_Bot.Library;

public class Composition(string filePath) : Model(filePath)
{
    public List<Part>? Parts
    {
        get
        {
            var parts = new List<Part>();

            var files = Directory.GetFiles(FilePath);
            foreach (var file in files)
            {
                var part = new Part(file) { Date = Date };
                parts.Add(part);
            }

            return parts;
        }
    }
}