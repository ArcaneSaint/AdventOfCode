namespace AdventOfCode.Helpers;

public static class DataHelper
{
    private const string FileFormat = "Data/{0}/{1}.txt";
    public static string GetInput(int year, int day)
    {
        var file = string.Format(FileFormat, year, day);
        if (File.Exists(file))
        {
            return File.ReadAllText(file);
        }

        throw new FileNotFoundException();
    }
}
