using System;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;

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