using System;
using System.Collections.Generic;
using System.Text;

namespace BananaBot
{
    class Util
    {
        public static string Substring(string input, int start, int end)
        {
            string result = "";

            if (input.Length < end)
                return input;
            else if (start < 0 || start > end)
                return input;
            else
            {
                for (int i = start; i < end; i++)
                {
                    result += input.ToCharArray()[i];
                }
            }
            return result;
        }
    }
}
