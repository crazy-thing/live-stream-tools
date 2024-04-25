using LiveStreamTools.Util;

namespace LiveStreamTools.Commands
{
    public static class Set
    {
        public static void SetCommand(string[] inputParts)
        {
            if (inputParts.Length < 2)
            {
                Console.WriteLine($"No option provided. {Program.useHelp}");
                return;
            }

            SettingsManager.SetSettings(inputParts);
        }
    }
}
