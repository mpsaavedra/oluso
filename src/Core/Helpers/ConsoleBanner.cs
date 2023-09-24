using System;
using System.Collections.Generic;
using System.Linq;

namespace Oluso.Helpers;

/// <summary>
/// print Banner style console messages
/// </summary>
public static class ConsoleBanner
    {
        private static bool _initialized = false;
        private static readonly Dictionary<char, string> _letters = new Dictionary<char, string>();

        /// <summary>
        /// write a text as banner into the console
        /// </summary>
        /// <param name="text"></param>
        public static void Write(string text) =>
            Write(text, null, null);

        /// <summary>
        /// write a text as banner into the console
        /// </summary>
        /// <param name="text"></param>
        /// <param name="foregroundColor"></param>
        public static void Write(string text, ConsoleColor? foregroundColor) =>
            Write(text, foregroundColor, null);

        /// <summary>
        /// write text as a banner into the console.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="foregroundColor"></param>
        /// <param name="backgroundColor"></param>
        public static void Write(string text, ConsoleColor? foregroundColor, ConsoleColor? backgroundColor)
        {
            var oldColor = Console.ForegroundColor;
            var bgColor = Console.BackgroundColor;
            foregroundColor ??= oldColor ;
            backgroundColor ??= bgColor ;
            Console.ForegroundColor = foregroundColor.Value;
            Console.BackgroundColor = backgroundColor.Value;
            Console.Write(Banner(text));
            Console.ForegroundColor = oldColor;
            Console.BackgroundColor = bgColor;
        }

        /// <summary>
        /// returns a banner from provided text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Banner(string text)
        {
            Preload();
            text = text.ToLower();
            var result = "";
            var word = text.Where(chr => _letters.ContainsKey(chr)).Aggregate("", (current, chr) => current + chr);
            for (var i = 0; i < 6; i++)
            {
                foreach (var chr in word)
                {
                    result += _letters[chr].Split('|')[i];
                }

                result += "\n";
            }

            result += "\n"; // carrier after line

            return result;
        }

        private static void Preload()
        {
            if (_initialized)
                return;
            // add letters
            _letters.Add('a', " █████╗ |██╔══██╗|███████║|██╔══██║|██║  ██║|╚═╝  ╚═╝");
            _letters.Add('b', "██████╗ |██╔══██╗|██████╔╝|██╔══██╗|██████╔╝|╚═════╝ ");
            _letters.Add('c', " ██████╗|██╔════╝|██║     |██║     |╚██████╗| ╚═════╝");
            _letters.Add('d', "██████╗ |██╔══██╗|██║  ██║|██║  ██║|██████╔╝|╚═════╝ ");
            _letters.Add('e', "███████╗|██╔════╝|██████╗ |██╔═══╝ |███████╗|╚══════╝");
            _letters.Add('f', "███████╗|██╔════╝|██████╗ |██╔═══╝ |██║     |╚═╝     ");
            _letters.Add('g', " █████╗ |██╔═══╝ |██║████╗|██║  ██║| █████╔╝| ╚════╝ ");
            _letters.Add('h', "██╗  ██╗|██║  ██║|███████║|██╔══██║|██║  ██║|╚═╝  ╚═╝");
            _letters.Add('i', "██╗|██║|██║|██║|██║|╚═╝");
            _letters.Add('j', "██████╗|╚═██╔═╝|  ██║  |  ██║  |████║  |╚═══╝  ");
            _letters.Add('k', "██╗  ██╗|██║██╔═╝|████╔╝  |██╔██╗  |██║  ██╗|╚═╝  ╚═╝");
            _letters.Add('l', "██╗   |██║   |██║   |██║   |█████╗|╚════╝");
            _letters.Add('m', "███╗  ██╗|████╗███║|██╔███╔█║|██║╚══╝█║|██║    █║|╚═╝    ╚╝");
            _letters.Add('n', "███╗  █╗|████╗ █║|██╔██╗█║|██║ ███║|██║  ██║|╚═╝  ╚═╝");
            _letters.Add('o', " █████╗ |██╔══██╗|██║  ██║|██║  ██║|╚█████╔╝| ╚════╝ ");
            _letters.Add('p', "██████╗ |██╔══██╗|██████╔╝|██╔═══╝ |██║     |╚═╝     ");
            _letters.Add('q', " █████╗ |██╔══██╗|██║  ██║|╚█████╔╝| ╚═══██╗|     ╚═╝");
            _letters.Add('r', "██████╗ |██╔══██╗|█████╔═╝|██╔═██╗ |██║  ██╗|╚═╝  ╚═╝");
            _letters.Add('s', "███████╗|██╔════╝|███████╗|╚════██║|███████║|╚══════╝");
            _letters.Add('t', "██████╗|╚═██╔═╝|  ██║  |  ██║  |  ██║  |  ╚═╝  ");
            _letters.Add('u', "██╗  ██╗|██║  ██║|██║  ██║|██║  ██║| █████╔╝| ╚════╝ ");
            _letters.Add('v', "██╗  ██╗|██║  ██║|██║  ██║| ██╗██╔╝|  ███╔╝ |  ╚══╝  ");
            _letters.Add('w', "██╗    ██╗|██║    ██║|██║ █╗ ██║|██║███╗██║|╔███╔███╔╝|╚═══╝╚══╝ ");
            _letters.Add('x', "██╗  ██╗| ██╗██╔╝|  ███╔╝ | ██╔██╗ |██╔╝ ██╗|╚═╝  ╚═╝");
            _letters.Add('y', "██╗  ██╗| ██╗ ██║|  █████║|  ╚══██║|  █████║|  ╚════╝");
            _letters.Add('z', "███████╗|╚═══██╔╝|  ███╔╝ | ██╔═╝  |███████╗|╚══════╝");
            _letters.Add(' ', "     |     |     |     |     |     |    ");
            _letters.Add('.', "     |     |     |████╗|████║|╚═══╝");
            _initialized = true;
        }
    }