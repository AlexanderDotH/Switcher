using Newtonsoft.Json;

namespace Switcher.Backend.Structs;

public class Settings
{
    [JsonProperty("Server")] 
    public EnumServerType ServerType { get; set; }
}