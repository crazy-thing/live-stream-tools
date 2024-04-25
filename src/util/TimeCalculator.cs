using System.Security.Cryptography.X509Certificates;
using LiveStreamTools.Core;

namespace LiveStreamTools.Util
{
    public class TimeCalculator
    {
        public static DateTime CalculateAutoStartDateTime(DayOfWeek targetDayOfWeek, string targetTime)
        {
            DateTime dateTime = CalculateNextDayOfWeek(targetDayOfWeek);
            string dateStr = dateTime.ToString("yyyy-MM-dd");
            if (!DateTime.TryParseExact(dateStr + " " + targetTime, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime selectedAutoStartDateTime))
            {
                Console.WriteLine("Error: Invalid date and time format. Please use the format yyyy:MM-dd HH:mm:ss ");
                return selectedAutoStartDateTime;
            }
            return selectedAutoStartDateTime;
        }

        public static DateTime CalculateNextDayOfWeek(DayOfWeek targetDayOfWeek)
        {
            DateTime currentDate = DateTime.Now;
            int daysUntilTargetDay = ((int)targetDayOfWeek - (int)currentDate.DayOfWeek + 7) % 7;
            DateTime nextDay = currentDate.Date.AddDays(daysUntilTargetDay);
            return nextDay;
        }
    }
}