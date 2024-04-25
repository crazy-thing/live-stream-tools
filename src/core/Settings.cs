using System.Text.Json;
using LiveStreamTools.Models;

namespace LiveStreamTools.Core
{
    public static class Settings
    {
        private static readonly string settingsJsonPath = "./settings.json";
        public static SettingsModel? settings;

        public static void LoadSettings()
        {
            try
            {
                if (!File.Exists(settingsJsonPath))
                {
                    SaveDefaultSettings();
                }

                string json = File.ReadAllText(settingsJsonPath);
                settings = JsonSerializer.Deserialize<SettingsModel>(json);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Settings file not found");
            }
        }

        public static void SaveSettings()
        {
            string settingsJson = JsonSerializer.Serialize(settings);
            File.WriteAllText(settingsJsonPath, settingsJson);
            Console.WriteLine("Settings saved");
        }

        public static void SaveDefaultSettings()
        {
            var defaultSettings = new SettingsModel();
            string settingsJson = JsonSerializer.Serialize(defaultSettings);
            File.WriteAllText(settingsJsonPath, settingsJson);
        }

        public static SettingsModel? GetSettings()
        {
            return settings;
        }
    }   
}