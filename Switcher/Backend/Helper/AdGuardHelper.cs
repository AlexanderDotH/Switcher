using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Switcher.Utils;

namespace Switcher.Backend.Helper;

public class AdGuardHelper
{
    public static async Task SetDNSServer()
    {
        NetworkInterface @interface = Networking.GetActiveEthernetOrWifiNetworkInterface();

        await Networking.SetIPv4DNS("94.140.14.140", "94.140.14.141", @interface);
        await Networking.SetIPv6DNS("2a00:5a60::ad1:0ff", "2a00:5a60::ad2:0ff", @interface);
    }
    
    public static async Task DeleteDNSServer()
    {
        NetworkInterface @interface = Networking.GetActiveEthernetOrWifiNetworkInterface();

        await Networking.DeleteIPv4DNS(@interface);
        await Networking.DeleteIPv6DNS(@interface);
    }

    public static bool IsDNSSet()
    {
        return Networking.IsDNSSet("94.140.14.140", "94.140.14.141");
    }
}