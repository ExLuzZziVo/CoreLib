#region

using System;
using CoreLib.CORE.Helpers.IntHelpers;
using CoreLib.CORE.Helpers.StringHelpers;

#endregion

namespace CoreLib.CORE.Helpers.DecimalHelpers
{
    public static class DecimalExtensions_Ru
    {
        /// <summary>
        /// Преобразует число в строку, представляющую собой денежную сумму в рублях и копейках
        /// </summary>
        /// <param name="value">Число</param>
        /// <param name="mode">Тип округления. Значение по умолчанию: <see cref="MidpointRounding.AwayFromZero"/></param>
        /// <returns>Строка, представляющая собой денежную сумму в рублях и копейках</returns>
        public static string ToRubleString(this decimal value, MidpointRounding mode = MidpointRounding.AwayFromZero)
        {
            var processedValue = Math.Round(value, 2, mode);

            var integerPart = decimal.ToInt32(processedValue);
            var fractionalPart = Math.Abs(decimal.ToInt32((processedValue % 1) * 100));

            var integerStringPart = integerPart.ToRubleString();
            var fractionalStringPart = fractionalPart.ToLongIntString();

            if (fractionalStringPart.EndsWith("один"))
            {
                fractionalStringPart =
                    fractionalStringPart.Substring(0, fractionalStringPart.Length - 2) + "на копейка";
            }
            else if (fractionalStringPart.EndsWith("два"))
            {
                fractionalStringPart = fractionalStringPart.Substring(0, fractionalStringPart.Length - 1) + "е копейки";
            }
            else if (fractionalStringPart.EndsWithAny("три", "четыре"))
            {
                fractionalStringPart += " копейки";
            }
            else
            {
                fractionalStringPart += " копеек";
            }

            return $"{integerStringPart} {fractionalStringPart}";
        }
    }
}
