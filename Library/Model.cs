namespace RNMSO_Library_Bot.Library;

public abstract class Model(string filePath)
{
    public string Title { get; set; } = Path.GetFileName(filePath);
    public required DateOnly Date { get; set; }
    public string FilePath { get; set; } = filePath;
}