using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SMI.Code.Extensions
{
    public static class StringExtension
    {
        public static string ToPath(this string title)
        {
            string str = title?.ToLower();

            if(string.IsNullOrEmpty(str))
                return "";

            str = Regex.Replace(str, @"\W", "_", RegexOptions.Multiline);
            str = Regex.Replace(str, @"_+", "-", RegexOptions.Multiline);
            Dictionary<string, string> wordsreplace = new Dictionary<string, string>() {
                {"а","a"},{"б","b"},{"в","v"},{"г","g"},{"д","d"},{"е","e"},{"ё","e"},{"ж","j"},{"з","z"},
                {"и","i"},{"й","i"},{"к","k"},{"л","l"},{"м","m"},{"н","n"},{"о","o"},{"п","p"},{"р","r"},
                {"с","s"},{"т","t"},{"у","u"},{"ф","f"},{"х","h"},{"ц","ts"},{"ч","ch"},{"ш","sh"},{"щ","sch"},
                {"ъ",""},{"ы","i"},{"ь",""},{"э","e"},{"ю","yu"},{"я","ya"}
            };

            string aliasReturn = "";
            foreach (char ch in str)
                aliasReturn += (wordsreplace.ContainsKey(ch.ToString())) ? wordsreplace[ch.ToString()] : ch.ToString();

            return aliasReturn.Trim('-');
        }
    }
}
