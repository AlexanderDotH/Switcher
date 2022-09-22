using System;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Switcher.Utils;

public class Networking
{

    public static NetworkInterface GetActiveEthernetOrWifiNetworkInterface()
    {
        var Nic = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(
            a => a.OperationalStatus == OperationalStatus.Up &&
                 (a.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || a.NetworkInterfaceType == NetworkInterfaceType.Ethernet) &&
                 a.GetIPProperties().GatewayAddresses.Any(g => (string)g.Address.AddressFamily.ToString() == "InterNetwork"));

        return Nic;
    }

    public static async Task SetIPv4DNS(string server1, string server2, NetworkInterface @interface)
    {
        await ProcessUtils.StartProcess(
            string.Format("{0}\\{1}", Environment.GetFolderPath(Environment.SpecialFolder.System), "netsh.exe"), 
            string.Format("interface ipv4 add dnsserver \"{0}\" {1} index=0", @interface.Name, server1), false);
        
        await ProcessUtils.StartProcess(
            string.Format("{0}\\{1}", Environment.GetFolderPath(Environment.SpecialFolder.System), "netsh.exe"), 
            string.Format("interface ipv4 add dnsserver \"{0}\" {1} index=1", @interface.Name, server2), false);
    }
    
    public static async Task SetIPv6DNS(string server1, string server2, NetworkInterface @interface)
    {
        await ProcessUtils.StartProcess(
            string.Format("{0}\\{1}", Environment.GetFolderPath(Environment.SpecialFolder.System), "netsh.exe"), 
            string.Format("interface ipv6 add dnsserver \"{0}\" {1} index=0", @interface.Name, server1), false);
        
        await ProcessUtils.StartProcess(
            string.Format("{0}\\{1}", Environment.GetFolderPath(Environment.SpecialFolder.System), "netsh.exe"), 
            string.Format("interface ipv6 add dnsserver \"{0}\" {1} index=1", @interface.Name, server2), false);
    }
    
    public static async Task DeleteIPv6DNS(NetworkInterface @interface)
    {
        await ProcessUtils.StartProcess(
            string.Format("{0}\\{1}", Environment.GetFolderPath(Environment.SpecialFolder.System), "netsh.exe"), 
            string.Format("interface ipv6 delete dnsserver \"{0}\" all", @interface.Name), false);
    }
    
    public static async Task DeleteIPv4DNS(NetworkInterface @interface)
    {
        await ProcessUtils.StartProcess(
            string.Format("{0}\\{1}", Environment.GetFolderPath(Environment.SpecialFolder.System), "netsh.exe"), 
            string.Format("interface ipv4 delete dnsserver \"{0}\" all", @interface.Name), false);
    }
    
    
    public static void SetDNS(string dnsString1, string dnsString2)
    {
        string[] Dns = { dnsString1, dnsString2 };
        var CurrentInterface = GetActiveEthernetOrWifiNetworkInterface();
        if (CurrentInterface == null) return;

        ManagementClass objMC = new ManagementClass("Win32_NetworkAdapterConfiguration");
        ManagementObjectCollection objMOC = objMC.GetInstances();
        foreach (ManagementObject objMO in objMOC)
        {
            if ((bool)objMO["IPEnabled"])
            {
                if (objMO["Description"].ToString().Equals(CurrentInterface.Description))
                {
                    ManagementBaseObject objdns = objMO.GetMethodParameters("SetDNSServerSearchOrder");
                    if (objdns != null)
                    {
                        objdns["DNSServerSearchOrder"] = Dns;
                        objMO.InvokeMethod("SetDNSServerSearchOrder", objdns, null);
                    }
                }
            }
        }
    }
    
    public static void UnsetDNS()
    {
        var CurrentInterface = GetActiveEthernetOrWifiNetworkInterface();
        if (CurrentInterface == null) return;

        ManagementClass objMC = new ManagementClass("Win32_NetworkAdapterConfiguration");
        ManagementObjectCollection objMOC = objMC.GetInstances();
        foreach (ManagementObject objMO in objMOC)
        {
            if ((bool)objMO["IPEnabled"])
            {
                if (objMO["Description"].ToString().Equals(CurrentInterface.Description))
                {
                    ManagementBaseObject objdns = objMO.GetMethodParameters("SetDNSServerSearchOrder");
                    if (objdns != null)
                    {
                        objdns["DNSServerSearchOrder"] = null;
                        objMO.InvokeMethod("SetDNSServerSearchOrder", objdns, null);
                    }
                }
            }
        }
    }
    
    public static bool IsDNSSet(string dnsServer1, string dnsServer2)
    {
        NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

        foreach (NetworkInterface networkInterface in networkInterfaces)
        {
            foreach (IPAddress dnsAddress in networkInterface.GetIPProperties().DnsAddresses)
            {
                string dnsServer = dnsAddress.ToString();
                if (dnsServer.Contains(dnsServer1) || dnsServer.Contains(dnsServer2))
                    return true;
            }
        }

        return false;
    }
}