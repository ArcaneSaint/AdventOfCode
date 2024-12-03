namespace AdventOfCode.Helpers;

public static class DataHelper
{
    private const string FileFormat = "Data/{0}/{1}.txt";
    private const string TestFileFormat = "Data/{0}/Test/{1}.txt";
    public static string[] GetInput(int year, int day, bool test = false)
    {
        var file = string.Format(test ? TestFileFormat : FileFormat, year, day);
        if (File.Exists(file))
        {
            return File.ReadAllLines(file);
        }

        throw new FileNotFoundException();
    }
}
