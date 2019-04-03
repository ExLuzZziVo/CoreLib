#region

using System.Globalization;
using System.Text.RegularExpressions;

#endregion

namespace UIServiceLib.CORE.Helpers.StringHelpers
{
    public static class RegexExtensions
    {
        public const string PositiveNumberWithSpacesRegExp = "[^0-9 ]+";

        public const string PositiveAndNegativeNumberRegExp = "[^0-9-]+";

        public const string PositiveNumberRegExp = "[^0-9]+";

        public const string IpAddressRegExp =
            @"^(0[0-7]{10,11}|0(x|X)[0-9a-fA-F]{8}|(\b4\d{8}[0-5]\b|\b[1-3]?\d{8}\d?\b)|((2[0-5][0-5]|1\d{2}|[1-9]\d?)|(0(x|X)[0-9a-fA-F]{2})|(0[0-7]{3}))(\.((2[0-5][0-5]|1\d{2}|\d\d?)|(0(x|X)[0-9a-fA-F]{2})|(0[0-7]{3}))){3})$";

        public const string EmailAddressRegExp =
            @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

        public const string RussianPhoneNumberRegExp = @"\+7-?\(?\d{3}\)?-? *\d{3}-? *-?\d{2} *-?\d{2}";

        public static readonly string PositiveAndNegativeFractionalNumberRegExp =
            $"[^0-9{CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator}-]+";

        public static readonly string PositiveFractionalNumberRegExp =
            $"[^0-9{CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator}]+";

        public static readonly string PositiveAndNegativePercentRegExp =
            $"[^0-9{CultureInfo.CurrentCulture.NumberFormat.PercentDecimalSeparator}-]+";

        public static readonly string PositivePercentRegExp =
            $"[^0-9{CultureInfo.CurrentCulture.NumberFormat.PercentDecimalSeparator}]+";

        public static readonly string PositiveAndNegativeMoneyRegExp =
            $"[^0-9{CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator}-]+";

        public static readonly string PositiveMoneyRegExp =
            $"[^0-9{CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator}]+";

        public static bool IsRussianPhoneNumber(this string source)
        {
            return !source.IsNullOrEmptyOrWhiteSpace() && Regex.IsMatch(source,
                       RussianPhoneNumberRegExp, RegexOptions.IgnoreCase);
        }

        public static bool IsEmailAddress(this string source)
        {
            return !source.IsNullOrEmptyOrWhiteSpace() &&
                   Regex.IsMatch(source, EmailAddressRegExp, RegexOptions.IgnoreCase);
        }

        public static bool IsIpAddress(this string source)
        {
            return !source.IsNullOrEmptyOrWhiteSpace() &&
                   Regex.IsMatch(source, IpAddressRegExp, RegexOptions.IgnoreCase);
        }
    }
}