#nullable  enable
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oluso.Extensions;

/// <summary>
/// Dictionary related extensions
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// add or update a value to the dictionary, it first validate that the dictionary,
    /// the key and the value are not empty or null. if the key exists it checks that
    /// the value is not already in the list if not it adds the value if not exist add
    /// the value to the key list in the dictionary.
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static IDictionary<string, IEnumerable<string>> ToAddOrUpdate(
        this IDictionary<string, IEnumerable<string>>? dictionary, string key, string value)
    {
        var validDictionary = dictionary.ValidateThrow(key, value);

        if (validDictionary.ContainsKey(key))
        {
            if (!validDictionary[key].Contains(value))
            {
                validDictionary[key] = validDictionary[key].Concat(new string[] { value });
            }
        }
        else
        {
            validDictionary.Add(key, new List<string> { value });
        }

        return validDictionary;
    }

    /// <summary>
    /// returns true if the ley is in the dictionary and the value is in the key list
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool Contains(this IDictionary<string, IEnumerable<string>>? dictionary, string key, string value)
    {
        var validDictionary = dictionary.ValidateThrow(key, value);

        return validDictionary.ContainsKey(key) && validDictionary[key].Contains(value);
    }

    /// <summary>
    /// returns all values in a given dictionary key
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static IEnumerable<string>? GetValues(this IDictionary<string, IEnumerable<string>>? dictionary, string key)
    {
        var validDictionary = dictionary.ValidateThrow(key);

        return validDictionary.ContainsKey(key) ? validDictionary[key] : null;
    }

    /// <summary>
    /// validate the dictionary key checking if is null or if the key is null
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    private static IDictionary<string, IEnumerable<string>> ValidateThrow(
        this IDictionary<string, IEnumerable<string>>? dictionary, string key)
    {
        var errors = new List<string>();

        if (Is.NullOrEmpty(dictionary))
        {
            errors.Add(Messages.NullOrEmpty(nameof(dictionary)));
        }

        if (Is.NullOrEmpty(key))
        {
            errors.Add(Messages.NullOrEmpty(nameof(key)));
        }

        if (errors.Any())
        {
            Insist.Throw<Exception>(errors);
        }

        return dictionary!;
    }

    /// <summary>
    /// validate that dictionary, the key and the value are not null or empty
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    private static IDictionary<string, IEnumerable<string>> ValidateThrow(
        this IDictionary<string, IEnumerable<string>>? dictionary, string key, string value)
    {
        dictionary.ValidateThrow(key);

        value.IsNullOrEmptyThrow(nameof(value));

        return dictionary!;
    }
}