using System.Diagnostics;
using LiveStreamTools.Api;
using LiveStreamTools.Core;
using LiveStreamTools.Models;

namespace LiveStreamTools.Core
{
    public class BibleVersesWriter
    {
        private static readonly string TemplatePath = "./verses-template.html";
        private static readonly string VariablesPath = "./verses-variables.txt";


        public static void StartBibleVerses(string bibleVersesName = null, string versesFilePath = null)
        {
            // Set default values if not provided
            bibleVersesName ??= $"verses{Program.nameToIds.Count + 1}";
            versesFilePath ??= Settings.settings.BibleVersesFilePath;

            // Add to the dictionary
            Program.nameToIds.Add(bibleVersesName, Guid.NewGuid().ToString());

            // Start the task
            CancellationTokenSource cts = new CancellationTokenSource();
            Task.Run(() => StartBibleVersesInternal(cts, bibleVersesName, versesFilePath));

            // Add task info
            lock (Program.lockObject)
            {
                Program.tasks.Add(bibleVersesName, new TaskInfo { TaskType = "Bible-Verses", CancellationTokenSource = cts });
            }
        }

        private static async Task StartBibleVersesInternal(CancellationTokenSource cts, string bibleVersesName, string filePath)
        {
            Console.WriteLine("Started bible verses with name of: " + bibleVersesName);

            string genre = Settings.settings.BibleVersesGenre;
            string translation = Settings.settings.BibleVersesTranslation;

            while (!cts.IsCancellationRequested)
            {
                // Fetch Bible verse
                BibleVerseModel bibleVerseModel = await BibleVerses.GetBibleVerse(genre, translation);

                // Update template
                UpdateTemplate(filePath, bibleVerseModel);

                // Wait for interval
                await Task.Delay(Settings.settings.BibleVersesLoopInterval);
            }
        }

        private static void UpdateTemplate(string filePath, BibleVerseModel bibleVerseModel)
        {
            // Read template
            string template = File.ReadAllText(TemplatePath);

            // Read variables from the variables file
            Dictionary<string, string> variableDict = new Dictionary<string, string>();
            foreach (string line in File.ReadLines(VariablesPath))
            {
                string[] parts = line.Split('=');
                if (parts.Length == 2)
                {
                    variableDict[parts[0].Trim()] = parts[1].Trim();
                }
            }

            // Replace placeholders with actual verse data
            template = template.Replace("{bibleVerseInfo}", $"{bibleVerseModel.book.name} {bibleVerseModel.chapterId}:{bibleVerseModel.verseId}")
                            .Replace("{bibleVerse}", bibleVerseModel.verse);

            // Replace variable styles in the template
            foreach (var variable in variableDict)
            {
                string variablePlaceholder = "{" + variable.Key + "}";
                template = template.Replace(variablePlaceholder, variable.Value);
            }

            // Write back to file
            File.WriteAllText(filePath, template);
        }

        public static void EditBibleVersesVariables()
        {
            // Open variables file with notepad or create a new one if it doesn't exist
            if (File.Exists(VariablesPath))
            {
                Process.Start("notepad.exe", VariablesPath);
            }
            else
            {
                CreateVariablesFile();
            }
        }

        private static void CreateVariablesFile()
        {
            // Create default variables file content
            string defaultVariables =
            @"
            verseInfoFont=Arial
            verseInfoFontSize=48
            verseInfoTextColor=#fff

            verseFont=Arial
            verseFontSize=60
            verseTextColor=#fff
            ";

            // Write content to file
            File.WriteAllText(VariablesPath, defaultVariables);
        }
    }
}