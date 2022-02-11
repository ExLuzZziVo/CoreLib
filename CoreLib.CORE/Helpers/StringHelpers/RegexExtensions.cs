#region

using System.Globalization;
using System.Text.RegularExpressions;

#endregion

namespace CoreLib.CORE.Helpers.StringHelpers
{
    public static class RegexExtensions
    {
        /// <summary>
        /// Positive integer with random spaces
        /// </summary>
        public const string PositiveNumberWithSpacesPattern = @"^[0-9\s]+$";

        /// <summary>
        /// Positive and negative integers
        /// </summary>
        public const string PositiveAndNegativeNumberPattern = @"^-?[0-9]+$";

        /// <summary>
        /// Positive integers
        /// </summary>
        public const string PositiveNumberPattern = @"^[0-9]+$";

        /// <summary>
        /// ASCII symbols
        /// </summary>
        public const string ASCIIPattern = @"^[\x00-\x7F]*$";

        /// <summary>
        /// HEX
        /// </summary>
        public const string HexPattern = @"(?!00)([a-fA-F0-9]{2})*";
        
        /// <summary>
        /// GTIN
        /// </summary>
        public const string GTINPattern = @"^((\d{8})|(\d{12,14}))$";

        /// <summary>
        /// Positive and negative fractional numbers
        /// </summary>
        public static readonly string PositiveAndNegativeFractionalNumberPattern =
            $"^-?[0-9{CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator}]+$";

        /// <summary>
        /// Positive negative fractional numbers
        /// </summary>
        public static readonly string PositiveFractionalNumberPattern =
            $"^[0-9{CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator}]+$";

        /// <summary>
        /// Positive and negative percents
        /// </summary>
        public static readonly string PositiveAndNegativePercentPattern =
            $"^-?[0-9{CultureInfo.CurrentCulture.NumberFormat.PercentDecimalSeparator}]+$";

        /// <summary>
        /// Positive percents
        /// </summary>
        public static readonly string PositivePercentPattern =
            $"^[0-9{CultureInfo.CurrentCulture.NumberFormat.PercentDecimalSeparator}]+$";

        /// <summary>
        /// Positive and negative decimals
        /// </summary>
        public static readonly string PositiveAndNegativeMoneyPattern =
            $"^-?[0-9{CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator}]+$";

        /// <summary>
        /// Positive decimals
        /// </summary>
        public static readonly string PositiveMoneyPattern =
            $"^[0-9{CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator}]+$";

        /// <summary>
        /// EAN8
        /// </summary>
        public const string BarcodeEAN8Pattern = @"^\d{8}$";

        /// <summary>
        /// EAN13
        /// </summary>
        public const string BarcodeEAN13Pattern = @"^\d{13}$";

        /// <summary>
        /// UPCA
        /// </summary>
        public const string BarcodeUPCAPattern = @"^\d{12}$";

        /// <summary>
        /// UPCE
        /// </summary>
        public const string BarcodeUPCEPattern = @"^\d{6}$";

        /// <summary>
        /// ITF
        /// </summary>
        public const string BarcodeITFPattern = @"^\d{6}$";

        /// <summary>
        /// ITF14
        /// </summary>
        public const string BarcodeITF14Pattern = @"^\d{14}$";

        /// <summary>
        /// CODE39
        /// </summary>
        public const string BarcodeCODE39Pattern = @"^([*]?)(?:[0-9A-Z/.,%$+\s-]+)*(\1)$";

        /// <summary>
        /// CODE93
        /// </summary>
        public const string BarcodeCODE93Pattern =
            @"^[*](?:[0-9A-Z/.,%$+\s-]|(\(\$\)[A-Z])|(\(%\)[A-Z])|(\(\+\)[A-Z])|(\(/\)[A-CF-JLZ])+)*[*]$";

        /// <summary>
        /// CODE128
        /// </summary>
        public const string BarcodeCODE128Pattern = ASCIIPattern;

        /// <summary>
        /// CODABAR
        /// </summary>
        public const string BarcodeCODABARPattern = @"^[0-9A-D/.$+:-]*$";

        /// <summary>
        /// GS1-128
        /// </summary>
        public const string BarcodeGS1_128Pattern = @"^(\(\d{2,4}\)[A-Za-z0-9]+)*$";

        /// <summary>
        /// PDF417
        /// </summary>
        public const string BarcodePDF417Pattern = @"^[\x00-\xFF]*$";

        /// <summary>
        /// CODE39 EXTENDED
        /// </summary>
        public const string BarcodeCODE39_EXTENDEDPattern = ASCIIPattern;

        /// <summary>
        /// IP-address
        /// </summary>
        public const string IpAddressPattern =
            @"^(0[0-7]{10,11}|0(x|X)[0-9a-fA-F]{8}|(\b4\d{8}[0-5]\b|\b[1-3]?\d{8}\d?\b)|((2[0-5][0-5]|1\d{2}|[1-9]\d?)|(0(x|X)[0-9a-fA-F]{2})|(0[0-7]{3}))(\.((2[0-5][0-5]|1\d{2}|\d\d?)|(0(x|X)[0-9a-fA-F]{2})|(0[0-7]{3}))){3})$";

        /// <summary>
        /// IPv6-address
        /// </summary>
        //  https://stackoverflow.com/questions/32368008/regular-expression-that-matches-all-valid-format-ipv6-addresses
        public const string IpV6AddressPattern =
            @"(?:^|(?<=\s))(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){6,6}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}(?::((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))?|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)(?::((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))?|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))(?=\s|$)";
        
        /// <summary>
        /// IP-address with port
        /// </summary>
        public const string IpAddressWithPortPattern =
            @"^(0[0-7]{10,11}|0(x|X)[0-9a-fA-F]{8}|(\b4\d{8}[0-5]\b|\b[1-3]?\d{8}\d?\b)|((2[0-5][0-5]|1\d{2}|[1-9]\d?)|(0(x|X)[0-9a-fA-F]{2})|(0[0-7]{3}))(\.((2[0-5][0-5]|1\d{2}|\d\d?)|(0(x|X)[0-9a-fA-F]{2})|(0[0-7]{3}))){3}):([1-9]{1,4}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]|6553[0-5])$";
        
        /// <summary>
        /// Email address
        /// </summary>
        public const string EmailAddressPattern =
            @"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)";

        /// <summary>
        /// Url
        /// </summary>
        // ToDo Punycode?: ^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?(((?!-))(xn--|_{1,1})?[a-z0-9-]{0,61}[a-z0-9]{1,1}\.)+(xn--)?([a-z0-9][a-z0-9\-]{0,60}|[a-z0-9-]{1,30}\.[a-z]{2,})(\/.*)?$
        public const string UrlPattern =
            @"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$";

        /// <summary>
        /// Base64
        /// </summary>
        public const string Base64Pattern = @"^(?:[A-Za-z\d+/]{4})*(?:[A-Za-z\d+/]{3}=|[A-Za-z\d+/]{2}==)?$";
        
        /// <summary>
        /// Checks if provided string is email address
        /// </summary>
        /// <param name="source">Target string</param>
        /// <returns>True if provided string is email address</returns>
        public static bool IsEmailAddress(this string source)
        {
            return !source.IsNullOrEmptyOrWhiteSpace() &&
                   Regex.IsMatch(source, EmailAddressPattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Checks if provided string is IP-address
        /// </summary>
        /// <param name="source">Target string</param>
        /// <returns>True if provided string is IP-address</returns>
        public static bool IsIpAddress(this string source)
        {
            return !source.IsNullOrEmptyOrWhiteSpace() &&
                   Regex.IsMatch(source, IpAddressPattern, RegexOptions.IgnoreCase);
        }
    }
}