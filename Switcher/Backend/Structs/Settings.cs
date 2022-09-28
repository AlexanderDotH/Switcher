using Newtonsoft.Json;

namespace Switcher.Backend.Structs;

public class Settings
{
    [JsonProperty("Strict")] 
    public bool StrictMode { get; set; }
}