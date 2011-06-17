using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;

// Adapted from http://smartypeeps.blogspot.com/2006/12/windows-environment-variables.html
// See also http://support.microsoft.com/kb/104011
// See also http://www.switchonthecode.com/tutorials/csharp-snippet-tutorial-editing-the-windows-registry
namespace SetEnvVar
{
    internal class Program
    {
        /* Declare the Win32 API for propagating the environment variable */
        [DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        [return : MarshalAs(UnmanagedType.Bool)]
        public static extern bool SendMessageTimeout(IntPtr hWnd, int Msg, int wParam, string lParam, int fuFlags,
                                                     int uTimeout, ref int lpdwResult);

        public const int HWND_BROADCAST = 0xffff;  /* Constant to broadcast message to all windows */
        public const int WM_SETTINGCHANGE = 0x001A;
        public const int SMTO_NORMAL = 0x0000;
        public const int SMTO_BLOCK = 0x0001;
        public const int SMTO_ABORTIFHUNG = 0x0002;
        public const int SMTO_NOTIMEOUTIFNOTHUNG = 0x0008;

        private static void Main(string[] args)
        {
            if (args.Length != 3) {
                usage();
                Environment.Exit(1);
            }
            RegistryKey myKey = null;
            if (args[0] == "system") {
                myKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment", true);
            } else if (args[0] == "user") {
                myKey = Registry.CurrentUser.OpenSubKey("Environment", true);
            } else {
                usage();
                Environment.Exit(1);                
            }
            myKey.SetValue(args[1], args[2], RegistryValueKind.String);

            /* Notify all windows that environment has changed. */
            int result = 0;
            SendMessageTimeout((IntPtr) HWND_BROADCAST, WM_SETTINGCHANGE, 0, "Environment",
                               SMTO_BLOCK | SMTO_ABORTIFHUNG | SMTO_NOTIMEOUTIFNOTHUNG, 30000, ref result);
            Environment.Exit(result);
        }

        static void usage() {
            Console.WriteLine("USAGE: SetEnvVar  system|user  <variable-name> <variable-value>");
        }
    }
}
