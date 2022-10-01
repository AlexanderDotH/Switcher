using Avalonia;
using Avalonia.ReactiveUI;
using System;
using System.Diagnostics;
using System.Reflection;
using Avalonia.Controls.Shapes;
using Avalonia.Logging;
using Switcher.Utils;

namespace Switcher
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            if (Process.GetProcessesByName (Process.GetCurrentProcess().ProcessName).Length > 1)
               return;

            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace(LogEventLevel.Debug, LogArea.Property, LogArea.Layout)
                .UseReactiveUI();
    }
}