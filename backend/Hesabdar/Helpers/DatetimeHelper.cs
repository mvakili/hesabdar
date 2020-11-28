using System;
using System.Collections.Generic;
using System.Linq;

namespace Hesabdar.Helpers
{
    public static class DatetimeHelper
    {
        public static List<DateTime> GenerateDatesInRange(DateTime startDate, DateTime endDate)
        {
            return Enumerable.Range(0, (int)(endDate - startDate).TotalDays)
                                  .Select(x => startDate.AddDays(x + 1))
                                  .ToList();
        }

    }
}
