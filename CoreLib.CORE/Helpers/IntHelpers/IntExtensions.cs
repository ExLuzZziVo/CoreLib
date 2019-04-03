using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UIServiceLib.CORE.Helpers.StringHelpers;

namespace UIServiceLib.CORE.Helpers.IntHelpers
{
    public static class IntExtensions
    {

        [Obfuscation(Exclude = true, Feature = "dynamic proxy")]
        public static string ToLongIntString(this int value)
        {
            var number = 0;
            var result = string.Empty;
            if (value < 0)
            {
                number = -value;
                result = "минус";
            }
            else if (value == 0)
            {
                return "ноль";
            }
            else if (value > 0)
            {
                number = value;
                result = "";
            }

            var arrayInt = new int[4];
            var arrayString = new[,]
            {
                {" миллиард", " миллиарда", " миллиардов"},
                {" миллион", " миллиона", " миллионов"},
                {" тысяча", " тысячи", " тысяч"},
                {"", "", ""}
            };
            arrayInt[0] = (number - number % 1000000000) / 1000000000;
            arrayInt[1] = (number % 1000000000 - number % 1000000) / 1000000;
            arrayInt[2] = (number % 1000000 - number % 1000) / 1000;
            arrayInt[3] = number % 1000;

            for (var i = 0; i < 4; i++)
            {
                if (arrayInt[i] != 0)
                {
                    if ((arrayInt[i] - arrayInt[i] % 100) / 100 != 0)
                        switch ((arrayInt[i] - arrayInt[i] % 100) / 100)
                        {
                            case 1:
                                result += " сто";
                                break;
                            case 2:
                                result += " двести";
                                break;
                            case 3:
                                result += " триста";
                                break;
                            case 4:
                                result += " четыреста";
                                break;
                            case 5:
                                result += " пятьсот";
                                break;
                            case 6:
                                result += " шестьсот";
                                break;
                            case 7:
                                result += " семьсот";
                                break;
                            case 8:
                                result += " восемьсот";
                                break;
                            case 9:
                                result += " девятьсот";
                                break;
                        }
                    if ((arrayInt[i] % 100 - arrayInt[i] % 100 % 10) / 10 != 1)
                        switch ((arrayInt[i] % 100 - arrayInt[i] % 100 % 10) / 10)
                        {
                            case 2:
                                result += " двадцать";
                                break;
                            case 3:
                                result += " тридцать";
                                break;
                            case 4:
                                result += " сорок";
                                break;
                            case 5:
                                result += " пятьдесят";
                                break;
                            case 6:
                                result += " шестьдесят";
                                break;
                            case 7:
                                result += " семьдесят";
                                break;
                            case 8:
                                result += " восемьдесят";
                                break;
                            case 9:
                                result += " девяносто";
                                break;
                        }
                    switch (arrayInt[i] % 10)
                    {
                        case 1:
                            if (i == 2) result += " одна";
                            else result += " один";
                            break;
                        case 2:
                            if (i == 2) result += " две";
                            else result += " два";
                            break;
                        case 3:
                            result += " три";
                            break;
                        case 4:
                            result += " четыре";
                            break;
                        case 5:
                            result += " пять";
                            break;
                        case 6:
                            result += " шесть";
                            break;
                        case 7:
                            result += " семь";
                            break;
                        case 8:
                            result += " восемь";
                            break;
                        case 9:
                            result += " девять";
                            break;
                    }

                    switch (arrayInt[i] % 100)
                    {
                        case 10:
                            result += " десять";
                            break;
                        case 11:
                            result += " одиннадцать";
                            break;
                        case 12:
                            result += " двенадцать";
                            break;
                        case 13:
                            result += " тринадцать";
                            break;
                        case 14:
                            result += " четырнадцать";
                            break;
                        case 15:
                            result += " пятнадцать";
                            break;
                        case 16:
                            result += " шестнадцать";
                            break;
                        case 17:
                            result += " семнадцать";
                            break;
                        case 18:
                            result += " восемннадцать";
                            break;
                        case 19:
                            result += " девятнадцать";
                            break;
                    }
                }
                else
                {
                    switch (arrayInt[i] % 100)
                    {
                        case 10:
                            result += " десять";
                            break;
                        case 11:
                            result += " одиннадцать";
                            break;
                        case 12:
                            result += " двенадцать";
                            break;
                        case 13:
                            result += " тринадцать";
                            break;
                        case 14:
                            result += " четырнадцать";
                            break;
                        case 15:
                            result += " пятнадцать";
                            break;
                        case 16:
                            result += " шестнадцать";
                            break;
                        case 17:
                            result += " семнадцать";
                            break;
                        case 18:
                            result += " восемннадцать";
                            break;
                        case 19:
                            result += " девятнадцать";
                            break;
                    }
                }

                if (arrayInt[i] % 100 >= 10 && arrayInt[i] % 100 <= 19) result += " " + arrayString[i, 2] + " ";
                else
                    switch (arrayInt[i] % 10)
                    {
                        case 1:
                            result += " " + arrayString[i, 0] + " ";
                            break;
                        case 2:
                        case 3:
                        case 4:
                            result += " " + arrayString[i, 1] + " ";
                            break;
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                            result += " " + arrayString[i, 2] + " ";
                            break;
                    }
            }

            return result.TrimWholeString();
        }

        public static string ToStringWithZero(this int source)
        {
            return source.ToString().Length == 1 ? $"0{source}" : source.ToString();
        }

        public static bool IsEven(this int source)
        {
            return (source % 2) == 0;
        }

    }
}
