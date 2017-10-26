using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ANYCHATAPI;
using System.Windows;
using System.Windows.Controls;

namespace MagicCube.Util
{
    public class VideoUtil
    {

        // 视频消息定义
        public const int WM_GV_CONNECT = AnyChatCoreSDK.WM_GV_CONNECT;	// 客户端连接服务器，wParam（BOOL）表示是否连接成功
        public const int WM_GV_LOGINSYSTEM = AnyChatCoreSDK.WM_GV_LOGINSYSTEM;	// 客户端登录系统，wParam（INT）表示自己的用户ID号，lParam（INT）表示登录结果：0 成功，否则为出错代码，参考出错代码定义
        public const int WM_GV_ENTERROOM = AnyChatCoreSDK.WM_GV_ENTERROOM;	// 客户端进入房间，wParam（INT）表示所进入房间的ID号，lParam（INT）表示是否进入房间：0成功进入，否则为出错代码
        public const int WM_GV_MICSTATECHANGE = AnyChatCoreSDK.WM_GV_MICSTATECHANGE;	// 用户的音频设备状态变化消息，wParam（INT）表示用户ID号，lParam（BOOL）表示该用户是否已打开音频采集设备
        public const int WM_GV_USERATROOM = AnyChatCoreSDK.WM_GV_USERATROOM;	// 用户进入（离开）房间，wParam（INT）表示用户ID号，lParam（BOOL）表示该用户是进入（TRUE）或离开（FALSE）房间
        public const int WM_GV_LINKCLOSE = AnyChatCoreSDK.WM_GV_LINKCLOSE;	// 网络连接已关闭，该消息只有在客户端连接服务器成功之后，网络异常中断之时触发，wParam（INT）表示连接断开的原因
        public const int WM_GV_ONLINEUSER = AnyChatCoreSDK.WM_GV_ONLINEUSER;	// 收到当前房间的在线用户信息，进入房间后触发一次，wParam（INT）表示在线用户数（包含自己），lParam（INT）表示房间ID
        public const int WM_GV_FORTUNEMENU = AnyChatCoreSDK.WM_GV_FORTUNEMENU;	// 用户选择了一项财富菜单项，wParam（INT）表示用户ID号，lParam（INT）表示财富菜单标记，指示是选择了哪一项菜单
        public const int WM_GV_ROOMWAITQUEUE = AnyChatCoreSDK.WM_GV_ROOMWAITQUEUE;	// 用户收到当前房间等待队列消息，wParam（INT）表示用户前面的队列长度，lParam（INT）表示当前房间总的等待队列长度
        public const int WM_GV_ENTERREQUEST = AnyChatCoreSDK.WM_GV_ENTERREQUEST;	// 用户申请进入房间消息，wParam（INT）表示用户ID号，lParam（BOOL）表示该用户是申请进入（TRUE）房间或离开（FALSE）房间等待队列
        public const int WM_GV_CAMERASTATE = AnyChatCoreSDK.WM_GV_CAMERASTATE;	// 用户摄像头状态发生变化，wParam（INT）表示用户ID号，lParam（INT）表示摄像头的当前状态，定义为：GV_CAMERA_STATE_XXXX
        public const int WM_GV_CHATMODECHG = AnyChatCoreSDK.WM_GV_CHATMODECHG;	// 用户聊天模式发生变化，wParam（INT）表示用户ID号，lParam（INT）表示用户的当前聊天模式
        public const int WM_GV_ACTIVESTATE = AnyChatCoreSDK.WM_GV_ACTIVESTATE;	// 用户活动状态发生变化，wParam（INT）表示用户ID号，lParam（INT）表示用户的当前活动状态
        public const int WM_GV_P2PCONNECTSTATE = AnyChatCoreSDK.WM_GV_P2PCONNECTSTATE;	// 本地用户与其它用户的P2P网络连接状态发生变化，wParam（INT）表示其它用户ID号，lParam（INT）表示本地用户与其它用户的当前P2P网络连接状态
        public const int WM_GV_VIDEOSIZECHG = AnyChatCoreSDK.WM_GV_VIDEOSIZECHG;	// 用户视频分辩率发生变化，wParam（INT）表示用户ID号，lParam（INT）表示用户的视频分辨率组合值（低16位表示宽度，高16位表示高度）
        public const int WM_GV_USERINFOUPDATE = AnyChatCoreSDK.WM_GV_USERINFOUPDATE;	// 用户信息更新通知，wParam（INT）表示用户ID号，lParam（INT）表示更新类别
        public const int WM_GV_FRIENDSTATUS = AnyChatCoreSDK.WM_GV_FRIENDSTATUS;	// 好友在线状态变化，wParam（INT）表示好友用户ID号，lParam（INT）表示用户的当前活动状态：0 离线， 1 上线
        public const int WM_GV_PRIVATEREQUEST = AnyChatCoreSDK.WM_GV_PRIVATEREQUEST;	// 用户发起私聊请求，wParam（INT）表示发起者的用户ID号，lParam（INT）表示私聊请求编号，标识该请求
        public const int WM_GV_PRIVATEECHO = AnyChatCoreSDK.WM_GV_PRIVATEECHO;	// 用户回复私聊请求，wParam（INT）表示回复者的用户ID号，lParam（INT）为出错代码
        public const int WM_GV_PRIVATEEXIT = AnyChatCoreSDK.WM_GV_PRIVATEEXIT;	// 用户退出私聊，wParam（INT）表示退出者的用户ID号，lParam（INT）为出错代码
        public const int WM_GV_EXTENDBTNPRESS = AnyChatCoreSDK.WM_GV_EXTENDBTNPRESS;	// 用户按下扩展按钮，wParam（INT）表示按钮所对应的用户ID号，lParam（DWORD）指示按钮（左下角）所在屏幕位置(x,y)，用户可以利用该参数显示菜单等
        public const int WM_GV_SDKWARNING = AnyChatCoreSDK.WM_GV_SDKWARNING;	// SDK警告信息，当SDK在运行过程中自检发现异常状态时，将向上层发送该消息，wParam（INT）表示警告代码，定义为：GV_ERR_WARNING_XXXX

        public const int BRAC_VIDEOCALL_EVENT_REQUEST = AnyChatCoreSDK.BRAC_VIDEOCALL_EVENT_REQUEST;	// 呼叫请求
        public const int BRAC_VIDEOCALL_EVENT_REPLY = AnyChatCoreSDK.BRAC_VIDEOCALL_EVENT_REPLY;	// 呼叫请求回复
        public const int BRAC_VIDEOCALL_EVENT_START = AnyChatCoreSDK.BRAC_VIDEOCALL_EVENT_START;	// 视频呼叫会话开始事件
        public const int BRAC_VIDEOCALL_EVENT_FINISH = AnyChatCoreSDK.BRAC_VIDEOCALL_EVENT_FINISH;	// 挂断（结束）呼叫会话

        public const int AC_ERROR_VIDEOCALL_CANCEL = 100101;       //  源用户主动放弃会话
        public const int AC_ERROR_VIDEOCALL_OFFLINE = 100102;      // 目标用户不在线
        public const int AC_ERROR_VIDEOCALL_BUSY = 100103;       /// 目标用户忙
        public const int AC_ERROR_VIDEOCALL_REJECT = 100104;       // 目标用户拒绝会话
        public const int AC_ERROR_VIDEOCALL_TIMEOUT = 100105;       // 会话请求超时
        public const int AC_ERROR_VIDEOCALL_DISCONNECT = 100106;       // 网络断线

        private static VideoUtil instance = null;

        public const int LOCAL_USERID = -1;

        #region AnyChat内核区，单例区，不可修改
        // 回调句柄定义
        private AnyChatCoreSDK.NotifyMessage_CallBack OnNotifyMessageCallback = null;
        private AnyChatCoreSDK.VideoCallEvent_CallBack OnVideoCallEventCallBack = null;

        //static AnyChatCoreSDK.VideoData_CallBack OnVideoDataCallback = new
        //    AnyChatCoreSDK.VideoData_CallBack(VideoData_CallBack);

        // 委托句柄定义
        //public static AnyChatCoreSDK.NotifyMessage_CallBack NotifyMessageHandler = null;
        //public static AnyChatCoreSDK.VideoData_CallBack VideoDataHandler = null;
        private AnyChatCoreSDK.NotifyMessage_CallBack NotifyMessageHandler = null;
        private AnyChatCoreSDK.VideoCallEvent_CallBack VideoCallEventHandler = null;

        private VideoUtil()
        {
            OnNotifyMessageCallback = new AnyChatCoreSDK.NotifyMessage_CallBack(NotifyMessage_CallBack);
            OnVideoCallEventCallBack = new AnyChatCoreSDK.VideoCallEvent_CallBack(VideoCallEvent_CallBack);
            // 设置回调函数
            AnyChatCoreSDK.SetNotifyMessageCallBack(OnNotifyMessageCallback, 0);
            AnyChatCoreSDK.SetVideoCallEventCallBack(OnVideoCallEventCallBack, 0);
            //AnyChatCoreSDK.SetVideoDataCallBack(AnyChatCoreSDK.PixelFormat.BRAC_PIX_FMT_RGB24, OnVideoDataCallback, 0);

            ulong dwFuncMode = /*AnyChatCoreSDK.BRAC_FUNC_VIDEO_CBDATA | */AnyChatCoreSDK.BRAC_FUNC_VIDEO_AUTODISP | AnyChatCoreSDK.BRAC_FUNC_AUDIO_AUTOPLAY | AnyChatCoreSDK.BRAC_FUNC_CHKDEPENDMODULE
                | AnyChatCoreSDK.BRAC_FUNC_AUDIO_VOLUMECALC | AnyChatCoreSDK.BRAC_FUNC_NET_SUPPORTUPNP | AnyChatCoreSDK.BRAC_FUNC_FIREWALL_OPEN
                | AnyChatCoreSDK.BRAC_FUNC_AUDIO_AUTOVOLUME | AnyChatCoreSDK.BRAC_FUNC_CONFIG_LOCALINI;

            // 初始化SDK
            AnyChatCoreSDK.InitSDK(IntPtr.Zero, dwFuncMode);

            //NotifyMessageHandler = new AnyChatCoreSDK.NotifyMessage_CallBack(NotifyMessageCallbackDelegate);
            //VideoDataHandler = new AnyChatCoreSDK.VideoData_CallBack(VideoDataCallbackDelegate);
        }

        public static VideoUtil GetInstance()
        {
            if (instance == null)
            {
                instance = new VideoUtil();
            }

            return instance;
        }

        // AnyChat 内核回调，不能操作界面
        public void NotifyMessage_CallBack(int dwNotifyMsg, int wParam, int lParam, int userValue)
        {
            if (NotifyMessageHandler != null)
                NotifyMessageHandler(dwNotifyMsg, wParam, lParam, userValue);
        }
        // AnyChat 内核回调，不能操作界面
        public void VideoCallEvent_CallBack(int dwEventType, int dwUserId, int dwErrorCode, int dwFlags, int dwParam, string lpUserStr, int lpUserValue)
        {
            if (VideoCallEventHandler != null)
                VideoCallEventHandler(dwEventType, dwUserId, dwErrorCode, dwFlags, dwParam, lpUserStr, lpUserValue);
        }

        //// AnyChat内核回调
        //public static void VideoData_CallBack(int userId, IntPtr buf, int len, AnyChatCoreSDK.BITMAPINFOHEADER bitMap, int userValue)
        //{
        //    if (VideoDataHandler != null)
        //        VideoDataHandler(userId, buf, len, bitMap, userValue);
        //}

        //// 静态委托
        //public void VideoDataCallbackDelegate(int userId, IntPtr buf, int len, AnyChatCoreSDK.BITMAPINFOHEADER bitMap, int userValue)
        //{
        //}
        #endregion

        #region 对外方法

        public void LoginVideo(string userId, string password)
        {
            //连接、登录
            AnyChatCoreSDK.Connect(ConfUtil.ANYCHAT_SERVER_ADDR, ConfUtil.ANYCHAT_SERVER_PORT);
            AnyChatCoreSDK.Login(userId, password, 0);
        }

        //进入房间开始视频
        public void StartVideo(int roomId)
        {
            AnyChatCoreSDK.EnterRoom(roomId, "", 0);
        }

        //关闭视频释放音视频资源
        public void CloseVideo()
        {
            AnyChatCoreSDK.LeaveRoom(-1);
            AnyChatCoreSDK.Logout();
           
        }

        //析构释放
        public void Dispose()
        {
            AnyChatCoreSDK.Release();
        }

        //设置回调函数
        public void SetVideoNotifyMessageCallBack(AnyChatCoreSDK.NotifyMessage_CallBack NotifyMessageHandler)
        {
            if (NotifyMessageHandler != null)
            {
                this.NotifyMessageHandler = NotifyMessageHandler;
            }
        }

        //设置回调函数
        public void SetVideoCallEventCallBack(AnyChatCoreSDK.VideoCallEvent_CallBack VideoCallEventHandler)
        {
            if (VideoCallEventHandler != null)
            {
                this.VideoCallEventHandler = VideoCallEventHandler;
            }
        }


        //设置对方用户视频显示
        public void SetUserVideoPos(int userId, Window windowContainer, int left, int top, int width, int height)
        {
            try
            {
                IntPtr mHandle = ((System.Windows.Interop.HwndSource)PresentationSource.FromVisual(windowContainer)).Handle;
                AnyChatCoreSDK.SetVideoPos(userId, mHandle, left, top, left+width, top+height);
            }
            catch
            {

            }
        }

        //设置本地视频显示
        public void SetLocalVideoPos(Window windowContainer, int left, int top, int width, int height)
        {
            try
            {
                IntPtr mHandle = ((System.Windows.Interop.HwndSource)PresentationSource.FromVisual(windowContainer)).Handle;
                AnyChatCoreSDK.SetVideoPos(LOCAL_USERID, mHandle, left, top, left + width, top + height);
            }
            catch
            {

            }   
        }

        //初始化本地视频显示
        public bool InitLocalVideo(Window windowContainer, int left, int top, int width, int height)
        {
            try
            {
                IntPtr mHandle = ((System.Windows.Interop.HwndSource)PresentationSource.FromVisual(windowContainer)).Handle;
                if (0 == AnyChatCoreSDK.SetVideoPos(LOCAL_USERID, mHandle, left, top, left + width, top + height)
                    && 0 == AnyChatCoreSDK.UserSpeakControl(LOCAL_USERID, true)
                    && 0 == AnyChatCoreSDK.UserCameraControl(LOCAL_USERID, true))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }


        //初始化远程用户视频显示
        public bool InitUserVideo(int userId, Window windowContainer, int left, int top, int width, int height)
        {
            try
            {
                bool isSuccess = false;
                // 获取当前房间用户列表
                int usercount = 0;
                AnyChatCoreSDK.GetOnlineUser(null, ref usercount);
                if (usercount <= 0)
                {
                    return isSuccess;
                }

                int[] useridarray = new int[usercount];
                AnyChatCoreSDK.GetOnlineUser(useridarray, ref usercount);
                
                for (int i = 0; i < usercount; i++)
                {
                    //挑选当前用户
                    if (userId != useridarray[i])
                        continue;

                    // 判断该用户的视频是否已打开
                    int usercamerastatus = 0;
                    if (AnyChatCoreSDK.QueryUserState(useridarray[i], AnyChatCoreSDK.BRAC_USERSTATE_CAMERA, ref usercamerastatus, sizeof(int)) != 0)
                    {
                        return isSuccess;
                    }
                    if (usercamerastatus == 2)
                    {
                        IntPtr mHandle = ((System.Windows.Interop.HwndSource)PresentationSource.FromVisual(windowContainer)).Handle;
                        if (0 == AnyChatCoreSDK.SetVideoPos(userId, mHandle, left, top, left + width, top + height)
                            && 0 == AnyChatCoreSDK.UserSpeakControl(userId, true)
                            && 0 == AnyChatCoreSDK.UserCameraControl(userId, true))
                        {
                            isSuccess = true;
                        }
                    }
                    break;
                }
                return isSuccess;
            }
            catch
            {
                return false;
            }
        }
        

        public int OpenCloseUserSpeak(int userid, bool isOpen)
        {
            return AnyChatCoreSDK.UserSpeakControl(userid, isOpen);
        }
        public int OpenCloseUserVideo(int userid, bool isOpen)
        {
            return AnyChatCoreSDK.UserCameraControl(userid, isOpen);
        }

        //邀请视频
        public int InviteVideo(int targetUserId, string userName)
        {
            return AnyChatCoreSDK.VideoCallControl(AnyChatCoreSDK.BRAC_VIDEOCALL_EVENT_REQUEST, targetUserId, 0, 0, 0, userName);
        }

        //接受视频邀请
        public int AcceptInvitedVideo(int targetUserId)
        {
            return AnyChatCoreSDK.VideoCallControl(AnyChatCoreSDK.BRAC_VIDEOCALL_EVENT_REPLY, targetUserId, 0, 0, 0, string.Empty);
        }

        //拒绝视频邀请
        public int RejectInvitedVideo(int targetUserId)
        {
            return AnyChatCoreSDK.VideoCallControl(AnyChatCoreSDK.BRAC_VIDEOCALL_EVENT_REPLY, targetUserId, AC_ERROR_VIDEOCALL_REJECT, 0, 0, string.Empty);
        }

        //取消视频邀请
        public int CancelInvitedVideo(int targetUserId)
        {
            return AnyChatCoreSDK.VideoCallControl(AnyChatCoreSDK.BRAC_VIDEOCALL_EVENT_REPLY, targetUserId, AC_ERROR_VIDEOCALL_CANCEL, 0, 0, string.Empty);
        }



        #endregion

        public int GetRandomRemoteUserIdForTest()
        {
            // 获取当前房间用户列表
            int usercount = 0;
            AnyChatCoreSDK.GetOnlineUser(null, ref usercount);
            if (usercount <= 0)
            {
                return VideoUtil.LOCAL_USERID;
            }

            int[] useridarray = new int[usercount];
            AnyChatCoreSDK.GetOnlineUser(useridarray, ref usercount);

            for (int i = 0; i < usercount; i++)
            {
                // 判断该用户的视频是否已打开
                int usercamerastatus = 0;
                if (AnyChatCoreSDK.QueryUserState(useridarray[i], AnyChatCoreSDK.BRAC_USERSTATE_CAMERA, ref usercamerastatus, sizeof(int)) != 0)
                {
                    continue;
                }
                if (usercamerastatus == 2)
                {
                    return useridarray[i];
                }
            }

            return useridarray[0];
        }
    }
}
