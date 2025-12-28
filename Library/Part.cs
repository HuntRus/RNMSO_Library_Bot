namespace RNMSO_Library_Bot.Library;

public class Part(string filePath) : Model(filePath)
{
    public string Group
    {
        get
        {
            var filename = Title;
            int firstLength = 0;
            for (var i = 0; char.IsLetter(filename[i]); i++) // end of filename limit
            {
                firstLength = i;
            }

            filename = filename[firstLength..];

            return User.GetGroupFromFilename(filename);
        }
    }

}