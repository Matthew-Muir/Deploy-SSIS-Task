using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;

namespace deployNET
{
    class Program
    {
        public static bool isAdmin()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        static void Main(string[] args)
        {
            #region Validation
            bool gacUtilExists = File.Exists(@"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\gacutil.exe");
            bool taskDirectoryExists = Directory.Exists(@"C:\Program Files\Microsoft SQL Server\150\DTS\Tasks");
            bool x86taskDirectoryExists = Directory.Exists(@"C:\Program Files (x86)\Microsoft SQL Server\150\DTS\Tasks");
            bool sqlInstalled = taskDirectoryExists || x86taskDirectoryExists;
            bool admin = isAdmin();
            #endregion

            if (gacUtilExists && sqlInstalled && admin)
            {
                //Create necessary directory for task dll if it doesn't exist already
                if (!x86taskDirectoryExists)
                {
                    Directory.CreateDirectory(@"C:\Program Files (x86)\Microsoft SQL Server\150\DTS\Tasks");
                }

                //Create installation directory to stage dlls and BATCH script.
                Directory.CreateDirectory(@"C:\install");
                File.WriteAllBytes(@"C:\install\WinSCPnet.dll", Resource1.WinSCPnet);
                File.WriteAllBytes(@"C:\install\SecureFTP.dll", Resource1.SecureFTP);
                File.WriteAllBytes(@"C:\install\SFTPUI.dll", Resource1.SFTPUI);
                File.WriteAllText(@"C:\install\cai.bat", Resource1.copyandinstall);
                Process.Start(@"C:\install\cai.bat");

                //provide user option keep the installation files.
                Console.WriteLine("Installation successful");
                Console.WriteLine("\nAn Installation folder containing the dlls and BATCH file was created at C:\\install\nIf you'd like to keep this folder for reference Press Y.\nOr else press any other key to finish installation and delete setup folder.");
                if (Console.ReadKey().Key != ConsoleKey.Y)
                {
                    Directory.Delete(@"C:\install", true);
                }
                
            }
            //Exit paths of program if unable to install.y
            else
            {
                if (!gacUtilExists)
                {
                    Console.WriteLine("GAC utility not found or doesn't exist on local machine");
                }
                if (!sqlInstalled)
                {
                    Console.WriteLine("Neccesary SQL program directories not found");
                }
                if (!admin)
                {
                    Console.WriteLine("Installer application not ran as administator");
                }
                Console.WriteLine("Unable to proceed with installation. Press any key to exit");
                Console.ReadKey();
            }
        }
    }
}
