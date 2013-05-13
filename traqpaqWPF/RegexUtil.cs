using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace traqpaqWPF
{
    public enum PHPreturn
    {
        USERNAME_EXISTS = 0,
        USERNAME_DNE = 1,
        EMPTY_STRING_ERROR = 2,
        PDO_EXCEPTION = 3,
        REGEX_PARSE_ERROR = 4,
        LOGIN_SUCCESSFUL = 5,
        INCORRECT_PASSWORD = 6
    }

    public static class RegexUtil
    {
        //bool invalid = false;

        public static bool IsValidEmail(string strIn)
        {
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names. 
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", DomainMapper,
                                    RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            // Return true if strIn is in valid e-mail format. 
            try
            {
                return Regex.IsMatch(strIn,
                    @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private static string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                //invalid = true;
                throw;
            }
            return match.Groups[1].Value + domainName;
        }
    }

    public static class PHP
    {
        /// <summary>
        /// Parse the result string returned from PHP using regex
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static PHPreturn get_PHP_return(string result)
        {
            Regex rx = new Regex(@"(?<=\[)\d(?=\])");
            Match match = rx.Match(result);
            int value;
            if (match.Success && int.TryParse(match.Value, out value))
            {
                return (PHPreturn)value;
            }
            else return PHPreturn.REGEX_PARSE_ERROR;
        }
    }
}
