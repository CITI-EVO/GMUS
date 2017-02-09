using System;
using System.Collections.Generic;

namespace CITI.EVO.Tools.Helpers
{
    /// <summary>
    /// Class that implements Boyer-Moore and related exact string-matching algorithms
    /// </summary>
    public class BoyerMoore
    {
        private readonly int[] badCharacterShift;
        private readonly int[] goodSuffixShift;
        private readonly int[] suffixes;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pattern">Pattern for search</param>
        public BoyerMoore(String pattern)
        {
            if (String.IsNullOrEmpty(pattern))
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            Pattern = pattern;

            /* Preprocessing */
            badCharacterShift = BuildBadCharacterShift(pattern);
            suffixes = FindSuffixes(pattern);
            goodSuffixShift = BuildGoodSuffixShift(pattern, suffixes);
        }

        /// <summary>
        /// Pattern
        /// </summary>
        public String Pattern { get; private set; }

        /// <summary>
        /// Build the bad character shift array.
        /// </summary>
        /// <param name="pattern">Pattern for search</param>
        /// <returns>bad character shift array</returns>
        private static int[] BuildBadCharacterShift(String pattern)
        {
            var badCharacterShift = new int[256];

            for (int c = 0; c < badCharacterShift.Length; ++c)
            {
                badCharacterShift[c] = pattern.Length;
            }

            for (int i = 0; i < pattern.Length - 1; ++i)
            {
                badCharacterShift[pattern[i]] = pattern.Length - i - 1;
            }

            return badCharacterShift;
        }

        /// <summary>
        /// Find suffixes in the pattern
        /// </summary>
        /// <param name="pattern">Pattern for search</param>
        /// <returns>Suffix array</returns>
        private static int[] FindSuffixes(String pattern)
        {
            int f = 0;

            int patternLength = pattern.Length;
            var suffixes = new int[pattern.Length + 1];

            suffixes[patternLength - 1] = patternLength;
            int g = patternLength - 1;

            for (int i = patternLength - 2; i >= 0; --i)
            {
                if (i > g && suffixes[i + patternLength - 1 - f] < i - g)
                {
                    suffixes[i] = suffixes[i + patternLength - 1 - f];
                }
                else
                {
                    if (i < g)
                    {
                        g = i;
                    }

                    f = i;
                    while (g >= 0 && (pattern[g] == pattern[g + patternLength - 1 - f]))
                    {
                        --g;
                    }

                    suffixes[i] = f - g;
                }
            }

            return suffixes;
        }

        /// <summary>
        /// Build the good suffix array.
        /// </summary>
        /// <param name="pattern">Pattern for search</param>
        /// <param name="suff"></param>
        /// <returns>Good suffix shift array</returns>
        private static int[] BuildGoodSuffixShift(String pattern, int[] suff)
        {
            int patternLength = pattern.Length;
            var goodSuffixShift = new int[pattern.Length + 1];

            for (int i = 0; i < patternLength; ++i)
            {
                goodSuffixShift[i] = patternLength;
            }

            int j = 0;
            for (int i = patternLength - 1; i >= -1; --i)
            {
                if (i == -1 || suff[i] == i + 1)
                {
                    for (; j < patternLength - 1 - i; ++j)
                    {
                        if (goodSuffixShift[j] == patternLength)
                        {
                            goodSuffixShift[j] = patternLength - 1 - i;
                        }
                    }
                }
            }

            for (int i = 0; i <= patternLength - 2; ++i)
            {
                goodSuffixShift[patternLength - 1 - suff[i]] = patternLength - 1 - i;
            }

            return goodSuffixShift;
        }

        /// <summary>
        /// Return all matched of the pattern in the specified text using the .NET String.indexOf API
        /// </summary>
        /// <param name="text">text to be searched</param>
        /// <returns>IEnumerable which returns the indexes of pattern matches</returns>
        public IEnumerable<int> BCLMatch(String text)
        {
            return BCLMatch(text, 0);
        }

        /// <summary>
        /// Return all matched of the pattern in the specified text using the .NET String.indexOf API
        /// </summary>
        /// <param name="text">text to be searched</param>
        /// <param name="startingIndex">Index at which search begins</param>
        /// <returns>IEnumerable which returns the indexes of pattern matches</returns>
        public IEnumerable<int> BCLMatch(String text, int startingIndex)
        {
            int patternLength = Pattern.Length;
            int index = startingIndex;

            //var indexSet = new SortedSet<int>();

            while ((index = text.IndexOf(Pattern, index, StringComparison.InvariantCultureIgnoreCase)) > -1)
            {
                yield return index;

                //indexSet.Add(index);
                index += patternLength;
            }

            //return indexSet;
        }

        /// <summary>
        /// Return all matches of the pattern in specified text using the Horspool algorithm
        /// </summary>
        /// <param name="text">text to be searched</param>
        /// <returns>IEnumerable which returns the indexes of pattern matches</returns>
        public IEnumerable<int> HorspoolMatch(String text)
        {
            return HorspoolMatch(text, 0);
        }

        /// <summary>
        /// Return all matches of the pattern in specified text using the Horspool algorithm
        /// </summary>
        /// <param name="text">text to be searched</param>
        /// <param name="startingIndex">Index at which search begins</param>
        /// <returns>IEnumerable which returns the indexes of pattern matches</returns>
        public IEnumerable<int> HorspoolMatch(String text, int startingIndex)
        {
            int patternLength = Pattern.Length;
            int textLength = text.Length;

            //var indexSet = new SortedSet<int>();

            /* Searching */
            int index = startingIndex;
            while (index <= textLength - patternLength)
            {
                int unmatched;
                for (unmatched = patternLength - 1; unmatched >= 0 && Pattern[unmatched] == text[unmatched + index]; --unmatched)
                {
                }

                if (unmatched < 0)
                {
                    yield return index;

                    //indexSet.Add(index);

                    unmatched = 0;
                }

                index += badCharacterShift[text[unmatched + patternLength - 1]];
            }

            //return indexSet;
        }

        /// <summary>
        /// Return all matches of the pattern in specified text using the Boyer-Moore algorithm
        /// </summary>
        /// <param name="text">text to be searched</param>
        /// <returns>IEnumerable which returns the indexes of pattern matches</returns>
        public IEnumerable<int> BoyerMooreMatch(String text)
        {
            return BoyerMooreMatch(text, 0);
        }

        /// <summary>
        /// Return all matches of the pattern in specified text using the Boyer-Moore algorithm
        /// </summary>
        /// <param name="text">text to be searched</param>
        /// <param name="startingIndex">Index at which search begins</param>
        /// <returns>IEnumerable which returns the indexes of pattern matches</returns>
        public IEnumerable<int> BoyerMooreMatch(String text, int startingIndex)
        {
            int patternLength = Pattern.Length;
            int textLength = text.Length;

            //var indexSet = new SortedSet<int>();

            /* Searching */
            int index = startingIndex;
            while (index <= textLength - patternLength)
            {
                int unmatched;
                for (unmatched = patternLength - 1; unmatched >= 0 && (Pattern[unmatched] == text[unmatched + index]); --unmatched)
                {
                }

                if (unmatched < 0)
                {
                    yield return index;

                    //indexSet.Add(index);

                    index += goodSuffixShift[0];
                }
                else
                {
                    index += Math.Max(goodSuffixShift[unmatched], badCharacterShift[text[unmatched + index]] - patternLength + 1 + unmatched);
                }
            }

            //return indexSet;
        }

        /// <summary>
        /// Return all matches of the pattern in specified text using the Turbo Boyer-Moore algorithm
        /// </summary>
        /// <param name="text">text to be searched</param>
        /// <returns>IEnumerable which returns the indexes of pattern matches</returns>
        public IEnumerable<int> TurboBoyerMooreMatch(String text)
        {
            return TurboBoyerMooreMatch(text, 0);
        }

        /// <summary>
        /// Return all matches of the pattern in specified text using the Turbo Boyer-Moore algorithm
        /// </summary>
        /// <param name="text">text to be searched</param>
        /// <param name="startingIndex">Index at which search begins</param>
        /// <returns>IEnumerable which returns the indexes of pattern matches</returns>
        public IEnumerable<int> TurboBoyerMooreMatch(String text, int startingIndex)
        {
            int patternLength = Pattern.Length;
            int textLength = text.Length;

            //var indexSet = new SortedSet<int>();

            /* Searching */
            int index = startingIndex;
            int overlap = 0;
            int shift = patternLength;

            while (index <= textLength - patternLength)
            {
                int unmatched = patternLength - 1;

                while (unmatched >= 0 && (Pattern[unmatched] == text[unmatched + index]))
                {
                    --unmatched;
                    if (overlap != 0 && unmatched == patternLength - 1 - shift)
                    {
                        unmatched -= overlap;
                    }
                }

                if (unmatched < 0)
                {
                    yield return index;

                    //indexSet.Add(index);

                    shift = goodSuffixShift[0];
                    overlap = patternLength - shift;
                }
                else
                {
                    int matched = patternLength - 1 - unmatched;
                    int turboShift = overlap - matched;
                    int bcShift = badCharacterShift[text[unmatched + index]] - patternLength + 1 + unmatched;

                    shift = Math.Max(turboShift, bcShift);
                    shift = Math.Max(shift, goodSuffixShift[unmatched]);

                    if (shift == goodSuffixShift[unmatched])
                    {
                        overlap = Math.Min(patternLength - shift, matched);
                    }
                    else
                    {
                        if (turboShift < bcShift)
                        {
                            shift = Math.Max(shift, overlap + 1);
                        }

                        overlap = 0;
                    }
                }

                index += shift;
            }

            //return indexSet;
        }

        /// <summary>
        /// Return all matches of the pattern in specified text using the Apostolico-GiancarloMatch algorithm
        /// </summary>
        /// <param name="text">text to be searched</param>
        /// <returns>IEnumerable which returns the indexes of pattern matches</returns>
        public IEnumerable<int> ApostolicoGiancarloMatch(String text)
        {
            return ApostolicoGiancarloMatch(text, 0);
        }

        /// <summary>
        /// Return all matches of the pattern in specified text using the Apostolico-GiancarloMatch algorithm
        /// </summary>
        /// <param name="text">text to be searched</param>
        /// <param name="startingIndex">Index at which search begins</param>
        /// <returns>IEnumerable which returns the indexes of pattern matches</returns>
        public IEnumerable<int> ApostolicoGiancarloMatch(String text, int startingIndex)
        {
            int patternLength = Pattern.Length;
            int textLength = text.Length;
            var skip = new int[patternLength];

            //var indexSet = new SortedSet<int>();

            /* Searching */
            int index = startingIndex;
            while (index <= textLength - patternLength)
            {
                int unmatched = patternLength - 1;
                while (unmatched >= 0)
                {
                    int skipLength = skip[unmatched];
                    int suffixLength = suffixes[unmatched];

                    if (skipLength > 0)
                    {
                        if (skipLength > suffixLength)
                        {
                            if (unmatched + 1 == suffixLength)
                            {
                                unmatched = (-1);
                            }
                            else
                            {
                                unmatched -= suffixLength;
                            }

                            break;
                        }

                        unmatched -= skipLength;
                        if (skipLength < suffixLength)
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (Pattern[unmatched] == text[unmatched + index])
                        {
                            --unmatched;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                int shift;
                if (unmatched < 0)
                {
                    yield return index;

                    //indexSet.Add(index);

                    skip[patternLength - 1] = patternLength;
                    shift = goodSuffixShift[0];
                }
                else
                {
                    skip[patternLength - 1] = patternLength - 1 - unmatched;
                    shift = Math.Max(goodSuffixShift[unmatched], badCharacterShift[text[unmatched + index]] - patternLength + 1 + unmatched);
                }

                index += shift;

                for (int copy = 0; copy < patternLength - shift; ++copy)
                {
                    skip[copy] = skip[copy + shift];
                }

                for (int clear = 0; clear < shift; ++clear)
                {
                    skip[patternLength - shift + clear] = 0;
                }
            }

            //return indexSet;
        }
    }
}
