//
// C# (Windows) XP+
// MSol.Update
// v 0.1, 09.02.2023
// artem.karimov@weadmire.io
// en,ru,1251,utf-8
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Xml;
using Microsoft.Win32;

namespace MSol.Update
{
    internal partial class Updater
    {
        #region PARAMETERS
        private static Dictionary<string, object> UpdaterParams = new Dictionary<string, object>()
        {
            {"hideConsole",false},
            {"showToastNotify",false},
            {"closeMSolDeskProc",false},             
            {"closeMSolSvcProc",false}, 
            {"runMSolSvcAfterInstall",false}, 
            {"runMSolDeskAfterInstall",false},
            {"rebootOnCompleted",false},
            {"toastOnCompleted",false},
            {"deleteSourceOnCompleted",false},
            {"updateVersionText","0.0.0.0"}, 
            {"updateCommonText",""},
            {"startupDelay",(int)0},
            {"completedDelay",(int)0}
        };
        private static bool hideConsole { get { return (bool)UpdaterParams["hideConsole"]; } set { UpdaterParams["hideConsole"] = value; } }
        private static bool showToastNotify { get { return (bool)UpdaterParams["showToastNotify"]; } set { UpdaterParams["showToastNotify"] = value; } }
        private static bool closeMSolDeskProc { get { return (bool)UpdaterParams["closeMSolDeskProc"]; } set { UpdaterParams["closeMSolDeskProc"] = value; } }
        private static bool closeMSolSvcProc { get { return (bool)UpdaterParams["closeMSolSvcProc"]; } set { UpdaterParams["closeMSolSvcProc"] = value; } }
        private static bool runMSolSvcAfterInstall { get { return (bool)UpdaterParams["runMSolSvcAfterInstall"]; } set { UpdaterParams["runMSolSvcAfterInstall"] = value; } }
        private static bool runMSolDeskAfterInstall { get { return (bool)UpdaterParams["runMSolDeskAfterInstall"]; } set { UpdaterParams["runMSolDeskAfterInstall"] = value; } }
        private static bool rebootOnCompleted { get { return (bool)UpdaterParams["rebootOnCompleted"]; } set { UpdaterParams["rebootOnCompleted"] = value; } }
        private static bool toastOnCompleted { get { return (bool)UpdaterParams["toastOnCompleted"]; } set { UpdaterParams["toastOnCompleted"] = value; } }
        private static bool deleteSourceOnCompleted { get { return (bool)UpdaterParams["deleteSourceOnCompleted"]; } set { UpdaterParams["deleteSourceOnCompleted"] = value; } }
        private static string updateVersionText { get { return (string)UpdaterParams["updateVersionText"]; } set { UpdaterParams["updateVersionText"] = value; } }
        private static string updateCommonText { get { return (string)UpdaterParams["updateCommonText"]; } set { UpdaterParams["updateCommonText"] = value; } }
        private static int startupDelay { get { return (int)UpdaterParams["startupDelay"]; } set { UpdaterParams["startupDelay"] = value; } }
        private static int completedDelay { get { return (int)UpdaterParams["completedDelay"]; } set { UpdaterParams["completedDelay"] = value; } }
        #endregion PARAMETERS        

        #region REQUIRED

        /// <summary>
        ///     INIT BLOCK
        /// </summary>
        /// <param name="args"></param>
        private static bool Initialize(string[] args)
        {
            Type t = typeof(Updater);
            MethodInfo[] methods = t.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (MethodInfo m in methods)
                foreach (Attribute att in m.GetCustomAttributes())
                    if (att is UpdateVersionAttribute)
                        if ((((UpdateVersionAttribute)att).version == UPDATE_VERSION) && ((UpdateVersionAttribute)att).Init)
                        {
                            m.Invoke(t, new object[] { args });
                            return true;
                        };
            return false;
        }

        /// <summary>
        ///     MAIN BLOCK
        /// </summary>
        /// <param name="args"></param>
        private static bool RunUpdate(string[] args)
        {
            Type t = typeof(Updater);
            MethodInfo[] methods = t.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (MethodInfo m in methods)
                foreach (Attribute att in m.GetCustomAttributes())
                    if (att is UpdateVersionAttribute)
                        if ((((UpdateVersionAttribute)att).version == UPDATE_VERSION) && ((UpdateVersionAttribute)att).Main)
                        {
                            object res = null;
                            res = m.Invoke(t, new object[] { args });
                            if ((res != null) && (res.ToString() == false.ToString())) return false;
                            return true;
                        };
            return false;
        }
        
        public static void Start(string[] args)
        {
            Exception error = null;
            bool res = false;
            UpdateLog.Write("# ----------- #", false, true, "");            
            try { res = Initialize(args); if (!res) error = new Exception("UPDATE_VERSION INCORRECT");  } catch (Exception ex) { error = ex.InnerException != null ? ex.InnerException : ex; res = false; };
            if (!res) { UpdateLog.Write($"Update Failed on Start: {error?.Message}", true, true, ""); return; };
            if (hideConsole) Helper.HideConsole();
            if (showToastNotify) Helper.ShowToastStartup(updateVersionText);
            if (startupDelay > 0) Thread.Sleep(startupDelay);
            UpdateLog.Write($"Starting Update v{updateVersionText}...", true, true, "");
            if (!string.IsNullOrEmpty(updateCommonText)) UpdateLog.Write($"Common: {updateCommonText}");
            BeforeRun(out bool MSolDeskIsRunning, out bool MSolSvcIsRunning);
            res = false;
            try { res = RunUpdate(args); } catch (Exception ex) { error = ex.InnerException != null ? ex.InnerException : ex; res = false; };
            bool successfull = res && (error == null);
            bool reboot = successfull && rebootOnCompleted;
            if (!reboot) AfterRun(MSolDeskIsRunning, MSolSvcIsRunning);
            UpdateLog.Write($"Update v{updateVersionText} " + (successfull ? "Successfull" : "Failed: ") + error?.Message.ToString(), true, true, "");
            if(reboot) UpdateLog.Write("Reboot Required");
            if (toastOnCompleted) Helper.ShowToastCompleted(updateVersionText, successfull, reboot ? "Reboot required" : error?.Message);                                                
            if (completedDelay > 0) Thread.Sleep(completedDelay);
            if (successfull && deleteSourceOnCompleted) DeleteSourceFiles();
            if (reboot) Helper.RebootRequired();
            Environment.Exit(successfull ? 0 : 1);
        }

        private static void BeforeRun(out bool MSolDeskIsRunning, out bool MSolSvcIsRunning)
        {
            MSolDeskIsRunning = ProcWindows.IsRunning("MSolDesk") || ProcWindows.IsRunning("MSolDesk.exe");
            MSolSvcIsRunning = ProcWindows.IsRunning("MSolSvc") || ProcWindows.IsRunning("MSolSvc.exe");

            if (closeMSolDeskProc)
            {
                UpdateLog.Write("Closing MSolDesk... ", true, false);
                Exception error = null;
                try { ProcWindows.Kill("MSolDesk.exe"); } catch (Exception ex) { error = ex; }; // Kill Main Proc
                try { ProcWindows.Kill("MSolDesk"); } catch (Exception ex) { error = ex; };     // Kill Main Proc
                UpdateLog.Write(error == null ? "OK" : $"Failed: {error?.Message}", false, true, "");
            };
            if (closeMSolSvcProc)
            {
                UpdateLog.Write("Closing MSolSvc... ", true, false);
                Exception error = null;
                try { Helper.StopMSolSvcService(); } catch (Exception ex) { error = ex; };
                try { ProcWindows.Kill("MSolSvc.exe"); } catch (Exception ex) { error = ex; };  // Kill Svc Proc
                try { ProcWindows.Kill("MSolSvc"); } catch (Exception ex) { error = ex; };      // Kill Svc Proc
                UpdateLog.Write(error == null ? "OK" : $"Failed: {error?.Message}", false, true, "");
            };
        }

        private static void AfterRun(bool startMSolDesk, bool startMSolSvc)
        {
            if (runMSolSvcAfterInstall || (startMSolSvc && closeMSolSvcProc))
            {
                UpdateLog.Write("Starting MSolSvc... ", true, false);
                try
                {
                    Helper.StartMSolSvcService();
                    UpdateLog.Write("OK", false, true, "");
                }
                catch (Exception ex)
                {
                    UpdateLog.Write($"Failed: {ex?.Message}", false, true, "");
                };
            };
            if (runMSolDeskAfterInstall || (startMSolDesk && closeMSolDeskProc))
            {
                UpdateLog.Write("Starting MSolDesk... ", true, false);
                try
                {
                    Helper.RunMSolDesk("-notify:11"); // ACTION_AFTER_UPDATER = 0x0B; /* Run Application After Updater */            
                    UpdateLog.Write("OK", false, true, "");
                }
                catch (Exception ex)
                {
                    UpdateLog.Write($"Failed: {ex?.Message}", false, true, "");
                };
            };
        }

        #endregion REQUIRED

        public static bool LoadParamatersFromUpdateTxt()
        {
            string path = Path.Combine(Helper.CurrentDirectory(), "Update.txt");
            if (!File.Exists(path))
            {
                try { SaveParamatersToUpdateTxt(); } catch { };
                return false;
            };
            using (StreamReader sr = new StreamReader(path))
                while(!sr.EndOfStream)
                {
                    string line = sr.ReadLine().Trim();
                    if (string.IsNullOrEmpty(line) || line.IndexOf("=") < 0 || line.StartsWith("[") || line.StartsWith("]") || line.StartsWith("#") || line.StartsWith(";")) continue;
                    string[] kv = line.Split(new char[] { '=' }, 2);
                    if (kv[1].ToLower() == "false") { UpdaterParams[kv[0]] = false; continue; };
                    if (kv[1].ToLower() == "true") { UpdaterParams[kv[0]] = true; continue; };
                    if (string.IsNullOrEmpty(kv[1]) || (!char.IsDigit(kv[1][0]))) UpdaterParams[kv[0]] = kv[1].Trim('"');
                    else UpdaterParams[kv[0]] = kv[1].Contains(".") ? (object)kv[1].Trim('"') : (object)int.Parse(kv[1]);                    
                };
            return true;
        }

        public static void SaveParamatersToUpdateTxt()
        {
            string path = Path.Combine(Helper.CurrentDirectory(), "Update[sample].txt");
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine("[UpdaterParams]");
                foreach (KeyValuePair<string,object> kv in UpdaterParams)
                    sw.WriteLine($"{kv.Key}={kv.Value}");
            };
        }

        public static bool DeleteSourceFiles()
        {
            try { (new DirectoryInfo(Helper.CurrentDirectory())).Delete(true); } catch { };
            try
            {
                string text = "@echo off\r\nping 127.0.0.1 -n 6 > nul\r\ndel Update.exe\r\ndel Update.pdb\r\ndel Update.cmd";
                string path = Path.Combine(Helper.CurrentDirectory(), "Update.cmd");
                using (StreamWriter sw = new StreamWriter(path)) sw.Write(text);
                Helper.RunCmd(path, null, Helper.CurrentDirectory(), true);
            }
            catch { };
            return true;
        }
    }

    internal static class Helper
    {
        private static DateTime LastNotify = DateTime.MinValue;
        private const int MaxNotifyInterval = 15;

        #region WinAPI

        [Flags]
        private enum ExitWindows : uint
        {
            // ONE of the following five:
            EWX_LogOff = 0x00,
            EWX_ShutDown = 0x01,
            EWX_Reboot = 0x02,
            EWX_Force = 0x04,
            EWX_PowerOff = 0x08,
            EWX_ForceIfHung = 0x10,
            EWX_RestartApps = 0x40,
            // plus AT MOST ONE of the following two:                        
            EWX_HYBRID_SHUTDOWN = 0x00400000
        }

        [Flags]
        private enum ShutdownReason : uint
        {
            MajorApplication = 0x00040000,
            MajorHardware = 0x00010000,
            MajorLegacyApi = 0x00070000,
            MajorOperatingSystem = 0x00020000,
            MajorOther = 0x00000000,
            MajorPower = 0x00060000,
            MajorSoftware = 0x00030000,
            MajorSystem = 0x00050000,

            MinorBlueScreen = 0x0000000F,
            MinorCordUnplugged = 0x0000000b,
            MinorDisk = 0x00000007,
            MinorEnvironment = 0x0000000c,
            MinorHardwareDriver = 0x0000000d,
            MinorHotfix = 0x00000011,
            MinorHung = 0x00000005,
            MinorInstallation = 0x00000002,
            MinorMaintenance = 0x00000001,
            MinorMMC = 0x00000019,
            MinorNetworkConnectivity = 0x00000014,
            MinorNetworkCard = 0x00000009,
            MinorOther = 0x00000000,
            MinorOtherDriver = 0x0000000e,
            MinorPowerSupply = 0x0000000a,
            MinorProcessor = 0x00000008,
            MinorReconfig = 0x00000004,
            MinorSecurity = 0x00000013,
            MinorSecurityFix = 0x00000012,
            MinorSecurityFixUninstall = 0x00000018,
            MinorServicePack = 0x00000010,
            MinorServicePackUninstall = 0x00000016,
            MinorTermSrv = 0x00000020,
            MinorUnstable = 0x00000006,
            MinorUpgrade = 0x00000003,
            MinorWMI = 0x00000015,

            FlagUserDefined = 0x40000000,
            FlagPlanned = 0x80000000
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ExitWindowsEx(ExitWindows uFlags, ShutdownReason dwReason);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool IsWow64Process(IntPtr hProcess, out bool Wow64Process);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool Wow64DisableWow64FsRedirection(out IntPtr OldValue);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        #endregion WinAPI

        public static void HideConsole() { try { ShowWindow(GetConsoleWindow(), 0); } catch { }; }

        public static void ShowConsole() { try { ShowWindow(GetConsoleWindow(), 5); } catch { }; }


        /// <summary>
        ///     Start MSolSvc Service
        /// </summary>
        /// <returns></returns>
        public static bool StartMSolSvcService() { return ProcessService("/start /silent"); }

        /// <summary>
        ///     Stop MSolSvc Service
        /// </summary>
        /// <returns></returns>
        public static bool StopMSolSvcService() { return ProcessService("/stop /silent"); }

        /// <summary>
        ///     Install MSolSvc Service
        /// </summary>
        /// <returns></returns>
        public static bool InstallMSolSvcService() { return ProcessService("/install /silent"); }

        /// <summary>
        ///     Uninstall MSolSvc Service
        /// </summary>
        /// <returns></returns>
        public static bool UninstallMSolSvcService() { return ProcessService("/uninstall /silent"); }

        /// <summary>
        ///     Application Path
        /// </summary>
        public static string AppExePath
        {
            get
            {
                try
                {
                    string path = "";
                    using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Magic Solutions\MSolDesk")) { path = rk.GetValue("ApplicationPath", null)?.ToString(); };
                    if (!string.IsNullOrEmpty(path)) return path;
                }
                catch { };
                try
                {
                    string path = "";
                    using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Magic Solutions\MSolDesk")) { path = rk.GetValue("ApplicationFile", null)?.ToString(); };
                    if (!string.IsNullOrEmpty(path)) return Path.GetDirectoryName(path);
                }
                catch { };
                return (new DirectoryInfo(CurrentDirectory())).Parent.FullName;
            }
        }

        /// <summary>
        ///     MSolDesk Application Data Path
        /// </summary>
        public static string AppDataSvcPath
        { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments).Replace("Documents", "AppData"), @"Magic Solutions\MSolSvc"); } }

        /// <summary>
        ///     MSolSvc Application Data Path
        /// </summary>
        public static string AppDataDeskPath
        { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments).Replace("Documents", "AppData"), @"Magic Solutions\MSolDesk"); } }

        /// <summary>
        ///     Server Base Url (https://.../api/v1/back/) without {domain}
        /// </summary>
        public static string ServerBaseURL
        {
            get
            {
                try
                {
                    string path = "";
                    using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Magic Solutions\MSolDesk")) { path = rk.GetValue("ServerBaseURL", null)?.ToString(); };
                    if (!string.IsNullOrEmpty(path)) return path;
                }
                catch { };
                return null;
            }
        }

        /// <summary>
        ///     Server Base Url (https://.../api/v1/back/) with {domain}
        /// </summary>
        public static string ServerUrl 
        { get { return ServerBaseURL.Replace("{DOMAIN}", "herokuapp.com"); } }

        /// <summary>
        ///     Current Directory
        /// </summary>
        /// <returns></returns>
        public static string CurrentDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
            // return Application.StartupPath;
            // return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            // return System.IO.Directory.GetCurrentDirectory();
            // return Environment.CurrentDirectory;
            // return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            // return System.IO.Path.GetDirectory(Application.ExecutablePath);
        }               

        public static bool IsAdmin()
        {
            //[DllImport("shell32.dll", SetLastError = True)]
            //[return: MarshalAs(UnmanagedType.Bool)]
            //static extern bool IsUserAnAdmin(void);

            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            };
        }

        public static bool Reboot(bool force = true)
        {
            try
            {
                if (force)
                    ExitWindowsEx(ExitWindows.EWX_Reboot | ExitWindows.EWX_ForceIfHung, ShutdownReason.MajorHardware | ShutdownReason.MinorInstallation | ShutdownReason.FlagPlanned);
                else
                    ExitWindowsEx(ExitWindows.EWX_Reboot, ShutdownReason.MajorHardware | ShutdownReason.MinorInstallation | ShutdownReason.FlagPlanned);
                return true;
            }
            catch (Exception ex) { return false; };
        }

        public static bool RebootRequired()
        {
            try
            {
                ExitWindowsEx(ExitWindows.EWX_Reboot, ShutdownReason.MajorApplication | ShutdownReason.MajorSoftware | ShutdownReason.MinorInstallation | ShutdownReason.FlagPlanned);
                return true;
            }
            catch (Exception ex) { return false; };
        }

        public static void OpenMachineInfoGuide()
        {
            try
            {
                bool bWow64 = false;
                if (IsWow64Process(System.Diagnostics.Process.GetCurrentProcess().Handle, out bWow64) && bWow64)
                {
                    IntPtr OldValue = IntPtr.Zero;
                    bool bRet = Wow64DisableWow64FsRedirection(out OldValue);
                };
                System.Diagnostics.Process.Start("msinfo32");
            }
            catch { };
        }

        public static void OpenOSAbout()
        {
            try
            {
                bool bWow64 = false;
                if (IsWow64Process(System.Diagnostics.Process.GetCurrentProcess().Handle, out bWow64) && bWow64)
                {
                    IntPtr OldValue = IntPtr.Zero;
                    bool bRet = Wow64DisableWow64FsRedirection(out OldValue);
                };
                System.Diagnostics.Process.Start("ms-settings:about");
                //System.Diagnostics.Process.Start("winver");                
            }
            catch { };
        }

        public static void OpenUACGuide()
        {
            try { System.Diagnostics.Process.Start("useraccountcontrolsettings"); } catch { };
        }

        public static bool RunMSolDesk(string param = null)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(Path.Combine(Helper.AppExePath, "MSolDesk.exe"));
            startInfo.Verb = "runas";
            //startInfo.UseShellExecute = true;
            startInfo.UseShellExecute = false;
            startInfo.Arguments = param;
            try { Process.Start(startInfo); return true; }
            catch (Exception ex) { return false; };
        }

        public static void RunAsNotAdmin(string exe, string args = null, string workingdir = null, bool hideWindow = false)
        {
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo("explorer.exe");
            startInfo.UseShellExecute = true;
            if (!string.IsNullOrEmpty(workingdir)) startInfo.WorkingDirectory = workingdir;
            startInfo.Arguments = $"\"{exe}\"";
            if (!string.IsNullOrEmpty(args)) startInfo.Arguments += $" {args}";
            if (hideWindow) startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            System.Diagnostics.Process.Start(startInfo);
        }

        public static void RunAsAdmin(string exe, string args = null, string workingdir = null, bool hideWindow = false)
        {
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(exe, args);
            startInfo.UseShellExecute = false;
            if (!string.IsNullOrEmpty(workingdir)) startInfo.WorkingDirectory = workingdir;
            startInfo.Verb = "runas";
            if (hideWindow) startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            System.Diagnostics.Process.Start(startInfo);
        }

        public static void RunCmd(string exe, string args = null, string workingdir = null, bool hideWindow = false)
        {
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(exe, args);
            startInfo.UseShellExecute = true;
            if (!string.IsNullOrEmpty(workingdir)) startInfo.WorkingDirectory = workingdir;
            startInfo.Verb = "runas";
            if(hideWindow) startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            System.Diagnostics.Process.Start(startInfo);
        }

        /// <summary>
        ///     Run MSolSvc.exe with params
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private static bool ProcessService(string param)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            startInfo.FileName = Path.Combine(Helper.AppExePath, "MSolSvc.exe");
            startInfo.Verb = "runas";
            //startInfo.UseShellExecute = true;
            startInfo.UseShellExecute = false;
            startInfo.StandardOutputEncoding = Encoding.GetEncoding(1251);//Encoding.ASCII;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;
            startInfo.Arguments = param;

            string output = "";
            try
            {
                Process proc = new Process();
                proc.StartInfo = startInfo;
                proc.OutputDataReceived += (delegate (object sender, DataReceivedEventArgs e) { output += e.Data + "\r\n"; });
                proc.Start();
                proc.BeginOutputReadLine();
                { try { output += proc.StandardOutput.ReadToEnd(); } catch { }; };
                if (!proc.HasExited) proc.WaitForExit();
                return proc.ExitCode == 0;
            }
            catch (Exception ex) { return false; };
        }

        public static bool IsToastAppRunning()
        {
            try { return Process.GetProcessesByName("ShowToast")[0].MainWindowHandle != IntPtr.Zero; } catch { };
            return false;
        }

        public static bool ShowToastStartup(string updateVersionText)
        {
            return ShowNotifyViaProxyApp(0, 
                $"Update v{updateVersionText} found", 
                new string[] { "Processing update..." },
                "Installing new features...", 
                null,  // No Buttons
                null,  // Default Logo
                60, // 1 min
                0); // Do Nothing
        }

        public static bool ShowToastCompleted(string updateVersionText, bool successfull, string message)
        {
            LastNotify = DateTime.MinValue;
            return ShowNotifyViaProxyApp(0,
                $"Update v{updateVersionText} " + (successfull ? "successfull" : "failed"),
                new string[] { successfull ? "Processing done" : "Something went wrong" },
                message,
                null,  // No Buttons
                null,  // Default Logo
                3600, // 1 hour
                0);   // Do Nothing
        }

        public static bool ShowNotifyViaProxyApp(int Action, string Header, string[] Text = null, string attrText = null, (string text, int action)[] Buttons = null, string logo = null, int timeOut = 900, int eventData = 0)
        {
            if (IsToastAppRunning()) return false; // No Toast If Run
            if ((DateTime.Now.Subtract(LastNotify)).TotalSeconds <= MaxNotifyInterval) return false;
            LastNotify = DateTime.Now;

            ProcessStartInfo psi = new ProcessStartInfo(Path.Combine(XMLSaved<int>.CurrentDirectory(), Path.Combine(AppExePath, "ShowToast.exe")), "-showtoast");
            psi.WorkingDirectory = AppExePath;
            psi.EnvironmentVariables["SHOWTOAST_TIMER"] = timeOut.ToString();
            psi.EnvironmentVariables["SHOWTOAST_HEADER"] = Header;
            if (Text != null)
                for (int i = 0; i < Text.Length; i++)
                    psi.EnvironmentVariables[$"SHOWTOAST_TEXT_{i + 1}"] = Text[i];
            if (!string.IsNullOrEmpty(attrText))
                psi.EnvironmentVariables["SHOWTOAST_ATTR_TEXT"] = attrText;
            if (Buttons != null) for (int i = 0; i < Buttons.Length; i++)
                {
                    psi.EnvironmentVariables[$"SHOWTOAST_BTNTXT_{i + 1}"] = Buttons[i].text;
                    psi.EnvironmentVariables[$"SHOWTOAST_BTNACT_{i + 1}"] = Buttons[i].action.ToString();
                };
            psi.EnvironmentVariables["SHOWTOAST_ACTION"] = Action.ToString();

            if (string.IsNullOrEmpty(logo))
                psi.EnvironmentVariables["SHOWTOAST_LOGO_ICON"] = Path.Combine(AppExePath, "logo.png");
            else
                psi.EnvironmentVariables["SHOWTOAST_LOGO_ICON"] = logo;

            psi.EnvironmentVariables["SHOWTOAST_LOGO_CIRCLE"] = "1";
            psi.UseShellExecute = false;
            psi.WindowStyle = ProcessWindowStyle.Normal;
            try { Process proc = Process.Start(psi); return true; } catch { return false; };
        }
    }

    internal static class ProcWindows
    {
        #region WinAPI
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);
        #endregion WinAPI

        public static void ShowLaunched()
        {
            string currProcName = System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location);
            Process[] procs = Process.GetProcessesByName(currProcName);
            foreach (Process proc in procs)
                if ((proc.MainWindowHandle != IntPtr.Zero) && (proc.MainWindowHandle != Process.GetCurrentProcess().MainWindowHandle))
                {
                    ShowWindow(proc.MainWindowHandle, 5);
                    SetForegroundWindow(proc.MainWindowHandle);
                    break;
                };
        }

        public static void Kill()
        {
            string currProcName = System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location);
            Process[] procs = Process.GetProcessesByName(currProcName);
            foreach (Process proc in procs)
                if ((proc.MainWindowHandle != IntPtr.Zero) && (proc.MainWindowHandle != Process.GetCurrentProcess().MainWindowHandle))
                {
                    try { TerminateProcess(proc.Handle, 0); } catch { };
                    try { proc.Kill(); } catch { };
                    break;
                };
        }

        public static void Kill(string procName)
        {
            Process[] procs = Process.GetProcessesByName(procName);
            foreach (Process proc in procs)
                if ((proc.MainWindowHandle != IntPtr.Zero) && (proc.MainWindowHandle != Process.GetCurrentProcess().MainWindowHandle))
                {
                    try { TerminateProcess(proc.Handle, 0); } catch { };
                    try { proc.Kill(); } catch { };
                    break;
                };
        }

        public static void Kill(int procId)
        {
            try
            {
                Process proc = Process.GetProcessById(procId);
                if ((proc.MainWindowHandle != IntPtr.Zero) && (proc.MainWindowHandle != Process.GetCurrentProcess().MainWindowHandle))
                {
                    try { TerminateProcess(proc.Handle, 0); } catch { };
                    try { proc.Kill(); } catch { };
                };
            }
            catch { };
        }

        public static bool IsRunning(string procName)
        {
            try
            {
                Process[] procs = Process.GetProcessesByName(procName);
                foreach (Process proc in procs)
                    if ((proc.MainWindowHandle != IntPtr.Zero) && (proc.MainWindowHandle != Process.GetCurrentProcess().MainWindowHandle))
                        return true;
            }
            catch { };
            return false;
        }
    }

    internal static class UpdateLog
    {
        private static Mutex writeMutex = new Mutex();
        private static string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments).Replace("Documents", "AppData"), @"Magic Solutions\MSolSvc");
        private static string fileName = "update.log";
        private static Encoding _enc = Encoding.UTF8;

        private static string FileName { get { return Path.Combine(filePath, fileName); } }
        static UpdateLog() { try { if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath); } catch { }; }

        public static void Write(string msg, bool addDateTime = true, bool CRLF = true, string prefix = ".. ")
        {
            string text = "";
            if (addDateTime) text += string.Format("{0}: ", DateTime.UtcNow.ToString("yyyyMMddHHmmss"));
            if (!string.IsNullOrEmpty(prefix)) text += prefix;
            if (!string.IsNullOrEmpty(msg)) text += msg;
            if (CRLF) text += "\r\n";

            try { Console.Write(text); } catch { };

            writeMutex.WaitOne();
            FileStream fs = null;
            try
            {
                fs = new FileStream(FileName, FileMode.Append, FileAccess.Write);
                byte[] data = _enc.GetBytes(text);
                fs.Write(data, 0, data.Length);
                fs.Flush();
            }
            catch { }
            finally { if (fs != null) fs.Close(); };
            writeMutex.ReleaseMutex();
        }
    }

    public class UpdateVersionAttribute : System.Attribute
    {
        public string version = "";
        private bool init = false;
        private bool main = false;

        public bool Init { get { return init; } set { init = value; if (value) main = false; } }
        public bool Main { get { return main; } set { main = value; if (value) init = false; } }

        public UpdateVersionAttribute(string version) { this.version = version; }
    }
}
