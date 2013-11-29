using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.ExtensionManager;
using Microsoft.VisualStudio.Settings;
using Microsoft.Win32;

namespace Root_VSIX
{
    public static class Installer
    {
        static int Main(string[] args)
        {
            if (args.Length != 2 && args.Length != 3)
            {
                Console.Error.WriteLine(typeof(Installer).Assembly.GetName().Name);
                Console.Error.WriteLine("Installs local VSIX extensions to custom Visual Studio RootSuffixes");
                Console.Error.WriteLine();
                Console.Error.WriteLine("Usage:");
                Console.Error.WriteLine();
                Console.Error.WriteLine("  " + typeof(Installer).Assembly.GetName().Name + " [<VS version>] <RootSuffix> <Path to VSIX>");
                Console.Error.WriteLine();
                Console.Error.WriteLine("The Visual Studio version must be specified as the internal version number (12.0 is 2013).");
                Console.Error.WriteLine("If omitted, the extension will be installed to the latest version of Visual Studio installed on the computer.");
                return 1;
            }

            string version;
            if (args.Length == 3)
            {
                version = args[1];
                args = args.Skip(1).ToArray();
            }
            else
            {
                version = FindLatestVsVersion().ToString();
            }

            if (!File.Exists(args[1]))
            {
                Console.Error.WriteLine("Cannot find VSIX file " + args[1]);
                return 1;
            }
            var vsix = ExtensionManagerService.CreateInstallableExtension(args[1]);

            Console.WriteLine("Installing " + vsix.Header.Name + " version " + vsix.Header.Version + " to Visual Studio " + version + " /RootSuffix " + args[0]);
            Install(GetVersionExe(version), vsix, args[0]);

            return 0;
        }

        public static decimal FindLatestVsVersion()
        {
            using (var software = Registry.LocalMachine.OpenSubKey("SOFTWARE"))
            using (var ms = software.OpenSubKey("Microsoft"))
            using (var vs = ms.OpenSubKey("VisualStudio"))
                return vs.GetSubKeyNames()
                        .Select(s =>
                {
                    decimal v;
                    if (!decimal.TryParse(s, out v))
                        return new decimal?();
                    return v;
                })
                .Where(d => d.HasValue)
                .OrderByDescending(d => d)
                .First()
                .Value;
        }

        public static string GetVersionExe(string version)
        {
            return Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\VisualStudio\" + version + @"\Setup\VS", "EnvironmentPath", null) as string;
        }

        public static void Install(string vsExe, IInstallableExtension vsix, string rootSuffix)
        {
            using (var esm = ExternalSettingsManager.CreateForApplication(vsExe, rootSuffix))
            {
                var ems = new ExtensionManagerService(esm);
                ems.Install(vsix, perMachine: false);
            }
        }
    }
}
