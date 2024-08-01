using Interop.UIAutomationClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Markup;

namespace bime
{
    /// <summary>
    /// WinCandidate.xaml 的交互逻辑
    /// </summary>
    public partial class WinCandidate : Window
    {

        IntPtr oldMonitor = IntPtr.Zero;
        public  void DetectMonitorChange()
        {
            IntPtr monitor = Win32.MonitorFromWindow(States.CandiHwnd, 2);

            if (monitor != oldMonitor)
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
            }

        }


        public void Update()
        {




            if (Candi.Code == "")
            {
  
                HideCandidate();
                return;


            }
            else if (Config.GetBool("竖排候选")) //竖排
            {

                if (Config.GetBool("隐藏候选"))
                {
                    States.acHeight = Config.GetDouble("字体大小") + 16;
                }
                else
                {

                    States.acHeight = (double)(Candi.Current.Count + 1) * Config.GetDouble("字体大小") * 1.5 + 16;
                }



                StringBuilder sb = new StringBuilder(512);

                string SplitCode = "\n";
                string SplitCandi = "\n";
                string SplitComment = "";

                if (Config.GetString("编码伪装")!= "")
                    sb.Append(GetMask(Candi.Code));
         //           sb.Append(GetMose( Candi.Code.Length));
      //          sb.Append(new string('·', Candi.Code.Length));
                else
                    sb.Append(Candi.Code);






                if (Candi.TotalCandiCount > 0 && !Config.GetBool("隐藏候选"))
                {

                    int CandiCount = 1;

                    sb.Append(SplitCode);



                    foreach (var c in Candi.Current)
                    {


                        if (CandiCount > 1)
                            sb.Append(SplitCandi);

                        if (Config.GetBool("显示候选序号"))
                        {
                            sb.Append(CandiCount);
                            sb.Append(" ");
                        }

                        sb.Append(Candi.ConvertCandi(c).Replace("\r\n", "\\n").Replace("\n", "\\n"));



                        string appendStr = GetSplitAndComment(c);

                        if (appendStr != null && appendStr.Length > 0)
                        {
                            sb.Append(SplitComment);
                            sb.Append("〔");
                            sb.Append(appendStr);
                            sb.Append("〕");
                        }

                        CandiCount++;
                    }


                }




                Disp.Text = sb.ToString();


                var lines = Disp.Text.Split('\n');
                int maxChar = 0;

                foreach (var line in lines)
                {
                    if (maxChar < line.Length)
                        maxChar = line.Length;
                }
                States.acWidth = Math.Max(Disp.MinWidth, (maxChar - 4) * Config.GetDouble("字体大小") + 16);
         //       States.acWidth = (maxChar - 4) * Config.GetDouble("字体大小") + 16;
                //  GetVerticalBias();

                ShowCandidate();




            }
            else //横排
            {
                StringBuilder sb = new StringBuilder(512);

                string SplitCode = " ";
                string SplitCandi = "  ";
                string SplitComment = "";

                if (Config.GetString("编码伪装") != "")
                    sb.Append(GetMask(Candi.Code));
         //       sb.Append(GetMose(Candi.Code.Length));
             //   sb.Append(new string('·', Candi.Code.Length));
                else
                    sb.Append(Candi.Code);

                if (Config.GetBool("隐藏候选"))
                {
                    States.acHeight = Config.GetDouble("字体大小") + 16;
                }
                else
                {

                    States.acHeight =  Config.GetDouble("字体大小") * 1.5 + 16;
                }


                if (Candi.TotalCandiCount > 0 && !Config.GetBool("隐藏候选"))
                {
                    for (int i = 0; i < 7 - Candi.Code.Length; i++)
                        sb.Append(SplitCode);

                    int CandiCount = 1;
                    foreach (var c in Candi.Current)
                    {



                        if (CandiCount > 1)
                            sb.Append(SplitCandi);

                        if (Config.GetBool("显示候选序号"))
                        {
                            sb.Append(CandiCount);
                            sb.Append(" ");
                        }

                        sb.Append(Candi.ConvertCandi(c).Replace("\r\n", "\\n").Replace("\n", "\\n"));


                        string appendStr = GetSplitAndComment(c);

                        if (appendStr != null && appendStr.Length > 0)
                        {
                            sb.Append(SplitComment);
                            sb.Append("〔");
                            sb.Append(appendStr);
                            sb.Append("〕");
                        }

                        CandiCount++;
                    }



                }

                Disp.Text = sb.ToString();


                //heng
          //      States.acWidth = (Disp.Text.Length - 4) * Config.GetDouble("字体大小") + 16;
                States.acWidth = Math.Max(Disp.MinWidth, (Disp.Text.Length - 8) * Config.GetDouble("字体大小") - Candi.Current.Count * Config.GetDouble("字体大小") + 10);

                ShowCandidate();





            }

            //修选有变动时，检测下边缘是否超出屏幕
     //       var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;

     //       double bottom = desktopWorkingArea.Bottom;
      //      double right = desktopWorkingArea.Right;
            double bottom;
            double right;

   //         DetectMonitorChange();

            bottom = States.ScreenBottom; right = States.ScreenRight;




            if (States.acY < bottom && States.acY > bottom - States.acHeight - 2)
                this.Top = bottom - States.acHeight - 2;
            else
                this.Top = States.acY;



            if (States.acX < right && States.acX > right - States.acWidth - 2)
                this.Left = right - States.acWidth - 2;
            else
               this.Left = States.acX;


            if (States.IsDebug)
            {
    //            Trace.WriteLine($"states.acX  {States.acX} this.left {this.Left}");
            }

            string GetSplitAndComment(string word)
            {

                string comment = "";
                List<string> splitList = new List<string>();
                List<string> codeList = new List<string>();

                if (MainWindow.State == CompositionState.CnPinyin)
                {

                    StringInfo si = new StringInfo(word);
                    for (int i = 0; i < si.LengthInTextElements; i++)
                    {
                        string single = si.SubstringByTextElements(i, 1);

                        if (MainWindow.Split.ContainsKey(single))
                        {
                            splitList.Add(MainWindow.Split[single]);

                        }
                        else
                        {
                            splitList.Clear();
                            break;
                        }
                    }

                    for (int i = 0; i < si.LengthInTextElements; i++)
                    {
                        string single = si.SubstringByTextElements(i, 1);

                        if (MainWindow.FMB.ContainsKey(single))
                        {

                            codeList.Add(MainWindow.FMB[single]);
                        }
                        else
                        {

                            codeList.Clear();
                            break;
                        }
                    }



                    if (splitList.Count > 0 && codeList.Count > 0)
                        return String.Join("·", splitList) + " | " + String.Join("·", codeList);
                    else if (splitList.Count > 0)
                        return String.Join("·", splitList);
                    else if (codeList.Count > 0)
                        return String.Join("·", codeList);
                    else
                        return "";
                }
                else
                {
                    if (Config.GetBool("显示拆分") && MainWindow.Split.Count > 0)
                    {
                        StringInfo si = new StringInfo(word);
                        for (int i = 0; i < si.LengthInTextElements; i++)
                        {
                            string single = si.SubstringByTextElements(i, 1);

                            if (MainWindow.Split.ContainsKey(single))
                            {
                                splitList.Add(MainWindow.Split[single]);
                            }
                            else
                            {
                                splitList.Clear();
                            }
                        }


                    }

                    if (Config.GetBool("显示注释") && MainWindow.Comment.Count > 0)
                    {
                        MainWindow.Comment.TryGetValue(word, out comment);

                    }



                    if (splitList.Count > 0)
                    {
                        string rt = String.Join("·", splitList);

                        if (comment != null && comment.Length > 0)
                            rt += " " + comment;

                        return rt;

                    }
                    else if (comment != null && comment.Length > 0)
                        return comment;
                    else
                        return "";
                }


            }

        }

 

        public void ShowCandidate()
        {




            if (this.Visibility == Visibility.Collapsed)
            {
                this.Visibility = Visibility.Visible;
            }

            if (this.Opacity == 0)
                this.Opacity = 1;




            if (States.UpdateTopMost)
            {
                this.Hide();
                this.Show();


                States.UpdateTopMost = false;
            }




        }



        public void HideCandidate()
        {
            if (this.Opacity > 0)
                this.Opacity = 0;



        }

        public static WinCandidate Current
        {
            get
            {
                foreach (var s in App.Current.Windows)
                {
                    if (s is WinCandidate)
                    {
                        return (WinCandidate)s;

                    }

                }

                return null;
            }

        }

        public WinCandidate()
        {
            InitializeComponent();


        }


        static Dictionary<IntPtr, int> UiaBlackList = new Dictionary<IntPtr, int>();
        static Dictionary<IntPtr, int> UiaBlackList2 = new Dictionary<IntPtr, int>();

        static Dictionary<IntPtr, int> UiaBlackList22 = new Dictionary<IntPtr, int>();
        public string GetWindowTitle()
        {

            IntPtr hwnd = Win32.GetForegroundWindow();
            if (hwnd != IntPtr.Zero)
            {
                int length = Win32.GetWindowTextLength(hwnd);
                StringBuilder sb = new StringBuilder(length * 2 + 1);
                Win32.GetWindowText(hwnd, sb, sb.Capacity);
                return sb.ToString();
            }
            else
                return "";
        }

        public string GetWindowClassName()
        {

            IntPtr hwnd = Win32.GetForegroundWindow();
            if (hwnd != IntPtr.Zero)
            {
                var g = new StringBuilder(512);
                Win32.GetClassName(hwnd, g, 256);
                return g.ToString();
            }
            else
                return "";
        }





        //     static IntPtr oldHwnd = IntPtr.Zero;
        double ScreenDpiX = 96.0;
        double ScreenDpiY = 96.0;
        double TargetDpi = 96.0;

        private double GetVerticalBias(double top, double bottom)
        {


            double half = (bottom - top) / 2;

            if (half < 1.25 * Config.GetDouble("字体大小"))
                return 0;



            States.maxHeight = Math.Max(States.maxHeight, this.Height);


            double parm1 = States.maxHeight; //不超过整个候选高度

            double parm2 = 2 * Config.GetDouble("字体大小");//不超过两倍字体大小

            double parm3 = half;





      //      double candiHeight = States.maxHeight * ScreenDpiY / 96.0;

            double bias = Math.Min(Math.Min(parm1, parm2), parm3);

            if (States.IsDebug)
            {
                //        double rt = bias;
                //         Trace.WriteLine( $"top{top} botton {bottom} old {States.maxHeight} rt{rt}");
            }


            return bias;



        }

        public void UpdatePosition()
        {
            //           Task.Run(new Action(() => { UpdatePositionTask(); }));
            //      }
            //      public void UpdatePositionTask()
            //       {




            double x = 0;
            double y = 0;

            //  bool follow = true;

            int method = 0;
            //      IntPtr hwnd = GetForegroundWindow();


            IntPtr hwnd = States.Hwnd;
            //hwnd有变化则重读dpi

            if (States.UpdateDPI)
            {
                try
                {
                    uint dpiX, dpiY;
                    IntPtr monitor = Win32.MonitorFromWindow(hwnd, 2);
                    Win32.GetDpiForMonitor(monitor, 0, out dpiX, out dpiY);
                    ScreenDpiX = dpiX;
                    ScreenDpiY = dpiY;
                    TargetDpi = Win32.GetDpiForWindow(hwnd);




                    if (States.IsDebug)
                    {
                        //      Trace.Write($"monitor {monitor} ");
                    }

                    //           oldHwnd = hwnd;
                }
                catch (Exception)
                {

                    ScreenDpiX = 96.0;
                    ScreenDpiY = 96.0;
                    TargetDpi = 96.0;
                }
                States.UpdateDPI = false;

            }

            if (States.IsDebug)
            {
                //            Trace.WriteLine($"ScreenDpix{ScreenDpiX}  screedpiy{ScreenDpiY} Targedpi{TargetDpi}");
            }









            //  Thread.CurrentThread.SetApartmentState(ApartmentState.STA);


            //方法1.1  跟随


            if (Math.Abs(x) < 0.1 && Math.Abs(y) < 0.1)
            {




                x = States.CaretX;
                y = States.CaretY;


                double offsetX;
                double offsetY;
                /*
                if ((int)States.DPIWare == 18)
                {
                    x = x * ScreenDpiX / TargetDpi;
                    y = y * ScreenDpiY / TargetDpi;

                    offsetX = x - States.WinX;
                    offsetY = y - States.WinY;



                    if (States.CaretX == 0 || States.CaretY == 0)
                    {
                        x = 0;
                        y = 0;
                    }
                    else
                    {
                        x = States.WinX / ScreenDpiX * 96.0 + offsetX / ScreenDpiX * 96.0;

                        y = States.WinY / ScreenDpiY * 96.0 + offsetY / ScreenDpiX * 96.0;
                    }
                }
                else

                */
                {
                    offsetX = x - States.WinX;
                    offsetY = y - States.WinY;



                    if (States.CaretX == 0 || States.CaretY == 0)
                    {
                        x = 0;
                        y = 0;
                    }
                    else
                    {
                        x = States.WinX / ScreenDpiX * 96.0 + offsetX / TargetDpi * 96.0;

                        y = States.WinY / ScreenDpiY * 96.0 + offsetY / TargetDpi * 96.0;
                    }
                }








                if (States.IsDebug)
                {
                    //         Trace.WriteLine($"dpiawaer {States.DPIWare} carteX {States.CaretX} caretY {States.CaretY} targetDPI {TargetDpi} sceenDPIX {ScreenDpiX} screen DPIY {ScreenDpiY} win {States.WinX}  {States.WinY} xy {x} {y} offset x{offsetX} offset y {offsetY}");
                }

                //    x = x / ScreenDpiX * 96.0;
                //    y = y / ScreenDpiY * 96.0;

                method = 11;






                // follow = true;

                try
                {




                }
                catch (Exception)
                {
                    x = 0;
                    y = 0;
                }
                finally
                { }

            }



            //方法1  跟随
            if (Math.Abs(x) < 0.1 && Math.Abs(y) < 0.1)
            {
                try
                {
                    GUITHREADINFO? guiInfo = Win32.GetGuiThreadInfo(hwnd);
                    if (guiInfo != null)
                    {
                        IntPtr hwndCaret = guiInfo.Value.hwndCaret;
                        RECT rect = new RECT();

                        Win32.GetWindowRect(hwndCaret, ref rect);

                        x = rect.left + guiInfo.Value.rectCaret.right * ScreenDpiX / TargetDpi;
                        y = rect.top + guiInfo.Value.rectCaret.bottom * ScreenDpiY / TargetDpi;

                        x = x / ScreenDpiX * 96.0;
                        y = y / ScreenDpiY * 96.0;



                        if (States.IsDebug)
                        {
                            //                       Trace.WriteLine($"wleft{rect.left}  wtop{rect.top}  cleft{guiInfo.Value.rectCaret.right * ScreenDpiX / TargetDpi } ctop {rect.top + guiInfo.Value.rectCaret.bottom * ScreenDpiY / TargetDpi}");
                        }

                        method = 1;
                        // follow = true;
                    }
                }
                catch (Exception)
                {
                    x = 0;
                    y = 0;
                }
                finally
                { }

            }


            //方法2 跟随
            if (Math.Abs(x) < 0.1 && Math.Abs(y) < 0.1 && !UiaBlackList.ContainsKey(hwnd) && !States.IsJs) //方法2
            {
                bool thisWorked = false;



                try
                {

                    IUIAutomationElement focusedElement;


                    //   focusedElement = States.Focused;

                    //  if (focusedElement == null)
                    focusedElement = States.root.GetFocusedElement();


                    if (focusedElement != null)
                    {
                        var textPt = focusedElement.GetCurrentPattern(UIA_PatternIds.UIA_TextPatternId) as IUIAutomationTextPattern;
                        if (textPt != null)
                        {
                            var sel = textPt.GetSelection();
                            if (sel != null && sel.Length > 0)
                            {
                                var t = sel.GetElement(0);
                                var k = t.GetBoundingRectangles();
                                if (k != null && k.Length > 0)
                                {
                                    x = (k[0] + k[2]);// * 96 / ScreenDpiX;//* ScreenDpiX / 96; //TargetDpi; ; 物理坐标
                                    y = (k[1] + k[3]);// * 96 / ScreenDpiY; //* ScreenDpiY / 96;// TargetDpi; ;

                                    x = x / ScreenDpiX * 96.0; //逻辑坐标
                                    y = y / ScreenDpiY * 96.0;


                                    method = 2;
                                    //follow = true;
                                    thisWorked = true;
                                }
                            }
                        }
                    }



                    //   var focusedElement = States.root.GetFocusedElement();


                    //          AutomationElement focusedElement = AutomationElement.FocusedElement;




                }
                catch (Exception)
                {
                    x = 0;
                    y = 0;
                    UiaBlackList[hwnd] = 1;
                }
                finally
                {

                }



                if (!thisWorked)
                {
                    UiaBlackList[hwnd] = 1;
                }
            }


            //方法2.2 跟随
            if (Math.Abs(x) < 0.1 && Math.Abs(y) < 0.1 && !UiaBlackList22.ContainsKey(hwnd) && !States.IsJs) //方法2
            {
                bool thisWorked = false;



                try
                {

                    IUIAutomationElement focusedElement;

                    //      focusedElement = States.Focused;
                    //     if (focusedElement == null)
                    focusedElement = States.root.GetFocusedElement();
                    if (focusedElement != null)
                    {
                        var textPt2 = focusedElement.GetCurrentPattern(UIA_PatternIds.UIA_TextPattern2Id) as IUIAutomationTextPattern2;


                        if (textPt2 != null)
                        {
                            int active;
                            var sel = textPt2.GetCaretRange(out active);
                            if (sel != null)
                            {

                                var k = sel.GetBoundingRectangles();
                                if (k != null && k.Length > 0)
                                {
                                    x = (k[0] + k[2]);// * 96 / ScreenDpiX;//* ScreenDpiX / 96; //TargetDpi; ;
                                    y = (k[1] + k[3]);// * 96 / ScreenDpiY; //* ScreenDpiY / 96;// TargetDpi; ;

                                    x = x / ScreenDpiX * 96.0; //逻辑坐标
                                    y = y / ScreenDpiY * 96.0;

                                    method = 22;
                                    //follow = true;
                                    thisWorked = true;
                                }
                            }
                        }
                    }





                    //          AutomationElement focusedElement = AutomationElement.FocusedElement;



                }
                catch (Exception)
                {
                    x = 0;
                    y = 0;
                    UiaBlackList22[hwnd] = 1;
                }
                finally
                {

                }



                if (!thisWorked)
                {
                    UiaBlackList22[hwnd] = 1;
                }
            }

            //方法2.3 居中
            if (Math.Abs(x) < 0.1 && Math.Abs(y) < 0.1 && !UiaBlackList2.ContainsKey(hwnd) && !States.IsJs) //方法2.3
            {
                bool thisWorked = false;



                try
                {

                    IUIAutomationElement focusedElement;


                    //      focusedElement = States.Focused;
                    //      if (focusedElement == null)
                    focusedElement = States.root.GetFocusedElement();
                    if (focusedElement != null)
                    {

                        var r = focusedElement.CurrentBoundingRectangle;



                        x = (r.left + r.right) / 2;
                        if (r.right - r.left > 20)
                            x -= 5;

                        y = r.bottom;

                        x = x / ScreenDpiX * 96.0; //逻辑坐标
                        y = y / ScreenDpiY * 96.0;


                        double yBias = GetVerticalBias(r.top / ScreenDpiY * 96.0, r.bottom / ScreenDpiY * 96.0);




                        y -= yBias;
                        method = -23;

                        thisWorked = true;

                    }









                }
                catch (Exception)
                {
                    x = 0;
                    y = 0;
                    UiaBlackList2[hwnd] = 1;
                }
                finally
                {

                }



                if (!thisWorked)
                {
                    UiaBlackList2[hwnd] = 1;
                }
            }




            if (Math.Abs(x) < 0.1 && Math.Abs(y) < 0.1) //方法3
            {
                try
                {
                    RECT r = new RECT();
                    Win32.GetWindowRect(hwnd, ref r);
                    x = (r.right + r.left) / 2;
                    if (r.right - r.left > 20)
                        x -= 5;






                    y = r.bottom;


                    x = x / ScreenDpiX * 96.0; //逻辑坐标
                    y = y / ScreenDpiY * 96.0;


                    double yBias = GetVerticalBias(r.top / ScreenDpiY * 96.0, r.bottom / ScreenDpiY * 96.0);


                    y -= yBias;
                    method = -3;
                    //follow = false;
                }
                catch (Exception)
                {
                    x = 0;
                    y = 0;
                }
                finally
                { }


            }



            //移动
            if (Math.Abs(x) > 1 && Math.Abs(y) > 1)
            {
                if (method > 0) //follow)
                {
                    x += 3;
                    y += 2;
                }
                else
                {
                    y += 2;
                }
                //       var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;

                //        double bottom = desktopWorkingArea.Bottom;
                //        double right = desktopWorkingArea.Right;
                double bottom;
                double right;



                bottom = States.ScreenBottom; right = States.ScreenRight;

                States.acY = y;

                if (y < bottom && y > bottom - States.acHeight - 2) //顶部在这个屏幕内，但下边缘超出了该屏幕
                {
                    y = bottom - States.acHeight - 2;

                }


                States.acX = x; //计算出来的X


                if (x < right && x > right - States.acWidth - 2)//左边在这个屏幕内，但右边缘超出了该屏幕
                {
                    x = right - States.acWidth - 2;

                }


                if (States.IsDebug)
                {
                    //              Trace.WriteLine($" desktop {desktopWorkingArea.Bottom} this height{this.ActualHeight} originy {ymin}");
                }


                this.Left = x;
                this.Top = y;


                if (States.IsDebug)
                {
        //            Trace.WriteLine($"states.acX  {States.acX} this.left {this.Left}");
                }

            }







            ShowCandidate();




        }



        private string GetMask(string code)
        {
            StringInfo lut = new StringInfo(Config.GetString("编码伪装"));

            string charLut = "abcdefghijklmnopqrstuvwxyz;";


            int moseLen = code.Length;

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i< code.Length; i ++)
            {
                string c = code.Substring(i, 1);

                int pos = charLut.IndexOf(c);
                if (pos == -1)
                    pos = 0;

                pos = pos % lut.LengthInTextElements;



                string wc = lut.SubstringByTextElements(pos,1);
                sb.Append(wc);
            }

           return sb.ToString();
        }


        public void Input_MouseUp(object sender, MouseButtonEventArgs e)
        {

            if (e.ChangedButton == MouseButton.Middle)
                MainWindow.Current?.ToggleCandidates();


        }

        private void Disp_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double ftsize = Config.GetDouble("字体大小");
            ftsize += e.Delta / 120 * 0.5;
            if (ftsize < 3)
                ftsize = 3;
            if (ftsize > 200)
                ftsize = 200;

            Config.Set("字体大小", ftsize, 1);
            UpdateFonts();


        }

        private FontFamily GetFontFamily()
        {
            string fontName = Config.GetString("字体");

            if (fontName == null || fontName.Length == 0)
                return null;

            if (fontName.Substring(0, 1) == "#")
            {

                string currentPath = System.AppDomain.CurrentDomain.BaseDirectory;
                Uri uri = new Uri(currentPath + "字体\\");
                FontFamily fm = new FontFamily(uri, "./" + fontName);


                return fm;
            }
            else
            {
                return new FontFamily(fontName);
            }
        }

        public void UpdateFonts()
        {
            Disp.FontFamily = GetFontFamily();
            Disp.FontSize = Config.GetDouble("字体大小");
            if (Config.GetBool("隐藏候选"))
            {
                // Disp.Padding = new Thickness(6, 5, 6, 5);
                int pd = (int)(Math.Round(Config.GetDouble("字体大小") * 1 * 0.4));
                Disp.Padding = new Thickness(pd, 2, pd, 2);
                Disp.LineHeight = Config.GetDouble("字体大小") * 1;
                Disp.MinWidth = 0;// Config.GetDouble("字体大小") * 4 * 0.72 + 5;

                //        winCandidate.Height = 20;
            }
            else if (Config.GetBool("竖排候选"))
            {
                Disp.MinWidth = Config.GetDouble("字体大小") * 8 * 0.72 + 15;
                Disp.LineHeight = Config.GetDouble("字体大小") * 1.5;
                Disp.Padding = new Thickness(12, 8, 8, 7);
            }
            else
            {
                Disp.Padding = new Thickness(8, 8, 8, 8);
                Disp.MinWidth = Config.GetDouble("字体大小") * 4 * 0.72 + 15;
                Disp.LineHeight = Config.GetDouble("字体大小") * 1;
            }


        }
        
        public void UpdateColor()
        {


                  BorderCandi.Background = Themes.CurrentTheme().Background;
                   BorderCandi.BorderBrush = Themes.CurrentTheme().Border;
                 Disp.Foreground = Themes.CurrentTheme().Foreground;
            BorderCandi.BorderThickness = Themes.CurrentTheme().BorderWidth;

            BorderCandi.CornerRadius = Themes.CurrentTheme().Corner;
        }

        private void MenuConfig_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Current?.ShowConfigWin();

        }

        private void MenuReload_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Current?.ReloadMB();

        }


        private void MenuAddCi_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Current?.ShowWinAddCi();

        }

        private void MenuFolder_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Current?.OpenMBFolder();

        }

        private void MenuOfficial_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Current?.OpenOfficial();

        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            States.CandiHwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;


        }

    }
}
