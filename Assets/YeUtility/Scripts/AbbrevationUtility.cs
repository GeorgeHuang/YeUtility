using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace YeUtility
{
    public static class AbbrevationUtility
    {
        private static readonly SortedDictionary<int, string> Abbrevations = new SortedDictionary<int, string>
        {
            {1000,"K"},
            {1000000, "M" },
            {1000000000, "B" }
        };

        public static string AbbreviateNumber(float number)
        {
            for (var i = Abbrevations.Count - 1; i >= 0; i--)
            {
                var pair = Abbrevations.ElementAt(index: i);
                if (!(Mathf.Abs(number) >= pair.Key)) continue;
                var roundedNumber = Mathf.FloorToInt(number / pair.Key);
                return roundedNumber + pair.Value;
            }
            return number.ToString(CultureInfo.InvariantCulture);
        }
    }
}