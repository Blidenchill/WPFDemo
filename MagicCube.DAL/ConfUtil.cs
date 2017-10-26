using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.DAL
{
    public static class ConfUtil
    {
        public const string netError = "网络不稳定导致失败，请您稍后重试。";
        public const string serverError = "服务器异常";
        public const string judgeSucess = "sucess"; 
        public const string platform = "1102";
        public const int PageContent = 15;
        public const string FeedbackPass = "合适";
        public const string FeedbackFail = "不合适";
        //静默显示时间间隔（分钟）
        public const int INSTANT_SHOW_TIME = 2;
        //最大显示消息数量
        public const int MAX_MESSAGE_NUM = 1000;
        #region "HttpAddr"


        /// <summary>
        /// 服务器访问基地址
        /// </summary>
        public const string ServerDomain = @"https://desktop-application.mofanghr.com/";

        //public const string ServerDomain = @"https://web-application.mofanghr.com/";

        ////开发环境
        //public const string ServerDomain = @"http://10.0.2.14:3264/";

        //测试环境
        //public const string ServerDomain = @"http://10.0.2.15:3264/";
        //晓旭
        //public const string ServerDomain = @"http://10.0.9.238:3264/";

        //public const string ServerDomain = @"http://10.0.7.22:3264/";
        //
        //public const string ServerDomain = @"http://10.0.2.15:3264/";
        //道远
        //public const string ServerDomain = @"http://10.0.9.216:3264/";

        //
        //public const string ServerDomain = @"http://10.0.3.208:3264/";

        //磊哥
        //public const string ServerDomain = @"http://10.0.3.213:3264/";

        //IE测试环境
        public const string ServerIE = @"https://portal.mofanghr.com/";
        //完善信息跳转页面
        //public const string ServerJumpToIE = ServerIE + "transfer?userID={0}&token={1}&time={2}&spm={3}";
        //public const string ServerJumpToAddV = ServerIE + "transfer?userID={0}&token={1}&time={2}&open=addV";
        
        //注册网址
        public const string ServerIERegister = ServerIE + "user/login?type=register";
        //忘记密码
        public const string ServerIEGetPassword = ServerIE + "user/forget-pwd?mobile={0}"; 

        public const string ServerUser = ServerDomain + "user/";
        public const string ServerMemberUser = ServerDomain + "member/user/";
        public const string ServerMember = ServerDomain + "member/";
        /// <summary>
        /// 手机号登录
        /// </summary>
        public const string AddrLoginByMobile = ServerUser + "loginByMobile.json?version={0}&params={1}";
        /// <summary>
        /// 账号密码登录
        /// </summary>
        public const string AddrLoginByPassword = ServerUser + "loginByPassword.json?version={0}&params={1}";

        /// <summary>
        /// 自动登录
        /// </summary>
        public const string AddrLoginByUserId = ServerUser + "loginByUserId.json?version={0}&params={1}";

        /// <summary>
        /// 注册
        /// </summary>
        public const string AddrRegister = ServerUser + "registerByMobile.json?version={0}&params={1}"; 

        /// <summary>
        /// 发送验证码
        /// </summary>
        public const string AddrSendVertifyCode = ServerUser + "sendVerifyCode.json?version={0}&params={1}";

        public const string AddrImgVertifyCode = ServerUser + "getImageVerifyCode.json?version={0}&params={1}";
        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        public const string AddrGetUserBasicInfo = ServerMemberUser + "getHrUser.json?version={0}&params={1}";
        /// <summary>
        /// 保存用户信息
        /// </summary>
        public const string AddrSaveUserInfo = ServerMemberUser + "saveHrUser.json?version={0}";
        /// <summary>
        /// 上传文件
        /// </summary>
        public const string AddrUploadFile = ServerDomain + "member/file/upload.json?version={0}&params={1}";
        /// <summary>
        /// 保存认证信息图片
        /// </summary>
        public const string AddrUploadAuthen = ServerMemberUser + "saveHRAuth.json?version={0}&params={1}";

        public const string AddrGetAuthenState = ServerMemberUser + "getHRAuth.json?version={0}&params={1}";
        /// <summary>
        /// 根据手机号检查用户信息
        /// </summary>
        public const string AddrCheckMobile = ServerUser + "checkMobile.json?version={0}&params={1}";
        /// <summary>
        /// 获取公司信息
        /// </summary>
        public const string AddrGetCompanyInfo = ServerMember + "company/get.json?version={0}&params={1}";
        /// <summary>
        /// 判断公司名是否存在
        /// </summary>
        public const string AddrCompanyNameExist = ServerMember + "company/getByName.json?version={0}&params={1}";
        /// <summary>
        /// 保存公司信息
        /// </summary>
        public const string AddrSaveCompany = ServerMember + "company/save.json?version={0}";
        /// <summary>
        /// 获取简历详情
        /// </summary>
        public const string AddrGetResumeDetialInfo = ServerMemberUser + "getResumeDetail.json?version={0}&params={1}";
        /// <summary>
        /// 修改手机号
        /// </summary>
        public const string AddrModifyTelephone = ServerMemberUser + "updateMobile.json?version={0}&params={1}";
        /// <summary>
        /// 获取简历分享二维码
        /// </summary>
        public const string AddrGetQrCode = ServerMember + "getUserIDMD5.json?version={0}&params={1}";
        /// <summary>
        /// 获取M币资产列表
        /// </summary>
        public const string AddrGetMCoinList = ServerMember + "userCoin/list.json?version={0}&params={1}";
        /// <summary>
        /// 获取积分资产列表
        /// </summary>
        public const string AddrGetIntegralList = ServerMember + "userPoint/list.json?version={0}&params={1}";
        /// <summary>
        /// 获取剩余下载数目列表
        /// </summary>
        public const string AddrGetDownloadList = ServerMember + "userServiceLog/list.json?version={0}&params={1}";


        #region 职位
        /// <summary>
        /// 发布职位 post
        /// </summary>
        public const string AddrCreateJob = ServerMember + "job/create.json?version={0}";
        /// <summary>
        /// 更新职位 post
        /// </summary>
        public const string AddrUpdateJob = ServerMember + "job/update.json?version={0}";
        /// <summary>
        /// 职位列表
        /// </summary>
        public const string AddrJobList = ServerMember + "job/getJobsByUserID.json?version={0}&params={1}";
        /// <summary>
        /// 职位列表
        /// </summary>
        public const string AddrJobDetail = ServerMember + "detail/getJob.json?version={0}&params={1}";
        /// <summary>
        /// 批量上线
        /// </summary>
        public const string AddrJobListOnline = ServerMember + "batch/job/onlineOrOffline.json?version={0}&params={1}";
        /// <summary>
        /// 批量刷新
        /// </summary>
        public const string AddrJobListRefresh = ServerMember + "batch/job/refresh.json?version={0}&params={1}";
        /// <summary>
        /// 是否可以编辑职位
        /// </summary>
        public const string AddrJobLimit = ServerMember + "job/limit.json?version={0}&params={1}";
        #endregion


        #region 会话
        /// <summary>
        /// 发送消息接口
        /// </summary>
        public const string AddrSendMessage = ServerMember + "message/send.json?version={0}";
        /// <summary>
        /// 批量发送IM
        /// </summary>
        public const string AddrBatchSend = ServerMember + "message/batchSend.json?version={0}&params={1}";
        /// <summary>
        /// 批量发送IM(邀请投递)
        /// </summary>
        public const string AddrBatchInvite = ServerMember + "message/batchInvite.json?version={0}";
        /// <summary>
        /// 获取未读消息数
        /// </summary>
        public const string AddrMessageUnReadCount = ServerMember + "message/getUnReadCount.json?version={0}&params={1}";
        /// <summary>
        /// 获取会话列表
        /// </summary>
        public const string AddrMessageSessionList = ServerMember + "message/session/list.json?version={0}&params={1}";
        /// <summary>
        /// 删除单个会话列表
        /// </summary>
        public const string AddrMessageSessionDelete = ServerMember + "message/session/delete.json?version={0}&params={1}";
        /// <summary>
        /// 批量更新消息为已读
        /// </summary>
        public const string AddrMessageUpdateToRead = ServerMember + "message/batchUpdateToRead.json?version={0}&params={1}";
        /// <summary>
        /// 获取消息记录
        /// </summary>
        public const string AddrMessageList = ServerMember + "message/list.json?version={0}&params={1}";
        /// <summary>
        /// 未读消息置已读
        /// </summary>
        public const string AddrUpdateToRead = ServerMember + "message/updateToRead.json?version={0}&params={1}";
        /// <summary>
        /// 获取单个会话
        /// </summary>
        public const string AddrMessageSessionGet = ServerMember + "message/session/getSessionIgnoreStatus.json?version={0}&params={1}";
        /// <summary>
        /// 获取快捷回复列表
        /// </summary>
        public const string AddrGetQuipReplyList = ServerMember + "message/userConfig/getShortcutReply.json?version={0}&params={1}";
        /// <summary>
        /// 保存快捷回复列表
        /// </summary>
        public const string AddrSaveQuipReplyList = ServerMember + "message/userConfig/saveShortcutReply.json?version={0}";
        #endregion


        /// <summary>
        /// 更新密码
        /// </summary>
        public const string AddrUpdatePassword = ServerMemberUser + "updatePassword.json?version={0}&params={1}";

        public const string AddrForgetPassword = ServerUser + "forgetPassword.json?version={0}&params={1}";
        /// <summary>
        /// 获取用户（GetUser）信息
        /// </summary>
        public const string AddrGetUser = ServerMemberUser + "get.json?version={0}&params={1}";
        /// <summary>
        /// 获取职位数量
        /// </summary>
        public const string AddrGetJobAccount = ServerMember + "account/statistics.json?version={0}&params={1}";
     
        public const string AddrUpdateSearch = ServerMember + "resume/search.json?version={0}&params={1}";


        public const string AddrResumeRecommend = ServerMember + "resume/recommend.json?version={0}&params={1}";

        public const string AddrConditionSaveList = ServerMember + "resume/searchCondition/list.json?version={0}&params={1}";

        public const string AddrConditionUpload = ServerMember + "resume/searchCondition/save.json?version={0}&params={1}";

        public const string AddrConditionDelete = ServerMember + "resume/searchCondition/delete.json?version={0}&params={1}";

        public const string AddrGetKeyWordsAutoAssociate = ServerMember + "resume/search/suggest.json?verison={0}&params={1}";
        //获取公司联想
        public const string AddrGetCompanyAutoAssociate = ServerMember + "company/getListByName.json?version={0}&params={1}";

        

        #region 关系
        /// <summary>
        /// 查看感兴趣列表
        /// </summary>
        public const string AddrViewAndColectList = ServerMember + "viewAndColect/relation/resume/list.json?version={0}&params={1}";
        /// <summary>
        /// 收藏简历
        /// </summary>
        public const string AddrCollectResume = ServerMember + "relation/collectResume.json?version={0}&params={1}";
        /// <summary>
        /// 取消收藏
        /// </summary>
        public const string AddrCancelCollectResume = ServerMember + "relation/cancelCollectResume.json?version={0}&params={1}";
        /// <summary>
        /// 收藏列表
        /// </summary>
        public const string AddrCollectResumeList = ServerMember + "relation/collectResume/multi/list.json?version={0}&params={1}";
        /// <summary>
        /// 收藏个数
        /// </summary>
        public const string AddrCollectCount = ServerMember + "relation/collectResume/count.json?version={0}&params={1}";
        /// <summary>
        /// 查看联系方式
        /// </summary>
        public const string AddrViewCantect = ServerMember + "relation/upsert.json?version={0}&params={1}";

        #endregion



        /// <summary>
        /// 登录签到
        /// </summary>
        public const string AddrSignIn = ServerMember + "signIn/insert.json?version={0}&params={1}";
        /// <summary>
        /// 登录签到验证
        /// </summary>
        public const string AddrSignInVertify = ServerMember + "signIn/verify.json?version={0}&params={1}";
        /// <summary>
        /// 登录签到明细
        /// </summary>
        public const string AddrSignInDetail = ServerMember + "signIn/detail.json?version={0}&params={1}";

        /// <summary>
        /// 发送验证码
        /// </summary>
        public const string AddrDeliveryCount = ServerMember + "delivery/count.json?version={0}&params={1}";
        /// <summary>
        /// 邀请投递
        /// </summary>
        public const string AddrHrSendOffer = ServerMember + "delivery/insert.json?version={0}&params={1}";

        public const string AddrResumeList = ServerMember + "delivery/listByCondition.json?version={0}&params={1}";
        /// <summary>
        /// 
        /// </summary>
        public const string AddrHrSendOfferMult = ServerMember + "batch/delivery/insert.json?version={0}&params={1}";

        #region 标记、标签
        /// <summary>
        /// 获取简历标记管理列表
        /// </summary>
        public const string AddrResumTagsList = ServerMember + "userSign/list.json?version={0}&params={1}";
        /// <summary>
        /// 添加简历标记
        /// </summary>
        public const string AddrResumTagsAdd = ServerMember + "userSign/insert.json?version={0}&params={1}";
        /// <summary>
        /// 修改简历标记
        /// </summary>
        public const string AddrResumTagsModify = ServerMember + "userSign/update.json?version={0}&params={1}";
        /// <summary>
        /// 删除简历标记
        /// </summary>
        public const string AddrResumTagsDelete = ServerMember + "userSign/delete.json?version={0}&params={1}";
        /// <summary>
        /// 获取公司标签列表
        /// </summary>
        public const string AddrCompanyTagsList = ServerMember + "tag/multiList.json?version={0}&params={1}";
        /// <summary>
        /// 简历打标记
        /// </summary>
        public const string AddrResumeSign = ServerMember + "delivery/sign.json?version={0}&params={1}";
        /// <summary>
        /// 简历取消标记
        /// </summary>
        public const string AddrResumeUnSign = ServerMember + "delivery/unSign.json?version={0}&params={1}";
        /// <summary>
        /// 职位诱惑批量删除新增标签
        /// </summary>
        public const string AddrTagCreate = ServerMember + "tag/multiUpsert.json?version={0}&params={1}";
        /// <summary>
        /// 职位诱惑获取标签列表
        /// </summary>
        public const string AddrTagList = ServerMember + "tag/multiList.json?version={0}&params={1}";
        #endregion


        public const string AddrResumeConfirm = ServerMember + "delivery/confirm.json?version={0}&params={1}";

        public const string AddrFreePhone = ServerMember + "freePhone/callBothSide/inner.json?version={0}&params={1}";

        public const string AddrRedFunction = ServerMember + "assist/redFunction.json?version={0}&params={1}";
        public const string AddrClearRedFunction = ServerMember + "assist/saveRelationCount.json?version={0}&params={1}";

        public const string AddrGetByDeliveryID = ServerMember + "delivery/getByHRUserIDJHUserID.json?version={0}&params={1}";
        public const string AddrGetUserSignList = ServerMember + "delivery/getByID.json?version={0}&params={1}";

        public const string AddrGetSearchJob = ServerMember + "search/job/Condition.json?version={0}&params={1}";

        public const string AddrGetMB = ServerMember + "userService/get.json?version={0}&params={1}";


        public const string AddrVideoSign = ServerMember + "videoSig/getSig.json?version={0}&params={1}";

        public const string AddrVideoInvite = ServerMember + "videoConnect/invite.json?version={0}&params={1}";

        public const string AddrResumeDownload = ServerMember + "user/downloadResume.json?version={0}&params={1}";

        public const string AddrMessageNotice = ServerMember + "message/notice/pop.json?version={0}&params={1}";
      
        public const string AddrEmailSend = ServerMember + "email/send.json?version={0}&params={1}";

        /// <summary>
        ///  此接口为通用接口 post serviceSign 目前S_Coin 获取mb数量
        /// </summary>
        public const string AddrO2OMB = ServerMember + "o2o/userService.json?version={0}&params={1}";

        public const string AddrInterviewSessionList = ServerMember + "o2o/getSessionAndJobList.json?version={0}&params={1}";

        public const string AddrInterviewJobSessionTree = ServerMember + "o2o/getJobList.json?version={0}&params={1}";

        public const string AddrInterviewNum = ServerMember+"o2o/getStatistics.json?version={0}&params={1}";

        public const string AddrInterviewUnconfimNum = ServerMember + "o2o/getWaitConfirmVisitStatistics.json?version={0}&params={1}";

        public const string AddrInterviewAffirm = ServerMember + "o2o/getWaitConfirmVisitList.json?version={0}&params={1}";

        public const string AddrInterviewSessionInterview = ServerMember + "o2o/getInterviewByJobOrSession.json?version={0}&params={1}";

        public const string AddrInterviewVisitConfirm = ServerMember + "o2o/visit/confirm.json?version={0}&params={1}";
        public const string AddrInterviewRefuseConfirm = ServerMember + "o2o/reserveConfirm.json?version={0}&params={1}";
        public const string AddrUserFunction = ServerMember + "o2o/list/userFunction?version={0}&params={1}";

        public const string AddrCoinList = ServerMember + "userCoin/list.json?version={0}&params={1}";

        public const string AddrInterviewSessionInterviewList = ServerMember + "o2o/getResumeListByJobSession.json?version={0}&params={1}";

        public const string AddrInterviewAnswer = ServerMember + "o2o/getAnswer.json?version={0}&params={1}";


        public const string AddrCreatOpinion = ServerMember + "job/create/opinion.json?version={0}&params={1}";

        public const string AddrTemplateInsert = ServerMember + "job/template/insert.json?version={0}";

        public const string AddrTemplateGet = ServerMember + "job/template/get.json?version={0}";

        public const string AddrTemplateGetList = ServerMember + "job/template/get/list.json?version={0}";

        public const string AddrTemplateDelete = ServerMember + "job/template/delete.json?version={0}";

        public const string AddrCommentInsert = ServerMember + "resumeComment/insert.json?version={0}&params={1}";

        public const string AddrCommentDelete = ServerMember + "resumeComment/delete.json?version={0}&params={1}";
        #endregion

        #region "localAddr"
        public static string LocalHomePath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\mofanghr\";
        #endregion

        #region o2o
        public const string ServerOffline = ServerDomain + "member/offline/";
        /// <summary>
        /// 此接口用于查看面试首页进行中与已结束职位分别多少
        /// </summary>
        //public const string ServerInterviewNum = ServerOffline + "interview/num";
        /// <summary>
        /// 用于查看面试首页有待确认场次
        /// </summary>
        //public const string ServerInterviewUnconfimNum = ServerOffline + "interview/status/count";
        ///// <summary>
        ///// 面试首页今天列表
        ///// </summary>
        //public const string ServerInterviewToday = ServerOffline + "batch/underway/interview/today";
        /// <summary>
        /// 面试首页其他天
        /// </summary>
        //public const string ServerInterviewOtherday = ServerOffline + "batch/underway/interview/otherday";
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
        //public const string ServerInterviewUserFunction = ServerOffline + "list/user-function";


        #endregion

        public const string AddrLogOut = ServerMemberUser + "logout.json?version={0}";
    }
}
