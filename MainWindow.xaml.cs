using Accessibility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shell;
using System.Windows.Threading;



namespace bime
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    /*
    public static class DispatcherHelper
    {
        [SecurityPermissionAttribute(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(ExitFrames), frame);
            try { Dispatcher.PushFrame(frame); }
            catch (InvalidOperationException) { }
        }
        private static object ExitFrames(object frame)
        {
            ((DispatcherFrame)frame).Continue = false;
            return null;
        }
    }
    */

    public enum CompositionState
    {
        En, //英文
        CnIdle,//中文无编码
        CnComposing, //中文有编码
        CnUpperCase,//中文大字起头
        CnPinyin//拼音反查
    }





    public partial class MainWindow : Window
    {

        #region
        const int VK_LBUTTON = 0x01; //鼠标左键
        const int VK_RBUTTON = 0x02; //鼠标右键
        const int VK_CANCEL = 0x03; //控制中断处理
        const int VK_MBUTTON = 0x04; //中间鼠标按钮 (三键鼠标)
        const int VK_XBUTTON1 = 0x05; //X1 鼠标按钮
        const int VK_XBUTTON2 = 0x06; //X2 鼠标按钮

        const int VK_BACK = 0x08; //BACKSPACE 密钥
        const int VK_TAB = 0x09; //Tab 键

        const int VK_CLEAR = 0x0C; //CLEAR 键
        const int VK_RETURN = 0x0D; //Enter 键

        const int VK_SHIFT = 0x10; //SHIFT 键
        const int VK_CONTROL = 0x11; //Ctrl 键
        const int VK_MENU = 0x12; //Alt 键
        const int VK_PAUSE = 0x13; //PAUSE 键
        const int VK_CAPITAL = 0x14; //CAPS LOCK 键
        const int VK_KANA = 0x15; //IME Kana 模式
        const int VK_HANGUEL = 0x15; //IME 朝鲜文库埃尔模式 (保持兼容性;使用 VK_HANGUL)
        const int VK_HANGUL = 0x15; //IME Hanguel 模式
        const int VK_IME_ON = 0x16; //IME On
        const int VK_JUNJA = 0x17; //IME Junja 模式
        const int VK_FINAL = 0x18; //IME 最终模式
        const int VK_HANJA = 0x19; //IME Hanja 模式
        const int VK_KANJI = 0x19; //IME Kanji 模式
        const int VK_IME_OFF = 0x1A; //IME 关闭
        const int VK_ESCAPE = 0x1B; //ESC 键
        const int VK_CONVERT = 0x1C; //IME 转换
        const int VK_NONCONVERT = 0x1D; //IME 不转换
        const int VK_ACCEPT = 0x1E; //IME 接受
        const int VK_MODECHANGE = 0x1F; //IME 模式更改请求
        const int VK_SPACE = 0x20; //空格键
        const int VK_PRIOR = 0x21; //PAGE UP 键
        const int VK_NEXT = 0x22; //PAGE DOWN 键
        const int VK_END = 0x23; //END 键
        const int VK_HOME = 0x24; //HOME 键
        const int VK_LEFT = 0x25; //向左键
        const int VK_UP = 0x26; //向上键
        const int VK_RIGHT = 0x27; //向右键
        const int VK_DOWN = 0x28; //向下键
        const int VK_SELECT = 0x29; //SELECT 键
        const int VK_PRINT = 0x2A; //PRINT 键
        const int VK_EXECUTE = 0x2B; //EXECUTE 键
        const int VK_SNAPSHOT = 0x2C; //打印屏幕键
        const int VK_INSERT = 0x2D; //INS 密钥
        const int VK_DELETE = 0x2E; //DEL 键
        const int VK_HELP = 0x2F; //帮助密钥

        const int VK_LWIN = 0x5B; //左Windows键 (自然键盘)
        const int VK_RWIN = 0x5C; //右Windows键 (自然键盘)
        const int VK_APPS = 0x5D; //应用程序键 (自然键盘)

        const int VK_SLEEP = 0x5F; //计算机休眠键
        const int VK_NUMPAD0 = 0x60; //数字键盘 0 键
        const int VK_NUMPAD1 = 0x61; //数字键盘 1 键
        const int VK_NUMPAD2 = 0x62; //数字键盘 2 键
        const int VK_NUMPAD3 = 0x63; //数字键盘 3 键
        const int VK_NUMPAD4 = 0x64; //数字键盘 4 键
        const int VK_NUMPAD5 = 0x65; //数字键盘 5 键
        const int VK_NUMPAD6 = 0x66; //数字键盘 6 键
        const int VK_NUMPAD7 = 0x67; //数字键盘 7 键
        const int VK_NUMPAD8 = 0x68; //数字键盘 8 键
        const int VK_NUMPAD9 = 0x69; //数字键盘 9 键
        const int VK_MULTIPLY = 0x6A; //乘键
        const int VK_ADD = 0x6B; //添加密钥
        const int VK_SEPARATOR = 0x6C; //分隔符键
        const int VK_SUBTRACT = 0x6D; //减去键
        const int VK_DECIMAL = 0x6E; //十进制键
        const int VK_DIVIDE = 0x6F; //除键
        const int VK_F1 = 0x70; //F1 键
        const int VK_F2 = 0x71; //F2 键
        const int VK_F3 = 0x72; //F3 键
        const int VK_F4 = 0x73; //F4 键
        const int VK_F5 = 0x74; //F5 键
        const int VK_F6 = 0x75; //F6 键
        const int VK_F7 = 0x76; //F7 键
        const int VK_F8 = 0x77; //F8 键
        const int VK_F9 = 0x78; //F9 键
        const int VK_F10 = 0x79; //F10 键
        const int VK_F11 = 0x7A; //F11 键
        const int VK_F12 = 0x7B; //F12 键
        const int VK_F13 = 0x7C; //F13 键
        const int VK_F14 = 0x7D; //F14 键
        const int VK_F15 = 0x7E; //F15 键
        const int VK_F16 = 0x7F; //F16 键
        const int VK_F17 = 0x80; //F17 键
        const int VK_F18 = 0x81; //F18 键
        const int VK_F19 = 0x82; //F19 键
        const int VK_F20 = 0x83; //F20 键
        const int VK_F21 = 0x84; //F21 键
        const int VK_F22 = 0x85; //F22 键
        const int VK_F23 = 0x86; //F23 键
        const int VK_F24 = 0x87; //F24 键

        const int VK_NUMLOCK = 0x90; //NUM LOCK 密钥
        const int VK_SCROLL = 0x91; //SCROLL LOCK 键

        const int VK_LSHIFT = 0xA0; //左 SHIFT 键
        const int VK_RSHIFT = 0xA1; //右 SHIFT 键
        const int VK_LCONTROL = 0xA2; //左 Ctrl 键
        const int VK_RCONTROL = 0xA3; //右 Ctrl 键
        const int VK_LMENU = 0xA4; //左 Alt 键
        const int VK_RMENU = 0xA5; //右 ALT 键
        const int VK_BROWSER_BACK = 0xA6; //浏览器后退键
        const int VK_BROWSER_FORWARD = 0xA7; //浏览器前进键
        const int VK_BROWSER_REFRESH = 0xA8; //浏览器刷新键
        const int VK_BROWSER_STOP = 0xA9; //浏览器停止键
        const int VK_BROWSER_SEARCH = 0xAA; //浏览器搜索键
        const int VK_BROWSER_FAVORITES = 0xAB; //浏览器收藏键
        const int VK_BROWSER_HOME = 0xAC; //浏览器“开始”和“主页”键
        const int VK_VOLUME_MUTE = 0xAD; //静音键
        const int VK_VOLUME_DOWN = 0xAE; //音量减小键
        const int VK_VOLUME_UP = 0xAF; //音量增加键
        const int VK_MEDIA_NEXT_TRACK = 0xB0; //下一曲目键
        const int VK_MEDIA_PREV_TRACK = 0xB1; //上一曲目键
        const int VK_MEDIA_STOP = 0xB2; //停止媒体键
        const int VK_MEDIA_PLAY_PAUSE = 0xB3; //播放/暂停媒体键
        const int VK_LAUNCH_MAIL = 0xB4; //启动邮件键
        const int VK_LAUNCH_MEDIA_SELECT = 0xB5; //选择媒体键
        const int VK_LAUNCH_APP1 = 0xB6; //启动应用程序 1 键
        const int VK_LAUNCH_APP2 = 0xB7; //启动应用程序 2 键

        const int VK_OEM_1 = 0xBA; //用于其他字符;它可能因键盘而异。 对于美国标准键盘，“;：”键
        const int VK_OEM_PLUS = 0xBB; //对于任何国家/地区，“+”键
        const int VK_OEM_COMMA = 0xBC; //对于任何国家/地区，“，键
        const int VK_OEM_MINUS = 0xBD; //对于任何国家/地区，“-”键
        const int VK_OEM_PERIOD = 0xBE; //对于任何国家/地区，“.”键
        const int VK_OEM_2 = 0xBF; //用于其他字符;它可能因键盘而异。 对于美国标准键盘，“/？” key
        const int VK_OEM_3 = 0xC0; //用于其他字符;它可能因键盘而异。 对于美国标准键盘，“~”键


        const int VK_OEM_4 = 0xDB; //用于其他字符;它可能因键盘而异。 对于美国标准键盘，“[{”键
        const int VK_OEM_5 = 0xDC; //用于其他字符;它可能因键盘而异。 对于美国标准键盘，“\|”键
        const int VK_OEM_6 = 0xDD; //用于其他字符;它可能因键盘而异。 对于美国标准键盘，“]}”键
        const int VK_OEM_7 = 0xDE; //用于其他字符;它可能因键盘而异。 对于美国标准键盘，“单引号/双引号”键
        const int VK_OEM_8 = 0xDF; //用于其他字符;它可能因键盘而异。


        const int VK_OEM_102 = 0xE2; //<>美国标准键盘上的键，或\\|非美国 102 键键盘上的键

        const int VK_PROCESSKEY = 0xE5; //IME PROCESS 密钥

        const int VK_PACKET = 0xE7; //用于将 Unicode 字符当作键击传递。 该 VK_PACKET 键是用于非键盘输入方法的 32 位虚拟键值的低字。 有关详细信息，请参阅“备注”，以及KEYBDINPUTSendInputWM_KEYDOWNWM_KEYUP


        const int VK_ATTN = 0xF6; //Attn 键
        const int VK_CRSEL = 0xF7; //CrSel 键
        const int VK_EXSEL = 0xF8; //ExSel 密钥
        const int VK_EREOF = 0xF9; //擦除 EOF 密钥
        const int VK_PLAY = 0xFA; //播放键
        const int VK_ZOOM = 0xFB; //缩放键
        const int VK_NONAME = 0xFC; //预留
        const int VK_PA1 = 0xFD; //PA1 键
        const int VK_OEM_CLEAR = 0xFE; //清除键

        #endregion







        #region win32
        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);
        static int hKeyboardHook = 0; //声明键盘钩子处理的初始值
        public const int WH_KEYBOARD_LL = 13;   //线程键盘钩子监听鼠标消息设为2，全局键盘监听鼠标消息设为13
        public const int WH_KEYBOARD = 20;   //线程键盘钩子监听鼠标消息设为2，全局键盘监听鼠标消息设为13
        HookProc KeyboardHookProcedure; //声明KeyboardHookProcedure作为HookProc类型
        static bool Skip = false;



        //键盘结构




        private const int WM_KEYDOWN = 0x100;//KEYDOWN
        private const int WM_KEYUP = 0x101;//KEYUP
        private const int WM_SYSKEYDOWN = 0x104;//SYSKEYDOWN
        private const int WM_SYSKEYUP = 0x105;//SYSKEYUP





        private string ic = "";
        public static CompositionState State = CompositionState.CnIdle;
        private string InputCode
        {
            get
            {
                return ic;
            }
            set
            {
                ic = value;



                switch (State)
                {
                    case CompositionState.CnComposing:
                        if (MB.ContainsKey(InputCode))
                            Candi.Update(InputCode, MB[InputCode]);
                        else
                            Candi.Update(InputCode, null);
                        break;
                    case CompositionState.CnPinyin:
                        if (InputCode.Length > 1 && PYMB.ContainsKey(InputCode.Substring(1)))
                            Candi.Update(InputCode, PYMB[InputCode.Substring(1)]);
                        else
                            Candi.Update(InputCode, null);
                        break;
                    case CompositionState.CnIdle:
                        if (InputCode == "·")
                        {
                            List<string> tmp = new List<string>();
                            tmp.Add("·");
                            Candi.Update(InputCode, tmp);
                        }
                        else
                        {
                            if (MB.ContainsKey(InputCode))
                                Candi.Update(InputCode, MB[InputCode]);
                            else
                                Candi.Update(InputCode, null);
                        }
                        break;
                    case CompositionState.CnUpperCase:
                        Candi.Update(InputCode, null);
                        break;
                    default:

                        break;

                }



            }
        }
        private bool SHORT_SYMBOL_SEMICOLON = false; //快捷符号分号
        private bool SHORT_SYMBOL_SLASH = false; //快捷符号分号
        private bool SHORT_SYMBOL_Z = false; //Z键引导
        private bool SHORT_SYMBOL = false;

        HashSet<string> AutoShortSymbol = new HashSet<string>();
        private void Toggle()
        {
            if (Config.GetBool("自动切换系统语言"))
                ImiHelper.ChangeLanguageDisabled();

            switch (State)
            {
                case CompositionState.En:
                    StateTrans(CompositionState.CnIdle);
                    break;
                case CompositionState.CnIdle:
                    StateTrans(CompositionState.En);
                    break;
                case CompositionState.CnComposing:
                case CompositionState.CnUpperCase:
                case CompositionState.CnPinyin:
                    CommitCode();
                    StateTrans(CompositionState.En);
                    break;
                default:
                    StateTrans(CompositionState.En);
                    break;
            }




        }






        static int buffer_len = 0;

        /*
        public static void Delay(int milliSecond)
        {
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) < milliSecond)//毫秒
            {
                DispatcherHelper.DoEvents();
            }
        }
        */
        private void UpdateBuffer(string bf)
        {

            if (UseClipboard)
            {
                UpdateBuffer_Old(bf);
                return;
            }






            if (bf == "" && buffer_len > 0)
            {
                SimulateInputKey(VK_BACK);
                buffer_len = 0;
                return;
            }


            Skip = true;

            var numlock = Win32.GetKeyState(VK_NUMLOCK) > 0;

            buffer_len = new StringInfo(bf).LengthInTextElements;

            int inputLen = bf.Length * 2 + 2 + buffer_len * 2 + (numlock ? 4 : 0);

            INPUT[] input = new INPUT[inputLen];


            int counter = 0;
            for (int i = 0; i < bf.Length * 2; i += 2)
            {
                input[counter].type = 1;
                input[counter].mkhi.ki.wVk = 0;//dwFlags 为KEYEVENTF_UNICODE 即4时，wVk必须为0
                input[counter].mkhi.ki.wScan = bf[counter / 2];
                input[counter].mkhi.ki.dwFlags = 4;//输入UNICODE字符
                input[counter].mkhi.ki.time = 0;
                input[counter].mkhi.ki.dwExtraInfo = IntPtr.Zero;
                counter++;
                input[counter].type = 1;
                input[counter].mkhi.ki.wVk = 0;
                input[counter].mkhi.ki.wScan = bf[counter / 2];
                input[counter].mkhi.ki.dwFlags = 6;
                input[counter].mkhi.ki.time = 0;
                input[counter].mkhi.ki.dwExtraInfo = IntPtr.Zero;
                counter++;
            }

            if (numlock)
            {

                input[counter].type = 1;//模拟键盘
                input[counter].mkhi.ki.wVk = VK_NUMLOCK;
                input[counter].mkhi.ki.dwFlags = 0;//按下
                counter++;
                input[counter].type = 1;//模拟键盘
                input[counter].mkhi.ki.wVk = VK_NUMLOCK;
                input[counter].mkhi.ki.dwFlags = 2;//抬起
                counter++;

            }


            //shift down
            {
                input[counter].type = 1;//模拟键盘
                input[counter].mkhi.ki.wVk = VK_LSHIFT;
                input[counter].mkhi.ki.dwFlags = 0;//抬起
                counter++;
            }


            //left
            for (int i = 0; i < buffer_len * 2; i += 2)
            {
                input[counter].type = 1;//模拟键盘
                input[counter].mkhi.ki.wVk = VK_LEFT;
                input[counter].mkhi.ki.dwFlags = 0;//按下
                counter++;
                input[counter].type = 1;//模拟键盘
                input[counter].mkhi.ki.wVk = VK_LEFT;
                input[counter].mkhi.ki.dwFlags = 2;//抬起
                counter++;
            }

            //shift up
            {
                input[counter].type = 1;//模拟键盘
                input[counter].mkhi.ki.wVk = VK_LSHIFT;
                input[counter].mkhi.ki.dwFlags = 2;//抬起
                counter++;
            }



            if (numlock)
            {
                input[counter].type = 1;//模拟键盘
                input[counter].mkhi.ki.wVk = VK_NUMLOCK;
                input[counter].mkhi.ki.dwFlags = 0;//按下
                counter++;
                input[counter].type = 1;//模拟键盘
                input[counter].mkhi.ki.wVk = VK_NUMLOCK;
                input[counter].mkhi.ki.dwFlags = 2;//抬起
                counter++;
            }


            Win32.SendInput((uint)inputLen, input, Marshal.SizeOf((object)default(INPUT)));

            Skip = false;

        }

        private void UpdateBuffer_Old(string bf)
        {

            //      bool ShiftDown = Win32.GetKeyState(VK_LSHIFT) < 0;

            Skip = true;
            /*
            for (int i = 0; i < buffer_len; i++)
            {
                SimulateInputKey(VK_DELETE);
            }*/

            if (bf == "" && buffer_len > 0)
                SimulateInputKey(VK_BACK);
            else
                Send(bf, true);



            var numlock = Win32.GetKeyState(VK_NUMLOCK) > 0;

            if (numlock)
                SimulateInputKey(VK_NUMLOCK);

            buffer_len = new StringInfo(bf).LengthInTextElements;
            SimulateInputKeyDown(VK_LSHIFT);

            //     SimKeyDown(0xe02a);

            //   SimKeyDown(0xe036);

            for (int i = 0; i < buffer_len; i++)
            {
                //         bool sd = GetKeyState(VK_LSHIFT) < 0;



                //   SimKeyDown(0xe046);
                //  SimKeyUp(0xe046);

                //    System.Windows.Forms.SendKeys.SendWait("+{left}");
                //        System.Windows.Forms.SendKeys.SendWait("+{left}");
                SimulateInputKey(VK_LEFT);
                //      SimKeyDown(0xe04b);
                //      SimKeyUp(0xe04b);
            }
            //   SimKeyUp(0xe036);
            SimulateInputKeyUp(VK_LSHIFT);


            if (numlock)
                SimulateInputKey(VK_NUMLOCK);
            Skip = false;

        }


        public void UpdateCandidateEmb()
        {

            string DispTxt = "";





            if (Candi.Current.Count > 0)
            {
                //  DispTxt = Candi.Current.First();
                DispTxt = Candi.ConvertEmb( Candi.ConvertCandi(Candi.Current.First()));
            }
            else
            {
                DispTxt =Candi.ConvertEmb( InputCode);
            }



            UpdateBuffer(DispTxt);



            if (winCandidate.Visibility == Visibility.Visible)
            {
                winCandidate.Visibility = Visibility.Collapsed;
            }

        }



        private void SyncJsHit(int commitLen)
        {
            States.HitDiff -= commitLen;

            if (States.HitDiff > 0)
            {
                for (int i = 0; i < States.HitDiff; i++)
                {
                    //         if (States.IsJs)
                    //          {
                    //               SimulateInputKey(VK_F22);
                    //           }
                    //       SimulateInputKey(VK_END);

                    if (States.IsMuyi)
                        SimulateInputKey(VK_F22);

                }
                States.HitDiff = 0;
            }

        }

        private string SyncJsLength(int commitLen)
        {

            string rt = "";
            States.HitDiff -= commitLen;

            if (States.HitDiff > 0)
            {
                for (int i = 0; i < States.HitDiff; i++)
                {
                    if (States.IsJs || States.IsMuyi)
                    {
                        rt += "\0";
                    }
                    //       SimulateInputKey(VK_END);



                }
                States.HitDiff = 0;
            }


            return rt;
        }

        private void AutoCommitCandi(string ch)
        {

            if (!MB.ContainsKey(InputCode + ch))
            {
                InputCode = "";

                SendBack();
                return;
            }



            string toSend = MB[InputCode + ch][0];
            InputCode = "";

            if (toSend.Length > 0)
                Send(toSend);
            else
                SendBack();


        }
        private void CommitCandi(int pos)
        {

            string toSend = "";
            if (Candi.Current.Count - 1 >= pos)
            {
                toSend = Candi.Current[pos];
            }

            InputCode = "";


            if (toSend.Length > 0)
                Send(toSend);
            else
                SendBack();


        }
        private void CommitCandi_old(int pos)
        {



            if (InputCode != "")
            {
                string tmpb = InputCode;
                InputCode = "";


                if (MB.ContainsKey(tmpb) && MB[tmpb].Count >= pos + 1)
                {
                    Send(MB[tmpb][pos]);
                }
                else
                {
                    SendBack();
                }

            }


        }

        private void CommitDing(string s)
        {

            string tmpb = InputCode;

            if (Config.GetBool("空码自动清屏"))
            {

                string toSend = "";

                if (Candi.Current.Count > 0)
                    toSend = Candi.Current.First();




                Send(toSend);
                InputCode = s;




            }
            else
            {
                string toSend = "";

                if (Candi.Current.Count > 0)
                {
                    toSend = Candi.Current.First();
                    InputCode = s;


                    Send(toSend);

                }
                else
                {
                    BufferAdd(s);
                }






            }
       //     StateTrans(CompositionState.CnComposing);
        }

        private void CommitCode()
        {
            if (InputCode != "")
            {



                string toSend = InputCode;
                InputCode = "";

                Send(toSend);




            }

        }

        //  Stack<string> SendHistory = new Stack<string>();


        public bool UseClipboard
        {
            get
            {
                return Config.GetBool("使用剪贴板上屏") || States.UseClipboard;
            }
        }

        private void Send(string msg, bool skipRepeatBuffer = false)
        {



            if (string.IsNullOrEmpty(msg))
                return;

            switch (msg)
            {
                case "{嵌入模式}":
                    Config.Set("嵌入式候选（娱乐）", !Config.GetBool("嵌入式候选（娱乐）"));
                    winCandidate?.Update();
                        return;
                case "{录制}":
                    SendHistory.StartStop();
                    UpdateStatesBar();
                    return;
                case "{添加}":
                case "{加词}":
                    ShowWinAddCi();
                    return;
                case "{隐藏候选}":
                    ToggleCandidates();
                    return;
                default:
                    break;
            }

            msg = Candi.ConvertCandi(msg);




            //         if (States.IsMuyi)
            //               msg += SyncJsLength(msg.Length);





            if (SendHistory.IsRecording)
            { }
            else if (msg == "[[")
            {
                SimKeyDown(0x1a);
                SimKeyUp(0x1a);
                SimKeyDown(0x1a);
                SimKeyUp(0x1a);
            }
            else if (UseClipboard || msg.Contains("\n"))
                Send2(msg);
            else if (States.IsJs)
                Send4(msg);
            else
                Send3(msg);


            SyncJsHit(msg.Length);

            /*
            string wrap_msg;
            switch (msg)
            {
                case "{+}":
                    wrap_msg = "+";
                    break;
                case "{%}":
                    wrap_msg = "%";
                    break;
                case "{^}":
                    wrap_msg = "^";
                    break;
                case "{~}":
                    wrap_msg = "~";
                    break;
                default:
                    wrap_msg = msg;
                    break;
            }


            
            StringInfo si = new StringInfo(wrap_msg);
            */
            StringInfo si = new StringInfo(msg);
            for (int i = 0; i < si.LengthInTextElements; i++)
                SendHistory.Push(si.SubstringByTextElements(i, 1));


            //          if (SendHistory.Count > 10000)
            //            SendHistory.Clear();







            if (!skipRepeatBuffer)
                States.RepeatBuffer = msg;

            if (si.LengthInTextElements > 0)
                States.LastNum = IsDigit(si.SubstringByTextElements(si.LengthInTextElements - 1, 1)); //上一次上屏文本末尾是数字







        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool IsDigit(string s)
        {
            char c = s[0];
            return c >= '0' && c <= '9';
        }



        #endregion 
        private void Send3(string text)
        {
            //        Stop();
            //       SimulateInputString('t');
            //      Start();

            Skip = true;
            INPUT[] input = new INPUT[text.Length * 2];

            for (int i = 0; i < text.Length * 2; i += 2)
            {
                input[i].type = 1;
                input[i].mkhi.ki.wVk = 0;//dwFlags 为KEYEVENTF_UNICODE 即4时，wVk必须为0
                input[i].mkhi.ki.wScan = text[i / 2];
                input[i].mkhi.ki.dwFlags = 4;//输入UNICODE字符
                input[i].mkhi.ki.time = 0;
                input[i].mkhi.ki.dwExtraInfo = IntPtr.Zero;
                input[i + 1].type = 1;
                input[i + 1].mkhi.ki.wVk = 0;
                input[i + 1].mkhi.ki.wScan = text[i / 2];
                input[i + 1].mkhi.ki.dwFlags = 6;
                input[i + 1].mkhi.ki.time = 0;
                input[i + 1].mkhi.ki.dwExtraInfo = IntPtr.Zero;
            }
            Win32.SendInput((uint)text.Length * 2, input, Marshal.SizeOf((object)default(INPUT)));
            Skip = false;
        } //使用sendinput实现

        private void Send4(string text)
        {
            IntPtr hwnd = Win32.GetForegroundWindow();
            if (String.IsNullOrEmpty(text))
                return;
            GUITHREADINFO? guiInfo = Win32.GetGuiThreadInfo(hwnd);
            if (guiInfo != null)
            {
                for (int i = 0; i < text.Length; i++)
                {
                    Win32.SendMessage(guiInfo.Value.hwndFocus, 0x0102, (IntPtr)(int)text[i], IntPtr.Zero);

                }
            }

        }



        private void Send31(string text)
        {
            Skip = true;

            //        System.Windows.Forms.SendKeys.SendWait(text);

            Skip = false;
            return;
        }
        public void SimulateInputString(char c)
        {
            INPUT[] input = new INPUT[2];

            input[0].type = 1;
            input[0].mkhi.ki.wVk = 0;//dwFlags 为KEYEVENTF_UNICODE 即4时，wVk必须为0
            input[0].mkhi.ki.wScan = c;
            input[0].mkhi.ki.dwFlags = 4;//输入UNICODE字符
            input[0].mkhi.ki.time = 0;
            input[0].mkhi.ki.dwExtraInfo = IntPtr.Zero;
            input[1].type = 1;
            input[1].mkhi.ki.wVk = 0;
            input[1].mkhi.ki.wScan = c;
            input[1].mkhi.ki.dwFlags = 6;
            input[1].mkhi.ki.time = 0;
            input[1].mkhi.ki.dwExtraInfo = IntPtr.Zero;
            Win32.SendInput(2u, input, Marshal.SizeOf((object)default(INPUT)));

        }

        public void SimulateInputKey(int key)
        {
            Skip = true;
            INPUT[] input = new INPUT[1];

            input[0].type = 1;//模拟键盘
            input[0].mkhi.ki.wVk = (ushort)key;
            input[0].mkhi.ki.dwFlags = 0;//按下
            Win32.SendInput(1u, input, Marshal.SizeOf((object)default(INPUT)));
            //          Sleep(50);

            input[0].type = 1;//模拟键盘
            input[0].mkhi.ki.wVk = (ushort)key;
            input[0].mkhi.ki.dwFlags = 2;//抬起
            Win32.SendInput(1u, input, Marshal.SizeOf((object)default(INPUT)));

            Skip = false;
        }

        public void SimulateInputKeyDown(int key)
        {
            Skip = true;
            INPUT[] input = new INPUT[1];

            input[0].type = 1;//模拟键盘
            input[0].mkhi.ki.wVk = (ushort)key;
            input[0].mkhi.ki.dwFlags = 0;//按下
            Win32.SendInput(1u, input, Marshal.SizeOf((object)default(INPUT)));
            //          Sleep(50);


            Skip = false;
        }
        public void SimulateInputKeyUp(int key)
        {
            Skip = true;
            INPUT[] input = new INPUT[1];



            input[0].type = 1;//模拟键盘
            input[0].mkhi.ki.wVk = (ushort)key;
            input[0].mkhi.ki.dwFlags = 2;//抬起
            Win32.SendInput(1u, input, Marshal.SizeOf((object)default(INPUT)));

            Skip = false;
        }

        const uint KEYEVENTF_SCANCODE = 0x0008;
        const uint KEYEVENTF_UNICODE = 0x0004;
        const uint KEYEVENTF_KEYUP = 0x0002;
        public void SimKeyDown(int key)
        {
            Skip = true;
            INPUT[] input = new INPUT[1];

            input[0].type = 1;//模拟键盘
            input[0].mkhi.ki.wVk = 0;
            input[0].mkhi.ki.time = 0;
            input[0].mkhi.ki.dwExtraInfo = IntPtr.Zero;
            input[0].mkhi.ki.dwFlags = KEYEVENTF_SCANCODE;
            input[0].mkhi.ki.wScan = (ushort)key;


            Win32.SendInput(1u, input, Marshal.SizeOf((object)default(INPUT)));
            Skip = false;
        }


        public void SimKeyUp(int key)
        {
            Skip = true;
            INPUT[] input = new INPUT[1];

            input[0].type = 1;//模拟键盘
            input[0].mkhi.ki.wVk = 0;
            input[0].mkhi.ki.time = 0;
            input[0].mkhi.ki.dwExtraInfo = IntPtr.Zero;
            input[0].mkhi.ki.dwFlags = KEYEVENTF_SCANCODE | KEYEVENTF_KEYUP;
            input[0].mkhi.ki.wScan = (ushort)key;


            Win32.SendInput(1u, input, Marshal.SizeOf((object)default(INPUT)));
            Skip = false;
        }

        private const uint VK_A = 65;
        private const uint VK_B = 66;
        private const uint VK_C = 67;
        private const uint VK_D = 68;
        private const uint VK_E = 69;
        private const uint VK_F = 70;
        private const uint VK_G = 71;
        private const uint VK_H = 72;
        private const uint VK_I = 73;
        private const uint VK_P = 80;
        private const uint VK_Q = 81;
        private const uint VK_R = 82;
        private const uint VK_S = 83;
        private const uint VK_T = 84;
        private const uint VK_U = 85;
        private const uint VK_V = 86;
        /*
        private void Send1(string msg)
        {
            Skip = true;

            string wrap_msg;
            switch (msg)
            {
                case "+":
                    wrap_msg = "{+}";
                    break;
                case "%":
                    wrap_msg = "{%}";
                    break;
                case "^":
                    wrap_msg ="{^}";
                    break;
                case "~" :
                    wrap_msg = "{~}";
                    break;
                default:
                    wrap_msg = msg;
                    break;
            }

            System.Windows.Forms.SendKeys.SendWait(wrap_msg);
            Skip = false;
        }
*/
        private void Send2(string text) //剪贴版
        {
            //        Stop();
            //       SimulateInputString('t');
            //      Start();

            Skip = true;

            //          if (States.Minor == 1)
            //        {
            //             System.Windows.Forms.SendKeys.SendWait("^v");
            //        }
            //       else
            {
                Win32.Win32SetText(text);

                CtrlV();
            }


            Skip = false;


            void CtrlV()
            {

                INPUT[] input = new INPUT[1];

                input[0].type = 1;//模拟键盘
                input[0].mkhi.ki.wVk = VK_LCONTROL;
                input[0].mkhi.ki.dwFlags = 0;//按下
                Win32.SendInput(1u, input, Marshal.SizeOf((object)default(INPUT)));



                input[0].type = 1;//模拟键盘
                input[0].mkhi.ki.wVk = (ushort)VK_V;
                input[0].mkhi.ki.dwFlags = 0;//按下
                Win32.SendInput(1u, input, Marshal.SizeOf((object)default(INPUT)));


                input[0].type = 1;//模拟键盘
                input[0].mkhi.ki.wVk = (ushort)VK_V;
                input[0].mkhi.ki.dwFlags = 2;//抬起
                Win32.SendInput(1u, input, Marshal.SizeOf((object)default(INPUT)));


                input[0].type = 1;//模拟键盘
                input[0].mkhi.ki.wVk = VK_LCONTROL;
                input[0].mkhi.ki.dwFlags = 2;//抬起
                Win32.SendInput(1u, input, Marshal.SizeOf((object)default(INPUT)));


                //         Sleep(50);
            }

        }




        #region table

        static Dictionary<string, List<string>> MB = new Dictionary<string, List<string>>(); //码表
        static Dictionary<string, string> ZMB = new Dictionary<string, string>(); //构字码表
        static Dictionary<string, List<string>> PYMB = new Dictionary<string, List<string>>(); //拼音码表
        public static Dictionary<string, string> FMB = new Dictionary<string, string>(); //全码表

        public static Dictionary<string, string> Comment = new Dictionary<string, string>(); //备注
        public static Dictionary<string, string> Split = new Dictionary<string, string>(); //拆分

        Dictionary<int, string> CnSymbol = new Dictionary<int, string>
          {
              {0xBB, "="},
              {0xBC, "，"},
              {0xBD, "-"},
              {0xBE, "。"},
              {0xC0, "·"},
              {0xDB, "【"},
              {0xDC, "、"},
              {0xDD, "】"},
              {0xBF, "、"},
              {0xBA, "；" }

          };

        Dictionary<int, string> ShiftCnSymbol = new Dictionary<int, string>
          {
              {0x30, "）"},
              {0x31, "！"},
              {0x32, "@"},
              {0x33, "#"},
              {0x34, "￥"},
              {0x35, "%"},
              {0x36, "……"},
              {0x37, "&"},
              {0x38, "*"},
              {0x39, "（"},
              {0xBB, "+"},
              {0xBC, "《"},
              {0xBD, "——"},
              {0xBE, "》"},
              {0xC0, "~"},
              {0xDB, "{"},
              {0xDC, "|"},
              {0xDD, "}"},
              {0xBF, "？"},
              {0xBA, "：" }


          };

        Dictionary<int, string> EnSymbol = new Dictionary<int, string>
          {
              {0xBB, "="},
              {0xBC, ","},
              {0xBD, "-"},
              {0xBE, "."},
              {0xC0, "`"},
              {0xDB, "["},
              {0xDC, "\\"},
              {0xDD, "]"},
              {0xBF, "/"},
              {0xBA, ";" },
              {0xDE, "\'" }

          };

        Dictionary<int, string> ShiftEnSymbol = new Dictionary<int, string>
          {
              {0x30, ")"},
              {0x31, "!"},
              {0x32, "@"},
              {0x33, "#"},
              {0x34, "$"},
              {0x35, "%"},
              {0x36, "^"},
              {0x37, "&"},
              {0x38, "*"},
              {0x39, "("},
              {0xBB, "+"},
              {0xBC, "<"},
              {0xBD, "_"},
              {0xBE, ">"},
              {0xC0, "~"},
              {0xDB, "{"},
              {0xDC, "|"},
              {0xDD, "}"},
              {0xBF, "?"},
              {0xBA, ":" },
              {0xDE, "\"" }


          };

        int QUOT = 0xDE;



        #endregion



        public enum CustomOps
        {
            Delete,
            Top,
            Add,
            Advance
        }
        public struct CustomItem 
        {
            public string code;
            public string name;
            public CustomOps ops;
            
        }

        public  string ConstructCi(string name)
        {
            string rt = "";

            string[] skip =
            {
                "\n",
                "\t",
                "\r",
                " "

            };

            foreach (var s in CnSymbol.Values)
                name = name.Replace(s, "");

            foreach (var s in ShiftCnSymbol.Values)
                name = name.Replace(s, "");

            foreach (var s in EnSymbol.Values)
                name = name.Replace(s, "");

            foreach (var s in ShiftEnSymbol.Values)
                name = name.Replace(s, "");

            foreach (var s in skip)
                name = name.Replace(s, "");

            StringInfo ciInfo = new StringInfo(name);

            int len = ciInfo.LengthInTextElements;

            switch (len)
            {
                case 1:
                    if (FMB.ContainsKey(name))
                        rt = FMB[name];
                    else
                        rt = "";
                    break;


                case 2:
                    {
                        string a = GetFullCode(ciInfo.SubstringByTextElements(0, 1));
                        string b = GetFullCode(ciInfo.SubstringByTextElements(1, 1));

                        if (a.Length >= 2 && b.Length >= 2)
                            rt = a.Substring(0, 2) + b.Substring(0, 2);
                        break;
                    }
                case 3:
                    {
                        string a = GetFullCode(ciInfo.SubstringByTextElements(0, 1));
                        string b = GetFullCode(ciInfo.SubstringByTextElements(1, 1));
                        string c = GetFullCode(ciInfo.SubstringByTextElements(2, 1));
                        if (a.Length >= 1 && b.Length >= 1 && c.Length >= 2)
                        {
                            rt = a.Substring(0, 1) + b.Substring(0, 1) + c.Substring(0, 2);
                        }
                        break;
                    }

                default:
                    {
                        if (len >= 4)
                        {
                            string a = GetFullCode(ciInfo.SubstringByTextElements(0, 1));
                            string b = GetFullCode(ciInfo.SubstringByTextElements(1, 1));
                            string c = GetFullCode(ciInfo.SubstringByTextElements(2, 1));
                            string d = GetFullCode(ciInfo.SubstringByTextElements(ciInfo.LengthInTextElements - 1, 1));
                            if (a.Length >= 1 && b.Length >= 1 && c.Length >= 1 && d.Length >= 1)
                            {
                                rt = a.Substring(0, 1) + b.Substring(0, 1) + c.Substring(0, 1) + d.Substring(0, 1);
                            }
                        }
                        else
                            rt = "";

                        break;
                    }
            }

            return rt;
  


            string GetFullCode(string s)
            {
                if (FMB.ContainsKey(s))
                    return FMB[s];
                else
                    return "";
            }

        }
        struct MbItem
        {
            public string name;
            public string code;
            public int freq;
        };
        private void ReadMB() //读码表
        {



            MB.Clear();
            Comment.Clear();
            FMB.Clear();
            PYMB.Clear();
            Split.Clear();
            string path = "mb/" + Config.GetString("当前码表");

            States.CustomFile = "mb/" + Config.GetString("当前码表") + "/用户调整.txt";
            DirectoryInfo dir = new DirectoryInfo(path);
            if (!dir.Exists)
                dir.Create();

            char[] sp = { '\r', '\t' };

            List<MbItem> MbItems = new List<MbItem>();
            List<MbItem> PyMbItems = new List<MbItem>();

            List <CustomItem> Customs = new List<CustomItem>(); //加词

            List<MbItem> NoCode = new List<MbItem>();



            foreach (FileInfo file in dir.GetFiles("*.txt"))
            {

                ReadSingleFile(path + "/" + file.Name);
            }

            foreach (FileInfo file in dir.GetFiles("*.dict.yaml"))
            {

                ReadSingleFile(path + "/" + file.Name);
            }

            //尝试编码
            foreach (var nc in NoCode)
            {


                string code = ConstructCi(nc.name);
                if (code.Length > 0)
                {
                    MbItem m = new MbItem();
                    m.code = code;
                    m.name = nc.name;
                    m.freq = nc.freq;

                    MbItems.Add(m);
                }



            }

            //按权重排序

            //         MbItems = MbItems.OrderByDescending(o => o.freq).ToList();

            var ms = from x in MbItems orderby x.freq descending select x;

            foreach (MbItem r in ms)

       //         foreach (MbItem r in MbItems)
            {


                if (!MB.ContainsKey(r.code))
                {
                    MB[r.code] = new List<string>();

                }
                if (!MB[r.code].Contains(r.name))
                    MB[r.code].Add(r.name);

            }
   

            //检测快符
            Dictionary<string, int> map = new Dictionary<string, int>();

            bool zIsCode = false;
            foreach (var m in MB)
            {
                if (m.Key.Length > 0)
                {
                    string head = m.Key.Substring(0, 1);
                    map[head] = 1;

                    if (!zIsCode && head != "z" && m.Key.Contains("z"))
                        zIsCode = true;
                }




            }


            SHORT_SYMBOL_SEMICOLON = map.ContainsKey(";");


            SHORT_SYMBOL_SLASH = map.ContainsKey("/");

            SHORT_SYMBOL_Z = !zIsCode && map.ContainsKey("a");


            Dictionary<string, List<string>> MB_SYMBOL = new Dictionary<string, List<string>>();
            Dictionary<string, int> cnt2 = new Dictionary<string, int>();

            //建立快符自动上屏清单
            AutoShortSymbol.Clear();
            if (SHORT_SYMBOL_SEMICOLON || SHORT_SYMBOL_SLASH || SHORT_SYMBOL_Z)
            {
                foreach (var m in MB)
                {
                    if (SHORT_SYMBOL_SEMICOLON && m.Key.Length >= 1 && m.Key.Substring(0, 1) == ";"
                        || SHORT_SYMBOL_SLASH && m.Key.Length >= 1 && m.Key.Substring(0, 1) == "/"
                        || SHORT_SYMBOL_Z && m.Key.Length >= 1 && m.Key.Substring(0, 1) == "z"


                        )
                    {

                        MB_SYMBOL.Add(m.Key, m.Value);
                        cnt2[m.Key] = 0;

                    }

                }

                foreach (var m in MB_SYMBOL)
                {
                    for (int i = 0; i < m.Key.Length; i++)
                    {
                        string sub = m.Key.Substring(0, i + 1);
                        if (MB_SYMBOL.ContainsKey(sub))
                        {
                            cnt2[sub] += m.Value.Count;
                        }

                    }
                }




                foreach (var c in cnt2)
                {
                    if (c.Value == 1)
                        AutoShortSymbol.Add(c.Key);
                }




            }


            foreach (FileInfo file in dir.GetFiles("*.注释"))
            {

                ReadSingleComment(path + "/" + file.Name);
            }

            foreach (FileInfo file in dir.GetFiles("*.拆分"))
            {

                ReadSingleSplit(path + "/" + file.Name);
            }

            ProcCustom( Customs);
            string piyinPath = "拼音反查码表";

            DirectoryInfo pyDir = new DirectoryInfo(piyinPath);

            if (!pyDir.Exists)
                pyDir.Create();
            foreach (FileInfo file in pyDir.GetFiles("*.txt"))
            {

                ReadPinyinSingleFile(pyDir + "/" + file.Name);
            }


            //       PyMbItems = PyMbItems.OrderByDescending(o => o.freq).ToList();

            var ps = from x in PyMbItems orderby x.freq descending select x;

            foreach (MbItem r in ps)
            {


                if (!PYMB.ContainsKey(r.code))
                {
                    PYMB[r.code] = new List<string>();

                }

                if (!PYMB[r.code].Contains(r.name))
                    PYMB[r.code].Add(r.name);

            }

            PyMbItems = null;
            MbItems = null;

            ps = null;
            GC.Collect();



            void ReadSingleFileOld(string path1)
            {
                //   Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);


                //  Encoding enc = TxtFileEncoder.GetEncoding(path1, Encoding.GetEncoding("GB18030"));

                Encoding enc = TxtFileEncoder.GetEncoding(path1);//, Encoding.ASCII);

                char[] sp1 = { '\n' };
                string[] lines = File.ReadAllText(path1, enc).Replace("\r","").Split(sp1, StringSplitOptions.RemoveEmptyEntries);
                bool StartReading = true;

                if (lines.Contains("...") || lines.Contains("...\r"))
                    StartReading = false;

                foreach (string line in lines)
                {
                    if (line.Length == 0)
                    { continue; }

                    //注释，文件头
                    if (line.Substring(0, 1) == "#")
                        continue;

                    if (!StartReading)
                    {
                        if (line == "..." || line == "...\r")
                            StartReading = true;

                        continue;
                    }




                    int pos = line.IndexOf('#');
                    string trim;
                    if (pos < 0)
                        trim = line;
                    else
                        trim = line.Substring(0, pos);



                    if (trim.Length > "{添加}".Length && trim.Substring(0, "{添加}".Length) == "{添加}")
                    {
                        AddAdd(trim);
                        continue;
                    }

                    if (trim.Length > "{前移}".Length && trim.Substring(0, "{前移}".Length) == "{前移}")
                    {
                        AdvanceAdd(trim);
                        continue;
                    }

                    if (trim.Length > "{删除}".Length && trim.Substring(0, "{删除}".Length) == "{删除}")
                    {
                        DeleteAdd(trim);
                        continue;
                    }

                    if (trim.Length > "{置顶}".Length && trim.Substring(0, "{置顶}".Length) == "{置顶}")
                    {
                        TopAdd(trim);
                        continue;
                    }



                    //拆开
                    string[] parts = trim.Split(new char[] { '\t',' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 1)
                    {
                        MbItem r = new MbItem();
                        r.name = WrapDec( parts[0]);

                        if (parts.Length >= 3 && Int32.TryParse(parts[2], out int value2))
                        {
                            r.code = parts[1];
                            r.freq = value2;
                            MbItems.Add(r);

                        }
                        else if (parts.Length >= 3 && Int32.TryParse(parts[1], out int value1))
                        {
                            r.code = parts[2];
                            r.freq = value1;
                            MbItems.Add(r);

                        }
                        else if (parts.Length >= 2 && !Int32.TryParse(parts[1], out int value3)) //无权重有编码
                        {
                            r.code = parts[1];
                            r.freq = 0;
                            MbItems.Add(r);
                        }
                        else if (parts.Length >= 2 && Int32.TryParse(parts[1], out int value4)) //有权重无编码
                        {
                            r.code = "";
                            r.freq = value4;
                            NoCode.Add(r);
                        }
                        else if (parts.Length == 1) //有权重无编码
                        {
                            r.code = "";
                            r.freq = 0;
                            NoCode.Add(r);
                        }

                        if (r.code.Length > 0 &&(!FMB.ContainsKey(r.name)   || FMB[r.name].Length < r.code.Length))
                            FMB[r.name] = r.code;




                    }


                }
            }


            void ReadSingleFile(string path1)
            {


                Encoding enc = TxtFileEncoder.GetEncoding(path1);//, Encoding.ASCII);



                StreamReader sw = new StreamReader(path1, enc);

                string line;
                List<MbItem> items = new List<MbItem>();

                bool skipHead = false;
                do
                {
                    line = sw.ReadLine();
                    if (line == null)
                        break;
                    if (line.Length == 0)
                    { continue; }

                    //注释，文件头
                    if (line.Substring(0, 1) == "#")
                        continue;

                    if (!skipHead)
                    {
                        if (line == "..." || line == "...\r")
                        {
                            skipHead = true;

                            items.Clear();
                            continue;
                        }

                    }


                    int pos = line.IndexOf('#');
                    string trim;
                    if (pos < 0)
                        trim = line.Replace("\r","");
                    else
                        trim = line.Substring(0, pos).Replace("\r", "");



                    if (trim.Length > "{添加}".Length && trim.Substring(0, "{添加}".Length) == "{添加}")
                    {
                        AddAdd(trim);
                        continue;
                    }

                    if (trim.Length > "{前移}".Length && trim.Substring(0, "{前移}".Length) == "{前移}")
                    {
                        AdvanceAdd(trim);
                        continue;
                    }

                    if (trim.Length > "{删除}".Length && trim.Substring(0, "{删除}".Length) == "{删除}")
                    {
                        DeleteAdd(trim);
                        continue;
                    }

                    if (trim.Length > "{置顶}".Length && trim.Substring(0, "{置顶}".Length) == "{置顶}")
                    {
                        TopAdd(trim);
                        continue;
                    }



                    //拆开
                    string[] parts = trim.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 1)
                    {
                        MbItem r = new MbItem();
                        r.name = WrapDec(parts[0]);

                        if (parts.Length >= 3 && Int32.TryParse(parts[2], out int value2))
                        {
                            r.code = parts[1];
                            r.freq = value2;
                            items.Add(r);

                        }
                        else if (parts.Length >= 3 && Int32.TryParse(parts[1], out int value1))
                        {
                            r.code = parts[2];
                            r.freq = value1;
                            items.Add(r);

                        }
                        else if (parts.Length >= 2 && !Int32.TryParse(parts[1], out int value3)) //无权重有编码
                        {
                            r.code = parts[1];
                            r.freq = 0;
                            items.Add(r);
                        }
                        else if (parts.Length >= 2 && Int32.TryParse(parts[1], out int value4)) //有权重无编码
                        {
                            r.code = "";
                            r.freq = value4;
                            NoCode.Add(r);
                        }
                        else if (parts.Length == 1) //有权重无编码
                        {
                            r.code = "";
                            r.freq = 0;
                            NoCode.Add(r);
                        }

                        if (r.code.Length > 0 && (!FMB.ContainsKey(r.name) || FMB[r.name].Length < r.code.Length))
                            FMB[r.name] = r.code;




                    }


                }while (true);
                MbItems.AddRange(items);
                sw.Close();
            }

            void ReadPinyinSingleFile(string path1)
            {
                char[] sp1 = { '\n' };

                Encoding enc = TxtFileEncoder.GetEncoding(path1);//, Encoding.ASCII);

                StreamReader sw = new StreamReader(path1,enc);

                string line;
                List <MbItem> items = new List <MbItem>();

                bool skipHead = false;
                do
                {
                    line = sw.ReadLine();
                    if (line == null)
                        break;

                    if (line.Length == 0)
                        continue;

                    if (line.Substring(0, 1) == "#")
                        continue;

                    
                    if (!skipHead)
                    {
                        if (line == "..." || line == "...\r")
                        {
                            skipHead = true;

                            items.Clear();
                            continue;
                        }
 
                    }
                    


                    int pos = line.IndexOf('#');
                    string trim;
                    if (pos < 0)
                        trim = line;//.Replace("\r","");
                    else
                        trim = line.Substring(0, pos).Replace("\r", "");




                    string[] parts = trim.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 2)
                    {
                        MbItem r = new MbItem();
                        r.name = WrapDec(parts[0]);

                        if (parts.Length >= 3 && Int32.TryParse(parts[2], out int value2))
                        {
                            r.code = parts[1];
                            r.freq = value2;
                                                items.Add(r);


                        }
                        else if (parts.Length >= 3 && Int32.TryParse(parts[1], out int value1))
                        {
                            r.code = parts[2];
                            r.freq = value1;
                                              items.Add(r);

                        }
                        else if (parts.Length >= 2)
                        {
                            r.code = parts[1];
                            r.freq = 0;
                                              items.Add(r);
                        }





                    }

                } while (true);

 
                sw.Close();


                PyMbItems.AddRange(items);
                items = null;
                GC.Collect();
            }

            void ReadPinyinSingleFileOld2(string path1)
            {
                char[] sp1 = { '\n' };

                Encoding enc = TxtFileEncoder.GetEncoding(path1);//, Encoding.ASCII);

                StreamReader sw = new StreamReader(path1, enc);
                string line;
                bool skipHead = false;
                while ((line = sw.ReadLine()) != null)
                {
                    if (line == "..." || line == "...\r")
                    {
                        skipHead = true;

                        break;
                    }
                }
                sw.Close();

                sw = new StreamReader(path1, enc);
         //       List<MbItem> items = new List<MbItem>();

       //         bool skipHead = false;
                do
                {
                    line = sw.ReadLine();
                    if (line == null)
                        break;

                    if (line.Length == 0)
                        continue;

                    if (line.Substring(0, 1) == "#")
                        continue;


                    if (skipHead)
                    {
                        if (line == "..." || line == "...\r")
                        {
                            skipHead = false;
                        }
                        continue;

                    }



                    int pos = line.IndexOf('#');
                    string trim;
                    if (pos < 0)
                        trim = line;//.Replace("\r","");
                    else
                        trim = line.Substring(0, pos).Replace("\r", "");




                    string[] parts = trim.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 2)
                    {
                        MbItem r = new MbItem();
                        r.name = WrapDec(parts[0]);

                        if (parts.Length >= 3 && Int32.TryParse(parts[2], out int value2))
                        {
                            r.code = parts[1];
                            r.freq = value2;
                            
                           PyMbItems.Add(r);


                        }
                        else if (parts.Length >= 3 && Int32.TryParse(parts[1], out int value1))
                        {
                            r.code = parts[2];
                            r.freq = value1;
                            PyMbItems.Add(r);

                        }
                        else if (parts.Length >= 2)
                        {
                            r.code = parts[1];
                            r.freq = 0;
                            PyMbItems.Add(r);
                        }





                    }

                } while (true);


                sw.Close();


            }


            void ReadSingleComment(string path2)
            {
                char[] sp1 = { '\n' };

                Encoding enc = TxtFileEncoder.GetEncoding(path2);//, Encoding.ASCII);
                string[] lines = File.ReadAllText(path2, enc).Replace("\r", "").Split(sp1, StringSplitOptions.RemoveEmptyEntries);




                foreach (string line in lines)
                {
                    if (line.Substring(0, 1) == "#")
                        continue;



                    string[] parts = line.Split(sp, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length < 2)
                        continue;

                    if (!Comment.ContainsKey(parts[0]))
                    {
                        Comment.Add(parts[0], parts[1]);
                    }
                    else
                    {
                        Comment[parts[0]] += " " + parts[1];
                    }




                }
            }

            void ReadSingleSplit(string path2)
            {
                char[] sp1 = { '\n' };
                Encoding enc = TxtFileEncoder.GetEncoding(path2);//, Encoding.ASCII);
                string[] lines = File.ReadAllText(path2, enc).Replace("\r", "").Split(sp1, StringSplitOptions.RemoveEmptyEntries);




                foreach (string line in lines)
                {
                    if (line.Substring(0, 1) == "#")
                        continue;



                    string[] parts = line.Split(sp, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length < 2)
                        continue;

                    Split[parts[0]] = parts[1];



                }
            }


            void AddAdd(string line)
            {
                string head = "{添加}";

                line = line.Replace(head, "");
                var ls = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                if (ls.Length < 2)
                    return;

                CustomItem d = new CustomItem();
                d.code = ls[0];
                d.name = WrapDec( ls[1]);
                d.ops = CustomOps.Add;
                Customs.Add(d);
            }


            void AdvanceAdd(string line)
            {
                string head = "{前移}";

                line = line.Replace(head, "");
                var ls = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                if (ls.Length < 2)
                    return;

                CustomItem d = new CustomItem();
                d.code = ls[0];
                d.name = WrapDec(ls[1]);
                d.ops = CustomOps.Advance;
                Customs.Add(d);
            }


            void DeleteAdd (string line)
            {
                string head = "{删除}";

                line = line.Replace(head, "");
                var ls = line.Split(new char[] { ' ', '\t' },StringSplitOptions.RemoveEmptyEntries);

                if (ls.Length < 2)
                    return;

                CustomItem d = new CustomItem();
                d.code = ls[0];
                d.name = WrapDec(ls[1]);
                d.ops = CustomOps.Delete;
                Customs.Add(d);
            }

            void TopAdd(string line)
            {
                string head = "{置顶}";

                line = line.Replace(head, "");
                var ls = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                if (ls.Length < 2)
                    return;

                CustomItem d = new CustomItem();
                d.code = ls[0];
                d.name = WrapDec(ls[1]);

                d.ops = CustomOps.Top;
                Customs.Add(d);
            }






        }


        public  static string WrapDec (string s) //解码转义符
        {
            const string WrapConst = "Bime20231222BIME";
            Dictionary<string, string> MbWrap = new Dictionary<string, string> { { "\\t", "\t" }, { "\\n", "\r\n" }, { "\\s", " " } };

            s = s.Replace("\\\\", WrapConst);

            foreach (var t in MbWrap)
            {
                s = s.Replace(t.Key, t.Value);
            }

            s = s.Replace( WrapConst, "\\");
            return s;
        }

        public static string WrapEnc(string s) //编码转义符
        {
 
            Dictionary<string, string> MbWrap = new Dictionary<string, string> { {"\\","\\\\" }, {  "\t", "\\t"}, {  "\r\n", "\\n" }, { "\n", "\\n" }, {  " ", "\\s" } };



            foreach (var t in MbWrap)
            {
                s = s.Replace(t.Key, t.Value);
            }


            return s;
        }

        public void ProcCustom(List<CustomItem> Customs)
        {
            foreach (var d in Customs)
            {
                switch (d.ops)
                {
                    case CustomOps.Delete:
                        if (MB.ContainsKey(d.code) && MB[d.code].Contains(d.name))
                        {
                            MB[d.code].Remove(d.name);
                        }
                        break;
                    case CustomOps.Top:
                        if (MB.ContainsKey(d.code) && MB[d.code].IndexOf(d.name) > 0)
                        {
                            MB[d.code].Remove(d.name);
                            MB[d.code].Insert(0, d.name);
                        }
                        break;

                    case CustomOps.Advance:
                        if (MB.ContainsKey(d.code) && MB[d.code].IndexOf(d.name) > 0)
                        {
                            int pos = MB[d.code].IndexOf(d.name);
                            MB[d.code].Remove(d.name);
                            MB[d.code].Insert(pos -1, d.name);
                        }
                        break;
                    case CustomOps.Add:
                        if (!MB.ContainsKey(d.code))
                        {
                            MB.Add(d.code, new List<string>() { d.name });
                        }
                        else
                        {
                            if (!MB[d.code].Contains(d.name))
                                MB[d.code].Add(d.name);
                        }
                        break;
                    default:
                        break;
                }


            }
        }
        private void WrtiteReadme()
        {




            var uri = new Uri("pack://application:,,,/readme.md");
            var resourceStream = Application.GetResourceStream(uri);
            StreamReader sr = new StreamReader(resourceStream.Stream);
            string ReadmeTxt = sr.ReadToEnd();
            sr.Close();

            //    if (!File.Exists("说明.txt"))
            //      File.WriteAllText("说明.txt", ReadmeTxt);







        }



        bool IsShifted
        {
            get
            {
                return Win32.GetKeyState(VK_SHIFT) < 0;
            }

        }


        bool NotShifted
        {
            get
            {
                return Win32.GetKeyState(VK_SHIFT) >= 0;
            }

        }


        bool IsCtrled
        {
            get
            {
                return Win32.GetKeyState(VK_CONTROL) < 0;
            }

        }


        bool NotCtrled
        {
            get
            {
                return Win32.GetKeyState(VK_CONTROL) >= 0;
            }

        }




        public void Start()
        {
            // 安装键盘钩子
            if (hKeyboardHook == 0)
            {
                KeyboardHookProcedure = new HookProc(KeyboardHookProc);

                hKeyboardHook = Win32.SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookProcedure, Win32.GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0);

                //如果SetWindowsHookEx失败
                if (hKeyboardHook == 0)
                {
                    Stop();
                    throw new Exception("安装键盘钩子失败");
                }
            }
            Skip = false;
        }
        public void Stop()
        {
            bool retKeyboard = true;


            if (hKeyboardHook != 0)
            {
                retKeyboard = Win32.UnhookWindowsHookEx(hKeyboardHook);
                hKeyboardHook = 0;
            }

            if (!(retKeyboard))
                throw new Exception("卸载钩子失败！");
        }




        long LastKeyState = 1;
        int LastvkCode = 0;
        HashSet<int> ModifierVcode = new HashSet<int>
        {
            0xA0,
            0xA1,
            0xA2,
            0xA3,
            0xA4,
            0xA5,
            0x5B,
            0x5C


        };


        public static int CallNextHook(int idHook, int nCode, int wParam, IntPtr lParam, bool forceZero = false)
        {
            if (SendHistory.IsRecording && !forceZero)
                return 1;
            else
                return Win32.CallNextHookEx(idHook, nCode, wParam, lParam);
        }
        int soundCounter = 0;
        int soundIndex = 0;
        Random rnd = new Random();
        private void PlaySound(int keyType)
        {

            MediaPlayer player = new MediaPlayer();

            double vol = Config.GetDouble("按键音量0~100") / 100.0;

            if (vol < 0)
                vol = 0;

            if (vol > 1)
                vol = 1;
            player.Volume = vol;

  

            string path = "";
            switch (keyType)
            {
                case VK_SPACE:
                    player.Open(new Uri("sounds/KeySpace.wav", UriKind.Relative));
                    break;
                case VK_BACK:
                case VK_RETURN:
                case VK_ESCAPE:
                case VK_LSHIFT:
                case VK_RSHIFT:
                case VK_SHIFT:

                    player.Open(new Uri("sounds/KeyFunc.wav", UriKind.Relative));
                    break;
                default:
                    player.Open(new Uri("sounds/KeyNormal.wav", UriKind.Relative));
                    break;

            }


            player.Play();
        }

        private int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {






            if (Skip)
            {

                //return 0;
                return CallNextHook(hKeyboardHook, nCode, wParam, lParam, true);
            }


            int rt = 0;



            if (nCode < 0)
            {
                // return 0;
                return CallNextHook(hKeyboardHook, nCode, wParam, lParam, true);
            }


            KeyboardHookStruct InputKey = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));

            uint testbit = 1 << 7;
            long keyUpDown = InputKey.flags & testbit;


            if (keyUpDown == 0) //key down
            {







                States.Update();
                LastKeyState = 0;


                LastvkCode = InputKey.vkCode;

                if (!States.Off && Config.GetBool("开启打字音效(娱乐)"))
                    PlaySound(InputKey.vkCode);


                ///单独的修饰键key_down不处理
                if (ModifierVcode.Contains(InputKey.vkCode))
                {
                    return CallNextHook(hKeyboardHook, nCode, wParam, lParam, true);
                    //  return 0;
                }


              // alt键处理
                if (Win32.GetKeyState(VK_MENU) < 0)
                {
                    if (InputKey.vkCode == VK_OEM_5 && Config.GetBool("Alt+\\开启或禁用Bime"))
                    {
                        States.Off = !States.Off;
                        if (States.Off)
                        {
                            InputCode = "";
                            if (Config.GetBool("自动切换系统语言"))
                                ImiHelper.ChangeLanguageEnabeled();
                            this.Topmost = false;
                        }

                        else
                        {
                            this.Topmost = true;
                            if (Config.GetBool("自动切换系统语言"))
                                ImiHelper.ChangeLanguageDisabled();
                            if (State == CompositionState.En)
                                Toggle();
                        }

                        UpdateStatesBar();
                        return 1;
                    }
                    else if (State == CompositionState.CnComposing && InputKey.vkCode >= 49 && InputKey.vkCode <= 57 && NotShifted) //1-9 逐步调频
                    {

                        int num = InputKey.vkCode - 49 + 1;


                        UserAdvance(InputCode, Candi.Current[num - 1]);
                        return 1;



                    }
                    else
                    {
                        return CallNextHook(hKeyboardHook, nCode, wParam, lParam, true);
                        //   return 0;
                    }

                    //      return CallNextHook(hKeyboardHook, nCode, wParam, lParam, true);
                    // return 0;
                }

                if (States.Off)
                    return CallNextHook(hKeyboardHook, nCode, wParam, lParam, true);


                //ctrl键处理
                if (Win32.GetKeyState(VK_CONTROL) < 0)
                {
                    if (InputKey.vkCode == VK_SPACE && Config.GetBool("Ctrl+空格切换中英文")) //ctrl space中英文切换
                    {
                        Toggle();

                        UpdateStatesBar();
                        return 1;
                    }
                    else if (InputKey.vkCode == VK_OEM_PLUS && Config.GetBool("Ctrl+等号手动加词")) //ctrl 加号 手动加词
                    {
                        ShowWinAddCi();
                        return 1;
                    }
                    else if (State== CompositionState.CnComposing && InputKey.vkCode >= 49 && InputKey.vkCode <= 57) // ctrl 1-9 调频删词
                    {

                        int num = InputKey.vkCode - 49 + 1;


                        if (IsShifted && IsCtrled)
                        {
                            UserDelete(InputCode, Candi.Current[num - 1]);
                            return 1;
                        }
                        else if (IsCtrled)
                        {
                            UserTop(InputCode, Candi.Current[num - 1]);
                            return 1;
                        }


                    }
                    else
                    {
                        return CallNextHook(hKeyboardHook, nCode, wParam, lParam, true);
                        //   return 0;
                    }
                }
                //capslock
                if (InputKey.vkCode == VK_CAPITAL)
                {
                    CommitCandi(0);
                    return CallNextHook(hKeyboardHook, nCode, wParam, lParam, true);
                }

             if (Win32.GetKeyState(VK_LWIN) < 0 || Win32.GetKeyState(VK_RWIN) < 0)
                {
                    return CallNextHook(hKeyboardHook, nCode, wParam, lParam, true);
                    //  return 0;
                }

                if (Win32.GetKeyState(VK_CAPITAL) > 0)
                {

                    return CallNextHook(hKeyboardHook, nCode, wParam, lParam);
                }


                if (States.IsGdq)
                {
                    SimulateInputKey(VK_F14);
                }

            //    if (States.Off)
              //      rt = ProcEn(InputKey.vkCode);
               
                switch (State)
                {
                    case CompositionState.En:
                        rt = ProcEn(InputKey.vkCode);
                        break;
                    case CompositionState.CnUpperCase:
                        rt = ProcCnUpperCase(InputKey.vkCode);
                        break;
                    case CompositionState.CnIdle:
                        rt = ProcCnIdle(InputKey.vkCode);
                        break;
                    case CompositionState.CnComposing:
                        rt = ProcCnComposing(InputKey.vkCode);
                        break;
                    case CompositionState.CnPinyin:
                        rt = ProcCnPinyin(InputKey.vkCode);
                        break;
                    default:
                        rt = 0;
                        break;
                }


                back_double = false;
                back_single = false;
                if (rt == 0 && InputKey.vkCode == VK_BACK && SendHistory.Count > 1)
                {
                    if (States.IsGdq)
                        SimulateInputKey(VK_F15);



                    string last = SendHistory.Pop();

                    if (last == "“" || last == "”")
                    {
                        left_double = !left_double;
                        back_double = true;
                    }
                    else if (last == "‘" || last == "’")
                    {
                        left_single = !left_single;
                        back_single = true;
                    }




                }

                if (rt == 0)
                    return CallNextHook(hKeyboardHook, nCode, wParam, lParam);
                else
                    return 1;

            }
            else //key up
            {



                if (!States.Off && ModifierVcode.Contains(InputKey.vkCode) && InputKey.vkCode == LastvkCode && LastKeyState == 0) //单按了修饰键
                {
                    LastvkCode = InputKey.vkCode;
                    LastKeyState = 1;

                    switch (InputKey.vkCode)
                    {
                        case VK_LSHIFT:
                        case VK_RSHIFT:
                            if (Config.GetBool("shift切换中英文"))
                            {
                                Toggle();
                                //                        rt = 1;
                            }
                            break;
                            /*
                        case VK_RCONTROL:
                        case VK_RMENU:
                            if (Config.GetBool("右Ctrl或右Alt开启/禁用Bime"))
                            {
                                States.Off = !States.Off;
                                if (States.Off)
                                    InputCode = "";
                                else
                                    ImiHelper.ChangeLanguageDisabled();
                                UpdateStatesBar();
                            }
                            break;
                            */
                        default:
                            break;

                    }
                }

                LastvkCode = InputKey.vkCode;
                LastKeyState = 1;


                if (rt == 0)
                    return CallNextHook(hKeyboardHook, nCode, wParam, lParam, true);
                else
                    return 1;

            }


        }

        Timer ManualTimer = null;
        private void ManualTimerSet ()
        {
            if (ManualTimer != null)
            {
                ManualTimer.Dispose();
                ManualTimer = null;
            }


            if (InputCode.Length < 2)
                return;





            if (InputCode.ToLower() == "ds")
            {

                return;
            }


            double.TryParse(InputCode.Substring(2).Replace(",", "").Replace(" ", ""), out double d);
            d = d * 60 * 1000; //转化为毫秒

            ManualTimer = new Timer(ManualTimerSub, null, (int)d,  Timeout.Infinite);
            
        }

        private void ManualTimerSub (object obj)
        {

            MessageBox.Show("时间差不多咯！", "计时器", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
        }


        private string ConverToCnum ()
        {

            if (InputCode.Length < 2)
                return "数字格式错误!";

            decimal.TryParse(InputCode.Substring(1).Replace(",", "").Replace(" ", ""), out decimal dec);

            if (dec < 0)
                return "数字格式错误!";

            return ConvertToChinese(dec);



            string ConvertToChinese(decimal number)
            {

                string s = number.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");

                s = s.Replace("0B0A", "@");

                string d = Regex.Replace(s, @"(((?<=-)|(?!-)^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");

                string r = Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空整分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].ToString());

                return r;

            }
        }



        private Regex RgTimer = new Regex(@"^D[sS]([0-9,]*[.])?[0-9,]+$");
        private Regex RgCnum = new Regex(@"^S([0-9,]*[.])?[0-9,]+$");
        private int ProcCnUpperCase(int value) //中文有编码
        {





            if (value == VK_SPACE || value == VK_RETURN) //空格&回车
            {

               

                

                if (RgTimer.IsMatch(InputCode))
                {
                    ManualTimerSet();
                    BufferClear();
            
                }
                else if (RgCnum.IsMatch(InputCode))
                {
                    Send (ConverToCnum());
                    BufferClear();
                }
                else
                {
                    
                    CommitCode();
                }



    

                StateTrans(CompositionState.CnIdle);

                return KeyProccessed();
            }
            else if (value == VK_TAB) //tab
            {


                if (Config.GetBool("TAB清屏"))
                {

                    BufferClear();
                    StateTrans(CompositionState.CnIdle);
                    return KeyProccessed();

                }
                else { return 0; }




            }
            else if (Win32.GetKeyState(0x10) < 0 && ShiftEnSymbol.ContainsKey(value)) //shift符号
            {




                CommitCode();
                Send(ShiftEnSymbol[value]);

                StateTrans(CompositionState.CnIdle);

                return KeyProccessed();
            }
            else if (EnSymbol.ContainsKey(value))
            {

                if (value == VK_OEM_PERIOD || value == VK_OEM_COMMA)
                {

                    if (RgTimer.IsMatch(InputCode) || RgCnum.IsMatch(InputCode))
                    {

                        BufferAdd(EnSymbol[value]);

                        return KeyProccessed();
                    }

                }

                CommitCode();
                Send(EnSymbol[value]);

                StateTrans(CompositionState.CnIdle);

                return KeyProccessed();
            }
            else if (value >= 48 && value <= 57) //0-9
            {

                int num = value - 49 + 1;

                BufferAdd(num.ToString());

                return KeyProccessed();
            }
            else if (value >= 65 && value <= 90) //a-z
            {
                string ch = az[value - 65];
                if (Win32.GetKeyState(0x10) < 0)
                {
                    ch = ch.ToUpper();

                }


                BufferAdd(ch);

                return KeyProccessed();
            }
            else if (value == 8) //backspace
            {

                BufferBack();

                if (InputCode == "")
                    StateTrans(CompositionState.CnIdle);


                return KeyProccessed();
            }
            /*
            else if (value == VK_RETURN) //enter
            {



                CommitCode();
                StateTrans(CompositionState.CnIdle);

                return KeyProccessed();
            }
            */
            else if (value == 0x1B) //Esc
            {




                BufferClear();
                StateTrans(CompositionState.CnIdle);

                return KeyProccessed();
            }
            else
            {

                return 0;
            }

        }

        static List<string> az = new List<string> { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
        static List<string> AZ = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };


        bool left_single = true;
        bool left_double = true;
        bool back_double = false;//回改引号方向
        bool back_single = false;//回改单引方向
                                 //       bool last_num = false;
        private int ProcCnIdle(int value)  //中文无编码
        {
            SHORT_SYMBOL = false;

            if (value == 32) //空格
            {

                SendHistory.Push(" ");
                return 0;
            }
            else if (SHORT_SYMBOL_SEMICOLON && value == 0xBA && NotShifted)
            {

                if (AutoShortSymbol.Contains(";"))
                {

                    AutoCommitCandi(";");

                    StateTrans(CompositionState.CnIdle);
                }
                else
                {
                    BufferAdd(";");
                    SHORT_SYMBOL = true;
                    StateTrans(CompositionState.CnComposing);

                }

                return KeyProccessed();
            }
            else if (SHORT_SYMBOL_SLASH && value == 0xBF && NotShifted)
            {
                if (AutoShortSymbol.Contains("/"))
                {

                    AutoCommitCandi("/");

                    StateTrans(CompositionState.CnIdle);
                }
                else
                {
                    BufferAdd("/");
                    SHORT_SYMBOL = true;
                    StateTrans(CompositionState.CnComposing);

                }

                return KeyProccessed();
            }
            else if (SHORT_SYMBOL_Z && value == 0x5a && NotShifted)
            {

                if (AutoShortSymbol.Contains("z"))
                {

                    AutoCommitCandi("z");

                    StateTrans(CompositionState.CnIdle);
                }
                else
                {
                    BufferAdd("z");
                    SHORT_SYMBOL = true;
                    StateTrans(CompositionState.CnComposing);
                }


                return KeyProccessed();
            }

            else if (IsShifted && ShiftCnSymbol.ContainsKey(value)) //shift符号
            {

                Send(Config.GetBool("中文状态下使用英文标点") ? ShiftEnSymbol[value] : ShiftCnSymbol[value]);



                return KeyProccessed();
            }
            else if (Config.GetBool("`键拼音反查") && PYMB.Count > 0 && NotShifted && value == VK_OEM_3)
            {

                BufferAdd("·");
                StateTrans(CompositionState.CnPinyin);
                return KeyProccessed();
            }
            else if (CnSymbol.ContainsKey(value)) //符号
            {
                Send(Config.GetBool("中文状态下使用英文标点") ? EnSymbol[value]: CnSymbol[value]);



                return KeyProccessed();
            }
            else if (Win32.GetKeyState(0x10) < 0 && value == QUOT) //引号
            {



                if (!back_double && left_double || back_double && !left_double || SendHistory.Count > 0 && SendHistory.First() == "：" && !back_double)
                {
                    Send("“");
                    left_double = false;
                }
                else
                {
                    Send("”");
                    left_double = true;

                }






                return KeyProccessed();
            }
            else if (value == QUOT) //单引
            {

                if (!back_single && left_single || back_single && !left_single || SendHistory.Count > 0 && SendHistory.First() == "：" && !back_single)
                {
                    Send("‘");
                    left_single = false;
                }
                else
                {
                    Send("’");
                    left_single = true;
                }






                return KeyProccessed();
            }
            else if (value >= 65 && value <= 90 && Win32.GetKeyState(0x10) < 0) //大写字母
            {







                BufferAdd(AZ[value - 65]);

                StateTrans(CompositionState.CnUpperCase);

                return KeyProccessed();
            }
            else if (value == 163)
            {
                StateTrans(CompositionState.En);

                return KeyProccessed();

            }
            else if (value >= 48 && value <= 57) //1-9 //小写字母
            {
                int num = value - 49 + 1;

                SendHistory.Push(num.ToString());
                States.LastNum = true;
                return 0;
            }
            else if (value >= 65 && value <= 90)
            {


                if (Config.GetInt("自动上屏时间（毫秒）") > 0 && RevertKey(value))
                {
                    StateTrans(CompositionState.CnComposing);
                    return ProcCnComposing(value);
                    

                }

                else
                {
                    BufferAdd(az[value - 65]);
                    StateTrans(CompositionState.CnComposing);


                    return KeyProccessed();
                }



            }
            else if (value == 0x0D) //enter
            {



                SendHistory.Push("\n");

                return 0;
            }
            else
            {
                return 0;
            }

        }

        private int KeyProccessed()
        {
            States.HitDiff++; //击键累加
            return 1;
        }
        private int ProcCnComposing(int value) //中文有编码
        {





            if (Win32.GetKeyState(0x10) < 0 && ShiftCnSymbol.ContainsKey(value)) //大写符号
            {

                if (MB.ContainsKey(InputCode))
                {
                    CommitCandi(0);
                    Send(Config.GetBool("中文状态下使用英文标点") ? ShiftEnSymbol[value] : ShiftCnSymbol[value]);
                }
                else
                {
                    BufferClear();
                }

                StateTrans(CompositionState.CnIdle);



                return KeyProccessed();
            }
            else if (value == VK_OEM_6 && NotShifted && Config.GetString("翻页键") == "[ ]" || value == VK_OEM_PLUS && NotShifted && Config.GetString("翻页键") == "- =" || value == VK_TAB && NotShifted && Config.GetString("翻页键") == "Shift Tab/Tab" || value == VK_NEXT && NotShifted && Config.GetString("翻页键") == "PageUp/PageDown")//下翻页
            {
                Candi.NextPage();
                winCandidate.Update();
                return KeyProccessed();

            }
            else if (value == VK_OEM_4 && NotShifted && Config.GetString("翻页键") == "[ ]" || value == VK_OEM_MINUS && NotShifted && Config.GetString("翻页键") == "- =" || value == VK_TAB && IsShifted && Config.GetString("翻页键") == "Shift Tab/Tab" || value == VK_PRIOR && NotShifted && Config.GetString("翻页键") == "PageUp/PageDown")//上翻页
            {
                Candi.PrevPage();
                winCandidate.Update();
                return KeyProccessed();
            }
            else if (SHORT_SYMBOL_SEMICOLON && InputCode == ";" && (value == 0xBA || value == VK_SPACE && !MB.ContainsKey(";")))//双击快符
            {


                Send("；");
                buffer_len = 0;
                BufferClear();
                StateTrans(CompositionState.CnIdle);
                return KeyProccessed();

            }
            else if (SHORT_SYMBOL_SLASH && InputCode == "/" && (value == 0xBF || value == VK_SPACE && !MB.ContainsKey("/")))
            {


                Send("、");
                buffer_len = 0;
                BufferClear();

                StateTrans(CompositionState.CnIdle);
                return KeyProccessed();

            }
            else if (value == 32) //空格
            {

                CommitCandi(0);
                StateTrans(CompositionState.CnIdle);
                return KeyProccessed();
            }
            else if (Config.GetBool("分号次选") && value == 0xBA)// && MB.ContainsKey(InputCode) && MB[InputCode].Count >= 2) //分号次选
            {

                CommitCandi(1);

                StateTrans(CompositionState.CnIdle);
                return KeyProccessed();
            }
            else if (Config.GetBool("引号三选") && value == 0xDE)// && MB.ContainsKey(InputCode) && MB[InputCode].Count >= 3) //引号次选
            {

                CommitCandi(2);
                //     InputCode = "";

                StateTrans(CompositionState.CnIdle);
                return KeyProccessed();
            }

            else if (CnSymbol.ContainsKey(value)) //标点顶屏
            {


                if (MB.ContainsKey(InputCode))
                {
                    CommitCandi(0);
                    Send(Config.GetBool("中文状态下使用英文标点") ? EnSymbol[value] : CnSymbol[value]);
                }
                else
                {
                    BufferClear();
                }

                StateTrans(CompositionState.CnIdle);



                return KeyProccessed();
            }
            else if (Win32.GetKeyState(0x10) < 0 && value == QUOT)
            {


                if (MB.ContainsKey(InputCode))
                {
                    if (left_double)
                    {
                        CommitCandi(0);
                        Send("“");
                    }
                    else
                    {
                        CommitCandi(0);
                        Send("”");
                    }


                    left_double = !left_double;


                }
                else
                {
                    BufferClear();
                }


                StateTrans(CompositionState.CnIdle);




                return KeyProccessed();
            }
            else if (value == QUOT)
            {



                if (MB.ContainsKey(InputCode))
                {
                    if (left_single)
                    {
                        CommitCandi(0);
                        Send("‘");

                    }
                    else
                    {
                        CommitCandi(0);
                        Send("’");
                    }



                    left_single = !left_single;


                }
                else
                {
                    BufferClear();
                }

                StateTrans(CompositionState.CnIdle);



                return KeyProccessed();
            }
            else if (value >= 49 && value <= 57) //1-9 选重
            {

                int num = value - 49 + 1;
                if (MB.ContainsKey(InputCode))
                {
                    if (Candi.Current.Count >= num)
                    {

                            CommitCandi(num - 1);
                    }
                    else
                    {
                        CommitCandi(0);
                        Send(num.ToString());

                    }
                }
                else
                {
                    BufferClear();
                }
                StateTrans(CompositionState.CnIdle);
                return KeyProccessed();
            }
            else if (value >= 65 && value <= 90) //a-z
            {
                string ch = az[value - 65];
                if (SHORT_SYMBOL)
                {
                    if (AutoShortSymbol.Contains(InputCode + ch))
                    {

                        AutoCommitCandi(ch);
                      //  InputCode += ch;
                     //   CommitCandi(0);
                        StateTrans(CompositionState.CnIdle);
                    }
                    else
                    {
                        BufferAdd(ch);
                    }

                }
                else if (Config.GetInt("自动上屏时间（毫秒）") == 0 && InputCode.Length >= Config.GetInt("最大码长")) //==4,顶字上屏
                {
                    CommitDing(ch);
                    StateTrans(CompositionState.CnComposing);

                }
                else if (Config.GetInt("自动上屏时间（毫秒）") > 0 || InputCode.Length < Config.GetInt("最大码长")) //
                {

                    if (Config.GetInt("自动上屏时间（毫秒）") ==0 && Config.GetBool("最大码长无重自动上屏") && InputCode.Length == Config.GetInt("最大码长") - 1 && MB.ContainsKey(InputCode + ch) && MB[InputCode + ch].Count == 1) //四码唯一上屏
                    {

                        AutoCommitCandi(ch);
                //       InputCode += ch;
                 //       CommitCandi(0);

                        StateTrans(CompositionState.CnIdle);
                    }
                    /*
                    else if (SHORT_SYMBOL)
                    {
                        if (MB.ContainsKey(InputCode + ch))
                        {
                            bool unique = true;

                            foreach (var m in MB)
                            {
                                if (m.Key.IndexOf(InputCode + ch)  == 0)
                                {
                                    if (m.Key != InputCode + ch || m.Value.Count > 1)
                                    {
                                        unique = false;
                                        break;
                                    }



                                }
                            }

                            if (unique)
                            {
                                InputCode += ch;
                                CommitCandi(0);
                                StateTrans(CompositionState.CnIdle);
                            }
                            else
                            {
                                BufferAdd(ch);
                            }

                        }
                        else
                        {
                            BufferAdd(ch);
                        }
                    }
                    */

                    else
                    {
                        BufferAdd(ch);
                    }
                }

                return KeyProccessed();
            }


            else if (value == 8) //backspace
            {

                BufferBack();

                if (InputCode == "")
                    StateTrans(CompositionState.CnIdle);


                return KeyProccessed();
            }
            else if (value == 0x0D) //enter
            {


                if (!Config.GetBool("回车清屏"))
                {
                    CommitCode();
                }
                else
                {
                    BufferClear();
                }


                StateTrans(CompositionState.CnIdle);
                return KeyProccessed();
            }
            else if (value == VK_TAB) //tab
            {


                if (Config.GetBool("TAB清屏"))
                {

                    BufferClear();
                    StateTrans(CompositionState.CnIdle);
                    return KeyProccessed();

                }
                else { return 0; }




            }
            else if (value == 0x1B) //Esc
            {




                BufferClear();
                StateTrans(CompositionState.CnIdle);

                return KeyProccessed();
            }
            else
            {

                return 0;
            }

        }

        private int ProcCnPinyin(int value) //中文有编码
        {




            if (Win32.GetKeyState(0x10) < 0 && ShiftCnSymbol.ContainsKey(value)) //大写符号
            {

                if (InputCode.Length > 0 && PYMB.ContainsKey(InputCode.Substring(1)))
                {
                    CommitCandi(0);
                    Send(Config.GetBool("中文状态下使用英文标点") ? ShiftEnSymbol[value] : ShiftCnSymbol[value]);
                }
                else
                {
                    BufferClear();
                }

                StateTrans(CompositionState.CnIdle);



                return KeyProccessed();
            }
            else if (value == VK_OEM_6 && NotShifted && Config.GetString("翻页键") == "[ ]" || value == VK_OEM_PLUS && NotShifted && Config.GetString("翻页键") == "- =" || value == VK_TAB && NotShifted && Config.GetString("翻页键") == "Shift Tab/Tab" || value == VK_NEXT && NotShifted && Config.GetString("翻页键") == "PageUp/PageDown")//下翻页
            {
                Candi.NextPage();
                winCandidate.Update();
                return KeyProccessed();

            }
            else if (value == VK_OEM_4 && NotShifted && Config.GetString("翻页键") == "[ ]" || value == VK_OEM_MINUS && NotShifted && Config.GetString("翻页键") == "- =" || value == VK_TAB && IsShifted && Config.GetString("翻页键") == "Shift Tab/Tab" || value == VK_PRIOR && NotShifted && Config.GetString("翻页键") == "PageUp/PageDown")//上翻页
            {
                Candi.PrevPage();
                winCandidate.Update();
                return KeyProccessed();
            }
            else if (Config.GetBool("`键拼音反查") && PYMB.Count > 0 && InputCode == "·" && value == VK_OEM_3) //双击`
            {


                Send("·");
                buffer_len = 0;
                BufferClear();
                StateTrans(CompositionState.CnIdle);
                return KeyProccessed();

            }
            else if (SHORT_SYMBOL_SEMICOLON && InputCode == ";" && (value == 0xBA || value == VK_SPACE && !MB.ContainsKey(";")))//双击快符
            {


                Send("；");
                buffer_len = 0;
                BufferClear();
                StateTrans(CompositionState.CnIdle);
                return KeyProccessed();

            }
            else if (SHORT_SYMBOL_SLASH && InputCode == "/" && (value == 0xBF || value == VK_SPACE && !MB.ContainsKey("/")))
            {


                Send("、");
                buffer_len = 0;
                BufferClear();

                StateTrans(CompositionState.CnIdle);
                return KeyProccessed();

            }
            else if (value == 32) //空格
            {

                CommitCandi(0);
                StateTrans(CompositionState.CnIdle);
                return KeyProccessed();
            }
            else if (Config.GetBool("分号次选") && value == 0xBA)// && MB.ContainsKey(InputCode) && MB[InputCode].Count >= 2) //分号次选
            {

                CommitCandi(1);

                StateTrans(CompositionState.CnIdle);
                return KeyProccessed();
            }
            else if (Config.GetBool("引号三选") && value == 0xDE)// && MB.ContainsKey(InputCode) && MB[InputCode].Count >= 3) //引号次选
            {

                CommitCandi(2);
                //     InputCode = "";

                StateTrans(CompositionState.CnIdle);
                return KeyProccessed();
            }

            else if (CnSymbol.ContainsKey(value)) //标点顶屏
            {


                if (InputCode.Length > 0 && PYMB.ContainsKey(InputCode.Substring(1)))
                {
                    CommitCandi(0);
                    Send(Config.GetBool("中文状态下使用英文标点") ? EnSymbol[value] : CnSymbol[value]);
                }
                else
                {
                    BufferClear();
                }

                StateTrans(CompositionState.CnIdle);



                return KeyProccessed();
            }
            else if (Win32.GetKeyState(0x10) < 0 && value == QUOT)
            {


                if (InputCode.Length > 0 && PYMB.ContainsKey(InputCode.Substring(1)))
                {
                    if (left_double)
                    {
                        CommitCandi(0);
                        Send("“");
                    }
                    else
                    {
                        CommitCandi(0);
                        Send("”");
                    }


                    left_double = !left_double;


                }
                else
                {
                    BufferClear();
                }


                StateTrans(CompositionState.CnIdle);




                return KeyProccessed();
            }
            else if (value == QUOT)
            {



                if (InputCode.Length > 0 && PYMB.ContainsKey(InputCode.Substring(1)))
                {
                    if (left_single)
                    {
                        CommitCandi(0);
                        Send("‘");

                    }
                    else
                    {
                        CommitCandi(0);
                        Send("’");
                    }



                    left_single = !left_single;


                }
                else
                {
                    BufferClear();
                }

                StateTrans(CompositionState.CnIdle);



                return KeyProccessed();
            }
            else if (value >= 49 && value <= 57) //1-9 选重
            {

                int num = value - 49 + 1;
                if (InputCode.Length > 0 && PYMB.ContainsKey(InputCode.Substring(1)))
                {
                    if (Candi.Current.Count >= num)
                    {
                        CommitCandi(num - 1);
                    }
                    else
                    {
                        CommitCandi(0);
                        Send(num.ToString());

                    }
                }
                else
                {
                    BufferClear();
                }
                StateTrans(CompositionState.CnIdle);
                return KeyProccessed();
            }
            else if (value >= 65 && value <= 90) //a-z
            {
                string ch = az[value - 65];



                BufferAdd(ch);


                return KeyProccessed();
            }
            else if (value == 8) //backspace
            {

                BufferBack();

                if (InputCode == "")
                    StateTrans(CompositionState.CnIdle);


                return KeyProccessed();
            }
            else if (value == 0x0D) //enter
            {


                if (!Config.GetBool("回车清屏"))
                {
                    CommitCode();
                }
                else
                {
                    BufferClear();
                }


                StateTrans(CompositionState.CnIdle);
                return KeyProccessed();
            }
            else if (value == VK_TAB) //tab
            {


                if (Config.GetBool("TAB清屏"))
                {

                    BufferClear();
                    StateTrans(CompositionState.CnIdle);
                    return KeyProccessed();

                }
                else { return 0; }




            }
            else if (value == 0x1B) //Esc
            {




                BufferClear();
                StateTrans(CompositionState.CnIdle);

                return KeyProccessed();
            }
            else
            {

                return 0;
            }

        }


        private void SendBack()
        {



            if (States.IsGdq)
                SimulateInputKey(VK_F16);



        }

        private int ProcEn(int value)
        {



            if (value == 32) //空格
            {

                SendHistory.Push(" ");
            }
            else if (Win32.GetKeyState(0x10) < 0 && ShiftEnSymbol.ContainsKey(value)) //shift符号
            {

                SendHistory.Push(ShiftEnSymbol[value]);



                return 0;
            }
            else if (CnSymbol.ContainsKey(value))
            {
                SendHistory.Push(EnSymbol[value]);


                return 0;
            }
            else if (value >= 49 && value <= 57) //1-9
            {

                int num = value - 49 + 1;

                SendHistory.Push(num.ToString());

                return 0;
            }
            else if (value >= 65 && value <= 90) //a-z
            {
                string ch = az[value - 65];
                if (Win32.GetKeyState(0x10) < 0)
                {
                    ch = ch.ToUpper();

                }


                SendHistory.Push(ch);

                return 0;
            }
            else if (value == 8) //backspace
            {




                return 0;
            }
            else if (value == 0x0D) //enter
            {



                SendHistory.Push("\n");

                return 0;
            }
            else if (value == 0x1B) //Esc
            {




                return 0;
            }
            else
            {

                return 0;
            }

            return 0;

        }

        private DateTime oldTime = DateTime.Now.AddSeconds(9999);

        private void BufferAdd_abed(string s) //auto space used
        {



            DateTime newTime = DateTime.Now;

            double seconds = (DateTime.Now - oldTime).TotalMilliseconds;

            double thresh = Config.GetInt("自动上屏时间（毫秒）");

            double tmp = 80;
            if (InputCode.Length >=1)
            {
                string last2 = InputCode.Substring(InputCode.Length -1) + s;
                if (AutoSpace.KeyPairs.ContainsKey(last2))
                    tmp = AutoSpace.KeyPairs[last2];
            }

            thresh = thresh * (1 + tmp / 25) * 0.4;
;

            if (InputCode.Length == 0 || seconds < thresh)
            {
           //     BufferAdd_old(s);
                oldTime = DateTime.Now;


            }
            else
            {
                CommitDing(s);
                oldTime = DateTime.Now;

                winCandidate.UpdatePosition();

            }

            if (Config.GetInt("自动上屏时间（毫秒）") > 0)
            {

                double time = Config.GetInt("自动上屏时间（毫秒）") * 10;



   //             AutoSpace.SetTimer(750, AutoCommitSpace);
            }

        }

        private void BufferAdd(string s)  //auto space not used
        {
            InputCode += s;
            if (Config.GetInt("自动上屏时间（毫秒）") > 0)
            {

                double time = Config.GetInt("自动上屏时间（毫秒）");

                bool isAlpha = true;

                if (!az.Contains(s))
                    return;

                for (int i = 0; i < InputCode.Length;i++)
                {
                   string  sub = InputCode.Substring(i, 1);
                    if (!az.Contains(sub))
                    {
                        return;
                    }
                }


                             AutoSpace.SetTimer((long)time, AutoCommitSpace);
            }


        }

        private void BufferBack()
        {

            SendBack();

            if (InputCode.Length >= 1)
                InputCode = InputCode.Substring(0, InputCode.Length - 1);


        }

        private void BufferClear()
        {

            SendBack();
            InputCode = "";
            if (Config.GetInt("自动上屏时间（毫秒）") > 0)
                AutoSpace.ClearTimer();

        }


        private void StateTrans(CompositionState s)
        {

            if (!Config.GetBool("嵌入式候选（娱乐）"))
                switch (s)
                {
                    case CompositionState.CnIdle:
                        winCandidate.HideCandidate();
                        break;
                    case CompositionState.CnComposing:

                        winCandidate.UpdatePosition();
                        break;
                    case CompositionState.CnUpperCase:
                        winCandidate.UpdatePosition();
                        break;
                    case CompositionState.CnPinyin:
                        winCandidate.UpdatePosition();
                        break;
                    case CompositionState.En:
                        winCandidate.HideCandidate();

                        break;
                    default:
                        break;
                }

            State = s;

            UpdateStatesBar();




        }

        public void UpdateStatesBar()
        {
            if (States.Off)
            {
                Disp.Text = "禁";
                Disp.FontSize = 20;
                Disp.Foreground = Brushes.Blue;
                Bd.BorderBrush = Brushes.Blue;
                Disp.FontFamily = new FontFamily("楷体");

                if (Config.GetBool("隐藏状态栏"))
                {
                    TaskBarInfo.ProgressState = TaskbarItemProgressState.Error;
                    this.Opacity = 0;
                }
                else
                {
                    this.Opacity = 1;


                    TaskBarInfo.ProgressState = TaskbarItemProgressState.None;
                }

            }
            else if (State == CompositionState.En)
            {


                Disp.Text = SendHistory.IsRecording ? "R" : "EN";
                Disp.FontSize = 16;
                Disp.Foreground = Brushes.Red;
                Bd.BorderBrush = Brushes.Red;
                Disp.FontFamily = new FontFamily("等线");
                if (Config.GetBool("隐藏状态栏"))
                {
                    TaskBarInfo.ProgressState = SendHistory.IsRecording ? TaskbarItemProgressState.Indeterminate : TaskbarItemProgressState.None;
                    this.Opacity = 0;
                }
                else
                {
                    this.Opacity = 1;
                    TaskBarInfo.ProgressState = TaskbarItemProgressState.None;
                }

            }
            else
            {
                Disp.FontFamily = new FontFamily("等线");
                Disp.Text = SendHistory.IsRecording ? "录" : "中";
                Disp.Foreground = Brushes.Black;
                Disp.FontSize = 20;
                Bd.BorderBrush = Brushes.Gray;
                if (Config.GetBool("隐藏状态栏"))
                {
                    this.Opacity = 0;
                    TaskBarInfo.ProgressState = SendHistory.IsRecording ? TaskbarItemProgressState.Indeterminate: TaskbarItemProgressState.Paused;

                }
                else
                {
                    this.Opacity = 1;
                    TaskBarInfo.ProgressState = TaskbarItemProgressState.None;
                }
            }
        }

        #region 快捷键
        private void Toggle(object sender, ExecutedRoutedEventArgs e)
        {

            switch (State)
            {
                case CompositionState.CnIdle:
                    StateTrans(CompositionState.En);
                    break;
                case CompositionState.En:
                    StateTrans(CompositionState.En);
                    break;
                case CompositionState.CnComposing:
                case CompositionState.CnUpperCase:
                    CommitCode();
                    StateTrans(CompositionState.En);
                    break;


                default:
                    break;

            }

        }
        #endregion

        #region UI操作





        private void msclose(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }




        private void msmove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();

            }
        }

        public static MainWindow Current
        {
            get
            {
                foreach (var s in App.Current.Windows)
                {
                    if (s is MainWindow)
                    {
                        return (MainWindow)s;

                    }

                }

                return null;
            }

        }

        public void ToggleCandidates()
        {
            Config.Set("隐藏候选", !Config.GetBool("隐藏候选"));


            winCandidate.Update();
            InitDisplay();
        }




        WinConfig winConfig;
        WinAddCi winAddCi;
        AutoStart autoStart = new AutoStart();
        public void ReloadCfg()
        {


            autoStart.SetMeAutoStart(Config.GetBool("开机自动启动"));
            this.ShowInTaskbar = Config.GetBool("任务栏显示");
            UpdateStatesBar();
            States.WhiteListClipBoard.Clear();

            var wl = Config.GetString("使用剪贴板上屏白名单").Trim().Split(new string[] { ",", "，" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var w in wl)
            {
                States.WhiteListClipBoard.Add(w);
            }

            States.maxHeight = 0;
            winCandidate.Update();
            winCandidate.UpdatePosition();
            MB.Clear();
            WrtiteReadme();

            InitMBList();











            InitDisplay();

        }
        #endregion
        public WinCandidate winCandidate = new WinCandidate();
        public MainWindow()
        {
            InitializeComponent();




            WrtiteReadme();









            Start();

        }

        private void InitMBList()
        {
            MB.Clear();
            string path = "mb";
            DirectoryInfo dir = new DirectoryInfo(path);

            States.MBPaths.Clear();

            foreach (var d in dir.GetDirectories())
            {
                States.MBPaths.Add(d.Name);
            }

            if (!States.MBPaths.Contains(Config.GetString("当前码表")))
                Config.Set("当前码表", States.MBPaths.First());


            //update menu

            MenuSchema.Items.Clear();
            winCandidate.FloatMenu.Items.Clear();

            MenuTheme.Items.Clear();
            winCandidate.FloatMenuTheme.Items.Clear();

            foreach (string th in Themes.ThemeList.Keys)
            {
                MenuItem mi = new MenuItem();

                mi.Header = th;
                mi.Click += MenuTheme_Click;
                mi.IsCheckable = true;

                if (th == Config.GetString("主题"))
                    mi.IsChecked = true;

                MenuTheme.Items.Add(mi);
            }

            foreach (string th in Themes.ThemeList.Keys)
            {
                MenuItem mi = new MenuItem();

                mi.Header = th;
                mi.Click += MenuTheme_Click;
                mi.IsCheckable = true;

                if (th == Config.GetString("主题"))
                    mi.IsChecked = true;

                winCandidate?.FloatMenuTheme.Items.Add(mi);
            }


            foreach (string mb in States.MBPaths)
            {
                MenuItem mi = new MenuItem();

                mi.Header = mb;
                mi.Click += MenuMB_Click;
                mi.IsCheckable = true;

                if (mb == Config.GetString("当前码表"))
                    mi.IsChecked = true;

                MenuSchema.Items.Add(mi);

            }

            foreach (string mb in States.MBPaths)
            {
                MenuItem mi = new MenuItem();

                mi.Header = mb;
                mi.Click += MenuMB_Click;
                mi.IsCheckable = true;

                if (mb == Config.GetString("当前码表"))
                    mi.IsChecked = true;

                winCandidate.FloatMenu.Items.Add(mi);
            }



            UpdateJumplist();


            ReadMB();
        }

        private void UpdateJumplist()
        {
            //jumplist

            JumpList jumpList = JumpList.GetJumpList(App.Current);


            jumpList.JumpItems.Clear();

            string folderPath = AppDomain.CurrentDomain.BaseDirectory + "mb\\" + Config.GetString("当前码表");

            var jt3 = new JumpTask();
            jt3.Title = "码表文件夹";
            jt3.CustomCategory = "设置";
            jt3.Arguments = folderPath;
            jt3.ApplicationPath = "explorer.exe";
            //   jt3.IconResourceIndex = 71;
            jt3.IconResourcePath = "explorer.exe";
            jumpList.JumpItems.Add(jt3);

            var jt1 = new JumpTask();
            jt1.Title = "重载码表";
            jt1.CustomCategory = "设置";
            jt1.Arguments = "reload";
            jt1.IconResourceIndex = 54;
            jt1.IconResourcePath = "shell32.dll";
            jumpList.JumpItems.Add(jt1);

            

            var jt5 = new JumpTask();
            jt5.Title = "加词";
            jt5.CustomCategory = "设置";
            jt5.Arguments = "addci";
            jt5.IconResourceIndex = 154;
            jt5.IconResourcePath = "shell32.dll";
            jumpList.JumpItems.Add(jt5);

            var jt2 = new JumpTask();
            jt2.Title = "设置";
            jt2.CustomCategory = "设置";
            jt2.Arguments = "config";
            jt2.IconResourceIndex = 71;
            jt2.IconResourcePath = "shell32.dll";
            jumpList.JumpItems.Add(jt2);

            int counter = 0;

            foreach (string mb in States.MBPaths)
            {
                counter++;
                if (counter >= 10)
                    break;
                JumpTask jt = new JumpTask();
                //Create a new Calculator JumpTask
                jt.Title = mb;
                jt.Description = "切换至" + mb;
                jt.Arguments = "mb_" + mb;
                if (mb == Config.GetString("当前码表"))
                    jt.IconResourceIndex = 43;
                else
                    jt.IconResourceIndex = 50;
                jt.IconResourcePath = "shell32.dll";
                jt.CustomCategory = "方案";


                jumpList.JumpItems.Add(jt);
            }

            
            jumpList.Apply();
        }
        private void func1()
        {
            MessageBox.Show("1");
        }

        private void func2()
        {
            MessageBox.Show("2");
        }

        private void MenuMB_Click(object sender, RoutedEventArgs e)
        {

            var mi = (MenuItem)sender;
            /*
            foreach (var mb in MenuSchema.Items)
            {
                var mbt = (MenuItem)mb;
                if (mbt.Header.ToString() != mi.Header.ToString())
                    mbt.IsChecked = false;
                else
                    mbt.IsChecked = true;
            }
   
            foreach (var mb in winCandidate.FloatMenu.Items)
            {
                var mbt = (MenuItem)mb;
                if (mbt.Header.ToString() != mi.Header.ToString())
                    mbt.IsChecked = false;
                else
                    mbt.IsChecked = true;
            }
            */
            ChangeMB(mi.Header.ToString());



        }

        private void MenuTheme_Click(object sender, RoutedEventArgs e)
        {

            var mi = (MenuItem)sender;
            /*
            foreach (var mb in MenuSchema.Items)
            {
                var mbt = (MenuItem)mb;
                if (mbt.Header.ToString() != mi.Header.ToString())
                    mbt.IsChecked = false;
                else
                    mbt.IsChecked = true;
            }
   
            foreach (var mb in winCandidate.FloatMenu.Items)
            {
                var mbt = (MenuItem)mb;
                if (mbt.Header.ToString() != mi.Header.ToString())
                    mbt.IsChecked = false;
                else
                    mbt.IsChecked = true;
            }
            */
            ChangeTheme(mi.Header.ToString());



        }

        public void ChangeTheme(string themeName)
        {
            if (themeName == Config.GetString("主题"))
                return;

            if (!Themes.ThemeList.ContainsKey(themeName))
                return;

            Config.Set("主题", themeName);

            winCandidate?.UpdateColor();

            foreach (var mb in MenuTheme.Items)
            {
                var mbt = (MenuItem)mb;
                if (mbt.Header.ToString() != themeName)
                    mbt.IsChecked = false;
                else
                    mbt.IsChecked = true;
            }

            foreach (var mb in winCandidate.FloatMenuTheme.Items)
            {
                var mbt = (MenuItem)mb;
                if (mbt.Header.ToString() != themeName)
                    mbt.IsChecked = false;
                else
                    mbt.IsChecked = true;
            }

        }


        public void ChangeMB(string mbName)
        {
            if (mbName == Config.GetString("当前码表"))
                return;

            if (!States.MBPaths.Contains(mbName))
                return;

            Config.Set("当前码表", mbName);
            ReadMB();

            UpdateJumplist();

            foreach (var mb in MenuSchema.Items)
            {
                var mbt = (MenuItem)mb;
                if (mbt.Header.ToString() != mbName)
                    mbt.IsChecked = false;
                else
                    mbt.IsChecked = true;
            }

            foreach (var mb in winCandidate.FloatMenu.Items)
            {
                var mbt = (MenuItem)mb;
                if (mbt.Header.ToString() != mbName)
                    mbt.IsChecked = false;
                else
                    mbt.IsChecked = true;
            }

        }

        private void InitDisplay()
        {


            winCandidate?.UpdateFonts();
            winCandidate?.UpdateColor();


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Config.SetDefault
            (
                "开机自动启动", "否",
                "任务栏显示", "是",
                "自动切换系统语言", "是",
                "隐藏状态栏", "否",
                "主题", "默认",
                "中文状态下使用英文标点", "否",
                "shift切换中英文", "是",
                "Ctrl+空格切换中英文", "是",
                "Ctrl+等号手动加词", "是",
                "回车清屏", "否",
                "TAB清屏", "是",
                "竖排候选", "是",
                "显示候选序号", "是",
                "`键拼音反查", "是",
                "显示注释", "是",
                "显示拆分", "否",
    //            "Ctrl+U开启录制", "否",
                "Alt+\\开启或禁用Bime", "否",
                //             "右Ctrl或右Alt开启/禁用Bime", "否",
                "每页候选个数", "5",
                "翻页键", "- =",
                "分号次选", "是",
                "引号三选", "是",
                "隐藏候选", "否",
                "编码伪装", "",
                "空码自动清屏", "是",
                "最大码长", "4",
                "最大码长无重自动上屏", "是",
                "自动上屏时间（毫秒）", "",
                "隐藏候选", "否",
                "字体", "#霞鹜文楷 GB 屏幕阅读版",
                "字体大小", "17",
                "使用剪贴板上屏", "否",
                 "使用剪贴板上屏白名单", "Pure Writer.exe,Notepad.exe",
                "嵌入式候选（娱乐）", "否",
                "开始菜单使用嵌入式候选", "是",
                "开启打字音效(娱乐)","否",
                "按键音量0~100","30"
            );

            Config.ReadConfig();

            var wl = Config.GetString("使用剪贴板上屏白名单").Trim().Split(new string[] { ",", "，" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var w in wl)
            {
                States.WhiteListClipBoard.Add(w);
            }

            States.maxHeight = 0;
            InitMBList();
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;
            InitDisplay();



            if(Config.GetBool("自动切换系统语言"))
                ImiHelper.ChangeLanguageDisabled();
            States.SwitchedToEn = true;

            tm1 = new Timer(UpdateCaretPos, null, 0, 200);
 
            try
            {
               vdm  = new VirtualDesktopManager();
               tm2 = new Timer(UpdateVirtualDesktop, null, 0, 1000);
            }
            catch (Exception)
            {

  
            }


            this.ShowInTaskbar = Config.GetBool("任务栏显示");
            UpdateStatesBar();

        }
        Timer tm1;

        Timer tm2;

        public void ShowWinAddCi()
        {
            if (WinAddCi.Current != null)
            {
                winAddCi.Focus();
                winAddCi.Activate();
                winAddCi.InitText();
            }
            else
            {
                winAddCi = new WinAddCi();
                winAddCi.Show();
                winAddCi.Activate();
                winAddCi.InitText();
            }
        }

        public void ShowConfigWin()
        {
            /*
            foreach (Window item in Application.Current.Windows)
            {
                if (item is WinConfig)
                {
                    item.Focus();
                    item.Activate();
                    return;
                }

            }*/



            if (WinConfig.Current != null)
            {
                winConfig.Focus();
                winConfig.Activate();
            }
            else
            {
                winConfig = new WinConfig();
                winConfig.Show();
                winConfig.Activate();
                UpdateJumplist();
            }


        }




        private void MenuConfig_Click(object sender, RoutedEventArgs e)
        {
            ShowConfigWin();

        }

        private void MenuFolder_Click(object sender, RoutedEventArgs e)
        {

            OpenMBFolder();
        }

        private void MenuOfficial_Click(object sender, RoutedEventArgs e)
        {

            OpenOfficial();
        }

        public void OpenOfficial()
        {
            Process p = new Process();
            p.StartInfo.FileName = "https://github.com/lvyww/bime";
            p.Start();
        }

        public void OpenMBFolder()
        {
            string folderPath = AppDomain.CurrentDomain.BaseDirectory + "mb\\" + Config.GetString("当前码表");
            Process.Start(folderPath);
        }

        public void ReloadMB()
        {
            this.Activate();
            MessageBox.Show("码表重新读取完成！");
            InitMBList();
            ReadMB();
            InitDisplay();
        }
        private void ReloadMB_Click(object sender, RoutedEventArgs e)
        {



            ReloadMB();

        }

        private void MenuAddCi_Click (object sender, RoutedEventArgs e)
        {



            ShowWinAddCi();

        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Closed(object sender, EventArgs e)
        {

            Application.Current.Shutdown();
        }




        VirtualDesktopManager vdm;


        IntPtr oldHwnd = IntPtr.Zero;
        public void UpdateCaretPos(object param)
        {

            /*
            if (Themes.DayNightChange())
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    States.UpdateColor = true;
                }));
            }
            */
            int x = 0;
            int y = 0;
            int wx = 0;
            int wy = 0;
            //  Thread.CurrentThread.SetApartmentState(ApartmentState.STA);
            if (States.Off || Config.GetBool("嵌入式候选（娱乐）"))
            { return; }
            IntPtr hwnd = Win32.GetForegroundWindow();

            if (hwnd != oldHwnd)
            {
 //               States.UpdateTopMost = true;
                States.UpdateDPI = true;
                oldHwnd = hwnd;
            }

            if (hwnd == IntPtr.Zero)
            {


      //          Dispatcher.Invoke(new Action(() =>
      //          {
                    States.CaretX = x;
                    States.CaretY = y;
                    States.WinX = wx;
                    States.WinY = wy;
        //        }));


                return;
            }
            var guiInfo = Win32.GetGuiThreadInfo(hwnd);

            if (guiInfo == null)
            {
        //        Dispatcher.Invoke(new Action(() =>
         //       {
                    States.CaretX = x;
                    States.CaretY = y;
                    States.WinX = wx;
                    States.WinY = wy;
          //      }));
                return;
            }


            IntPtr hwndf = guiInfo.Value.hwndFocus;
            RECT rect = new RECT();
            Win32.GetWindowRect(hwndf, ref rect);


            object obj = null;
            Guid guid = typeof(IAccessible).GUID;

            try
            {
                int retVal = Win32.AccessibleObjectFromWindow(guiInfo.Value.hwndFocus, Win32.CARET, ref guid, ref obj);


                if (obj != null)
                {
                    IAccessible ia = (IAccessible)obj;


                    ia.accLocation(out int l, out int t, out int w, out int h, 0);

                    x = l + w;
                    y = t + h;
                    wx = rect.left;
                    wy = rect.top;

                }
            }
            catch (Exception)
            {

                //               throw new ArgumentOutOfRangeException( e.Message,"bime down");
            }
            finally
            {
        //        Dispatcher.Invoke(new Action(() =>
        //        {
                    States.CaretX = x;
                    States.CaretY = y;
                    States.WinX = wx;
                    States.WinY = wy;
         //       }));
            }





            //    CoUninitialize();

        }


        
        public void UserAdd (string code, string name)
        {

            CustomItem c = new CustomItem();
            c.code = code;
            c.name = name;
            c.ops = CustomOps.Add;

            List<CustomItem> clist = new List<CustomItem>();
            clist.Add(c);
            ProcCustom(clist);


            if (MB.ContainsKey(InputCode))
                Candi.Update(InputCode, MB[InputCode]);
            else
                Candi.Update(InputCode, null);
            try
            {
                StreamWriter sw = new StreamWriter(States.CustomFile, true);
                sw.WriteLine("{添加}" + code + "\t" + WrapEnc( name));
                sw.Close();
            }
            catch (Exception)
            {

           
            }
            finally
            {

            }


        }

        public void UserDelete (string code, string name)
        {

            try
            {

                CustomItem c = new CustomItem();
                c.code = code;
                c.name = name;
                c.ops = CustomOps.Delete;

                List<CustomItem> clist = new List<CustomItem>();
                clist.Add(c);
                ProcCustom(clist);
                if (MB.ContainsKey(InputCode))
                    Candi.Update(InputCode, MB[InputCode]);
                else
                    Candi.Update(InputCode, null);

                

                StreamWriter sw = new StreamWriter(States.CustomFile,true);
                sw.WriteLine("{删除}"+ code + "\t" + WrapEnc( name));
                sw.Close();


            }
            catch (Exception)
            {

              
            }
            finally
            {

            }

        }

        public void UserTop(string code, string name)
        {

            try
            {

                CustomItem c = new CustomItem();
                c.code = code;
                c.name = name;
                c.ops = CustomOps.Top;

                List<CustomItem> clist = new List<CustomItem>();
                clist.Add(c);
                ProcCustom(clist);
                if (MB.ContainsKey(InputCode))
                    Candi.Update(InputCode, MB[InputCode]);
                else
                    Candi.Update(InputCode, null);

                StreamWriter sw = new StreamWriter(States.CustomFile, true);
                sw.WriteLine("{置顶}" + code + "\t" + WrapEnc( name));
                sw.Close();


            }
            catch (Exception)
            {


            }
            finally
            {

            }

        }
        public void UserAdvance(string code, string name)
        {

            try
            {

                CustomItem c = new CustomItem();
                c.code = code;
                c.name = name;
                c.ops = CustomOps.Advance;

                List<CustomItem> clist = new List<CustomItem>();
                clist.Add(c);
                ProcCustom(clist);
                if (MB.ContainsKey(InputCode))
                    Candi.Update(InputCode, MB[InputCode]);
                else
                    Candi.Update(InputCode, null);

                StreamWriter sw = new StreamWriter(States.CustomFile, true);
                sw.WriteLine("{前移}" + code + "\t" + WrapEnc(name));
                sw.Close();


            }
            catch (Exception)
            {


            }
            finally
            {

            }

        }
        string LastCode = "";
        string LastSend = "";
        DateTime LastTime = DateTime.Now;


        private double CapDang (double d)
        {
            d = (d) / 100;
            if (d < 0.8)
                d = 0.8;

            if (d > 1.35)
                d = 1.35;

            return d;
        }
        private bool RevertKey (int value)
        {
            DateTime newTime = DateTime.Now;

            double seconds = (DateTime.Now - LastTime).TotalMilliseconds + Config.GetInt("自动上屏时间（毫秒）");

            double thresh = Config.GetInt("自动上屏时间（毫秒）");

            double tmp = 80;
            double tmp2 = 1;

            string newChar = az[value - 65];


            if (LastCode.Length >= 1)
            {
                string last2 = LastCode.Substring(LastCode.Length - 1,1) + newChar;
                if (AutoSpace.KeyPairs.ContainsKey(last2))
                    tmp = AutoSpace.KeyPairs[last2];

                if (LastCode.Substring(LastCode.Length - 1, 1) == newChar)
                    tmp2 = 1.3;
            }

            double c1 = 1 + (tmp) / 200;



            //全局
            string fullCode = LastCode + newChar;
            double c3 = 1;
            switch (fullCode.Length)
            {
                case 4:

                    c3 = CapDang(AutoSpace.CalDang4(fullCode)) ;
                    break;
                case 3:
                    c3 = CapDang(1.25 * AutoSpace.CalDang3(fullCode));
                    break;
                case 2:
                    c3 = CapDang(3.0 * AutoSpace.CalDang2(fullCode));
                    break;
                default:
                    break;
            }
            double c2 = tmp2;

            double max = Math.Max(c1, c3);
            max = Math .Max(max, c2);

            thresh = thresh * max;
            

            Trace.WriteLine(seconds + " " + c1 + " " + c3 + " "+ thresh);


            if (seconds < thresh)
            {
                InputCode = LastCode;

                int backlen = new StringInfo(LastSend).LengthInTextElements;

                EmbedBack(backlen);

                for (int i = 0; i < backlen; i++)
                {
       //             SimulateInputKey(VK_BACK);

                }
                return true;
            }
            else
                return false;

            



        }


        private void EmbedBack(int back_len)
        {











            Skip = true;

            var numlock = Win32.GetKeyState(VK_NUMLOCK) > 0;



            int inputLen = 2+  back_len * 2 + (numlock ? 4 : 0);

            INPUT[] input = new INPUT[inputLen];


            int counter = 0;


            if (numlock)
            {

                input[counter].type = 1;//模拟键盘
                input[counter].mkhi.ki.wVk = VK_NUMLOCK;
                input[counter].mkhi.ki.dwFlags = 0;//按下
                counter++;
                input[counter].type = 1;//模拟键盘
                input[counter].mkhi.ki.wVk = VK_NUMLOCK;
                input[counter].mkhi.ki.dwFlags = 2;//抬起
                counter++;

            }


            //shift down
            {
                input[counter].type = 1;//模拟键盘
                input[counter].mkhi.ki.wVk = VK_LSHIFT;
                input[counter].mkhi.ki.dwFlags = 0;//抬起
                counter++;
            }


            //left
            for (int i = 0; i < back_len; i ++)
            {
                input[counter].type = 1;//模拟键盘
                input[counter].mkhi.ki.wVk = VK_LEFT;
                input[counter].mkhi.ki.dwFlags = 0;//按下
                counter++;
                input[counter].type = 1;//模拟键盘
                input[counter].mkhi.ki.wVk = VK_LEFT;
                input[counter].mkhi.ki.dwFlags = 2;//抬起
                counter++;
            }

            //shift up
            {
                input[counter].type = 1;//模拟键盘
                input[counter].mkhi.ki.wVk = VK_LSHIFT;
                input[counter].mkhi.ki.dwFlags = 2;//抬起
                counter++;
            }



            if (numlock)
            {
                input[counter].type = 1;//模拟键盘
                input[counter].mkhi.ki.wVk = VK_NUMLOCK;
                input[counter].mkhi.ki.dwFlags = 0;//按下
                counter++;
                input[counter].type = 1;//模拟键盘
                input[counter].mkhi.ki.wVk = VK_NUMLOCK;
                input[counter].mkhi.ki.dwFlags = 2;//抬起
                counter++;
            }


            Win32.SendInput((uint)inputLen, input, Marshal.SizeOf((object)default(INPUT)));

            Skip = false;

        }


        private void CommitCandiAuto(int pos)
        {
            LastCode = InputCode;
            LastTime = DateTime.Now;
            string toSend = "";
            if (Candi.Current.Count - 1 >= pos)
            {
                toSend = Candi.Current[pos];
            }

            InputCode = "";

            LastSend = toSend;

            if (toSend.Length > 0)
            {
                Send(toSend);

            }

            else
                SendBack();


        }
        public void AutoCommitSpace (object parma)
        {
            if (Config.GetInt("自动上屏时间（毫秒）") > 0)
                AutoSpace.ClearTimer();


                     Dispatcher.Invoke(new Action(() =>
                      {
                          switch (State)
                          {
                              case CompositionState.CnComposing:
                                  CommitCandiAuto(0);
                                  StateTrans(CompositionState.CnIdle);
                                  break;
                              default:
                                  break;
                          }
                      }));


        }

        public void UpdateVirtualDesktop(object parma)
        {

            IntPtr focusHwnd = Win32.GetForegroundWindow();
            if (focusHwnd != IntPtr.Zero && States.CandiHwnd != IntPtr.Zero)
            {
                try
                {
                    Guid? g = vdm.GetWindowDesktopId(focusHwnd);
                    if (g != null)
                    {

                        if (!vdm.IsWindowOnCurrentVirtualDesktop(States.CandiHwnd))
                            vdm.MoveWindowToDesktop(States.CandiHwnd, g.Value);

                    }
                }
                catch (Exception)
                {


                }




            }


        }


    }
}
