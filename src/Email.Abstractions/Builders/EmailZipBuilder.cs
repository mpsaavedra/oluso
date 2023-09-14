using Oluso.Email.Models;

namespace Oluso.Email.Builders;

/// <summary>
/// fluent api <see cref="Models.Email"/> builder
/// </summary>
public class EmailZipBuilder
{
    private string? _zipPath = null;
    private bool _deleteAfterAttach;
    
#pragma warning disable CS8618
    private EmailZipBuilder()
#pragma warning restore CS8618
    {
    }

    /// <summary>
    /// returns a new <see cref="EmailZipBuilder"/> instance
    /// </summary>
    public static EmailZipBuilder New => new();

    /// <summary>
    /// <inheritdoc cref="EmailZip.ZipPathDirectory"/>
    /// </summary>
    /// <param name="zipPath"></param>
    /// <returns></returns>
    public EmailZipBuilder WithZipPath(string zipPath)
    {
        _zipPath = zipPath;
        return this;
    }
    
    /// <summary>
    /// <inheritdoc cref="EmailZip.DeleteAfterAttach"/>
    /// </summary>
    /// <param name="deleteAfetAttach"></param>
    /// <returns></returns>
    public EmailZipBuilder WithDeleteAfterAttach(bool deleteAfetAttach)
    {
        _deleteAfterAttach = deleteAfetAttach;
        return this;
    }

    /// <summary>
    /// return a new <see cref="EmailZip"/> instance created
    /// with provided information
    /// </summary>
    /// <returns></returns>
    public EmailZip Build()
    {
        if (_zipPath == null)
            throw new ApplicationException(Messages.EmailZipDirectoryCouldNotBeNull());
        return new(_zipPath, _deleteAfterAttach);
    }
}