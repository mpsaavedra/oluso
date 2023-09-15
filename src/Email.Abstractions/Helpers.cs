using System.IO.Compression;

namespace Oluso.Email;

#pragma warning disable CS1591
public static class Helpers
{
    public static List<(string filename, string filePath, string fileConvert, byte[] fileBytes)> ReadAllBytes(
        this string directoryPath, string extensions = "*.*")
    {
        IsDirectoryExistsThrow(directoryPath);
        var directorySelected = Directory.GetFiles(directoryPath, extensions);
        
        List<(string filename, string filePath, string fileConvert, byte[] fileBytes)> filesReading = new List<(string filename, string filePath, string fileConvert, byte[] fileBytes)>();

        foreach (var filePath in directorySelected)
        {
            IsFileExistsThrow(filePath);

            var fileBytes = File.ReadAllBytes(filePath);
            var fileConvert = Convert.ToBase64String(fileBytes);
            var filename = filePath.Substring(filePath.LastIndexOf(Path.DirectorySeparatorChar.ToString(),
                StringComparison.CurrentCulture) + 1);

            filesReading.Add((filename: filename, filePath: filePath, fileConvert: fileConvert, fileBytes: fileBytes));
        }

        return filesReading;
    }

    public static string CreateAndGetTemporaryDirectory(bool withRandomFolder = true)
    {
        string tempDirectory = withRandomFolder ? Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()) : Path.GetTempPath();

        try
        {
            Directory.CreateDirectory(tempDirectory);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new ApplicationException(ex.Message);
        }

        return tempDirectory;
    }

    public static string GetDirectoryFromProject(this string directoryPath)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(),"..", "..", "..", directoryPath);
        IsDirectoryExistsThrow(directoryPath);
        return path;
    }

    public static void DeleteFilesDirectory(string pathDirectory, bool deleteDirectory)
    {
        IsDirectoryExistsThrow(pathDirectory);

        foreach (var file in Directory.GetFiles(pathDirectory))
        {
            var attr = File.GetAttributes(file);
            if ((attr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                File.SetAttributes(file, attr ^ FileAttributes.ReadOnly);
            }
            
            File.Delete(file);
        }

        if (deleteDirectory)
        {
            Directory.Delete(pathDirectory);
        }
    }

    public static void ZipFiles(string filesToZipPathDirectory, string saveZipPathDirectory,
        bool removeZipPathDirectory, string zipName = "files.zip")
    {
        try
        {
            IsDirectoryExistsThrow(filesToZipPathDirectory);
            IsDirectoryExistsThrow(saveZipPathDirectory);

            ZipFile.CreateFromDirectory(filesToZipPathDirectory, Path.Combine(saveZipPathDirectory, zipName));
        }
        catch (UnauthorizedAccessException e)
        {
            if (removeZipPathDirectory)
                DeleteFilesDirectory(saveZipPathDirectory, true);

            throw new ApplicationException(e.Message);
        }
    }

    public static void Show(this string message, bool startLineSpace = false, bool lastLineSpace = false,
        bool readLine = false)
    {
        if (startLineSpace) Console.WriteLine();
        Console.WriteLine(message);
        if (lastLineSpace) Console.WriteLine();
        if (readLine) Console.ReadLine();
    }

    public static void IsDirectoryExistsThrow(this string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            throw new ApplicationException(Messages.DirectoryNotFound(directoryPath));
        }
    }

    public static void IsFileExistsThrow(this string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new ApplicationException(Messages.FileNotFound(filePath));
        }
    }

    public static List<T> IsNullAndCountThrow<T>(this List<T> value, string name)
    {
        if (value?.Any() != true)
        {
            throw new ApplicationException(Messages.RequiredParameter(name));    
        }
        
        return value;
    }

    public static TResult IsEqualTypeThrow<TSource, TResult>(this TSource value, string name)
    {
        TResult result = default!;
        if (value is not TResult)
        {
             throw new ApplicationException(Messages.MustBeOfType(result!.GetType(), value!.GetType()));
        }
        
        return (TResult)Convert.ChangeType(value, typeof(TResult));
    }
}
#pragma warning restore CS1591