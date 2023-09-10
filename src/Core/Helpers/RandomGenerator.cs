using System;

namespace Oluso.Helpers;

/// <summary>
/// a very simple random generator (this class should not be used for critical tasks)
/// </summary>
public class RandomGenerator
{
    /// <summary>
    /// generates a random string from codebase with provided length.
    /// <b>GENERATED STRING SHOULD NOT BE USE FOR CRITICAL TASKS</b>
    /// </summary>
    /// <param name="length"></param>
    /// <param name="codeBase"></param>
    /// <returns></returns>
    public static string NewRandomString(int length = 256, 
        string codeBase = "abcdefghjiklmnopqrstuvwxzyABCDEFGHJIKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+")
    {
        var rand = new Random();
        var result = "";
        for (var i = 0; i < length; i++)
        {
            var idx = rand.Next(codeBase.Length);
            result += codeBase[idx];
        }
        return result;
    }
}