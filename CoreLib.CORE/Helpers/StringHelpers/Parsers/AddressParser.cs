﻿#region

using System.Collections.Generic;
using System.Text.RegularExpressions;

#endregion

namespace CoreLib.CORE.Helpers.StringHelpers.Parsers
{
    public static class AddressParser
    {
        private static readonly Dictionary<string, string> AddressLongNamesDictionary = new Dictionary<string, string>
        {
            {" Район ", " р-н "},
            {" Территория ", " тер. "},
            {" Улус ", " У. "},
            {" аал ", " аал "},
            {" автодорога ", " автодорога "},
            {" аллея ", " аллея "},
            {" арбан ", " арбан "},
            {" аул ", " аул "},
            {" бульвар ", " б-р "},
            {" вал ", " вал "},
            {" волость ", " волость "},
            {" въезд ", " въезд "},
            {" выселки ", " высел. "},
            {" выселки(ок) ", " высел "},
            {" выселок ", " высел. "},
            {" гаражно - строительный кооператив ", " ГСК "},
            {" гаражно-строительный кооператив ", " ГСК "},
            {" городок ", " городок "},
            {" город ", " гор. "},
            {" городской округ ", " г/о "},
            {" городское поселение ", " г. п. "},
            {" дачный поселок ", " дп. "},
            {" деревня ", " дер. "},
            {" дорога ", " дор. "},
            {" железнодорожная будка ", " ж/д_будка "},
            {" железнодорожная казарма ", " ж/д_казарм "},
            {" железнодорожная платформа ", " ж/д_платф "},
            {" железнодорожная станция ", " ж/д_ст "},
            {" железнодорожный пост ", " ж/д_пост "},
            {" железнодорожный разъезд ", " ж/д_рзд "},
            {" животноводческая точка ", " жт. "},
            {" жилой район ", " жилрайон "},
            {" ж/д обгонный пункт ", " ж/д_оп "},
            {" ж/д остановочный пункт ", " ж/д_оп "},
            {" ж/д остановочный(обгонный) пункт ", " ж/д_оп "},
            {" заезд ", " заезд "},
            {" имени ", " Им. "},
            {" метро ", " м. "},
            {" заимка ", " заимка "},
            {" казарма ", " казарма "},
            {" канал ", " канал "},
            {" край ", " кр. "},
            {" квартал ", " кв-л "},
            {" квартира ", " кв. "},
            {" километр ", " км. "},
            {" кольцо ", " кольцо "},
            {" комната ", " ком. "},
            {" кордон ", " кордон "},
            {" корпус ", " корп. "},
            {" коса ", " коса "},
            {" курортный поселок ", " кп. "},
            {" леспромхоз ", " лпх. "},
            {" линия ", " линия "},
            {" массив ", " массив "},
            {" местечко ", " мест. "},
            {" микрорайон ", " мкр. "},
            {" мост ", " мост "},
            {" набережная ", " наб. "},
            {" населенный пункт ", " нп. "},
            {" остров ", " остров "},
            {" область ", " обл. "},
            {" парк ", " парк "},
            {" переезд ", " переезд "},
            {" переулок ", " пер. "},
            {" планировочный район ", " п/р "},
            {" платформа ", " платф. "},
            {" площадка ", " пл-ка "},
            {" площадь ", " пл. "},
            {" погост ", " погост "},
            {" полустанок ", " полустанок "},
            {" поселок городского типа ", " пгт. "},
            {" поселок и станция ", " п/ст "},
            {" поселок и(при) станция(и) ", " п/ст "},
            {" поселок при станции ", " п/ст "},
            {" поселок ", " пос. "},
            {" починок ", " починок "},
            {" почтовое отделение ", " п/о "},
            {" проезд ", " проезд "},
            {" промышленная зона ", " промзона "},
            {" просек ", " просек "},
            {" проселок ", " проселок "},
            {" проспект ", " пр-кт "},
            {" проток ", " проток "},
            {" проулок ", " проулок "},
            {" рабочий поселок ", " рп. "},
            {" разъезд ", " рзд. "},
            {" ряды ", " ряды "},
            {" река ", " рек. "},
            {" реcпублика ", " респ. "},
            {" садовое некоммерческое товарищество ", " СНТ "},
            {" сад ", " сад "},
            {" село ", " с. "},
            {" сельская администрация ", " с/а "},
            {" сельский округ ", " с/о "},
            {" сельское муницип. образование ", " с/мо "},
            {" сельское поселение ", " с/п "},
            {" сельсовет ", " с/с "},
            {" сквер ", " сквер "},
            {" слобода ", " сл. "},
            {" спуск ", " спуск "},
            {" станица ", " ст-ца "},
            {" станция ", " ст. "},
            {" строение ", " стр. "},
            {" сумон ", " сумон "},
            {" территория ", " тер. "},
            {" тракт ", " тракт "},
            {" тупик ", " туп. "},
            {" улица ", " ул. "},
            {" улус ", " У. "},
            {" участок ", " уч-к "},
            {" ферма ", " ферма "},
            {" хутор ", " х. "},
            {" шоссе ", " ш. "}
        };

        private static readonly Dictionary<string, string> AddressShortNamesDictionary = new Dictionary<string, string>
        {
            {" Б-Р ", " б-р "},
            {" Б-Р. ", " б-р "},
            {" Высел ", " высел. "},
            {" Высел. ", " высел. "},
            {" Гор ", " гор. "},
            {" Гор. ", " гор. "},
            {" Г ", " г. "},
            {" Г. ", " г. "},
            {" Гск ", " ГСК "},
            {" Гск. ", " ГСК "},
            {" Гп ", " г. п. "},
            {" Г П ", " г. п. "},
            {" Гп. ", " г. п. "},
            {" Го ", " г/о "},
            {" Г О ", " г/о "},
            {" Го. ", " г/о "},
            {" Г/о ", " г/о "},
            {" Г/о. ", " г/о "},
            {" Дер ", " дер. "},
            {" Дер. ", " дер. "},
            {" Дор ", " дор. "},
            {" Дор. ", " дор. "},
            {" Дп ", " дп. "},
            {" Дп. ", " дп. "},
            {" Д ", " д. "},
            {" Д. ", " д. "},
            {" Жилрайон ", " жилрайон "},
            {" Жт ", " жт. "},
            {" Жт. ", " жт. "},
            {" Ж/д_будка ", " ж/д_будка "},
            {" Ж/д_казарм ", " ж/д_казарм "},
            {" Ж/д_оп ", " ж/д_оп "},
            {" Ж/д_платф ", " ж/д_платф "},
            {" Ж/д_пост ", " ж/д_пост "},
            {" Ж/д_рзд ", " ж/д_рзд "},
            {" Ж/д_ст ", " ж/д_ст "},
            {" Им ", " им. "},
            {" Им. ", " им. "},
            {" Кв ", " кв. "},
            {" Кв-л ", " кв-л "},
            {" Кв-л. ", " кв-л "},
            {" Кв. ", " кв. "},
            {" Км ", " км. "},
            {" Км. ", " км. "},
            {" Ком ", " ком. "},
            {" Ком. ", " ком. "},
            {" Корп ", " корп. "},
            {" Корп. ", " корп. "},
            {" Кп ", " кп. "},
            {" Кп. ", " кп. "},
            {" Кр ", " кр. "},
            {" Кр. ", " кр. "},
            {" К ", " к. "},
            {" К. ", " к. "},
            {" Лпх ", " лпх. "},
            {" Лпх. ", " лпх. "},
            {" Мест ", " мест. "},
            {" Мест. ", " мест. "},
            {" Мкр ", " мкр. "},
            {" Мкр. ", " мкр. "},
            {" М ", " м. "},
            {" М. ", " м. "},
            {" Наб ", " наб. "},
            {" Наб. ", " наб. "},
            {" Нп ", " нп. "},
            {" Нп. ", " нп. "},
            {" Обл ", " обл. "},
            {" Обл. ", " обл. "},
            {" Пгт ", " пгт. "},
            {" Пгт. ", " пгт. "},
            {" Пер ", " пер. "},
            {" Пер. ", " пер. "},
            {" Платф ", " платф. "},
            {" Платф. ", " платф. "},
            {" Пл ", " пл. "},
            {" Пл-ка ", " пл-ка "},
            {" Пл-ка. ", " пл-ка "},
            {" Пл. ", " пл. "},
            {" Пр ", " пр. "},
            {" Пр. ", " пр. "},
            {" Пос ", " пос. "},
            {" Пос. ", " пос. "},
            {" Промзона ", " промзона "},
            {" Пр-кт ", " пр-кт "},
            {" Пр-кт. ", " пр-кт "},
            {" П/о ", " п/о "},
            {" П/р ", " п/р "},
            {" П/ст ", " п/ст "},
            {" Рек ", " рек. "},
            {" Рек. ", " рек. "},
            {" Респ ", " респ. "},
            {" Респ. ", " респ. "},
            {" Рзд ", " рзд. "},
            {" Рзд. ", " рзд. "},
            {" Рп ", " рп. "},
            {" Рп. ", " рп. "},
            {" Р-н ", " р-н "},
            {" Р-н. ", " р-н "},
            {" Р-Н ", " р-н "},
            {" Р-Н. ", " р-н "},
            {" Р-Он ", " р-н "},
            {" Р-Он. ", " р-н "},
            {" Сл ", " сл. "},
            {" Сл. ", " сл. "},
            {" Снт ", " СНТ "},
            {" Стр ", " стр. "},
            {" Стр. ", " стр. "},
            {" Ст ", " ст. "},
            {" Ст-ца ", " ст-ца "},
            {" Ст. ", " ст. "},
            {" С ", " с. "},
            {" С. ", " с. "},
            {" С/а ", " с/а "},
            {" С/мо ", " с/мо "},
            {" С/о ", " с/о "},
            {" С/п ", " с/п "},
            {" С/с ", " с/с "},
            {" Тер ", " тер. "},
            {" Тер. ", " тер. "},
            {" Туп ", " туп. "},
            {" Туп. ", " туп. "},
            {" Ул ", " ул. "},
            {" Ул. ", " ул. "},
            {" Х ", " х. "},
            {" Х. ", " х. "},
            {" Ш ", " ш. "},
            {" Ш. ", " ш. "}
        };

        private static readonly Dictionary<string, string> AddressEndingsDictionary = new Dictionary<string, string>
        {
            {"-Ый ", "-ый "},
            {"-Й ", "-й "},
            {"-Ой ", "-ой "},
            {"-Ий ", "-ий "},
            {"-Ая ", "-ая "},
            {"-Я ", "-я "},
            {"-Ья ", "-ья "},
            {"-Ье ", "-ье "},
            {"-Ое ", "-ое "},
            {"-Е ", "-е "},
            {"-г/о ", "-го "}
        };

        public static string FormatAddress(string source)
        {
            if (source.IsNullOrEmptyOrWhiteSpace())
                return string.Empty;

            source = source.FormatText().ToUpperAllFirstChars();

            foreach (var e in AddressLongNamesDictionary)
            {
                source = Regex.Replace(source, e.Key, e.Value, RegexOptions.IgnoreCase);
                source = Regex.Replace(source, e.Key.Remove(0, 1), e.Value.Remove(0, 1), RegexOptions.IgnoreCase);
            }

            foreach (var e in AddressShortNamesDictionary)
            {
                source = source.Replace(e.Key, e.Value);
                source = source.Replace(e.Key.Remove(0, 1), e.Value.Remove(0, 1));
                source = source.Replace(e.Key.Remove(e.Key.Length - 1, 1) + ",",
                    e.Value.Remove(e.Value.Length - 1, 1) + ",");
            }

            foreach (var e in AddressEndingsDictionary) source = source.Replace(e.Key, e.Value);

            return source.TrimWholeString();
        }

    }
}