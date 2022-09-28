using System.Diagnostics;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Switcher.Utils;

public class ProcessUtils
{
    public static Task StartProcess(string path, string arguments, bool visible, string verb = "runas")
    {
        ProcessStartInfo startInfo = new ProcessStartInfo(path);
        startInfo.Arguments = arguments;
        startInfo.Verb = verb;
        startInfo.CreateNoWindow = !visible;
        startInfo.UseShellExecute = true;
        startInfo.WindowStyle = ProcessWindowStyle.Hidden;

        Process process = new Process();
        process.StartInfo = startInfo;

        process.Start();
        
        return process.WaitForExitAsync();
    }
    
    public static bool IsAdministrator()
    {
       return (new WindowsPrincipal(WindowsIdentity.GetCurrent()))
                 .IsInRole(WindowsBuiltInRole.Administrator);
    }  
}