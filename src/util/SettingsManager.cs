namespace LiveStreamTools.Util
{
using System;
using System.Linq;
    using LiveStreamTools.Core;

    public class SettingsManager
    {
        private static string[] allowedAutoStartOpts = { "daily", "monday", "tuesday", "wednesday", "thursday", "friday", "saturday", "sunday" };
        private static string[] allowedFormats = { "dd:hh:mm:ss", "dd:hh:mm", "dd:hh", "dd", "hh:mm:ss", "hh:mm", "hh:ss", "hh", "mm:ss", "mm", "ss" };
        private static string[] allowedTranslations = { "ASV", "BBE", "DARBY", "KJV", "WEB", "YLT", "ESV", "NIV", "NLT" };
        public static string[] AllowedGenres = { "All", "Law", "History", "Wisdom", "Prophets", "Gospels", "Acts", "Epistles", "Apocalyptic" };

        public static void SetSettings(string[] inputParts)
        {
            try
            {
                if (inputParts.Length <= 1)
                {
                    Console.WriteLine($"No option provided. {Program.useHelp}");
                    return;
                }

                string setting = inputParts[1];
                string newSetting = string.Join(" ", inputParts.Skip(2));

                switch (setting)
                {
                    case "countdown-text":
                        SetCountdownText(newSetting);
                        break;
                    case "countdown-over-text":
                        SetCountdownOverText(newSetting);
                        break;
                    case "countdown-format":
                        SetCountdownFormat(newSetting);
                        break;
                    case "file-path":
                        SetFilePath(newSetting);
                        break;
                    case "auto-start-time":
                        SetAutoStartTime(inputParts);
                        break;
                    case "bible-verses-interval":
                        SetBibleVersesInterval(newSetting);
                        break;
                    case "bible-verses-file-path":
                        SetBibleVersesFilePath(newSetting);
                        break;
                    case "bible-verses-translation":
                        SetBibleVersesTranslation(newSetting);
                        break;
                    case "bible-verses-genre":
                        SetBibleVersesGenre(newSetting);
                        break;
                    default:
                        Console.WriteLine($"Invalid option. {Program.useHelp}");
                        break;
                }

                Settings.SaveSettings();
            }
            catch (Exception)
            {
                Console.WriteLine("An error occurred while processing the command.");
                throw;
            }
        }

        public static void SetCountdownText(string newText)
        {
            Settings.settings.CountdownText = newText;
            Console.WriteLine($"Countdown text set to: {newText}");
        }

        public static void SetCountdownOverText(string newText)
        {
            Settings.settings.CountdownOverText = newText;
            Console.WriteLine($"Countdown over text set to: {newText}");
        }

        public static void SetCountdownFormat(string newFormat)
        {
            if (allowedFormats.Contains(newFormat))
            {
                string formattedFormat = newFormat.Replace(":", "\\:");
                Settings.settings.CountdownFormat = formattedFormat;
                Console.WriteLine($"Countdown format set to: {formattedFormat}");
            }
            else
            {
                Console.WriteLine("Invalid format. Please enter a valid display format. (hh:mm:ss, hh:mm, hh:ss, hh,mm:ss,mm,ss).");
            }
        }

        public static void SetFilePath(string newPath)
        {
            Settings.settings.FilePath = newPath;
            Console.WriteLine($"File path set to: {newPath}");
        }

        public static void SetAutoStartTime(string[] inputParts)
        {
            if (inputParts.Length <= 3)
            {
                Console.WriteLine("Missing required options. Please enter a day of the week and time e.g. monday 16:00:00");
                return;
            }

            string dayOpt = inputParts[2];
            string targetTime = inputParts[3];
            int index = Array.IndexOf(allowedAutoStartOpts, dayOpt);
            DayOfWeek targetDayOfWeek = DateTime.Now.DayOfWeek;

            if (index != -1)
            {
                if (index != 0)
                {
                    targetDayOfWeek = (DayOfWeek)index;
                }
            }
            else
            {
                Console.WriteLine($"Invalid option '{dayOpt}' - Please use daily, monday, tuesday, wednesday, thursday, friday, saturday, sunday ");
                return;
            }

            Settings.settings.AutoCountdownDay = targetDayOfWeek;
            Settings.settings.AutoCountdownTime = targetTime;

            DateTime selectedAutoStartDateTime = TimeCalculator.CalculateAutoStartDateTime(targetDayOfWeek, targetTime);
            Console.WriteLine($"Auto-start-time set to: {selectedAutoStartDateTime}");
        }

        public static void SetBibleVersesInterval(string newInterval)
        {
            if (string.IsNullOrWhiteSpace(newInterval))
            {
                Console.WriteLine("Missing option. Please enter a number in seconds e.g. 10 ");
                return;
            }

            int newIntervalSeconds = int.Parse(newInterval);
            int newIntervalMs = newIntervalSeconds * 1000;

            Settings.settings.BibleVersesLoopInterval = newIntervalMs;
            Console.WriteLine($"Bible verses interval set to: {newIntervalSeconds} seconds");
        }

        public static void SetBibleVersesFilePath(string newPath)
        {
            Settings.settings.BibleVersesFilePath = newPath;
            Console.WriteLine($"Bible verses file path set to: {newPath}");
        }

        public static void SetBibleVersesTranslation(string newTranslation)
        {
            if (allowedTranslations.Contains(newTranslation))
            {
                Settings.settings.BibleVersesTranslation = newTranslation;
                Console.WriteLine($"Bible verses translation set to: {newTranslation}");
            }
            else
            {
                Console.WriteLine("Invalid translation. Please choose from {ASV, BBE, DARBY, KJV, WEB, YLT, ESV, NIV, NLT}");
            }
        }

        public static void SetBibleVersesGenre(string newGenre)
        {
            if (AllowedGenres.Contains(newGenre))
            {
                Settings.settings.BibleVersesGenre = newGenre;
                Console.WriteLine($"Bible verses genre set to: {newGenre}");
            }
            else
            {
                Console.WriteLine("Invalid genre. Please choose from {All, Law, History, Prophets, Gospels, Acts, Epistles, Apocalyptic}.");
            }
        }
    }
}