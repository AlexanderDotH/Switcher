using System;
using System.CodeDom;
using System.IO;
using Newtonsoft.Json;
using Switcher.Backend.Structs;

namespace Switcher.Backend.Handler;

public class SettingsHandler
{
    private readonly string _directory;
    private Settings _settings;
    
    private static SettingsHandler INSTANCE;

    public SettingsHandler()
    {
        this._directory = string.Format("{0}{1}Switcher{1}", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.DirectorySeparatorChar);

        if (!Directory.Exists(this._directory))
        {
            Directory.CreateDirectory(this._directory);
            this._settings = CreateDefaultSettings();
            this.Write();
        }
        else if (!File.Exists(this._directory + "Settings.json"))
        {
            this._settings = CreateDefaultSettings();
            this.Write();
        }
        else
        {
            this._settings = this.Read();
        }
    }

    public Settings CreateDefaultSettings()
    {
        Settings settings = new Settings();
        
        settings.ServerType = EnumServerType.Default;

        return settings;
    }

    public Settings Read()
    {
        FileInfo fi = new FileInfo(this._directory + "Settings.json");
        
        if (!fi.Exists)
            return null;

        StreamReader stream = fi.OpenText();
        if (stream == null)
            return null;

        Settings settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(fi.FullName));
        return settings;
    }
    
    public void Write()
    {
        FileInfo fi = new FileInfo(this._directory + "Settings.json");
        
        if (this._settings == null)
            return;

        string contentAsJson = JsonConvert.SerializeObject(this._settings, Formatting.Indented);
        File.WriteAllText(fi.FullName, contentAsJson);
    }

    public Settings Settings
    {
        get => _settings;
        set => _settings = value;
    }

    public static SettingsHandler Instance
    {
        get
        {
            if (INSTANCE == null)
                INSTANCE = new SettingsHandler();

            return INSTANCE;
        }
    }
}