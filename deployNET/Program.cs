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
        public static void VerifyAndInstall(string[] pathsList)
        {
            foreach (var path in pathsList)
            {
                if (Directory.Exists(path))
                {
                    File.WriteAllBytes(path + @"\SecureFTP.dll", Resource1.SecureFTP);
                    File.WriteAllBytes(path + @"\SFTPUI.dll", Resource1.SFTPUI);
                }
                else
                {
                    Directory.CreateDirectory(path);
                    File.WriteAllBytes(path + @"\SecureFTP.dll", Resource1.SecureFTP);
                    File.WriteAllBytes(path + @"\SFTPUI.dll", Resource1.SFTPUI);
                }
            }


        }
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
            #region SQLPaths
            string[] sql32BitVersions = new string[] {
             @"C:\Program Files (x86)\Microsoft SQL Server\150\DTS\Tasks",
             @"C:\Program Files (x86)\Microsoft SQL Server\140\DTS\Tasks",
             @"C:\Program Files (x86)\Microsoft SQL Server\130\DTS\Tasks",
             @"C:\Program Files (x86)\Microsoft SQL Server\120\DTS\Tasks",
             @"C:\Program Files (x86)\Microsoft SQL Server\110\DTS\Tasks"
            };

            string[] sql64BitVersions = new string[]
            {
                @"C:\Program Files\Microsoft SQL Server\150\DTS\Tasks",
                @"C:\Program Files\Microsoft SQL Server\140\DTS\Tasks",
                @"C:\Program Files\Microsoft SQL Server\130\DTS\Tasks",
                @"C:\Program Files\Microsoft SQL Server\120\DTS\Tasks",
                @"C:\Program Files\Microsoft SQL Server\110\DTS\Tasks"
            };
            

            #endregion

            #region Validation
            bool gacUtilExists = File.Exists(@"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\gacutil.exe");
            bool admin = isAdmin();
            bool sqlInstalled = Directory.Exists(@"C:\Program Files\Microsoft SQL Server") || Directory.Exists(@"C:\Program Files (x86)\Microsoft SQL Server");
            #endregion

            //If validation is good. Proceed to install.
            if (gacUtilExists && admin && sqlInstalled)
            {

                //Create installation directory to stage dlls and BATCH script.
                Directory.CreateDirectory(@"C:\sftpInstall");
                File.WriteAllBytes(@"C:\sftpInstall\WinSCPnet.dll", Resource1.WinSCPnet);
                File.WriteAllBytes(@"C:\sftpInstall\SecureFTP.dll", Resource1.SecureFTP);
                File.WriteAllBytes(@"C:\sftpInstall\SFTPUI.dll", Resource1.SFTPUI);
                File.WriteAllText(@"C:\sftpInstall\cai.bat", Resource1.copyandinstall);
                VerifyAndInstall(sql32BitVersions);
                VerifyAndInstall(sql64BitVersions);
                Process.Start(@"C:\sftpInstall\cai.bat");

                //provide user option keep the installation files.
                Console.WriteLine("Installation successful");
                Console.WriteLine("\nAn Installation folder containing the dlls and BATCH file was created at C:\\sftpInstall\nIf you'd like to keep this folder for reference Press Y.\nOr else press any other key to finish installation and delete setup folder.");
                if (Console.ReadKey().Key != ConsoleKey.Y)
                {
                    Directory.Delete(@"C:\sftpInstall", true);
                }
                
            }
            //Exit paths of program if unable to install.
            else
            {
                if (!gacUtilExists)
                {
                    Console.WriteLine("GAC utility not found or doesn't exist on local machine");
                }
                if (!admin)
                {
                    Console.WriteLine("Installer application not ran as administator");
                }
                if (!sqlInstalled)
                {
                    Console.WriteLine("Sql server installation not found on this machine.");
                }
                Console.WriteLine("Unable to proceed with installation. Press any key to exit");
                Console.ReadKey();

            }
        }


    }
}
