using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using MagicCube.HttpModel;

using System.Collections.ObjectModel;


namespace MagicCube.Model
{
    public class IMControlModel  : NotifyBaseModel
    {
        public IMControlModel()
        {
            TalkMsgs = new ObservableCollection<ViewModel.HttpMessage>();
        }

        public string prior { get; set; }

        //用户ID
        private long _UserID;
        public long UserID
        {
            get
            {
                return _UserID;
            }
            set
            {
                _UserID = value;
                NotifyPropertyChange(() => UserID);
            }
        }
        private string _SessionID;
        public string SessionID
        {
            get
            {
                return _SessionID;
            }
            set
            {
                _SessionID = value;
                NotifyPropertyChange(() => SessionID);
            }
        }

        private int _RecordId;
        public int RecordId
        {
            get
            {
                return _RecordId;
            }
            set
            {
                _RecordId = value;
                NotifyPropertyChange(() => RecordId);
            }
        }
        public int deliveryID { get; set; }
        //用户名
        private string _name;
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyPropertyChange(() => name);
            }
        }

        //图片路径
        private string _avatarUrl;
        public string avatarUrl
        {
            get
            {
                return _avatarUrl;
            }
            set
            {
                _avatarUrl = value;
                NotifyPropertyChange(() => avatarUrl);
            }
        }

        //年龄
        private string _age;
        public string age
        {
            get
            {
                return _age;
            }
            set
            {
                _age = value;
                NotifyPropertyChange(() => age);
            }
        }

        //工作年限
        private string _workingExp;
        public string workingExp
        {
            get
            {
                return _workingExp;
            }
            set
            {
                _workingExp = value;
                NotifyPropertyChange(() => workingExp);
            }
        }

        //学历
        private string _degree;
        public string degree
        {
            get
            {
                return _degree;
            }
            set
            {
                _degree = value;
                NotifyPropertyChange(() => degree);
            }
        }

        private long _jobId;
        public long jobId
        {
            get
            {
                return _jobId;
            }
            set
            {
                _jobId = value;
                NotifyPropertyChange(() => jobId);
            }
        }

        private string _resumeId;
        public string resumeId
        {
            get
            {
                return _resumeId;
            }
            set
            {
                _resumeId = value;
                NotifyPropertyChange(() => resumeId);
            }
        }

        //是否有新消息
        private bool _haveNewMsg;
        public bool HaveNewMsg
        {
            get
            {
                return _haveNewMsg;
            }
            set
            {
                _haveNewMsg = value;
                NotifyPropertyChange(() => HaveNewMsg);
            }
        }

        //未读信息数
        private int _unReadCount;
        public int UnReadCount
        {
            get
            {
                return _unReadCount;
            }
            set
            {
                _unReadCount = value;
                NotifyPropertyChange(() => UnReadCount);
            }
        }

        //最后通话时间
        private DateTime _lastTalk;
        public DateTime LastTalkTime
        {
            get
            {
                return _lastTalk;
            }
            set
            {
                _lastTalk = value;
                NotifyPropertyChange(() => LastTalkTime);
            }
        }
        private string _lastTalkMsg;
        public string LastTalkMsg
        {
            get
            {
                return _lastTalkMsg;
            }
            set
            {
                _lastTalkMsg = value;
                NotifyPropertyChange(() => LastTalkMsg);
            }
        }

        //简历投递职位
        private string _job;
        public string job
        {
            get
            {
                return _job;
            }
            set
            {
                _job = value;
                NotifyPropertyChange(() => job);
            }
        }

        private string _gender;
        public string gender
        {
            get
            {
                return _gender;
            }
            set
            {
                _gender = value;
                NotifyPropertyChange(() => gender);
            }
        }

        private bool _IsSelected;
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                _IsSelected = value;
                NotifyPropertyChange(() => IsSelected);
            }
        }
        public string flag { get; set; }
        public List<TagV> tagVs { get; set; }


        private ObservableCollection<ContentCommonReply> contentCommonReply = new ObservableCollection<ContentCommonReply>();
        public ObservableCollection<ContentCommonReply> ContentCommonReply
        {
            get { return contentCommonReply; }
            set
            {
                contentCommonReply = value;
                NotifyPropertyChange(() => ContentCommonReply);
            }
        }

        private bool isOpenResume = false;
        public bool IsOpenResum
        {
            get { return isOpenResume; }
            set
            {
                isOpenResume = value;
                NotifyPropertyChange(() => IsOpenResum);
            }
        }


        private ResumeDetailModel resumeDetail;
        public ResumeDetailModel ResumeDetail
        {
            get { return resumeDetail; }
            set
            {
                resumeDetail = value;
                NotifyPropertyChange(() => ResumeDetail);
            }
        }

        public ListType ListTypEnum { get; set; }


        private string _evaluation;
        public string evaluation
        {
            get
            {
                return _evaluation;
            }
            set
            {
                _evaluation = value;
                NotifyPropertyChange(() => evaluation);
            }
        }

        public string uniqueKey
        {
            get;
            set;
        }

        public ObservableCollection<MagicCube.ViewModel.HttpMessage> TalkMsgs
        {
            get;
            set;

        }

        private string sendMessageCache = string.Empty;
        public string SendMessageCache
        {
            get
            {
                return sendMessageCache;
            }
            set
            {
                this.sendMessageCache = value;
                //NotifyPropertyChange(() => SendMessageCache);
            }
        }

        #region "JobDynamic"


        private string updateTime = string.Empty;
        public string UpdateTime
        {
            get { return updateTime; }
            set
            {
                updateTime = value;
                NotifyPropertyChange(() => UpdateTime);
            }
        }

        private DateTime createTime;
        public DateTime CreateTime
        {
            get { return createTime; }
            set
            {
                createTime = value;
                NotifyPropertyChange(() => CreateTime);
            }
        }

        private bool isRead = true;
        public bool IsJobRead
        {
            get { return isRead; }
            set
            {
                isRead = value;
                NotifyPropertyChange(() => IsJobRead);
            }
        }

        private bool isCollectJob = false;
        public bool IsCollectJob
        {
            get { return isCollectJob; }
            set
            {
                isCollectJob = value;
                NotifyPropertyChange(() => IsCollectJob);
            }
        }


        private Visibility _gost = Visibility.Visible;
        public Visibility gost
        {
            get { return _gost; }
            set
            {
                _gost = value;
                NotifyPropertyChange(() => gost);
            }
        }
        #endregion

        #region Recommend
        private List<string> _lstPerInfo;
        public List<string> lstPerInfo
        {
            get
            {
                _lstPerInfo = new List<string>();
                if (!string.IsNullOrWhiteSpace(gender))
                {
                    _lstPerInfo.Add(gender);
                }
                if (!string.IsNullOrWhiteSpace(degree))
                {
                    _lstPerInfo.Add(degree);
                }
                if (!string.IsNullOrWhiteSpace(workingExp))
                {
                    _lstPerInfo.Add(workingExp);
                }

                return _lstPerInfo;
            }
            set
            {
                _lstPerInfo = value;
            }
        }
        private DateTime _RecommendTime;
        public DateTime RecommendTime
        {
            get { return _RecommendTime; }
            set
            {
                _RecommendTime = value;
                NotifyPropertyChange(() => RecommendTime);
            }
        }
        #endregion

    }

    public enum ListType
    {
        /// <summary>
        /// 沟通中
        /// </summary>
        CommunicateType,
        /// <summary>
        /// 感兴趣
        /// </summary>
        InterestType,
    }

}
