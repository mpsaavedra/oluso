using System;

namespace Oluso.Helpers;

/// <summary>
/// Useful utilities to retrieve versions from assemblies
/// </summary>
public static class SysVersion
{
    /// <summary>
    /// returns the type container or the current assembly version. if something
    /// fails it returns a simple 1.0.0
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string AssemblyVersion(Type? type = null)
    {
        try
        {
            type ??= typeof(Version);
            return type.Assembly.GetName().Version.ToString();
        }
        catch (Exception e)
        {
            return "1.0.0";
        }
    }

    /// <summary>
    /// returns the git short hash of the package. The git has is extracted
    /// from the version.txt resource or other file. if no file found then
    /// returns an empty string
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    public static string GitHash(string resource = "Core.version.txt")
    {
        try
        {
            return EmbeddedResourceManager.ReadString(resource) ?? "";
        }
        catch
        {
            return "";
        }
    }

    /// <summary>
    /// returns the full version which is the assembly version + if found the git hash
    /// </summary>
    /// <param name="type"></param>
    /// <param name="resource"></param>
    /// <returns></returns>
    public static string FullVersion(Type? type = null, string resource = "") =>
        AssemblyVersion(type) + (!string.IsNullOrWhiteSpace(GitHash(resource)) ? "+" + GitHash(resource) : "");
}