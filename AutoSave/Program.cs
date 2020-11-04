using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoSave
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            AutoLaunch();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var mainForm = new MainForm();
            var wrapper = new SingleInstanceApplicationWrapper(mainForm);
            wrapper.Run(args);
        }

        private static void AutoLaunch()
        {
            var direcroty = Path.Combine(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\Startup\", "AutoSave.lnk");
            if (System.IO.File.Exists(direcroty))
                return;
            RequireAdministrator();
            var shell = new WshShell();
            var shortcut = shell.CreateShortcut(direcroty) as IWshShortcut;
            shortcut.TargetPath = Application.ExecutablePath;
            shortcut.Arguments = "-auto";
            shortcut.WindowStyle = 2;
            shortcut.IconLocation = Application.ExecutablePath;
            shortcut.Save();
        }

        private static void RequireAdministrator()
        {
            var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            var principal = new System.Security.Principal.WindowsPrincipal(identity);
            if (principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
                return;
            var processInfo = new ProcessStartInfo(Application.ExecutablePath)
            {
                Verb = "runas"
            };
            Process.Start(processInfo);
            Environment.Exit(0);
        }
    }
}
