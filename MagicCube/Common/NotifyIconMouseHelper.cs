//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Timers;
//using System.Windows;
//namespace MagicCube.Common
//{



//    /*托盘图标鼠标进入离开事件
//     */

//    public delegate void MouseEnterNotifyStatusChangedHandel(object sender, bool isEnter);

//    public class NotifyIconMouseHelper
//    {
//        #region win32类库

//        [Flags()]
//        public enum ProcessAccess : int
//        {
//            /// <summary>Specifies all possible access flags for the process object.</summary>
//            AllAccess =
//                CreateThread | DuplicateHandle | QueryInformation | SetInformation | Terminate | VMOperation | VMRead |
//                VMWrite | Synchronize,

//            /// <summary>Enables usage of the process handle in the CreateRemoteThread function to create a thread in the process.</summary>
//            CreateThread = 0x2,

//            /// <summary>Enables usage of the process handle as either the source or target process in the DuplicateHandle function to duplicate a handle.</summary>
//            DuplicateHandle = 0x40,

//            /// <summary>Enables usage of the process handle in the GetExitCodeProcess and GetPriorityClass functions to read information from the process object.</summary>
//            QueryInformation = 0x400,

//            /// <summary>Enables usage of the process handle in the SetPriorityClass function to set the priority class of the process.</summary>
//            SetInformation = 0x200,

//            /// <summary>Enables usage of the process handle in the TerminateProcess function to terminate the process.</summary>
//            Terminate = 0x1,

//            /// <summary>Enables usage of the process handle in the VirtualProtectEx and WriteProcessMemory functions to modify the virtual memory of the process.</summary>
//            VMOperation = 0x8,

//            /// <summary>Enables usage of the process handle in the ReadProcessMemory function to' read from the virtual memory of the process.</summary>
//            VMRead = 0x10,

//            /// <summary>Enables usage of the process handle in the WriteProcessMemory function to write to the virtual memory of the process.</summary>
//            VMWrite = 0x20,

//            /// <summary>Enables usage of the process handle in any of the wait functions to wait for the process to terminate.</summary>
//            Synchronize = 0x100000
//        }

//        [StructLayout(LayoutKind.Sequential)]
//        private struct TRAYDATA
//        {
//            public IntPtr hwnd;
//            public UInt32 uID;
//            public UInt32 uCallbackMessage;
//            public UInt32 bReserved0;
//            public UInt32 bReserved1;
//            public IntPtr hIcon;
//        }

//        [StructLayout(LayoutKind.Sequential, Pack = 1)]
//        public struct TBBUTTON
//        {
//            public int iBitmap;
//            public int idCommand;
//            public IntPtr fsStateStylePadding;
//            public IntPtr dwData;
//            public IntPtr iString;
//        }

//        [Flags]
//        public enum AllocationType
//        {
//            Commit = 0x1000,
//            Reserve = 0x2000,
//            Decommit = 0x4000,
//            Release = 0x8000,
//            Reset = 0x80000,
//            Physical = 0x400000,
//            TopDown = 0x100000,
//            WriteWatch = 0x200000,
//            LargePages = 0x20000000
//        }

//        [Flags]
//        public enum MemoryProtection
//        {
//            Execute = 0x10,
//            ExecuteRead = 0x20,
//            ExecuteReadWrite = 0x40,
//            ExecuteWriteCopy = 0x80,
//            NoAccess = 0x01,
//            ReadOnly = 0x02,
//            ReadWrite = 0x04,
//            WriteCopy = 0x08,
//            GuardModifierflag = 0x100,
//            NoCacheModifierflag = 0x200,
//            WriteCombineModifierflag = 0x400
//        }

//        [DllImport("user32.dll", SetLastError = true)]
//        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

//        [DllImport("user32.dll", SetLastError = true)]
//        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass,
//            string lpszWindow);

//        [DllImport("user32.dll", SetLastError = true)]
//        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

//        [DllImport("kernel32.dll")]
//        private static extern IntPtr OpenProcess(ProcessAccess dwDesiredAccess,
//            [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwProcessId);

//        [DllImport("user32.dll", CharSet = CharSet.Auto)]
//        private static extern UInt32 SendMessage(IntPtr hWnd, UInt32 Msg, UInt32 wParam, IntPtr lParam);

//        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
//        private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress,
//            int dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

//        [DllImport("kernel32.dll", SetLastError = true)]
//        private static extern bool ReadProcessMemory(
//            IntPtr hProcess,
//            IntPtr lpBaseAddress,
//            out TBBUTTON lpBuffer,
//            int dwSize,
//            out int lpNumberOfBytesRead
//            );

//        [DllImport("kernel32.dll", SetLastError = true)]
//        private static extern bool ReadProcessMemory(
//            IntPtr hProcess,
//            IntPtr lpBaseAddress,
//            out Rect lpBuffer,
//            int dwSize,
//            out int lpNumberOfBytesRead
//            );

//        [DllImport("kernel32.dll", SetLastError = true)]
//        private static extern bool ReadProcessMemory(
//            IntPtr hProcess,
//            IntPtr lpBaseAddress,
//            out TRAYDATA lpBuffer,
//            int dwSize,
//            out int lpNumberOfBytesRead
//            );

//        [DllImport("psapi.dll")]
//        private static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName,
//            [In] [MarshalAs(UnmanagedType.U4)] int nSize);

//        [Flags]
//        public enum FreeType
//        {
//            Decommit = 0x4000,
//            Release = 0x8000,
//        }

//        [DllImport("kernel32.dll")]
//        private static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, FreeType dwFreeType);

//        [DllImport("kernel32.dll")]
//        [return: MarshalAs(UnmanagedType.Bool)]
//        private static extern bool CloseHandle(IntPtr hObject);

//        [StructLayout(LayoutKind.Sequential)]
//        public struct POINT
//        {
//            public int X;
//            public int Y;

//            public POINT(int x, int y)
//            {
//                this.X = x;
//                this.Y = y;
//            }

//            public override string ToString()
//            {
//                return ("X:" + X + ", Y:" + Y);
//            }
//        }

//        [DllImport("user32")]
//        public static extern bool GetClientRect(
//            IntPtr hwnd,
//            out Rect lpRect
//            );

//        [DllImport("user32.dll", CharSet = CharSet.Auto)]
//        public static extern bool GetCursorPos(out POINT pt);


//        [StructLayout(LayoutKind.Sequential)]
//        public struct Rect
//        {
//            public int Left;
//            public int Top;
//            public int Right;
//            public int Bottom;
//        }

//        [DllImport("user32.dll")]
//        private static extern int GetWindowRect(IntPtr hwnd, out Rect lpRect);

//        public const int WM_USER = 0x0400;
//        public const int TB_BUTTONCOUNT = WM_USER + 24;
//        public const int TB_GETBUTTON = WM_USER + 23;
//        public const int TB_GETBUTTONINFOW = WM_USER + 63;
//        public const int TB_GETITEMRECT = WM_USER + 29;

//        #endregion

//        #region 检测托盘图标相对于屏幕位置

//        private bool FindNotifyIcon(ref Rect rect)
//        {
//            Rect rectNotify = new Rect();
//            IntPtr hTrayWnd = FindTrayToolbarWindow(); //找到托盘窗口句柄
//            var isTrue = FindNotifyIcon(hTrayWnd, ref rectNotify);
//            if (isTrue == false)
//            {
//                hTrayWnd = FindTrayToolbarOverFlowWindow(); //找到托盘窗口句柄
//                isTrue = FindNotifyIcon(hTrayWnd, ref rectNotify);
//            }
//            rect = rectNotify;
//            return isTrue;
//        }

//        private IntPtr FindTrayToolbarWindow()
//        {
//            IntPtr hWnd = FindWindow("Shell_TrayWnd", null);
//            if (hWnd != IntPtr.Zero)
//            {
//                hWnd = FindWindowEx(hWnd, IntPtr.Zero, "TrayNotifyWnd", null);
//                if (hWnd != IntPtr.Zero)
//                {

//                    hWnd = FindWindowEx(hWnd, IntPtr.Zero, "SysPager", null);
//                    if (hWnd != IntPtr.Zero)
//                    {
//                        hWnd = FindWindowEx(hWnd, IntPtr.Zero, "ToolbarWindow32", null);

//                    }
//                }
//            }
//            return hWnd;
//        }

//        private IntPtr FindTrayToolbarOverFlowWindow()
//        {
//            IntPtr hWnd = FindWindow("NotifyIconOverflowWindow", null);
//            if (hWnd != IntPtr.Zero)
//            {
//                hWnd = FindWindowEx(hWnd, IntPtr.Zero, "ToolbarWindow32", null);
//            }
//            return hWnd;
//        }

//        private bool FindNotifyIcon(IntPtr hTrayWnd, ref Rect rectNotify)
//        {
//            UInt32 trayPid = 0;
//            Rect rectTray = new Rect();
//            GetWindowRect(hTrayWnd, out rectTray);
//            int count = (int)SendMessage(hTrayWnd, TB_BUTTONCOUNT, 0, IntPtr.Zero); //给托盘窗口发消息，得到托盘里图标

//            bool isFind = false;
//            if (count > 0)
//            {
//                GetWindowThreadProcessId(hTrayWnd, out trayPid); //取得托盘窗口对应的进程id
//                //获取托盘图标的位置


//                IntPtr hProcess = OpenProcess(ProcessAccess.VMOperation | ProcessAccess.VMRead | ProcessAccess.VMWrite,
//                    false, trayPid); //打开进程，取得进程句柄

//                IntPtr address = VirtualAllocEx(hProcess, //在目标进程中申请一块内存，放TBBUTTON信息
//                    IntPtr.Zero,
//                    1024,
//                    AllocationType.Commit,
//                    MemoryProtection.ReadWrite);


//                TBBUTTON btnData = new TBBUTTON();
//                TRAYDATA trayData = new TRAYDATA();

//                //  Console.WriteLine("Count:"+count);

//                var handel = Process.GetCurrentProcess().Id;
//                //  Console.WriteLine("curHandel:" + handel);
//                for (uint j = 0; j < count; j++)
//                {
//                    //   Console.WriteLine("j:"+j);
//                    var i = j;
//                    SendMessage(hTrayWnd, TB_GETBUTTON, i, address); //取得TBBUTTON结构到本地
//                    int iTmp = 0;
//                    var isTrue = ReadProcessMemory(hProcess,
//                        address,
//                        out btnData,
//                        Marshal.SizeOf(btnData),
//                        out iTmp);
//                    if (isTrue == false) continue;
//                    //这一步至关重要，不能省略
//                    //主要解决64位系统电脑运行的是x86的程序
//                    if (btnData.dwData == IntPtr.Zero)
//                    {
//                        btnData.dwData = btnData.iString;
//                    }
//                    ReadProcessMemory(hProcess, //从目标进程address处存放的是TBBUTTON
//                        btnData.dwData, //取dwData字段指向的TRAYDATA结构
//                        out trayData,
//                        Marshal.SizeOf(trayData),
//                        out iTmp);

//                    UInt32 dwProcessId = 0;
//                    GetWindowThreadProcessId(trayData.hwnd, //通过TRAYDATA里的hwnd字段取得本图标的进程id
//                        out dwProcessId);
//                    //获取当前进程id
//                    //  StringBuilder sb = new StringBuilder(256);
//                    //  GetModuleFileNameEx(OpenProcess(ProcessAccess.AllAccess, false, dwProcessId), IntPtr.Zero, sb, 256);
//                    //  Console.WriteLine(sb.ToString());
//                    if (dwProcessId == (UInt32)handel)
//                    {

//                        Rect rect = new Rect();
//                        IntPtr lngRect = VirtualAllocEx(hProcess, //在目标进程中申请一块内存，放TBBUTTON信息
//                            IntPtr.Zero,
//                            Marshal.SizeOf(typeof(Rect)),
//                            AllocationType.Commit,
//                            MemoryProtection.ReadWrite);
//                        i = j;
//                        SendMessage(hTrayWnd, TB_GETITEMRECT, i, lngRect);
//                        isTrue = ReadProcessMemory(hProcess, lngRect, out rect, Marshal.SizeOf(rect), out iTmp);

//                        //释放内存
//                        VirtualFreeEx(hProcess, lngRect, Marshal.SizeOf(rect), FreeType.Decommit);
//                        VirtualFreeEx(hProcess, lngRect, 0, FreeType.Release);

//                        int left = rectTray.Left + rect.Left;
//                        int top = rectTray.Top + rect.Top;
//                        int botton = rectTray.Top + rect.Bottom;
//                        int right = rectTray.Left + rect.Right;
//                        rectNotify = new Rect();
//                        rectNotify.Left = left;
//                        rectNotify.Right = right;
//                        rectNotify.Top = top;
//                        rectNotify.Bottom = botton;
//                        isFind = true;
//                        break;
//                    }
//                }
//                VirtualFreeEx(hProcess, address, 0x4096, FreeType.Decommit);
//                VirtualFreeEx(hProcess, address, 0, FreeType.Release);
//                CloseHandle(hProcess);
//            }
//            return isFind;
//        }

//        #endregion


//        public MouseEnterNotifyStatusChangedHandel MouseEnterNotifyStatusChanged;
//        private object moveObject = new object();
//        private bool isOver = false;
//        private Timer timer = null;

//        public void MouseEnter()
//        {
//            lock (moveObject)
//            {
//                if (isOver) return;

//                //加载鼠标进入事件
//                MouseEnter(true);
//                CreateCheckTimer();
//                timer.Enabled = true;
//            }
//        }

//        private void CreateCheckTimer()
//        {
//            if (timer != null) return;
//            timer = new Timer();
//            timer.Interval = 120;
//            timer.Elapsed += TimerOnElapsed;
//        }

//        private void TimerOnElapsed(object sender, ElapsedEventArgs arg)
//        {
//            //300毫秒检测一次
//            //判断鼠标是否在托盘图标内
//            //如果在，那么就不管
//            //如果不在，就加载鼠标离开事件,同时停止timer
//            var isEnter = CheckMouseIsEnter();
//            if (isEnter) return;
//            timer.Enabled = false;
//            MouseEnter(false);
//        }

//        private void MouseEnter(bool isEnter)
//        {
//            isOver = isEnter;
//            if (MouseEnterNotifyStatusChanged == null) return;
//            MouseEnterNotifyStatusChanged(this, isEnter);
//        }

//        public Point Point { get; set; }

//        private bool CheckMouseIsEnter()
//        {
//            //这里怎么检测呢
//            //我很无语啊 
//            //第一步：获取当前鼠标的坐标
//            //第二步：获取托盘图标的坐标
//            //   ???? 难难难难难难难难难难
//            try
//            {

//                Rect rectNotify = new Rect();
//                var isTrue = FindNotifyIcon(ref rectNotify);
//                if (isTrue == false) return false;
//                POINT point = new POINT();
//                GetCursorPos(out point);
//                //            Console.WriteLine(string.Format(@"
//                //Left={0} Top={1} Right={2} Bottom={3}", rectNotify.Left, rectNotify.Top, rectNotify.Right, rectNotify.Bottom));
//                //            Console.WriteLine(point.X + "  " + point.Y);
//                //第三步：比较鼠标图标是否在托盘图标的范围内
//                if (point.X >= rectNotify.Left && point.X <= rectNotify.Right &&
//                    point.Y >= rectNotify.Top && point.Y <= rectNotify.Bottom)
//                {
//                    Point = new Point(point.X, point.Y);
//                    return true;
//                }
//                else
//                {
//                    return false;
//                }
//            }
//            catch (Exception)
//            {
//                return false;
//            }
//        }
//    }

//}
