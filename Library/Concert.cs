namespace RNMSO_Library_Bot.Library;

public class Concert(string filePath) : Model(filePath)
{
    public List<Composition>? Compositions
    {
        get
        {
            var compositions = new List<Composition>();

            var files = Directory.GetDirectories(FilePath);
            foreach (var file in files)
            {
                var composition = new Composition(file) { Date = Date };
                compositions.Add(composition);
            }

            return compositions;
        }
    }
}
