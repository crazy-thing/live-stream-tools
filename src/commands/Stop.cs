using LiveStreamTools.Models;
using LiveStreamTools.Util;

namespace LiveStreamTools.Commands
{
    public static class Stop
    {
        public static void StopCommand(string[] inputParts)
        {
            if (inputParts.Length < 2 || !inputParts[1].StartsWith('"') || !inputParts[1].EndsWith('"'))
            {
                Console.WriteLine($"Missing or invalid option. Make sure to place option in quotations \" \".");
                return;
            }

            string taskName = inputParts[1].Trim('"');
            TaskHandler.StopTask(taskName);
        }
    }
}