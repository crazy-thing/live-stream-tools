using LiveStreamTools.Models;
using LiveStreamTools.Util;

namespace LiveStreamTools.Core
{
    public class Countdown
    {
        public static void StartCountdown(DateTime targetDateTime, string countdownName = null, string filePath = null)
        {
            countdownName ??= $"countdown{Program.nameToIds.Count + 1}";
            filePath ??= Settings.settings.FilePath;

            CancellationTokenSource cts = new CancellationTokenSource();
            Task.Run(() => StartCountdownInternal(targetDateTime, cts.Token, filePath, countdownName));

            Program.nameToIds.Add(countdownName, Guid.NewGuid().ToString());
            Program.tasks.Add(countdownName, new TaskInfo { TaskType = "Countdown", CancellationTokenSource = cts });

            Console.WriteLine($"Countdown started with name: {countdownName} \nCountdown end time: {targetDateTime}");
        }

        private static async Task StartCountdownInternal(DateTime targetDateTime, CancellationToken cancellationToken, string filePath, string countdownName)
        {
            string countdownOverText = Settings.settings.CountdownOverText;

            while (!cancellationToken.IsCancellationRequested && DateTime.Now < targetDateTime)
            {
                File.WriteAllText(filePath, string.Empty);

                string countdownText = Settings.settings.CountdownText;
                string countdownFormat = Settings.settings.CountdownFormat;

                TimeSpan timeRemaining = targetDateTime - DateTime.Now;
                string formattedTime = timeRemaining.ToString(countdownFormat);

                string paddedCountdownText = $"{countdownText}: {formattedTime}".PadRight(countdownText.Length);
                
                await File.AppendAllTextAsync(filePath, paddedCountdownText);

                await Task.Delay(1000);
            }

            await File.WriteAllTextAsync(filePath, countdownOverText);

            TaskHandler.StopTask(countdownName);
        }
    }
}