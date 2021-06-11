using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BLL_DOTNETCORE
{
    public static class RegexMatcher
    {
        /// <summary>
        /// Parses clicked entity
        /// </summary>
        /// <returns>1 - name of selected wallet, 2 - number of selected wallet</returns>
        public static (string, string) GetWalletNameAndNumber(this string inputLine)
        {
            var toParse = inputLine;
            var name = Regex.Match(toParse, @"name: [A-z]*[a-z]*[0-9]*").Value;
            var number = Regex.Match(toParse, @"number: [0-9]*").Value;
            name = name.Replace("name:", "").Trim();
            number = number.Replace("number:", "").Trim();
            return (name, number);
        }
        public static string GetWalletBalance(this string inputLine)
        {
            var toParse = inputLine;
            var balance = Regex.Match(toParse, @"Balance: [0-9]*").Value;
            balance = balance.Replace("Balance: ", "").Trim();
            return balance;
        }
    }
}
