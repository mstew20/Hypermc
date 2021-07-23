using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hypermc.Settings
{
    public class UserSettings : IUserSettings
    {
        private readonly string _appPath;
        private readonly string _settingsFile;

        public string MinecraftPath { get; set; }
        public string ModPacksPath { get; set; }

        public UserSettings()
        {
            // TODO: possibly move the file names to the appsettings.
            _appPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\.hypermc";
            _settingsFile = $@"{_appPath}\settings.json";

            MinecraftPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\.minecraft";
            ModPacksPath = $@"{MinecraftPath}\ModPacks";
        }

        public async Task Initialize()
        {
            if (!Directory.Exists(_appPath))
            {
                Directory.CreateDirectory(_appPath);
            }

            if (File.Exists(_settingsFile))
            {
                using var stream = new FileStream(_settingsFile, FileMode.Open, FileAccess.Read);
                var settings = await JsonSerializer.DeserializeAsync<UserSettings>(stream);

                MinecraftPath = settings.MinecraftPath;
                ModPacksPath = settings.ModPacksPath;
            }
        }

        public async Task UpdateSettings(string mcPath, string modPath)
        {
            MinecraftPath = mcPath;
            ModPacksPath = modPath;

            string settingsToSave = JsonSerializer.Serialize(this);
            await File.WriteAllTextAsync(_settingsFile, settingsToSave);
        }
    }
}
