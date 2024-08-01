using Interop.UIAutomationClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Media;

namespace bime
{

    static internal class States
    {
        public static HashSet<string> MBPaths = new HashSet<string>();
        public static OperatingSystem os = Environment.OSVersion;
        public static int Major = os.Version.Major;
        public static int Minor = os.Version.Minor;

        public static bool Off = false;

        public static bool LastNum = false;
        public static string RepeatBuffer = "重复上屏";

        public static IntPtr CandiHwnd = IntPtr.Zero;


        public static bool IsStartMenu = false;
        public static bool UseClipboard = false;
        private static IntPtr oldHwnd = IntPtr.Zero;
        public static IntPtr Hwnd = IntPtr.Zero;
        public static string WinTitle = string.Empty;
        public static string WinClass = string.Empty;
        public static string ProcessName = string.Empty;
        //   public static bool IsNewWindow = true;
        public static bool UpdateDPI = false;
        public static bool UpdateTopMost = false;
        public static bool UpdateColor = false;

        public static int HitDiff = 0;

        public static string  CustomFile = "";

        #region win32api


        #endregion

        public static CUIAutomation root = new CUIAutomation();

        public static IUIAutomationElement Focused = null;

        public static HashSet<string> WhiteListClipBoard = new HashSet<string>();


        public static bool IsDebug = false;
        public static bool SwitchedToEn = false;


        public static bool IsGdq = false;
        public static bool IsJs = false;
        public static bool IsMuyi = false;


        public static int CaretX = 0;
        public static int CaretY = 0;

        public static int WinX = 0;
        public static int WinY = 0;

        public static double maxHeight = 0;

        public static double  acHeight = 0;

        public static double acY = 0;

        public static double bottomY  = 0;

        public static double acWidth = 0;
        public static double acX = 0;

        public static double bottomX = 0;

        public static double  ScreenBottom = 1000;
        public static double ScreenRight = 1920;

  //      public static DPI_AWARENESS_CONTEXT DPIWare = DPI_AWARENESS_CONTEXT.DPI_AWARENESS_CONTEXT_UNAWARE;

        static IntPtr oldMonitor = IntPtr.Zero;
        static IntPtr oldCandiMonitor = IntPtr.Zero;
        public static void DetectMonitorChange()
        {
            IntPtr monitor = IntPtr.Zero;
            IntPtr CandiMonitor = IntPtr.Zero;
            try
            {
                monitor = Win32.MonitorFromWindow(Hwnd, 2);
                CandiMonitor = Win32.MonitorFromWindow(CandiHwnd, 2);
            }
            catch (Exception)
            {

                monitor = IntPtr.Zero;
                CandiMonitor = IntPtr.Zero;
            }


            if (monitor != oldMonitor || CandiHwnd != oldCandiMonitor)
            {
                try
                {
                    States.UpdateDPI = true;

                    Tuple<double, double> sinfo = Win32.GetWorkingArea(States.CandiHwnd);

                    States.ScreenBottom = sinfo.Item2;
                    States.ScreenRight = sinfo.Item1;
                }
                catch (Exception)
                {


                }

                oldMonitor = monitor;
                oldCandiMonitor = CandiMonitor;
            }

        }
        public static void Update()
        {

            Hwnd = Win32.GetForegroundWindow();

            DetectMonitorChange();

            if (Hwnd != IntPtr.Zero && (Hwnd != oldHwnd || oldHwnd == IntPtr.Zero))
            {
                /*
                try
                {
                    DPIWare = Win32.GetWindowDpiAwarenessContext(Hwnd);
                }
                catch (Exception)
                {
                    DPIWare = DPI_AWARENESS_CONTEXT.DPI_AWARENESS_CONTEXT_UNAWARE;


                }
                */
                maxHeight = 0;
                try
                {
                    WinClass = Win32.GetWindowClassName(Hwnd);
                    WinTitle = Win32.GetWindowTitle(Hwnd);
                    ProcessName = Win32.GetActiveProcessFileName(Hwnd);


                }
                catch (Exception)
                {


                }


                UseClipboard = WhiteListClipBoard.Contains(ProcessName);
                IsStartMenu = WinClass == "Windows.UI.Core.CoreWindow" && WinTitle == "搜索";
                oldHwnd = Hwnd;
                //       IsNewWindow = true;
                UpdateDPI = true;

                if (WinTitle != "Bime加词" && WinTitle != "Bime设置")
                     UpdateTopMost = true;

                IsGdq = WinTitle == "GainDutch" || WinTitle == "Pain打器" || WinTitle == "查找";

                IsJs = WinTitle.Contains("长流跟打器") || WinTitle.Contains("极速跟打器");
                IsMuyi = WinTitle.Contains("木易") || WinTitle.Contains("赛文跟打") || WinTitle.Contains("我爱打字网");
            }
            else
            {
                //         IsNewWindow = false;
            }

        }


    }
}
