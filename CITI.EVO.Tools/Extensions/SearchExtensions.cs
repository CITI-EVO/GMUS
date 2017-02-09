using System;
using System.Collections.Generic;
using CITI.EVO.Tools.Helpers;

namespace CITI.EVO.Tools.Extensions
{
    public static class SearchExtensions
    {
        /// <summary>
        /// Return all matched of the pattern in the specified text using the .NET String.indexOf API
        /// </summary>
        /// <param name="text">text to be searched</param>
        /// <param name="pattern">pattern to match</param>
        /// <returns>IEnumerable which returns the indexes of pattern matches</returns>
        public static IEnumerable<int> BCLMatch(this String text, String pattern)
        {
            var boyerMoor = new BoyerMoore(pattern);
            return boyerMoor.BCLMatch(text);
        }

        /// <summary>
        /// Return all matched of the pattern in the specified text using the .NET String.indexOf API
        /// </summary>
        /// <param name="text">text to be searched</param>
        /// <param name="pattern">pattern to match</param>
        /// <param name="startIndex">Index at which search begins</param>
        /// <returns>IEnumerable which returns the indexes of pattern matches</returns>
        public static IEnumerable<int> BCLMatch(this String text, String pattern, int startIndex)
        {
            var boyerMoor = new BoyerMoore(pattern);
            return boyerMoor.BCLMatch(text, startIndex);
        }

        /// <summary>
        /// Return all matches of the pattern in specified text using the Horspool algorithm
        /// </summary>
        /// <param name="text">text to be searched</param>
        /// <param name="pattern">pattern to match</param>
        /// <returns>IEnumerable which returns the indexes of pattern matches</returns>
        public static IEnumerable<int> HorspoolMatch(this String text, String pattern)
        {
            var boyerMoor = new BoyerMoore(pattern);
            return boyerMoor.HorspoolMatch(text);
        }

        /// <summary>
        /// Return all matches of the pattern in specified text using the Horspool algorithm
        /// </summary>
        /// <param name="text">text to be searched</param>
        /// <param name="pattern">pattern to match</param>
        /// <param name="startIndex">Index at which search begins</param>
        /// <returns>IEnumerable which returns the indexes of pattern matches</returns>
        public static IEnumerable<int> HorspoolMatch(this String text, String pattern, int startIndex)
        {
            var boyerMoor = new BoyerMoore(pattern);
            return boyerMoor.HorspoolMatch(text, startIndex);
        }

        /// <summary>
        /// Return all matches of the pattern in specified text using the Boyer-Moore algorithm
        /// </summary>
        /// <param name="text">text to be searched</param>
        /// <param name="pattern">pattern to match</param>
        /// <returns>IEnumerable which returns the indexes of pattern matches</returns>
        public static IEnumerable<int> BoyerMooreMatch(this String text, String pattern)
        {
            var boyerMoor = new BoyerMoore(pattern);
            return boyerMoor.BoyerMooreMatch(text);
        }

        /// <summary>
        /// Return all matches of the pattern in specified text using the Boyer-Moore algorithm
        /// </summary>
        /// <param name="text">text to be searched</param>
        /// <param name="pattern">pattern to match</param>
        /// <param name="startIndex">Index at which search begins</param>
        /// <returns>IEnumerable which returns the indexes of pattern matches</returns>
        public static IEnumerable<int> BoyerMooreMatch(this String text, String pattern, int startIndex)
        {
            var boyerMoor = new BoyerMoore(pattern);
            return boyerMoor.BoyerMooreMatch(text, startIndex);
        }

        /// <summary>
        /// Return all matches of the pattern in specified text using the Turbo Boyer-Moore algorithm
        /// </summary>
        /// <param name="text">text to be searched</param>
        /// <param name="pattern">pattern to match</param>
        /// <returns>IEnumerable which returns the indexes of pattern matches</returns>
        public static IEnumerable<int> TurboBoyerMooreMatch(this String text, String pattern)
        {
            var boyerMoor = new BoyerMoore(pattern);
            return boyerMoor.TurboBoyerMooreMatch(text);
        }

        /// <summary>
        /// Return all matches of the pattern in specified text using the Turbo Boyer-Moore algorithm
        /// </summary>
        /// <param name="text">text to be searched</param>
        /// <param name="pattern">pattern to match</param>
        /// <param name="startIndex">Index at which search begins</param>
        /// <returns>IEnumerable which returns the indexes of pattern matches</returns>
        public static IEnumerable<int> TurboBoyerMooreMatch(this String text, String pattern, int startIndex)
        {
            var boyerMoor = new BoyerMoore(pattern);
            return boyerMoor.TurboBoyerMooreMatch(text, startIndex);
        }

        /// <summary>
        /// Return all matches of the pattern in specified text using the Apostolico-GiancarloMatch algorithm
        /// </summary>
        /// <param name="text">text to be searched</param>
        /// <param name="pattern">pattern to match</param>
        /// <returns>IEnumerable which returns the indexes of pattern matches</returns>
        public static IEnumerable<int> ApostolicoGiancarloMatch(this String text, String pattern)
        {
            var boyerMoor = new BoyerMoore(pattern);
            return boyerMoor.ApostolicoGiancarloMatch(text);
        }

        /// <summary>
        /// Return all matches of the pattern in specified text using the Apostolico-GiancarloMatch algorithm
        /// </summary>
        /// <param name="text">text to be searched</param>
        /// <param name="pattern">pattern to match</param>
        /// <param name="startIndex">Index at which search begins</param>
        /// <returns>IEnumerable which returns the indexes of pattern matches</returns>
        public static IEnumerable<int> ApostolicoGiancarloMatch(this String text, String pattern, int startIndex)
        {
            var boyerMoor = new BoyerMoore(pattern);
            return boyerMoor.ApostolicoGiancarloMatch(text, startIndex);
        }
    }
}
