#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Oluso.Extensions;

/// <summary>
/// string related extensions 
/// </summary>
public static class StringExtensions
{
    private static readonly Regex _splitNameRegex = new Regex(@"[\W_]+");

    /// <summary>
    /// context text to bytes array
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static byte[] ToBytes(this string text) => Encoding.UTF8.GetBytes(text);

    /// <summary>
    /// removes values from string
    /// </summary>
    /// <param name="input"></param>
    /// <param name="removes"></param>
    /// <returns></returns>
    public static string? Remove(this string? input, params string[] removes)
    {
        if (Is.NullOrEmpty(input) || Is.NullOrAnyNull(input))
        {
            return input;
        }

        foreach (var remove in removes)
        {
            input = input?.Replace(remove, string.Empty);
        }

        return input;
    }

    /// <summary>
    /// replace some value
    /// </summary>
    /// <param name="input"></param>
    /// <param name="replace"></param>
    /// <param name="start"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static string? Replace(this string? input, string replace, int start, int count)
    {
        if (Is.NullOrEmpty(input) || Is.NullOrEmpty(replace))
            return input;

        if (start + count > input?.Length)
        {
            start = 0;
            count = input.Length;
        }

        var length = start + count;

        for (int i = start; i < length; i++)
        {
            input = input?.Remove(i, 1).Insert(i, replace);
        }

        return input;
    }

    /// <summary>
    /// replace special chars with normal characters
    /// </summary>
    /// <param name="input"></param>
    /// <param name="ignoreSpecialChars"></param>
    /// <returns></returns>
    public static string? ReplaceSpecialChars(this string? input, params string[] ignoreSpecialChars)
    {
        if (Is.NullOrEmpty(input))
        {
            return input;
        }

        var dictionary = new Dictionary<char, char[]>
        {
            { 'a', new[] { '�', '�', '�', '�', '�' } },
            { 'A', new[] { '�', '�', '�', '�', '�', '�' } },
            { 'c', new[] { '�' } },
            { 'C', new[] { '�' } },
            { 'e', new[] { '�', '�', '�', '�' } },
            { 'E', new[] { '�', '�', '�', '�' } },
            { 'i', new[] { '�', '�', '�', '�' } },
            { 'I', new[] { '�', '�', '�', '�' } },
            { 'o', new[] { '�', '�', '�', '�', '�', '�' } },
            { 'O', new[] { '�', '�', '�', '�', '�' } },
            { 'u', new[] { '�', '�', '�', '�' } },
            { 'U', new[] { '�', '�', '�', '�' } },
            { 'h', new[] { '\u0127' } },
            { 'n', new[] { '�' } },
            { 'N', new[] { '�' } },
        };

        var pattern = "[^0-9a-zA-Z ";

        if (ignoreSpecialChars?.Length > 0)
        {
            foreach (string c in ignoreSpecialChars)
            {
                pattern += c;
            }
        }

        pattern += "]+?";
        
#pragma warning disable CS8602
        input = dictionary.Keys
            .Aggregate(input, (x, y) => dictionary[y]
                .Aggregate(x, (z, c) => z.Replace(c, y)));
#pragma warning restore CS8602

        return new Regex(pattern).Replace(input, string.Empty);
    }

    /// <summary>
    /// normalize a given string removing all special characters
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string? Normalize(this string? input) =>
        input.ReplaceSpecialChars()?.Trim();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <param name="replace"></param>
    /// <returns></returns>
    public static string? ToEmailMaskUserName(this string? input, char replace = '*')
        => input.ToEmailMask(@"(?<=[\w]{1})[\w-\._\+%]*(?=[\w]{1}@)", replace);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <param name="replace"></param>
    /// <returns></returns>
    public static string? ToEmailMaskUserNameAndDomain(this string? input, char replace = '*')
        => input.ToEmailMask(@"(?<=(?:^|@)[^.]*)\B.\B", replace);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <param name="replace"></param>
    /// <returns></returns>
    public static string? ToEmailMaskUserNameAndDomainAndExtension(this string? input, char replace = '*')
        => input.ToEmailMask(@"(?<=(?:^|@)[^.]*)\B.\B|(?<=[\w]{1})[\w-\+%]*(?=[\w]{1})", replace);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <param name="pattern"></param>
    /// <param name="replace"></param>
    /// <returns></returns>
    public static string? ToEmailMask(this string? input, string pattern, char replace)
        => string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(pattern) || replace == default
            ? input
            : Regex.Replace(input, pattern, m => new string(replace, m.Length));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static Uri? ToUri(this string? input, Uri? defaultValue = null)
        => !string.IsNullOrWhiteSpace(input)
           && Uri.TryCreate(input, UriKind.RelativeOrAbsolute, out var result)
            ? result
            : defaultValue;


    /// <summary>
    /// generate a slug from a provided phrase
    /// https://adamhathcock.blog/2017/05/04/generating-url-slugs-in-net-core/
    /// </summary>
    /// <param name="phrase"></param>
    /// <returns></returns>
    public static string GenerateSlug(this string phrase)
    {
        string str = phrase.RemoveDiacritics().ToLower();
        // invalid chars           
        str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
        // convert multiple spaces into one space   
        str = Regex.Replace(str, @"\s+", " ").Trim();
        // cut and trim 
        str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
        str = Regex.Replace(str, @"\s", "-"); // hyphens   
        return str;
    }

    /// <summary>
    /// remove possible accent from the string
    /// https://stackoverflow.com/questions/249087/how-do-i-remove-diacritics-accents-from-a-string-in-net
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string RemoveDiacritics(this string text)
    {
        var s = new string(text.Normalize(NormalizationForm.FormD)
            .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            .ToArray());

        return s.Normalize(NormalizationForm.FormC);
    }


    /// <summary>
    /// replace all coincidences of the given original value with
    /// the provided value
    /// </summary>
    /// <param name="val">string to replace values from</param>
    /// <param name="original">value to be replace</param>
    /// <param name="replacement">value to replace with</param>
    /// <returns></returns>
    public static string ReplaceAll(this string val, string original, string replacement)
    {
        StringBuilder sb = new StringBuilder(val);
        sb.Replace(original, replacement);

        return sb.ToString();
    }

    /// <summary>
    /// Returns a Pascal representation of the provided string
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static string ToPascalize(this string val) =>
        val.ToCharArray()[0].ToString().ToLowerInvariant() + val.Substring(1);

    /// <summary>
    /// hash a given text using <see cref="HMACSHA256"/> and returns the Base64
    /// representation of the hashed text.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string HashHmac256(this string text, string key)
    {
        var encoding = new UTF8Encoding();
        Byte[] textBytes = encoding.GetBytes(text);
        Byte[] keyBytes = encoding.GetBytes(key);
        Byte[] hashBytes;

        using (var hash = new HMACSHA256(keyBytes))
            hashBytes = hash.ComputeHash(textBytes);

        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }

    /// <summary>
    /// Hash a text using the <see cref="KeyDerivation.Pbkdf2"/> algorithm.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="salt"></param>
    /// <param name="iterationCount"></param>
    /// <returns></returns>
    public static string ToHashPasswordPbdkf2(this string text, byte[] salt, int iterationCount)
    {
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: text,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: iterationCount,
            numBytesRequested: 128 / 8));
    }

    /// <summary>
    /// <inheritdoc cref="ToHashPasswordPbdkf2(string,int)"/> With 100 000 iterations.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="salt"></param>
    /// <returns></returns>
    public static string ToHashPasswordPbkdf2(this string text, byte[] salt) =>
        ToHashPasswordPbdkf2(text, salt, 100000);

    /// <summary>
    /// <inheritdoc cref="ToHashPasswordPbdkf2(string,byte[],int)"/>, generate a random 128 bytes salt
    /// </summary>
    /// <param name="text"></param>
    /// <param name="iterations"></param>
    /// <returns>tuple (string, byte[]) with the encrypted text and the salt used</returns>
    public static (string, byte[]) ToHashPasswordPbdkf2(this string text, int iterations)
    {
        byte[] salt = new byte[128 / 8];
        using (var rngCsp = new RNGCryptoServiceProvider())
            rngCsp.GetNonZeroBytes(salt);

        // TODO: perhaps we need to improve security making a larger salt
        // var salt = RandomNumberGenerator.GetBytes(128 / 8);

        return (text.ToHashPasswordPbdkf2(salt, iterations), salt);
    }

    /// <summary>
    /// split a given pascal string and insert a separator between words
    /// </summary>
    /// <example>
    /// var result = "ThisIsAWord".SplitPascal("-");
    ///  // result now is this-is-a-word
    /// </example>
    public static string ToSplitPascal(this string source, string separator = "-")
    {
        Regex r = new Regex("([A-Z]+[a-z]+)");
        string result = r.Replace(source, m =>
            (m.Value.Length > 3 ? m.Value : m.Value.ToLower()) + separator);
        return result;
    }

    /// <summary>
    ///  returns the last part of a canonical class name,
    /// </summary>
    public static string LastPart(this string source, char separator = '.')
    {
        return source.Split(separator).Last();
    }

    /// <summary>
    /// returns a valuable ICollection entry name from a value name provided
    /// by reflection
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToFormatCollectionEntry(this string value)
    {
        var typeName = value.Substring(0, value.IndexOf('`')).LastPart('.');
        var subTypeName = value.Substring(value.IndexOf('['));
        // ICollection`[asd]
        return typeName + subTypeName
            .Replace('[', '<')
            .Replace(']', '>');
    }

    /// <summary>
    /// returns the type of the collection string name
    /// </summary>
    public static string ToCollectionEntry(this string value)
    {
        // var start = value.IndexOf('<') + 1;
        // var end = value.IndexOf('>') - start;
        // return value.Substring(start, end).Substring(value.IndexOf('.') + 3);
        var start = value.LastIndexOf('.') + 1;
        var end = value.IndexOf('>') - start;
        var result = value.Substring(start, end);
        return result;
    }

    /// <summary>
    /// return true if string is null or empty
    /// </summary>
    public static bool IsNullOrEmpty(this string? value) =>
        string.IsNullOrEmpty(value);

    /// <summary>
    /// return true if string is null or whitespace
    /// </summary>
    public static bool IsNullOrWhiteSpace(this string? value) =>
        string.IsNullOrWhiteSpace(value);

    /// <summary>
    /// returns true if string is null, empty or whitespace
    /// </summary>
    public static bool IsNullEmptyOrWhiteSpace(this string? value) =>
        value.IsNullOrEmpty() || value.IsNullOrWhiteSpace();

    /// <summary>
    /// returns string as an integer if it could be parse, otherwise the defaultValue or null
    /// </summary>
    /// <param name="value">value to parse</param>
    /// <param name="defaultValue">optional default value to return, default null</param>
    /// <returns>int | null |  defaultValue</returns>
    public static int? ToInt(this string? value, int? defaultValue = null) =>
        int.TryParse(value, out var i) ? i : defaultValue;

    /// <summary>
    /// Attempts to pluralize the specified text according to the rules of the English language.
    /// </summary>
    /// <remarks>
    /// This function attempts to pluralize as many words as practical by following these rules:
    /// <list type="bullet">
    ///		<item><description>Words that don't follow any rules (e.g. "mouse" becomes "mice") are returned from a dictionary.</description></item>
    ///		<item><description>Words that end with "y" (but not with a vowel preceding the y) are pluralized by replacing the "y" with "ies".</description></item>
    ///		<item><description>Words that end with "us", "ss", "x", "ch" or "sh" are pluralized by adding "es" to the end of the text.</description></item>
    ///		<item><description>Words that end with "f" or "fe" are pluralized by replacing the "f(e)" with "ves".</description></item>
    ///	</list>
    /// </remarks>
    /// <param name="text">The text to pluralize.</param>
    /// <param name="number">If number is 1, the text is not pluralized; otherwise, the text is pluralized.</param>
    /// <returns>A string that consists of the text in its pluralized form.</returns>
    public static string ToPluralize(this string text, int number = 2)
    {
        if (number == 1)
        {
            return text;
        }
        else
        {
            // Create a dictionary of exceptions that have to be checked first
            // This is very much not an exhaustive list!
            Dictionary<string, string> exceptions = new Dictionary<string, string>()
            {
                { "man", "men" },
                { "woman", "women" },
                { "child", "children" },
                { "tooth", "teeth" },
                { "foot", "feet" },
                { "mouse", "mice" },
                { "belief", "beliefs" }
            };

            if (exceptions.ContainsKey(text.ToLowerInvariant()))
            {
                return exceptions[text.ToLowerInvariant()];
            }
            else if (text.EndsWith("y", StringComparison.OrdinalIgnoreCase) &&
                     !text.EndsWith("ay", StringComparison.OrdinalIgnoreCase) &&
                     !text.EndsWith("ey", StringComparison.OrdinalIgnoreCase) &&
                     !text.EndsWith("iy", StringComparison.OrdinalIgnoreCase) &&
                     !text.EndsWith("oy", StringComparison.OrdinalIgnoreCase) &&
                     !text.EndsWith("uy", StringComparison.OrdinalIgnoreCase))
            {
                return text.Substring(0, text.Length - 1) + "ies";
            }
            else if (text.EndsWith("us", StringComparison.InvariantCultureIgnoreCase))
            {
                // http://en.wikipedia.org/wiki/Plural_form_of_words_ending_in_-us
                return text + "es";
            }
            else if (text.EndsWith("ss", StringComparison.InvariantCultureIgnoreCase))
            {
                return text + "es";
            }
            else if (text.EndsWith("s", StringComparison.InvariantCultureIgnoreCase))
            {
                return text;
            }
            else if (text.EndsWith("x", StringComparison.InvariantCultureIgnoreCase) ||
                     text.EndsWith("ch", StringComparison.InvariantCultureIgnoreCase) ||
                     text.EndsWith("sh", StringComparison.InvariantCultureIgnoreCase))
            {
                return text + "es";
            }
            else if (text.EndsWith("f", StringComparison.InvariantCultureIgnoreCase) && text.Length > 1)
            {
                return text.Substring(0, text.Length - 1) + "ves";
            }
            else if (text.EndsWith("fe", StringComparison.InvariantCultureIgnoreCase) && text.Length > 2)
            {
                return text.Substring(0, text.Length - 2) + "ves";
            }
            else
            {
                return text + "s";
            }
        }
    }

    /// <summary>
    /// Determines whether the specified string is not <see cref="IsNullOrEmpty"/>.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns>
    ///   <c>true</c> if the specified <paramref name="value"/> is not <see cref="IsNullOrEmpty"/>; otherwise, <c>false</c>.
    /// </returns>
    public static bool HasValue(this string value)
    {
        return !string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Does string contain both uppercase and lowercase characters?
    /// </summary>
    /// <param name="s">The value.</param>
    /// <returns>True if contain mixed case.</returns>
    public static bool IsMixedCase(this string s)
    {
        if (s.IsNullOrEmpty())
            return false;

        var containsUpper = s.Any(Char.IsUpper);
        var containsLower = s.Any(Char.IsLower);

        return containsLower && containsUpper;
    }

    /// <summary>
    /// Converts a string to use camelCase.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The to camel case. </returns>
    public static string ToCamelCase(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        string output = ToPascalize(value);
        if (output.Length > 2)
            return char.ToLower(output[0]) + output.Substring(1);

        return output.ToLower();
    }

    // /// <summary>
    // /// Converts a string to use PascalCase.
    // /// </summary>
    // /// <param name="value">Text to convert</param>
    // /// <returns>The string</returns>
    // public static string ToPascalCase(this string value)
    // {
    //     return value.ToPascalCase(_splitNameRegex);
    // }
    //
    // /// <summary>
    // /// Converts a string to use PascalCase.
    // /// </summary>
    // /// <param name="value">Text to convert</param>
    // /// <param name="splitRegex">Regular Expression to split words on.</param>
    // /// <returns>The string</returns>
    // public static string ToPascalCase(this string value, Regex splitRegex)
    // {
    //     if (string.IsNullOrEmpty(value))
    //         return value;
    //
    //     var mixedCase = value.IsMixedCase();
    //     var names = splitRegex.Split(value);
    //     var output = new StringBuilder();
    //
    //     if (names.Length > 1)
    //     {
    //         foreach (string name in names)
    //         {
    //             if (name.Length > 1)
    //             {
    //                 output.Append(char.ToUpper(name[0]));
    //                 output.Append(mixedCase ? name.Substring(1) : name.Substring(1).ToLower());
    //             }
    //             else
    //             {
    //                 output.Append(name);
    //             }
    //         }
    //     }
    //     else if (value.Length > 1)
    //     {
    //         output.Append(char.ToUpper(value[0]));
    //         output.Append(mixedCase ? value.Substring(1) : value.Substring(1).ToLower());
    //     }
    //     else
    //     {
    //         output.Append(value.ToUpper());
    //     }
    //
    //     return output.ToString();
    // }

    /// <summary>
    /// Lowercases the first character of a given string.
    /// </summary>
    /// <param name="s">The string whose first character to lowercase.</param>
    /// <returns>The string with its first character lowercased.</returns>
    public static string? ToDecapitalize(this string? s)
    {
        if (s is null || char.IsLower(s[0]))
        {
            return s;
        }

        return char.ToLower(s[0]) + s.Substring(1);
    }

    /// <summary>
    /// Uppercases the first character of a given string.
    /// </summary>
    /// <param name="s">The string whose first character to uppercase.</param>
    /// <returns>The string with its first character uppercased.</returns>
    public static string? ToCapitalize(this string? s)
    {
        if (s is null || char.IsUpper(s[0]))
        {
            return s;
        }

        return char.ToUpper(s[0]) + s.Substring(1);
    }



    /// <summary>
    /// returns the name of nullable type
    /// </summary>
    public static string NullableTypeName(this string value)
    {
        return value;
    }

    /// <summary>
    /// executes a command in the bash, like: 
    /// <p><b>"<i>ls /</i>".Bash()</b></p>
    /// <p><i><b>NOTE:</b> This method it will only work with bash</i></p>
    /// </summary>
    /// <param name="cmd">command to execute</param>
    /// <returns></returns>
    public static string Bash(this string cmd)
    {
        var escapedArgs = cmd.Replace("\"", "\\\"");
        if (System.IO.File.Exists("/bin/bash"))
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            var result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }

        return string.Empty;
    }

    /// <summary>
    /// execute the provided command with the provided arguments and returns
    /// an string with the result
    /// </summary>
    /// <param name="command">command</param>
    /// <param name="arguments">arguments</param>
    /// <returns></returns>
    public static string Run(this string command, string? arguments = null)
    {
        var procStartInfo = new ProcessStartInfo(command, arguments);

        // The following commands are needed to redirect the standard output.
        // This means that it will be redirected to the Process.StandardOutput StreamReader.
        procStartInfo.RedirectStandardOutput = true;
        procStartInfo.UseShellExecute = false;
        // Do not create the black window.
        procStartInfo.CreateNoWindow = true;
        // Now we create a process, assign its ProcessStartInfo and start it
        System.Diagnostics.Process proc = new System.Diagnostics.Process();
        proc.StartInfo = procStartInfo;
        proc.Start();
        // Get the output into a string
        string result = proc.StandardOutput.ReadToEnd();

        return result;
    }

    /// <summary>
    /// Execute a command in hte system. Has the same definition as <see cref="Bash"/>,
    /// but it is multi platform.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public static string Command(this string command)
    {
        var parts = command.Split(' ');
        var cmd = parts[0];
        var args = parts.Length > 1 ? parts[1] : null;
        return cmd.Run(args);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sourceDirectoryPath"></param>
    /// <param name="destinationDirectoryPath"></param>
    /// <param name="customZipName"></param>
    /// <param name="compressionLevel"></param>
    /// <param name="removeSourceDirectoryPath"></param>
    public static void ZipCreate(
        this string? sourceDirectoryPath,
        string? destinationDirectoryPath,
        string? customZipName = null,
        CompressionLevel compressionLevel = CompressionLevel.Optimal,
        bool removeSourceDirectoryPath = false)
    {
        var validSourceDirectoryPath = sourceDirectoryPath.IsNullOrEmptyThrow(nameof(sourceDirectoryPath));

        customZipName = string.IsNullOrWhiteSpace(customZipName)
            ? sourceDirectoryPath?.Split('\\').Last()
            : customZipName;

        ZipFile.CreateFromDirectory(validSourceDirectoryPath, $"{destinationDirectoryPath}/{customZipName}.zip",
            compressionLevel, false);

        if (removeSourceDirectoryPath)
        {
            Directory.Delete(validSourceDirectoryPath, true);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sourceFilePath"></param>
    /// <param name="destinationDirectoryPath"></param>
    /// <param name="overwriteFiles"></param>
    public static void ZipExtract(
        this string? sourceFilePath,
        string? destinationDirectoryPath,
        bool overwriteFiles = false)
        => ZipFile.ExtractToDirectory(
            sourceFilePath.IsNullOrEmptyThrow(nameof(sourceFilePath)),
            destinationDirectoryPath.IsNullOrEmptyThrow(nameof(destinationDirectoryPath)));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string? Random(this string input, int length)
        => string.IsNullOrWhiteSpace(input)
            ? null
            : new string(Enumerable.Repeat(input, length)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static string ToFormat(this string value, params object[] args)
        => value.ToFormat(CultureInfo.CurrentCulture, args);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="formatProvider"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static string ToFormat(this string value, IFormatProvider formatProvider, params object[] args)
        => string.Format(formatProvider, value, args);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="folderName"></param>
    /// <returns></returns>
    public static string ToDirectoryTemporary(this string folderName)
    {
        var path = Path.Combine(Path.GetTempPath(), folderName);

        return Directory.CreateDirectory(path).FullName;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="directoryPath"></param>
    /// <param name="fileName"></param>
    /// <param name="ignoreCase"></param>
    /// <returns></returns>
    public static string? ToDirectoryFindFilePath(this string? directoryPath, string fileName, bool ignoreCase = true)
    {
        if (string.IsNullOrWhiteSpace(directoryPath) || string.IsNullOrWhiteSpace(fileName))
        {
            return null;
        }

        foreach (var fullFile in Directory.GetFiles(directoryPath))
        {
            var file = Path.GetFileName(fullFile).Split('.')[0];

            if (ignoreCase)
            {
                file = file.ToLower(CultureInfo.CurrentCulture);
                fileName = fileName.ToLower(CultureInfo.CurrentCulture);
            }

            if (file == fileName)
            {
                return fullFile;
            }
        }

        foreach (var path in Directory.GetDirectories(directoryPath))
        {
            var file = path.ToDirectoryFindFilePath(fileName);

            if (!string.IsNullOrEmpty(file))
            {
                return file;
            }
        }

        return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="directoryPath"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static FileInfo? ToDirectoryFileInfo(this string? directoryPath, string fileName)
    {
        if (string.IsNullOrWhiteSpace(directoryPath) || string.IsNullOrWhiteSpace(fileName))
        {
            return null;
        }

        return new DirectoryInfo(directoryPath).GetFiles(string.Concat(fileName, ".", "*")).Single();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="directoryPath"></param>
    /// <param name="recursive"></param>
    /// <param name="directoryDelete"></param>
    public static void ToDirectoryDeleteFiles(this string directoryPath, bool recursive = false,
        bool directoryDelete = false)
    {
        if (!string.IsNullOrWhiteSpace(directoryPath))
        {
            foreach (var file in Directory.GetFiles(directoryPath))
            {
                var attr = File.GetAttributes(file);

                if ((attr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    File.SetAttributes(file, attr ^ FileAttributes.ReadOnly);
                }

                File.Delete(file);
            }

            if (directoryDelete)
            {
                Directory.Delete(directoryPath, recursive);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="directoryPath"></param>
    /// <param name="fileName"></param>
    /// <param name="bytes"></param>
    /// <param name="ifNoExistDirectoryCreate"></param>
    public static void ToDirectorySaveFile(this string directoryPath, string fileName, byte[] bytes,
        bool ifNoExistDirectoryCreate = true)
    {
        if (!string.IsNullOrWhiteSpace(directoryPath) && !string.IsNullOrWhiteSpace(fileName) && bytes != null &&
            bytes.Length > 0)
        {
            if (ifNoExistDirectoryCreate && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var path = Path.Combine(directoryPath, fileName);

            File.WriteAllBytes(path, bytes);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="directoryPath"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static (string fileName, string filePath, string fileConvert, byte[] fileBytes, long fileLength, string
        fileContentType)? ToDirectoryReadFile(
            this string directoryPath,
            string fileName)
    {
        if (!string.IsNullOrWhiteSpace(directoryPath) || !string.IsNullOrWhiteSpace(fileName))
        {
            return null;
        }

        var fileInfo = ToDirectoryFileInfo(directoryPath, fileName);

        if (fileInfo is null)
        {
            return null;
        }

        var filePath = fileInfo.FullName;

        var fileBytes = File.ReadAllBytes(filePath);

        var fileConvert = Convert.ToBase64String(fileBytes);

        return (fileInfo.Name, filePath, fileConvert, fileBytes, fileInfo.Length, fileInfo.Extension);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="directoryPath"></param>
    /// <param name="searchOption"></param>
    /// <param name="includeExtensions"></param>
    /// <returns></returns>
    public static
        List<(string fileName, string filePath, string fileConvert, byte[] fileBytes, long fileLength, string
            fileContentType)?> ToDirectoryReadFiles(
            this string directoryPath,
            SearchOption searchOption = SearchOption.AllDirectories,
            params string[] includeExtensions)
    {
        if (string.IsNullOrWhiteSpace(directoryPath))
        {
            return null!;
        }

        List<(string fileName, string filePath, string fileConvert, byte[] fileBytes, long fileLength, string
            fileContentType)?> filesReading =
            new List<(string fileName, string filePath, string fileConvert, byte[] fileBytes, long fileLength, string
                fileContentType)?>();

        includeExtensions ??= new[] { "*.*" };

        foreach (var extension in includeExtensions)
        {
            foreach (var name in Directory.GetFiles(directoryPath, extension, searchOption))
            {
                var file = ToDirectoryReadFile(directoryPath, name);

                filesReading.Add(file);
            }
        }

        return filesReading;
    }

    /// <summary>
    /// turns some {TEnum} type string into an enum
    /// </summary>
    /// <param name="input"></param>
    /// <param name="defaultValue"></param>
    /// <typeparam name="TEnum"></typeparam>
    /// <returns></returns>
    public static TEnum ToEnum<TEnum>(this string? input, TEnum defaultValue = default)
        where TEnum : struct, Enum =>
        input.IsNullOrEmpty() || !Enum.TryParse(input, false, out TEnum result) ? default : result;

    /// <summary>
    /// turns some {TEnum} type int into an enum
    /// </summary>
    /// <param name="input"></param>
    /// <param name="defaultValue"></param>
    /// <typeparam name="TEnum"></typeparam>
    /// <returns></returns>
    public static TEnum ToEnum<TEnum>(this int input, TEnum defaultValue = default)
        where TEnum : struct, Enum =>
        Enum.GetName(typeof(TEnum), input).ToEnum(defaultValue);

    /// <summary>
    /// returns the <see cref="HashAlgorithmName"/> by his name or null if not one of
    /// <li>MD5</li>
    /// <li>SHA1</li>
    /// <li>SHA256</li>
    /// <li>SHA384</li>
    /// <li>SHA512</li>
    /// </summary>
    /// <param name="input"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static HashAlgorithmName? ToHashAlgorithmName(this string? input, HashAlgorithmName? defaultValue = null)
    {
        return input switch
        {
            string _ when HashAlgorithmName.MD5.ToString() == input => HashAlgorithmName.MD5,
            string _ when HashAlgorithmName.SHA1.ToString() == input => HashAlgorithmName.SHA1,
            string _ when HashAlgorithmName.SHA256.ToString() == input => HashAlgorithmName.SHA256,
            string _ when HashAlgorithmName.SHA384.ToString() == input => HashAlgorithmName.SHA384,
            string _ when HashAlgorithmName.SHA512.ToString() == input => HashAlgorithmName.SHA512,

            _ => defaultValue,
        };
    }
}