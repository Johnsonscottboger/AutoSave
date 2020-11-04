using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoSave
{
    public class SendInputUtil
    {
        [DllImport("user32.dll")]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("kernel32.dll")]
        private static extern int GetTickCount();

        private const int INPUT_KEYBOARD = 1;

        private const int KEYEVENTF_KEYUP = 0x0002;

        private const int VK_CONTROL = 0x11;

        private const int VK_S = 0x53;

        public static void CtrlS()
        {
            var input = new INPUT[4];
            input[0].type = input[1].type = input[2].type = input[3].type = INPUT_KEYBOARD;
            input[0].ki.wVk = input[3].ki.wVk = VK_CONTROL;
            input[1].ki.wVk = input[2].ki.wVk = VK_S;
            input[2].ki.dwFlags = input[3].ki.dwFlags = KEYEVENTF_KEYUP;
            SendInput(4, input, Marshal.SizeOf((object)default(INPUT)));
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct INPUT
    {
        [FieldOffset(0)]
        public Int32 type;
        [FieldOffset(4)]
        public KEYBDINPUT ki;
        [FieldOffset(4)]
        public MOUSEINPUT mi;
        [FieldOffset(4)]
        public HARDWAREINPUT hi;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MOUSEINPUT
    {
        public Int32 dx;
        public Int32 dy;
        public Int32 mouseData;
        public Int32 dwFlags;
        public Int32 time;
        public IntPtr dwExtraInfo;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct KEYBDINPUT
    {
        public Int16 wVk;
        public Int16 wScan;
        public Int32 dwFlags;
        public Int32 time;
        public IntPtr dwExtraInfo;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct HARDWAREINPUT
    {
        public Int32 uMsg;
        public Int16 wParamL;
        public Int16 wParamH;
    }
}
