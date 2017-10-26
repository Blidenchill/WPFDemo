using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.Util
{
    public static class ConfUtil
    {

        public const bool Debug = false;

        public const double VERSION = 1.0;


        public const string netError = "网络不稳定导致失败，请您稍后重试~";
        //灰度
        //public const string EMCHAT_CLIENT_ID = "YXA6MfjsgEsdEeWeyXdzL5-YNA";
        //public const string EMCHAT_CLIENT_SECRET = "YXA669ZjQSWw89lLM4oElm95xQXxcVk";
        //public const string EMCHAT_ORG_NAME = "mofanghr";
        //public const string EMCHAT_APP_NAME = "magicccube";
        //public const string ServerDomain = "http://p.mofangzp.top/";
        //public const string ServerIE = "http://www.mofangzp.top/";
        //public const string RabbitMQServer = "60.205.59.136";
        //public const int RabbitMQPort = 5672;
        //public const string TrackServer = "http://60.205.59.140:3320/";
        //生产前
        //public const string EMCHAT_CLIENT_ID = "YXA6DuHWEPSEEeW8HvU3IUmBFQ";
        //public const string EMCHAT_CLIENT_SECRET = "YXA6DB-Ov9eDiKFZxhw0nk-prFtaJBg";
        //public const string EMCHAT_ORG_NAME = "mofanghr";
        //public const string EMCHAT_APP_NAME = "magiccubdprod";
        //public const string ServerDomain = "http://218.57.8.108:6688/";
        //public const string ServerIE = "http://218.57.8.108/";
        //public const string RabbitMQServer = "am.mofanghr.com";
        //public const int RabbitMQPort = 5670;
        //public const string TrackServer = "http://10.0.2.72:3320/";

        //生产
        public const string EMCHAT_CLIENT_ID = "YXA6DuHWEPSEEeW8HvU3IUmBFQ";
        public const string EMCHAT_CLIENT_SECRET = "YXA6DB-Ov9eDiKFZxhw0nk-prFtaJBg";
        public const string EMCHAT_ORG_NAME = "mofanghr";
        public const string EMCHAT_APP_NAME = "magiccubdprod";
        public const string ServerDomain = "http://desktop.application.mofanghr.com/";
        public const string ServerIE = "http://www.mofanghr.com/";
        public const string RabbitMQServer = "am.mofanghr.com";
        public const int RabbitMQPort = 5670;
        public const string TrackServer = "http://log.mofanghr.com/";

        ////联网测试
        //public const string EMCHAT_CLIENT_ID = "YXA6MfjsgEsdEeWeyXdzL5-YNA";
        //public const string EMCHAT_CLIENT_SECRET = "YXA669ZjQSWw89lLM4oElm95xQXxcVk";
        //public const string EMCHAT_ORG_NAME = "mofanghr";
        //public const string EMCHAT_APP_NAME = "magicccube";
        //public const string ServerDomain = "http://face.idoss.com/";
        ////public const string ServerDomain = "http://10.0.2.18:6688/";
        //public const string ServerIE = "http://www.idoss.com/";
        //public const string RabbitMQServer = "10.0.2.18";
        //public const int RabbitMQPort = 5672;
        //public const string TrackServer = "http://10.0.2.72:3320/";

        ////228
        //public const string EMCHAT_CLIENT_ID = "YXA6MfjsgEsdEeWeyXdzL5-YNA";
        //public const string EMCHAT_CLIENT_SECRET = "YXA669ZjQSWw89lLM4oElm95xQXxcVk";
        //public const string EMCHAT_ORG_NAME = "mofanghr";
        //public const string EMCHAT_APP_NAME = "magicccube";
        //public const string ServerDomain = "http://10.0.2.228:6688/";
        //public const string ServerIE = "http://10.0.2.228:3100/";
        //public const string RabbitMQServer = "10.0.2.228";
        //public const int RabbitMQPort = 5672;
        //public const string TrackServer = "http://10.0.2.13:3320/";


        ////5
        //public const string EMCHAT_CLIENT_ID = "YXA6j0qZwPIpEeWxegk5pzbuAg";
        //public const string EMCHAT_CLIENT_SECRET = "YXA6GkLit0wpgQ8t9epK7nnhC_4mll8";
        //public const string EMCHAT_ORG_NAME = "mofanghr";
        //public const string EMCHAT_APP_NAME = "face8test";
        //public const string ServerDomain = "http://10.0.2.5:6688/";
        //public const string ServerIE = "http://10.0.2.5:3100/";
        //public const string RabbitMQServer = "10.0.2.5";
        //public const int RabbitMQPort = 5672;
        //public const string TrackServer = "http://10.0.2.72:3320/";

        //////勋哥
        //public const string EMCHAT_CLIENT_ID = "YXA6MfjsgEsdEeWeyXdzL5-YNA";
        //public const string EMCHAT_CLIENT_SECRET = "YXA669ZjQSWw89lLM4oElm95xQXxcVk";
        //public const string EMCHAT_ORG_NAME = "mofanghr";
        //public const string EMCHAT_APP_NAME = "magicccube";
        //public const string ServerDomain = "http://192.168.250.128:6688/";
        //public const string ServerIE = "http://192.168.250.128:3100/";
        //public const string RabbitMQServer = "10.0.3.197";
        //public const int RabbitMQPort = 5672;
        //public const string TrackServer = "http://10.0.2.72:9092/";

        //小旭
        //public const string EMCHAT_CLIENT_ID = "YXA6MfjsgEsdEeWeyXdzL5-YNA";
        //public const string EMCHAT_CLIENT_SECRET = "YXA669ZjQSWw89lLM4oElm95xQXxcVk";
        //public const string EMCHAT_ORG_NAME = "mofanghr";
        //public const string EMCHAT_APP_NAME = "magicccube";
        //public const string ServerDomain = "http://192.168.81.128:6688/";
        //public const string ServerIE = "http://192.168.81.128:3100/";
        //public const string RabbitMQServer = "10.0.3.197";
        //public const int RabbitMQPort = 5672;
        //public const string TrackServer = "http://192.168.81.128:3320/";

        ////2.227
        //public const string EMCHAT_CLIENT_ID = "YXA6MfjsgEsdEeWeyXdzL5-YNA";
        //public const string EMCHAT_CLIENT_SECRET = "YXA669ZjQSWw89lLM4oElm95xQXxcVk";
        //public const string EMCHAT_ORG_NAME = "mofanghr";
        //public const string EMCHAT_APP_NAME = "magicccube";
        //public const string ServerDomain = "http://10.0.2.227:6688/";
        //public const string ServerIE = "http://10.0.2.227:3100/";
        //public const string RabbitMQServer = "10.0.3.197";
        //public const int RabbitMQPort = 5672;
        //public const string TrackServer = "http://10.0.2.72:3320/";

        //博
        //public const string EMCHAT_CLIENT_ID = "YXA6MfjsgEsdEeWeyXdzL5-YNA";
        //public const string EMCHAT_CLIENT_SECRET = "YXA669ZjQSWw89lLM4oElm95xQXxcVk";
        //public const string EMCHAT_ORG_NAME = "mofanghr";
        //public const string EMCHAT_APP_NAME = "magicccube";
        //public const string ServerDomain = "http://10.0.3.203:6688/";
        //public const string ServerIE = "http://10.0.3.203:3100/";
        //public const string RabbitMQServer = "10.0.3.203";
        //public const int RabbitMQPort = 5672;

        public const string EMCHAT_URL = "wss://im-api.easemob.com/ws/";
        public const string EMCHAT_DOMAIN_NAME = "easemob.com";
        public static string EMCHAT_REST_URL = string.Format("https://a1.easemob.com/{0}/{1}/", EMCHAT_ORG_NAME, EMCHAT_APP_NAME);
        public const int EMCHAT_RECONNECT_TIMES = 3;
      


        //小面服务接口
        public const string ServerFace = ServerDomain + "face/";
        public const string ServerOffline = ServerDomain + "offline/";
        //外部链接
        public const string LinkCompanies = ServerDomain + "ep/companies-detail";
        public const string LinkMB = ServerDomain + "ep/mb/money-record";
        public const string LinkUser = ServerDomain + "ep/enterprise-users";
        public const string LinkEdit = ServerDomain + "ep/jobs/{0}/edit";
        public const string Linkpublish = ServerDomain + "ep/job/publish";

        public const string Link = ServerIE + "wc/enterprise-users/face/login?username={0}&password={1}&source=face&page={2}";
        //消息发送接口
        public const string ServerImSend =  ServerDomain + "im/messages/text/send";

        //套网页发布职位
        public const string ServerPublish = ServerDomain + "ep/face/job/publish";
        //套网页进行中的职位
        public const string ServerJobOpen = ConfUtil.ServerDomain + "ep/face/interview-chance/open";
        //套网页已过期的职位
        public const string ServerJobClose = ConfUtil.ServerDomain + "ep/face/interview-chance/closed";

        //发布于需要更新
        public const string ServerVersionGet = ServerFace + "version";
        //登录POST
        public const string ServerSign = ServerFace+"login";

        public const string ServerSignInfo = ServerFace + "login/first/info";

        //首次登录
        public const string ServerSignAward = ServerFace + "login-award-mb";

        public const string ServerToken = ServerFace + "im-record/token";
        //简历   求职人ID POST
        public const string ServerResume = ServerFace + "resumes/contact-records/{0}";
        public const string ServerHXResume = ServerFace + "resumes/contact-records/{0}/mini ";
        //付费联系 传方式 POST
        public const string ServerPayForVidio = "VIDIO_COMMUNICATION";
        public const string ServerPayForPhone = "TELEPHONE_COMMUNICATION";
        public const string ServerPayForShow = "SHOW_CONTACT_WAY";
        public const string ServerPay = ServerFace+"resumes/privacy";

        public const string ServerCallFor = ServerDomain + "ccr/call-for-interview-manage";

        //简历评价
        public const string FeedbackPass = "合适";
        public const string FeedbackPassMsg = "初步审核，您比较适合本职位，请主动联系我们或耐心等待，了解后续安排。";
        public const string FeedbackFail = "不合适";
        public const string FeedbackFailMsg = "很抱歉，您暂时不太适合本职位，感谢您对我们的支持，欢迎您关注本公司其他职位。";
        public const string ServerFeedback = ServerFace + "contact-records/{0}";
        //学历 经验列表
        public const string ServerFacets = ServerFace + "resumes/facets";
        //简历个数统计
        public const string ServerResumeCount = ServerFace + "resumes/count";
        //职位个数统计
        public const string ServerJobCount = ServerFace + "jobs/count";
        //获取公司信息
        public const string ServerCompany = ServerFace + "user/company";

        public const int PageContent = 15;

        //简历列表 传简历类别 
        public const string ServerResumeList = ServerFace + "contact-records/resumes";

        //是否可以发布职位
        public const string ServerCanPublic = ServerFace + "account/verify";
        public const string ServerPublicTotal = ServerFace + "jobs/publish-total";
        //job列表
        public const string ServerJob = ServerFace + "jobs/search-all";
        //job列表列表下的简历

        public const string ServerJobResumePass = "untreated";

        public const string ServerJobResume = ServerFace + "jobs/{0}/contact-records";

        //查IMName
        public const string ServerImName = ServerFace + "users/contact-records/{0}";

        //查询M币
        public const string ServerIntegral = ServerFace + "user/mb";

        public const string ANYCHAT_SERVER_ADDR = "demo.anychat.cn";
        public const int ANYCHAT_SERVER_PORT = 8906;
        public const int ANYCHAT_ECONNECT_TIMES = 3;

        //静默显示时间间隔（分钟）
        public const int INSTANT_SHOW_TIME = 2;
        //最大显示消息数量
        public const int MAX_MESSAGE_NUM = 1000;

        public static bool DEBUG = true;

        public static string dataPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+ @"\mofanghr\";

        //人才搜索相关
        public const string ServerSearching = ServerFace + "contact-records/resumes";
        //获取人才搜索查询条件
        public const string ServerConditionGet = ServerFace + "talent-search/facets";
        public const string ServerTelantSearch = ServerFace + "talent-search/data?start={0}&pageCount={1}";

        public const string ServerHistorySearch = ServerFace + "talent-search/history?start={0}&pageCount={1}";

        public const string ServerGetJobPublish = ServerFace + "jobs/search-all?status=open";

        public const string ServerGetConstactRecord = ServerFace + "im-record/invite?uniqueKey={0}&jobId={1}";
        //public const string ServerConditionGet = "http://192.168.0.206:3100/face/talent-search/facets";

        //public const string ServerTelantSearch = "http://192.168.0.206:3100/face/talent-search/data";

        public const string ServerTrackRecord = ServerFace + "log-trace";

        public const string ServerRusumeGet = ServerFace + "resumes/{0}";

        public const string ServerCommonReplySearch = ServerFace + "common-reply/search";
        public const string ServerCommonReplyEdit = ServerFace + "common-reply/edit";

        public const string ServerRecentList = ServerFace + "resumes/all";
        public const string ServerRecentDelete = ServerFace + "delete-applicant";

        public const string ServerJobManageList = ServerFace + "interview-chance/{0}?count={1}&start={2}&partialJobName={3}&source={4}";

        public const string ServerJobManageDetial = ServerFace + "jobs/{0}";

        public const string ServerJobManageOutLine = ServerFace + "jobs/close/{0}";
        public const string ServerJobManageRefresh = ServerFace + "jobs/refresh/{0}";
        public const string ServerJobManageRepublish = ServerFace + "jobs/republish/{0}";

        public const string ServerJobManageOutLineMulti = ServerFace + "jobs/close";
        public const string ServerJobManageRefreshMulti = ServerFace + "jobs/refresh";
        public const string ServerJobManageRepublishMulti = ServerFace + "jobs/republish";

        public const string ServerJobTreeThree = ServerDomain + "wc/category";

        public const string ServerJobDetialPublish = ServerFace + "jobs/publish";

        public const string ServerUploadFile = ServerFace + "upload_image/{0}";

        public const string ServerJobDetialEdit = ServerFace + "jobs/edit";

        public const string ServerSelfInfoSave = ServerFace + "enterprise-users/save";

        public const string ServerEnterpriseName = ServerFace + "jobs/enterprise/name";

        public const string ServerCompanyInfoSave = ServerFace + "companies-save";

        public const string ServerZhilianLogin = ServerFace + "thirdSites/Zhilian/login";

        public const string ServerZhilianPublish = ServerFace + "thirdSites/jobs/Zhilian/{0}/{1}/{2}/up";

        public const string ServerZhilianAliveTick = ServerFace + "thirdSites/Zhilian/alive";

        public const string ServerZhilianRefresh = ServerFace + "thirdSites/jobs/Zhilian/{0}/refresh";

        public const string ServerZhilianPublishZL = ServerFace + "thirdSites/jobs/Zhilian/{0}/{1}/publish";

        public const string ServerZhilianResumDownload = ServerFace + "thirdSites/Zhilian/job-down";

        public const string ServerJobDynamicList = ServerFace + "user-behaviors/search";

        public const string ServerJobDynamicIsRead = ServerFace + "user-behaviors/{0}/read/update";

        public const string ServerJobDynamicContactRecord = ServerFace + "contact-record/save";

        public const string ServerViewNotice = ServerFace + "view-resume?source={0}&uniqueKey={1}&jobId={2}";

        public const string ServerTalentSearchNotice = ServerFace + "view-resume?source={0}&uniqueKey={1}";

        public const string ServerRecordIdGeUniqueKey = ServerFace + "uniqueKey/contact-records/{0}";

        public const string ServerJobTree = ServerFace + "resumes/job/count";
        public const string ServerHasJobOpen = ServerFace + "jobs/total/open";
        public const string ServerTagsManageGetAll = ServerFace + "account-tags/get-all";

        public const string ServerTagsManageAdd = ServerFace + "account-tags/add";

        public const string ServerTagsManageDelete = ServerFace + "account-tags/{0}/del";

        public const string ServerTagsManageModify = ServerFace + "account-tags/{0}/update";

        public const string ServerResumeRemarkTags = ServerFace + "account-tags/resume-remarks/update";

        public const string ServerThirdCountExist = ServerFace + "thirdSites/{0}/{1}/exists";

        public const string ServerTime = ServerFace + "server/time";

        public const string ServerIMRecord = ServerFace + "im-records";

        public const string ServerRecommend = ServerFace + "im/recommend";

        public const string ServerRecommendRead = ServerFace + "im/recommend/click/{0}";

        public const string ServerModifyPassword = ServerFace + "reset-password";


        //埋点地址
        public const string TrackUpload = TrackServer + "logs/xiaopin";
        //public const string TrackUpload = TrackServer + "logs/app";

        public const string ServerResumeDetial = ServerFace + "resume-detail?recordId={0}&uniqueKey={1}&resumeId={2}";

        public const string ServerContactRecord = ServerFace + "filter-im";

        public const string ServerLastJob = ServerFace + "last/time/publish-job";
        

        /// <summary>
        /// 此接口用于查看面试首页进行中与已结束职位分别多少
        /// </summary>
        public const string ServerInterviewNum = ServerOffline + "interview/num";
        /// <summary>
        /// 用于查看面试首页有待确认场次
        /// </summary>
        public const string ServerUnconfimNum = ServerOffline + "interview/status/count";
        /// <summary>
        /// 面试首页今天列表
        /// </summary>
        public const string ServerInterviewToday = ServerOffline + "batch/underway/interview/today";
        /// <summary>
        /// 面试首页其他天
        /// </summary>
        public const string ServerInterviewOtherday = ServerOffline + "batch/underway/interview/otherday";
        /// <summary>
        /// 面试页待确认到访
        /// </summary>
        public const string ServerInterviewAffirm = ServerOffline + "goto/affirm";
        /// <summary>
        /// 时段预约面试
        /// </summary>
        public const string ServerInterviewTimeslotReserved = ServerOffline + "onsite/job/timeslot/reserved";
        /// <summary>
        /// 时段待确认到访
        /// </summary>
        public const string ServerInterviewTimeslotWaiting = ServerOffline + "onsite/job/timeslot/reserved/waiting";
        /// <summary>
        /// 时段已到访
        /// </summary>
        public const string ServerInterviewTimeslotPresent = ServerOffline + "onsite/job/timeslot/present";
        /// <summary>
        /// 答题详情
        /// </summary>
        public const string ServerInterviewQestionDetail = ServerOffline + "question-detail";
        /// <summary>
        /// 简历详情
        /// </summary>
        public const string ServerInterviewResumeDetail = ServerOffline + "resume-detail";
        /// <summary>
        /// 首页otherday分页
        /// </summary>
        public const string ServerInterviewOtherdayInternal = ServerOffline + "internal/batch/underway/interview/otherday";
        /// <summary>
        /// 面试页待确认到访分页
        /// </summary>
        public const string ServerInterviewAffirmInternal = ServerOffline + "internal/affirm";
        /// <summary>
        /// 确认到访 post interviewId name 
        /// </summary>
        public const string ServerInterviewResult = ServerOffline + "interviews/result";
        /// <summary>
        /// 确认未到访 post interviewId status result name 
        /// </summary>
        public const string ServerInterviewResultReview = ServerOffline + "interviews/result/review";
        /// <summary>
        /// 拒绝 post interviewId name 
        /// </summary>
        public const string ServerInterviewResultRefuse = ServerOffline + "interviews/result/refuse";
        /// <summary>
        ///  今天没数据最后一场 get
        /// </summary>
        public const string ServerInterviewLastData = ServerOffline + "last/data";
        /// <summary>
        ///  通过职位获取价钱 post jobId
        /// </summary>
        public const string ServerInterviewGetPrice = ServerOffline + "get/price";

        /// <summary>
        ///  此接口为通用接口 post serviceSign 目前S_Coin 获取mb数量
        /// </summary>
        public const string ServerInterviewUserService = ServerOffline + "inner/user-service";
        /// <summary>
        /// Get请求
        /// </summary>
        public const string ServerInterviewMCoinDetail = ServerOffline + "list/user-coin";
        /// <summary>
        /// 获取可用功能列表
        /// </summary>
        public const string ServerInterviewUserFunction = ServerOffline + "list/user-function";

        

   
    }
}
