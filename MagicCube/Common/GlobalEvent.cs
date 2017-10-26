using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.Common
{
    public class GlobalEvent
    {
        public static EventHandler<GlobalEventArgs> GlobalEventHandler;
    }

    public class GlobalEventArgs :EventArgs
    {
        private GlobalFlag flag;
        public GlobalFlag Flag
        {
            get { return flag; }
        }
        private object opacity;
        public object Opacity
        {
            get { return opacity; }
        }
        public GlobalEventArgs(GlobalFlag flag, object opacity)
        {
            this.flag = flag;
            this.opacity = opacity;
            
        }
    }

    public enum GlobalFlag
    {
        UpdateConnectList,
        //打开简历聊天对话框
        openResumTalk,

        //打开副窗口的标记管理遮罩
        openTagsManager,
        //打开副窗口的标记管理遮罩
        openTagsAdd,

        //环信账号顶号事件
        AccountClose,

        //刷新职位动态变已读
        DynamicIsRead,

        //打开职位选择对话框
        JobPublishList,
        //小聘2.0主窗口置顶
        WindowMost,

        //打开快捷回复设置界面
        IsOpenQuipReplySetting,

        //tip打开主窗口查看/感兴趣的人
        TipOpenDynamicUI,
        //tip打开窗口推荐的人
        TipOpenRecommandUI,
        /// <summary>
        /// 发送职位邀请
        /// </summary>
        HrSendOffer
        


    }
}
