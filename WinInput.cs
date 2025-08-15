using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BgMacro;
public class WinInput
{
    public const uint WM_MOUSEMOVE = 0x200;
    public const uint WM_SETCURSOR = 0x20;
    public const uint WM_LBUTTONDOWN = 0x201;
    public const uint WM_LBUTTONUP = 0x202;
    public const uint MK_LBUTTON = 0x0001;
    public const uint WM_INPUT = 0x00FF;
    public const uint WM_KEYDOWN = 0x0100;
    public const uint WM_KEYUP = 0x0101;
    public const uint WM_CHAR = 0x0102;
    public const uint WM_NCACTIVATE = 0x0086;
    public const uint WM_ACTIVATE = 0x0006;
    public const uint WM_ACTIVATEAPP = 0x001C;
    public const uint WM_KILLFOCUS = 0x0008;
    public const uint WM_IME_NOTIFY = 0x00;
    public const uint WM_SETFOCUS = 0x0007;

    public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr parameter);

    [StructLayout(LayoutKind.Sequential)]
    public struct RAWINPUT
    {
        /// <summary>
        /// Header for the data.yy
        /// </summary>
        public RAWINPUTHEADER Header;
        public Union Data;
        [StructLayout(LayoutKind.Explicit)]
        public struct Union
        {
            /// <summary>
            /// Mouse raw input data.
            /// </summary>
            [FieldOffset(0)]
            public RAWMOUSE Mouse;
            /// <summary>
            /// Keyboard raw input data.
            /// </summary>
            [FieldOffset(0)]
            public RAWKEYBOARD Keyboard;
            /// <summary00
            /// HID raw input data.
            /// </summary>
            [FieldOffset(0)]
            public RAWHID HID;
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct RAWMOUSE
    {
        [FieldOffset(0)]
        public ushort usFlags;
        [FieldOffset(4)]
        public uint ulButtons;
        [FieldOffset(4)]
        public ushort usButtonFlags;
        [FieldOffset(6)]
        public ushort usButtonData;
        [FieldOffset(8)]
        public uint ulRawButtons;
        [FieldOffset(12)]
        public int lLastX;
        [FieldOffset(16)]
        public int lLastY;
        [FieldOffset(20)]
        public uint ulExtraInformation;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RAWKEYBOARD
    {
        public ushort MakeCode;
        public ushort Flags;
        private readonly ushort Reserved;
        public ushort VKey;
        public uint Message;
        public ulong ExtraInformation;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RAWHID
    {
        public uint dwSizHid;
        public uint dwCount;
        public byte bRawData;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RAWINPUTHEADER
    {
        public uint dwType;
        public uint dwSize;
        public IntPtr hDevice;
        public IntPtr wParam;
    }

    public enum RawInputCommand
    {
        /// <summary>
        /// Get input data.
        /// </summary>
        Input = 0x10000003,
        /// <summary>
        /// Get header data.
        /// </summary>
        Header = 0x10000005
    }

    public const int KEYBOARD_OVERRUN_MAKE_CODE = 0xFF;
    public const int RI_KEY_BREAK = 0x01;
    public const int RIM_TYPEMOUSE = 0;
    public const int RIM_TYPEKEYBOARD = 1;
    public const int RIM_TYPEHID = 2;

    public const int MOUSE_MOVE_RELATIVE = 0x00;
    public const int MOUSE_MOVE_ABSOLUTE = 0x01;
    public const int MOUSE_VIRTUAL_DESKTOP = 0x02;
    public const int MOUSE_ATTRIBUTES_CHANGED = 0x04;
    public const int MOUSE_MOVE_NOCOALESCE = 0x08;


    public const int RI_MOUSE_LEFT_BUTTON_DOWN = 0x0001;
    public const int RI_MOUSE_LEFT_BUTTON_UP = 0x0002;


    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr PostMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool EnumChildWindows(IntPtr hwndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

    // Native Imports
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern int GetWindowTextLength(IntPtr hWnd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsWindowVisible(IntPtr hWnd);
}

public enum Keys
{
    VK_TAB = 0x0009,
    VK_0 = 0x0030,
    VK_1 = 0x0031,
    VK_2 = 0x0032,
    VK_3 = 0x0033,
    VK_4 = 0x0034,
    VK_5 = 0x0035,
    VK_6 = 0x0036,
    VK_7 = 0x0037,
    VK_8 = 0x0038,
    VK_9 = 0x0039,
    VK_A = 0x0041,
    VK_B = 0x0042,
    VK_C = 0x0043,
    VK_D = 0x0044,
    VK_E = 0x0045,
    VK_F = 0x0046,
    VK_G = 0x0047,
    VK_H = 0x0048,
    VK_I = 0x0049,
    VK_J = 0x004A,
    VK_K = 0x004B,
    VK_L = 0x004C,
    VK_M = 0x004D,
    VK_N = 0x004E,
    VK_O = 0x004F,
    VK_P = 0x0050,
    VK_Q = 0x0051,
    VK_R = 0x0052,
    VK_S = 0x0053,
    VK_T = 0x0054,
    VK_U = 0x0055,
    VK_V = 0x0056,
    VK_W = 0x0057,
    VK_X = 0x0058,
    VK_Y = 0x0059,
    VK_Z = 0x005A,
}


