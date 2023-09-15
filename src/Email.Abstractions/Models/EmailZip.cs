using Oluso.Email.Builders;

namespace Oluso.Email.Models;

/// <summary>
/// represents am email zip file
/// </summary>
public class EmailZip
{
    /// <summary>
    /// returns a new <see cref="EmailZip"/> instance
    /// </summary>
    /// <param name="zipPathDirectory"></param>
    /// <param name="deleteAfterAttach"></param>
    public EmailZip(string zipPathDirectory, bool deleteAfterAttach = true)
    {
        if (!Directory.Exists(zipPathDirectory))
            throw new ApplicationException(Messages.EmailZipDirectoryNotFound(zipPathDirectory));
        ZipPathDirectory = zipPathDirectory;
        DeleteAfterAttach = deleteAfterAttach;
    }
    
    /// <summary>
    /// returns a fluent api builder
    /// </summary>
    public static EmailZipBuilder Builder => EmailZipBuilder.New;
    
    /// <summary>
    /// path to the Zip directory
    /// </summary>
    public string ZipPathDirectory { get; }

    /// <summary>
    /// if true delete the attachment file after attach. Just in case the zip was newly created
    /// which means that the zipPathDirectory parameter point to a directory and not to a file.
    /// </summary>
    public bool DeleteAfterAttach { get; }
    
    /// <summary>
    /// returns true if the zipPathDirectory point to a zip file not to a directory
    /// </summary>
    public bool IsCompressed => 
        !string.IsNullOrEmpty(ZipPathDirectory) &&
         File.Exists(ZipPathDirectory) && 
        !Directory.Exists(ZipPathDirectory);
}