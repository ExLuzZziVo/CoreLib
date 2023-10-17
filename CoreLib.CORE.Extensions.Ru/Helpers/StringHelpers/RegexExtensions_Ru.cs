#region

using System.Text.RegularExpressions;

#endregion

namespace CoreLib.CORE.Helpers.StringHelpers
{
    public static class RegexExtensions_Ru
    {
        /// <summary>
        /// ИНН физ. лица/ИП
        /// </summary>
        public const string PhysicalVatinPattern = @"^\d{12}$";

        /// <summary>
        /// ИНН юр. лица
        /// </summary>
        public const string EntityVatinPattern = @"^\d{10}$";

        /// <summary>
        /// ИНН
        /// </summary>
        public const string VatinPattern = EntityVatinPattern + "|" + PhysicalVatinPattern;

        /// <summary>
        /// КПП
        /// </summary>
        public const string KPPPattern = @"^\d{9}$";

        /// <summary>
        /// БИК
        /// </summary>
        public const string BIKPattern = @"^\d{9}$";

        /// <summary>
        /// ОГРН
        /// </summary>
        public const string OGRNPattern = @"^\d{13}$";

        /// <summary>
        /// ОКПО
        /// </summary>
        public const string OKPOPattern = @"^\d{8}$|^\d{10}$";

        /// <summary>
        /// ОКАТО
        /// </summary>
        public const string OKATOPattern = @"^\d{2,11}$";

        /// <summary>
        /// ОКТМО
        /// </summary>
        public const string OKTMOPattern = @"^\d{8}$|^\d{11}$";

        /// <summary>
        /// ОКВЭД
        /// </summary>
        public const string OKVEDPattern =
            @"^(?:((\d{2}((\.\d{1,3})){0,2})))((?:(?:;)((\d{2}((\.\d{1,3})){0,2})))){0,20}$";

        /// <summary>
        /// Расчетный/Корреспондентский счет
        /// </summary>
        public const string BankAccountPattern = @"^\d{20}$";

        /// <summary>
        /// КБК
        /// </summary>
        public const string KBKPattern = @"^\d{20}$";

        /// <summary>
        /// Код маркировки меховых изделий
        /// </summary>
        public const string BarcodeFurPattern = @"^[A-Z]{2}-[0-9]{6}-[A-Z]{10}$";

        /// <summary>
        /// ЕГАИС-2.0
        /// </summary>
        public const string BarcodeEgais20Pattern = @"^[0-9A-Z]{68}$";

        /// <summary>
        /// ЕГАИС-3.0
        /// </summary>
        public const string BarcodeEgais30Pattern = @"^[0-9A-Z]{150}$";

        /// <summary>
        /// Российский номер телефона
        /// </summary>
        public const string RussianPhoneNumberPattern = @"\+7-?\(?\d{3}\)?-? *\d{3}-? *-?\d{2} *-?\d{2}";

        /// <summary>
        /// Url-адрес, включая кириллицу
        /// </summary>
        public const string UrlPattern =
            @"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?(((?<!\/|^)[\-\.]{1})?([a-zа-я0-9]+))*\.[a-zа-я]{2,5}(:[0-9]{1,5})?(\/.*)?$";

        /// <summary>
        /// Номер российского паспорта
        /// </summary>
        public const string RussianPassportPattern = @"^\d{4}\s\d{6}$";

        /// <summary>
        /// Номер российского загранпаспорта
        /// </summary>
        public const string RussianTravelPassportPattern = @"^\d{2}\s\d{7}$";

        /// <summary>
        /// Номер российского ВНЖ
        /// </summary>
        public const string RussianResidencePermitPattern = @"^\d{2}\s\d{7}$";

        /// <summary>
        /// Проверяет, является ли значение строки российским номером телефона
        /// </summary>
        /// <param name="source">Строка для проверки</param>
        /// <returns>Истину, если значение строки является российским номером телефона</returns>
        public static bool IsRussianPhoneNumber(this string source)
        {
            return !source.IsNullOrEmptyOrWhiteSpace() && Regex.IsMatch(source,
                RussianPhoneNumberPattern, RegexOptions.IgnoreCase);
        }
    }
}