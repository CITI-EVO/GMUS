using System;
using System.Numerics;
using System.Text;

namespace CITI.EVO.Tools.Helpers
{
    public static class NumberBaseConverter
    {
        private static readonly char[] chars = {
                                                  '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 

                                                  'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
                                                  'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',

                                                  '(', ')', '[', ']', ';', ',', '.', '{', '}', '<', '>', '`', '-', '=', '~', '!', '@', '#', '$', '%', '^', '&', '*', '_', '+', ':', 
                                                  '/', '"', '|', '?', '\'', '\\',

                                                  '☺', '☻', '♥', '♦', '♣', '♠', '•', '◘', '○', '◙', '♂', '♀', '♪', '♫', '☼', '►', '◄', '↕', '‼', '¶', '§', '▬', '↨', '↑', '↓', '→',
                                                  '←', '∟', '↔', '▲', '▼', 'é', 'â', 'ä', 'à', 'å', 'ç', 'ê', 'ë', 'è', 'ï', 'î', 'ì', 'Ä', 'Å', 'É', 'æ', 'Æ', 'ô', 'ö', 'ò', 'û',
                                                  'ù', 'ÿ', 'Ö', 'Ü', '¢', '£', '¥', '₧', 'ƒ', 'á', 'í', 'ó', 'ú', 'ñ', 'Ñ', 'ª', 'º', '¿', '⌐', '¬', '½', '¼', '¡', '«', '»', '╚', 
                                                  '╔', '╩', '╦', '╠', '═', '╬', '╧', '╨', '╤', '╥', '╙', '╒', '╓', '╫', '╪', '┘', '┌', '█', '▄', '▌', '▐', '▀', 'α', 'ß', 'Γ', 'π', 
                                                  'Σ', 'σ', 'µ', 'τ', 'Φ', 'Θ', 'Ω', 'δ', '∞', 'φ', 'ε', '∩', '≡', '±', '≤', '⌠', '⌡', '÷', '≈', '°', '∙', '·', '√', 'ⁿ', '²', '■', 
                                              };

        public static int MaxBase
        {
            get { return chars.Length; }
        }

        public static char[] Chars
        {
            get
            {
                var array = new char[chars.Length];
                Array.Copy(chars, array, array.Length);

                return array;
            }
        }

        public static String ChangeBase(String n, int fromBase, int toBase)
        {
            var number = ConvertFromBase(n, fromBase);
            return ConvertToBase(number, toBase);
        }

        public static String ConvertToBase(BigInteger n, int @base)
        {
            var result = new StringBuilder();

            while (n > 0)
            {
                BigInteger rem;
                n = BigInteger.DivRem(n, @base, out rem);

                var index = (int)rem;
                if (index >= @base)
                {
                    throw new Exception();
                }

                var @char = chars[index];

                result.Insert(0, @char);
            }

            return result.ToString();
        }

        public static BigInteger ConvertFromBase(String n, int @base)
        {
            var result = (BigInteger)0;

            for (int i = 0; i < n.Length; i++)
            {
                var exponent = n.Length - i - 1;

                var @char = n[i];

                var index = Array.IndexOf(chars, @char);
                if (index >= @base)
                {
                    throw new Exception();
                }

                result += index * BigInteger.Pow(@base, exponent);
            }

            return result;
        }
    }


}
