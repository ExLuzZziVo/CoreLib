#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

#endregion

namespace CoreLib.CORE.Helpers.StringHelpers.Parsers
{
    [Obfuscation(Exclude = false, Feature = "optimizations:EnumRemoval=false")]
    public enum DocumentType
    {
        [Description("паспорт гражданина РФ")]
        RU_Passport,
        [Description("ВНЖ")]
        RU_Residence,
        [Description("РВП")]
        RU_TempResidence
    }
    public static class DocumentAuthorityParser
    {
        private static readonly Dictionary<string, string> DocumentShortStringsDictionary = new Dictionary<string, string>
        {
            {"Гу", "ГУ"},
            {"Уфмс", "УФМС"},
            {"Оуфмс", "ОУФМС"},
            {"Мро", "МРО"},
            {"Мвд", "МВД"},
            {"Умвд", "УМВД"},
            {"Ом", "ОМ"},
            {"Оп", "ОП"},
            {"Пво", "ПВО"},
            {"Опвс", "ОПВС"},
            {"Мо", "МО"},
            {"Ро", "РО"},
            {"Гом", "ГОМ"},
            {"Ровд", "РОВД"},
            {"Рувд", "РУВД"},
            {"Овд", "ОВД"},
            {"Тп", "ТП"},
            {"Пс", "ПС"},
            {"Мп", "МП"},
            {"Рф", "РФ"},
            {"Пвс", "ПВС"},
            {"Овм", "ОВМ"},
            {"Увм", "УВМ"},
            {"Увд", "УВД"},
            {"Говд", "ГОВД"},
            {"Зао", "ЗАО"},
            {"Юзао", "ЮЗАО"},
            {"Сзао", "СЗАО"},
            {"Сао", "САО"},
            {"Юао", "ЮАО"},
            {"Вао", "ВАО"},
            {"Ювао", "ЮВАО"},
            {"Свао", "СВАО"},
            {"Район", "р-н"},
            {"Р-н", "р-н"},
            {"Р-Н", "р-н"},
            {"Района", "р-на"},
            {"Р-на", "р-на"},
            {"Р-На", "р-на"},
            {"Району", "р-ну"},
            {"Р-ну", "р-ну"},
            {"Р-Ну", "р-ну"},
            {"Районом", "р-ном"},
            {"Р-ном", "р-ном"},
            {"Р-Ном", "р-ном"},
            {"Районе", "р-не"},
            {"Р-не", "р-не"},
            {"Р-Не", "р-не"},
            {"Область", "обл."},
            {"Области", "обл."},
            {"Областью", "обл."},
            {"Обл.", "обл."},
            {"Край", "край"},
            {"Края", "края"},
            {"Краю", "краю"},
            {"Краем", "краем"},
            {"Крае", "крае"},
            {"Город", "гор."},
            {"Города", "гор."},
            {"Городу", "гор."},
            {"Городом", "гор."},
            {"Городе", "гор."},
            {"Гор.", "гор."},
            {"Республика", "респ."},
            {"Республики", "респ."},
            {"Республике", "респ."},
            {"Республику", "респ."},
            {"Республикой", "респ."},
            {"Респ.", "респ."},
            {"В", "в"},
            {"И", "и"},
            {"По", "по"},
            {"Г.о.", "г. о."},
            {"Г.п.", "г. п."},
            {"Го.", "г. о."},
            {"Гп.", "г. п."},
            {"Го", "г. о."},
            {"Гп", "г. п."},
            {"Кр.", "кр."},
            {"Г.", "г."},
            {"О.", "о."},
            {"П.", "п."},
            {"Пос.", "пос."},
            {"Р.", "р."}
        };
        private static readonly Dictionary<string,string> DocumentLongStringsDictionary = new Dictionary<string, string>
        {
            {"Городской Округ", "г. о."},
            {"Городского Округа", "г. о."},
            {"Городскому Округу", "г. о."},
            {"Городским Округом", "г. о."},
            {"Городском Округе", "г. о."},
            {"Округ", "округ"},
            {"Округа", "округа"},
            {"Округу", "округу"},
            {"Округом", "округом"},
            {"Округе", "округе"},
            {"Городское Поселение", "г. п."},
            {"Городского Поселения", "г. п."},
            {"Городскому Поселению", "г. п."},
            {"Городским Поселением", "г. п."},
            {"Городском Поселении", "г. п."},
            {"Поселение", "поселение"},
            {"Поселения", "поселения"},
            {"Поселению", "поселению"},
            {"Поселением", "поселением"},
            {"Поселении", "поселении"},
            {"Московской обл.", "МО"},
            {"Отделением Милиции", "ОМ"},
            {"Отделом Милиции", "ОМ"},
            {"Отдел Милиции", "ОМ"},
            {"Городским Отделением Милиции", "ГОМ"},
            {"Городским Отделом Милиции", "ГОМ"},
            {"Городской Отдел Милиции", "ГОМ"},
            {"Мр ОУФМС", "МРО УФМС"},
            {"Отделом Внутренних Дел", "ОВД"},
            {"Паспортным Столом", "ПС"},
            {"Межрайонным Отделом", "МРО"},
            {"Территориальным Пунктом", "ТП"},
            {"Миграционным Пунктом", "МП"}
        };
        public static string FormatDocumentAuthority(string source)
        {
            if (source.IsNullOrEmptyOrWhiteSpace())
                return string.Empty;
            source = source.FormatText().ToUpperAllFirstChars();
            var strArray = source.Split(' ');
            source = string.Empty;
            var i = 0;
            foreach (var str in strArray)
            {
                if (str == string.Empty)
                    continue;
                foreach (var d in DocumentShortStringsDictionary.Where(d => str == d.Key))
                {
                    strArray[i] = d.Value;
                    break;
                }
                source += $"{(i == 0 ? "" : " ")}{strArray[i]}";
                i++;
            }
            foreach (var e in DocumentLongStringsDictionary) source = source.Replace(e.Key, e.Value);
            return source.TrimWholeString();
        }

        public static string DocumentNumberSeparator(string number, DocumentType documentType)
        {
            if(number.IsNullOrEmptyOrWhiteSpace())
                throw new ArgumentNullException(nameof(number), "Document number must be specified!");
            string tempNumber;
            switch (documentType)
            {
                case DocumentType.RU_Passport:
                    tempNumber = number.Replace(" ", "");
                    if(tempNumber.Length!=10)
                        throw new ArgumentOutOfRangeException(nameof(number), "RU_Passport must be 10 characters length.");
                    return tempNumber.Insert(4, " ");
                case DocumentType.RU_Residence:
                    tempNumber = number.Replace(" ", "");
                    if (tempNumber.Length != 8)
                        throw new ArgumentOutOfRangeException(nameof(number), "RU_Residence must be 8 characters length.");
                    return tempNumber.Insert(2, " ");
                case DocumentType.RU_TempResidence:
                default:
                    return number;
            }
        }
    }
}