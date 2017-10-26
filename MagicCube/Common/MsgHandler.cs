using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MagicCube.Common
{
    enum XPMsg
    {
        XPVIDEO_LOGIN = 0x0500 + 2120,
        XPVIDEO_LOGOUT,
        XPVIDEO_ENTERROME,
        XPVIDEO_EXITROOM,
        XP_EXIT
    };
    /// <summary>
    /// Send message handler class.
    /// Call the method "SendMessageToTargetWindow" to send 
    /// the message what we want.
    /// </summary>
    public class MsgHandler
    {
        /// <summary>
        /// System defined message
        /// </summary>
        private const int WM_COPYDATA = 0x004A;

        /// <summary>
        /// User defined message
        /// </summary>
        private const int WM_DATA_TRANSFER = 0x0437;

        // FindWindow method, using Windows API
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern int FindWindow(string lpClassName, string lpWindowName);

        // IsWindow method, using Windows API
        [DllImport("User32.dll", EntryPoint = "IsWindow")]
        private static extern bool IsWindow(int hWnd);

        // SendMessage method, using Windows API
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(
            int hWnd,                   // handle to destination window
            int Msg,                    // message
            int wParam,                 // first message parameter
            ref COPYDATASTRUCT lParam   // second message parameter
        );

        // SendMessage method, using Windows API
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(
            int hWnd,                   // handle to destination window
            int Msg,                    // message
            int wParam,                 // first message parameter
            string lParam               // second message parameter
        );

        /// <summary>
        /// CopyDataStruct
        /// </summary>
        private struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            public IntPtr lpData;
        }

        /// <summary>
        /// Send message to target window
        /// </summary>
        /// <param name="wndName">The window name which we want to found</param>
        /// <param name="msg">The message to be sent, string</param>
        /// <returns>success or not</returns>
        public static bool SendMessageToTargetWindow(string wndName, string msg)
        {
            //Debug.WriteLine(string.Format("SendMessageToTargetWindow: Send message to target window {0}: {1}", wndName, msg));

            int iHWnd = FindWindow(null, wndName);
            if (iHWnd == 0)
            {
                string strError = string.Format("SendMessageToTargetWindow: The target window [{0}] was not found!", wndName);
                //MessageBox.Show(strError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                byte[] bytData = null;
                bytData = Encoding.Default.GetBytes(msg);

                COPYDATASTRUCT cdsBuffer;
                cdsBuffer.dwData = (IntPtr)100;
                cdsBuffer.cbData = bytData.Length;
                cdsBuffer.lpData = Marshal.AllocHGlobal(bytData.Length);
                Marshal.Copy(bytData, 0, cdsBuffer.lpData, bytData.Length);

                // Use system defined message WM_COPYDATA to send message.
                int iReturn = SendMessage(iHWnd, WM_COPYDATA, 0, ref cdsBuffer);
                if (iReturn < 0)
                {
                    string strError = string.Format("SendMessageToTargetWindow: Send message to the target window [{0}] failed!", wndName);
                   // MessageBox.Show(strError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //Debug.WriteLine(strError);
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Send message to target window
        /// </summary>
        /// <param name="wndName">The window name which we want to found</param>
        /// <param name="wParam">first parameter, integer</param>
        /// <param name="lParam">second parameter, string</param>
        /// <returns>success or not</returns>
        public static bool SendMessageToTargetWindow(string wndName, int wParam, string lParam)
        {
            //Debug.WriteLine(string.Format("SendMessageToTargetWindow: Send message to target window {0}: wParam:{1}, lParam:{2}", wndName, wParam, lParam));

            int iHWnd = FindWindow(null, wndName);
            if (iHWnd == 0)
            {
                string strError = string.Format("SendMessageToTargetWindow: The target window [{0}] was not found!", wndName);
                //MessageBox.Show(strError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Debug.WriteLine(strError);
                return false;
            }
            else
            {
                // Use our defined message WM_DATA_TRANSFER to send message.
                int iReturn = SendMessage(iHWnd, WM_DATA_TRANSFER, wParam, lParam);
                if (iReturn < 0)
                {
                    string strError = string.Format("SendMessageToTargetWindow: Send message to the target window [{0}] failed!", wndName);
                    //MessageBox.Show(strError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //Debug.WriteLine(strError);
                    return false;
                }

                return true;
            }
        }
    }

    public class MessageHelper
    {
        public const int WM_DOWNLOAD_COMPLETED = 0x00AA;
        public const int WM_LOGIN = 0x0078;
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr wnd, int msg, IntPtr wP, IntPtr lP);
        //private static extern int SendMessage
        //(
        //    IntPtr hWnd,    //目标窗体句柄
        //    int Msg,        //WM_COPYDATA
        //    int wParam,     //自定义数值
        //    ref  CopyDataStruct lParam //结构体
        //);

        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(IntPtr wnd, int msg, IntPtr wP, IntPtr lP);

       
    }

   
}
