using LiveStreamTools.Core;


namespace LiveStreamTools.Commands
{
    public static class Start
    {
        public static void StartCommand(string[] inputParts)
        {
            if (inputParts.Length <= 1)
            {
                Console.WriteLine($"No option provided. {Program.useHelp}");
                return;
            }

            string subCommand = inputParts[1].ToLower();

            switch (subCommand)
            {
                case "countdown":
                    StartCountdownCommand(inputParts);
                    break;
                case "bible-verses":
                    StartBibleVersesCommand(inputParts);
                    break;
                case "countdown-verses":
                    StartCountdownAndBibleVersesCommand(inputParts);
                    break;
                default:
                    Console.WriteLine($"Invalid option. {Program.useHelp}");
                    break;
            }
        }

        static void StartCountdownCommand(string[] inputParts)
        {
            if (inputParts.Length < 4)
            {
                Console.WriteLine("Missing or invalid options. Please enter a date and time in the format yyyy:mm-dd hh:mm:ss ");
                return;
            }

            string dateStr = inputParts[2];
            string timeStr = inputParts[3];

            if (!DateTime.TryParseExact(dateStr + " " + timeStr, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime selectedDateTime))
            {
                Console.WriteLine("Missing or invalid options. Please enter a date and time in the format yyyy:mm-dd hh:mm:ss");
                return;
            }

            string countdownName = GetOptionalParameter(inputParts, "name");
            string filePath = GetOptionalParameter(inputParts, "file-path");

            Countdown.StartCountdown(selectedDateTime, countdownName, filePath);
        }

        static void StartBibleVersesCommand(string[] inputParts)
        {
            string versesName = GetOptionalParameter(inputParts, "name");
            string versesFilePath = GetOptionalParameter(inputParts, "file-path");

            BibleVersesWriter.StartBibleVerses(versesName, versesFilePath);
        }

        static void StartCountdownAndBibleVersesCommand(string[] inputParts)
        {
            if (inputParts.Length < 4)
            {
                Console.WriteLine("Missing or invalid options. Please enter a date and time in the format yyyy:mm-dd hh:mm:ss ");
                return;
            }

            string dateStr = inputParts[2];
            string timeStr = inputParts[3];

            if (!DateTime.TryParseExact(dateStr + " " + timeStr, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime selectedDateTime))
            {
                Console.WriteLine("Missing or invalid options. Please enter a date and time in the format yyyy:mm-dd hh:mm:ss");
                return;
            }

            Countdown.StartCountdown(selectedDateTime);
            BibleVersesWriter.StartBibleVerses();
        }

        static string GetOptionalParameter(string[] inputParts, string option)
        {
            for (int i = 4; i < inputParts.Length - 1; i++)
            {
                if (inputParts[i].Equals(option, StringComparison.OrdinalIgnoreCase))
                {
                    if (i + 1 < inputParts.Length && inputParts[i + 1].StartsWith('"') && inputParts[i + 1].EndsWith('"'))
                    {
                        return inputParts[i + 1].Trim('"');
                    }
                    else
                    {
                        Console.WriteLine($"Missing or empty value given after '{option}'. Make sure to put \" \" around the value. Use 'help' command for more details.");
                        return null;
                    }
                }
            }
            return null;
        }
    }
}

