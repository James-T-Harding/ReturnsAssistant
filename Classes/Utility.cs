using System;

namespace Tax_Returns_Assistant
{
    static class Utility
    {
        static public DateTime ReadDate(string inp)
        {
            string[] DateArr = inp.Trim().Split('/');

            int day = Convert.ToInt32(DateArr[0]);
            int month = Convert.ToInt32(DateArr[1]);

            string years = DateArr[2].Length == 2 ? "20" + DateArr[2] : DateArr[2];

            int year = Convert.ToInt32(years);

            return new DateTime(year, month, day);
        }
    }
}
