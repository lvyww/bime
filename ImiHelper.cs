using System;
using System.Runtime.InteropServices;

namespace bime
{
    internal static class ImiHelper
    {
        [DllImport("user32.dll")]
        private static extern bool PostMessage(int hhwnd, uint msg, IntPtr wparam, IntPtr lparam);
        [DllImport("user32.dll")]
        private static extern IntPtr LoadKeyboardLayout(string pwszKLID, uint Flags);
        private static uint WM_INPUTLANGCHANGEREQUEST = 0x0050;
        private static int HWND_BROADCAST = 0xffff;
        private static string en_US = "00000409"; //英文
        private static string cn_ZH = "00000804";
        private static uint KLF_ACTIVATE = 1;

        public static void ChangeLanguageDisabled()
        {
            try
            {
                PostMessage(HWND_BROADCAST, WM_INPUTLANGCHANGEREQUEST, IntPtr.Zero, LoadKeyboardLayout(en_US, KLF_ACTIVATE));

            }
            catch (Exception)
            {

       
            }

        }
        // 还原操作
        public static void ChangeLanguageEnabeled()
        {

            try
            {
                PostMessage(HWND_BROADCAST, WM_INPUTLANGCHANGEREQUEST, IntPtr.Zero, LoadKeyboardLayout(cn_ZH, KLF_ACTIVATE));

            }
            catch (Exception)
            {

            }
        }
    }

}
