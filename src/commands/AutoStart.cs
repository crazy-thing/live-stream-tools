using LiveStreamTools.Core;

namespace LiveStreamTools.Commands
{
    public static class AutoStart
    {
        public static void EnableAutoStartCommand(string[] inputParts)
        {
            if (inputParts.Length < 2)
            {
                Console.WriteLine($"No option provided. {Program.useHelp}");
                return;
            }

            string subCommand = inputParts[1].ToLower();

            switch (subCommand)
            {
                case "countdown":
                    Settings.settings.AutoStartCountdown = true;
                    break;
                case "bible-verses":
                    Settings.settings.AutoStartBibleVersesLoop = true;
                    break;
                default:
                    Console.WriteLine($"Invalid command. {Program.useHelp}");
                    break;
            }

            Settings.SaveSettings();
        }

        public static void DisableAutoStartCommand(string[] inputParts)
        {
            if (inputParts.Length < 2)
            {
                Console.WriteLine($"No option provided. {Program.useHelp}");
                return;
            }

            string subCommand = inputParts[1].ToLower();

            switch (subCommand)
            {
                case "countdown":
                    Settings.settings.AutoStartCountdown = false;
                    break;
                case "bible-verses":
                    Settings.settings.AutoStartBibleVersesLoop = false;
                    break;
                default:
                    Console.WriteLine($"Invalid command. {Program.useHelp}");
                    break;
            }
        }        
    }
}