using System;
using System.Collections.Generic;
using System.Text;

namespace BLL_DOTNETCORE
{
    public static class DSCounter
    {
        public static int DS_Count(string s)
        {
            string substr = System
                                .Globalization
                                .CultureInfo
                                .CurrentCulture
                                .NumberFormat
                                .NumberDecimalSeparator[0].ToString();
            int count = (s.Length - s.Replace(substr, "").Length) / substr.Length;
            return count;
        }
    }
}
