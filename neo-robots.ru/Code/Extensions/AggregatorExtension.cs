using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SMI.Code.Extensions
{
    public static class AggregatorExtension
    {
        /// <summary>
        /// Очищает URL-адрес от www
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string ClearUrl(this string url)
        {
            string str = url?.ToLower().Trim();

            if(string.IsNullOrEmpty(str))
                return "";

            return str;
        }

        public static bool IsValidateUrl(this string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
