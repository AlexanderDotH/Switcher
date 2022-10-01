using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Switcher.Backend.Handler;
using Switcher.Backend.Structs;
using Switcher.Utils;

namespace Switcher.Backend.Helper;

public class AdGuardHelper
{
    public static async Task SetDNSServer()
    {
        NetworkInterface @interface = Networking.GetActiveEthernetOrWifiNetworkInterface();

        HostEntry entry = GetCurrentEntry();

        await Networking.SetIPv4DNS(entry.IpV4P1, entry.IpV4P2, @interface);
        await Networking.SetIPv6DNS(entry.IpV6P1, entry.IpV6P2, @interface);
    }
    
    public static async Task DeleteDNSServer()
    {
        NetworkInterface @interface = Networking.GetActiveEthernetOrWifiNetworkInterface();

        await Networking.DeleteIPv4DNS(@interface);
        await Networking.DeleteIPv6DNS(@interface);
    }

    public static bool IsDNSSet()
    {
        HostEntry entry = GetCurrentEntry();
        return Networking.IsDNSSet(entry.IpV4P1, entry.IpV4P2);
    }

    private static HostEntry GetCurrentEntry()
    {
        EnumServerType serverType = SettingsHandler.Instance.Settings.ServerType;

        switch (serverType)
        {
            case EnumServerType.Family:
            {
                return new HostEntry(
                    "94.140.14.15", 
                    "94.140.14.16", 
                    "2a10:50c0::bad1:ff", 
                    "2a10:50c0::bad2:ff");
                break;
            }
            case EnumServerType.No_Filter:
            {
                return new HostEntry(
                    "94.140.14.140", 
                    "94.140.14.141", 
                    "2a00:5a60::ad1:0ff", 
                    "2a00:5a60::ad2:0ff");
                break;
            }
            default:
            {
                return new HostEntry(
                    "94.140.14.14", 
                    "94.140.15.15", 
                    "2a10:50c0::ad1:ff", 
                    "2a10:50c0::ad2:ff");
                break;
            }
        }
    }
}