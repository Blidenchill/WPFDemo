using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MagicCube.ViewModel
{
    public class HttpSendMessageParams
    {
        //        fromUserID	是	long	发送消息的用户ID
        //fromUserIdentity	是	long	发送消息的用户身份 参照身份编码定义
        //toUserID	是	long	接收消息的用户ID
        //toUserIdentity	是	long	接收消息的用户身份 参照身份编码定义
        //content	是	string	消息内容
        //type	是	string	消息类型 参照消息类型编码
        //ip	是	string	调用方IP
        //clientVersion	是	string	调用方客户端版本
        //platform	是	string	调用方平台 参照调用方平台编码
        //pathID	是	string	pathID 产品提供
        public long fromUserID
        {
            get;
            set;
        }
        public string fromUserIdentity
        {
            get;
            set;
        }
        public long toUserID
        {
            get;
            set;
        }
        public string toUserIdentity
        {
            get;
            set;
        }
        public string content
        {
            get;
            set;
        }
        public string type
        {
            get;
            set;
        }
        public string ip
        {
            get;
            set;
        }
        public string platform
        {
            get;
            set;
        }
        public string clientVersion
        {
            get;
            set;
        }
        public string pathID
        {
            get;
            set;
        }
        public string jobID
        {
            get;
            set;
        }
    }
    public class HttpBatchSendParams
    {
        public List<HttpSendMessageParams> message
        {
            get;
            set;
        }
    }
    public class HttpMessage
    {
        //        sid	会话ID	sessionID	String	会话ID，由发送者ID和接受者ID生成唯一
        //mid	消息ID	messageID	String	
        //fid	发送者编号	fromUserID	Int64	
        //tid	接受者编号	toUserID	Int64	
        //brand	品牌	brand	String	默认:DZB
        //status	消息状态	status	Int32	0：未读 1：已读 -2：客服删除 -1：用户删除 -3：系统删除
        //ct	创建时间	createTime	DateTime	消息创建时间
        //ut	读取时间	updateTime	DateTime	消息更新时间
        //rt	读取时间	readTime	DateTime	
        //platform	调用方平台	platform	String	
        //version	客户端版本	version	String	
        //msg	消息体内容	msg	String	json格式，如：{"text":"hello"}
        public string sessionID
        {
            get;
            set;
        }
        public string msgID 
        {
            get;
            set;
        }
        public long fromUserID
        {
            get;
            set;
        }
        public long toUserID
        {
            get;
            set;
        }
        public int  status
        {
            get;
            set;
        }
        public DateTime createTime
        {
            get;
            set;
        }
        public string content
        {
            get;
            set;
        }
        public string type
        {
            get;
            set;
        }
        public string fromSide
        {
            get;
            set;
        }
        public string landingPage
        {
            get;set;
        }


    }
    public class HttpMessageRoot
    {
        /// <summary>
        /// MsgList
        /// </summary>
        public ObservableCollection<HttpMessage> msgList { get; set; }
        /// <summary>
        /// 6S0PPOn2
        /// </summary>
        public string sessionID { get; set; }
        /// <summary>
        /// 0
        /// </summary>
        public string block { get; set; }
    }
    public class HttpMessageUnReadParams
    {
        public long userID
        {
            get;
            set;
        }
        public long userIdentity
        {
            get;
            set;
        }
        public string sessionID
        {
            get;
            set;
        }
    }
    public class HttpMessageUnRead
    {
        public long userID
        {
            get;
            set;
        }
        public long unReadCount
        {
            get;
            set;
        }
        public long userIdentity
        {
            get;
            set;
        }
    }
    public class HttpMessageSessionListParams
    {
        public long userID
        {
            get;
            set;
        }
        public long userIdentity
        {
            get;
            set;
        }
        public int start
        {
            get;
            set;
        }
        public int size
        {
            get;
            set;
        }
        public string jobID
        {
            get;
            set;
        }
    }

    public class HttpMessageSessionDeleteParams
    {
        public long userID
        {
            get;
            set;
        }
        public long userIdentity
        {
            get;
            set;
        }
        public string sessionID
        {
            get;
            set;
        }
        public string clientVersion
        {
            get;
            set;
        }
        public string ip
        {
            get;
            set;
        }
        public string platform
        {
            get;
            set;
        }
    }

    public class HttpMessageUpdataToReadParams
    {
        public long userID
        {
            get;
            set;
        }
        public long userIdentity
        {
            get;
            set;
        }
        public string sessionID
        {
            get;
            set;
        }
        public string clientVersion
        {
            get;
            set;
        }
    }
    public class HttpMessageListParams
    {
        public long userID
        {
            get;
            set;
        }
        public long userIdentity
        {
            get;
            set;
        }
        public string sessionID
        {
            get;
            set;
        }
        public string start
        {
            get;
            set;
        }
        public int size
        {
            get;
            set;
        }
        public string type
        {
            get;
            set;
        }
        public long anotherUserID
        {
            get;
            set;
        }
        public long anotherUserIdentity
        {
            get;
            set;
        }
        public long jobID
        {
            get;
            set;
        }
    }
    public class HttpRecentListUser
    {
        /// <summary>
        /// 287
        /// </summary>
        public string userID { get; set; }
        /// <summary>
        /// http://public.mofanghr.com/head/2016/11/21/1D993F4ACC151404B845454EA8D443D2.jpg
        /// </summary>
        public string avatar { get; set; }
        /// <summary>
        /// aaa
        /// </summary>
        public string name { get; set; }

        public string mobile { get; set; }
    }
    public class HttpRecentListJob
    {

        public int jobID { get; set; }

        public string jobName { get; set; }
    }
    public class HttpRecentList
    {
        public HttpRecentListUser user { get; set; }
        public HttpRecentListJob job { get; set; }
        public int unReadCount { get; set; }        
        public string latestMsgDesc { get; set; }        
        public string lastSender { get; set; }        
        public int userID { get; set; }        
        public string lastSenderIdentity { get; set; }        
        public string sessionID { get; set; }        
        public string userIdentity { get; set; }        
        public string deadline { get; set; }      
        public DateTime updateDate { get; set; }       
        public string block { get; set; }

        public int jobID { get; set; }

        public string jobName { get; set; }

        public string prior { get; set; }
    }

    public class HttpGetSessionParams
    {
        public long userID
        {
            get;
            set;
        }
        public long userIdentity
        {
            get;
            set;
        }
        public long anotherUserID
        {
            get;
            set;
        }
        public long anotherUserIdentity
        {
            get;
            set;
        }

        public string jobID
        {
            get;
            set;
        }
    }
    public class HttpMessageNotice
    {
     
        public string content { get; set; }      
        public DateTime createTime { get; set; }      
        public string level { get; set; }      
        public string noticeID { get; set; }      
        public string status { get; set; }
        public string type { get; set; }
        public DateTime updateTime { get; set; }
    }
    public class HttpMessageNoticeSign
    {
        public string companyID { get; set; }
        public string notice { get; set; }
        public string taskSign { get; set; }
        public string taskState { get; set; }
        public string taskType { get; set; }
        public string userID { get; set; }
        public string userTaskID { get; set; }
    }

    public class HttpPhoneMessage
    {
        public string type { get; set; }
        public string msg { get; set; }
    }
}
