using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mi.PE.Internal
{
    internal static class Format
    {
        public static string ToString(byte b)
        {
            if ((b >= '0' && b <= '9')
                || (b >= 'A' && b <= 'Z')
                || (b >= 'a' && b <= 'z'))
                return "'" + (char)b + "'";
            else if (b == (byte)'\r')
                return "<CR>";
            else if (b == (byte)'\n')
                return "<LF>";
            else
                return b.ToString("X2") + "h";
        }

        public static string ToString(ushort u)
        {
            byte b0 = unchecked((byte)(u));
            byte b1 = unchecked((byte)(u >> 8));

            return ToString(b1, b0);
        }

        public static string ToString(uint u)
        {
            byte b0 = unchecked((byte)(u));
            byte b1 = unchecked((byte)(u >> 8));
            byte b2 = unchecked((byte)(u >> 16));
            byte b3 = unchecked((byte)(u >> 24));

            return ToString(b3, b2, b1, b0);
        }

        public static string ToString(byte b0, byte b1)
        {
            string result0 = ToString(b0);
            string result1 = ToString(b1);

            string result = result0 + result1;

            result =
                result.Replace("h", "").Replace("''", "") +
                (result.IndexOf('h') >= 0 ? "h" : "");

            return result;
        }

        public static string ToString(byte b0, byte b1, byte b2, byte b3)
        {
            string result0 = ToString(b0, b1);
            string result1 = ToString(b2, b3);

            string result = result0 + result1;

            result =
                result.Replace("h", "").Replace("''", "") +
                (result.IndexOf('h') >= 0 ? "h" : "");

            return result;
        }
    }
}