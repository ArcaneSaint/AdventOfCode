namespace AdventOfCode.Helpers;

public static class DataHelper
{
    private const string FileFormat = "Data/{0}/Data/{1:D2}.txt";
    private const string TestFileFormat = "Data/{0}/Test/{1:D2}.txt";

    private const string File2Format = "Data/{0}/Data/{1:D2}_{2}.txt";
    private const string TestFile2Format = "Data/{0}/Test/{1:D2}_{2}.txt";
    public static string[]? GetInput(int year, int day, bool test = false, int? part = null)
    {
        string file;
        if (part.HasValue)
        {
            file = string.Format(test ? TestFile2Format : File2Format, year, day, part);
        }
        else
        {
            file = string.Format(test ? TestFileFormat : FileFormat, year, day, part);
        }

        if (File.Exists(file))
        {
            return File.ReadAllLines(file);
        }

        return null;
    }
}
