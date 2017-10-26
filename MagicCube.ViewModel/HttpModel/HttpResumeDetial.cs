using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.ViewModel
{
    public class HttpResumeDetial
    {
        //        userID	用户ID	long	否	主键
        //name	姓名	string	否	
        //avatar	头像地址	string	否	
        //（以下为求职者信息）				
        //jhIdentityStatus	求职者身份状态	int	是	
        //gender	性别	int	是	
        //birthdate	出生日期	string	否	
        //marriage	婚姻状态	int	是	
        //education	学历	int	是	可回填
        //joinWorkDate	参加工作时间	string	否	年月,特殊年0000年表示在读，0001年表示应届
        //email	个人邮箱	string	否	
        //mobile	个人联系方式	string	否	
        //province	当前所在地_省	string	是	
        //city	当前所在地_市	string	是	
        //district	当前所在地_区	string	是
        //minSalary	期望工资_最小	int	是	1 ~ 100 , 0代表面议
        //maxSalary	期望工资_最大	int	是	1 ~ 100 , 0代表面议
        //careerObjectiveProvince	期望工作地点_省	string	是	
        //careerObjectiveCity	期望工作地点_市	string	是	
        //careerObjectiveDistrict	期望工作地点_区	string	是	
        //exptPosition	期望职位	string	否	存的是三级职能code；多个用逗号(,)分隔
        //exptType	求职类型	int	是	
        //exptIndustry	期望行业	string	否	多个用逗号(,)分隔
        //advantage	个人优势	string	否	
        //selfEval	自我评价	string	否	
        //language	掌握语言	string	否	，号分割
        //resumeCreateTime	简历创建时间	dateTime	否	
        //resumeSource	简历来源	int	是	
        //skill	专业技能	string	否	
        //personExperienceJob	个人经历_工作	Array	否	存储是个数组，数组中数据结构对应个人经历_工作
        //personExperienceProject	个人经历_项目	Array	否	存储是个数组，数组中数据结构对应个人经历_项目
        //personExperienceEducation	个人经历_教育	Array	否	存储是个数组，数组中数据结构对应个人经历_教育
        public long userID
        {
            get;
            set;
        }

        public string name
        {
            get;
            set;
        }
        public string avatar
        {
            get;
            set;
        }
        public int gender
        {
            get;
            set;
        }
        public bool isViewRelation
        {
            get;
            set;
        }
        public bool collectResume
        {
            get;
            set;
        }

        public int accountStatus
        {
            get;
            set;

        }
        public string genderDesc
        {
            get;
            set;
        }

        public string birthdate
        {
            get;
            set;
        }
        public string birthYear
        {
            get;
            set;
        }
        public string age
        {
            get;
            set;
        }
        public int education
        {
            get;
            set;
        }
        public string educationDesc
        {
            get;
            set;
        }
        //年月,特殊年0000年表示在读，0001年表示应届
        public string joinWorkDate
        {
            get;
            set;
        }
        public string workingExp
        {
            get;
            set;
        }
        public string email
        {
            get;
            set;
        }
        public string mobile
        {
            get;
            set;
        }
        public string province
        {
            get;
            set;
        }
        public string city
        {
            get;
            set;
        }
        public string district
        {
            get;
            set;
        }

        public int minSalary
        {
            get;
            set;
        }
        public int maxSalary
        {
            get;
            set;
        }
        public string careerObjectiveArea
        {
            get;
            set;
        }
        public string careerObjectiveAreaDesc
        {
            get;
            set;
        }
        public string exptPosition
        {
            get;
            set;
        }
        public string exptPositionDesc
        {
            get;
            set;
        }

        public string exptIndustry
        {
            get;
            set;
        }
        public string exptIndustryDesc
        {
            get;
            set;
        }

        public string status
        {
            get;
            set;
        }
        public string statusDesc
        {
            get;
            set;
        }

        //        advantage	个人优势	string	否	
        //selfEval	自我评价	string	否	
        //language	掌握语言	string	否	，号分割
        //resumeCreateTime	简历创建时间	dateTime	否	
        //resumeSource	简历来源	int	是	
        //skill	专业技能	string	否	
        //personExperienceJob	个人经历_工作	Array	否	存储是个数组，数组中数据结构对应个人经历_工作
        //personExperienceProject	个人经历_项目	Array	否	存储是个数组，数组中数据结构对应个人经历_项目
        //personExperienceEducation	个人经历_教育	Array	否	存储是个数组，数组中数据结构对应个人经历_教育
        public int resumeSource
        {
            get;
            set;
        }
        public string resumeSourceDesc
        {
            get;
            set;
        }
        public string advantage
        {
            get;
            set;
        }
        public string customSelfLabel
        {
            get;
            set;
        }
        public string selfEval
        {
            get;
            set;
        }
        public string language
        {
            get;
            set;
        }
        public string personExperienceJob
        {
            get;
            set;
        }
        public string personExperienceProject
        {
            get;
            set;
        }
        public string personExperienceEducation
        {
            get;
            set;
        }
        //public string loaction
        //{
        //    get
        //    {
        //        if (string.IsNullOrWhiteSpace(district))
        //            return district;
        //        if (string.IsNullOrWhiteSpace(province))
        //            return province;
        //        if (string.IsNullOrWhiteSpace(province))
        //            return province;
        //        return string.Empty;
        //    }
        //    set
        //    {
        //        value = null;
        //    }
        //}
        public DateTime resumeUpdateTime
        {
            get;
            set;
        }

        public List<HttpresumeComments> resumeComments
        {
            get;
            set;
        }
    }
    public class HttpresumeComments
    {
        /// <summary>
        /// 嘿嘿嘿嘿
        /// </summary>
        public string comment { get; set; }
        /// <summary>
        /// CanDelete
        /// </summary>
        public bool canDelete { get; set; }
        /// <summary>
        /// CompanyID
        /// </summary>
        public int companyID { get; set; }
        /// <summary>
        /// HrUserID
        /// </summary>
        public int hrUserID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ownerName { get; set; }
        /// <summary>
        /// 刘佳
        /// </summary>
        public string hrName { get; set; }
        /// <summary>
        /// JhUserID
        /// </summary>
        public int jhUserID { get; set; }
        /// <summary>
        /// CommentID
        /// </summary>
        public int commentID { get; set; }
        /// <summary>
        /// 2017-02-23 11:10:42
        /// </summary>
        public DateTime createTime { get; set; }
    }

    public class HttpRecommendResume
    {
        public long userID
        {
            get;
            set;
        }
        public string name
        {
            get;
            set;
        }
        public string mobile
        {
            get;
            set;
        }
        public long jobID
        {
            get;
            set;
        }
        public string jobName
        {
            get;
            set;
        }
        public string avatar
        {
            get;
            set;
        }
        public int gender
        {
            get;
            set;
        }
        public string genderDesc
        {
            get;
            set;
        }
        public int education
        {
            get;
            set;
        }
        public string educationDesc
        {
            get;
            set;
        }
        public string workingExp
        {
            get;
            set;
        }

    }
    public class HttpRecommend
    {
        public int count { get; set; }
        /// <summary>
        /// recommend_resume_v1.0
        /// </summary>
        public string algorithmID { get; set; }
        /// <summary>
        /// ResumeList
        /// </summary>
        public List<HttpRecommendResume> resumeList { get; set; }
        /// <summary>
        /// PageCount
        /// </summary>
        public int pageCount { get; set; }
    }

    public class HttpResumeTable
    {
        public long userID
        {
            get;
            set;
        }
        public string name
        {
            get;
            set;
        }
        public string avatar
        {
            get;
            set;
        }
        public int gender
        {
            get;
            set;
        }
        public string genderDesc
        {
            get;
            set;
        }

        public string birthdate
        {
            get;
            set;
        }
        public string birthYear
        {
            get;
            set;
        }
        public string mobile
        {
            get;
            set;
        }
        public string email
        {
            get;
            set;
        }

        public string age
        {
            get;
            set;
        }
        public int education
        {
            get;
            set;
        }
        public string educationDesc
        {
            get;
            set;
        }
        //年月,特殊年0000年表示在读，0001年表示应届
        public string joinWorkDate
        {
            get;
            set;
        }
        public string workingExp
        {
            get;
            set;
        }

        public string province
        {
            get;
            set;
        }
        public string city
        {
            get;
            set;
        }
        public string district
        {
            get;
            set;
        }
        public string personExperienceJob
        {
            get;
            set;
        }
        public DateTime resumeUpdateTime
        {
            get;
            set;
        }

        public int minSalary
        {
            get;
            set;
        }
        public int maxSalary
        {
            get;
            set;
        }
        public string careerObjectiveArea
        {
            get;
            set;
        }
        public string careerObjectiveAreaDesc
        {
            get;
            set;
        }
        public string exptPosition
        {
            get;
            set;
        }
        public string exptPositionDesc
        {
            get;
            set;
        }

        public string exptIndustry
        {
            get;
            set;
        }
        public string exptIndustryDesc
        {
            get;
            set;
        }

        public string status
        {
            get;
            set;
        }
        public string statusDesc
        {
            get;
            set;
        }

    }
    public class HttpResumeCount
    {
        public int initiative
        {
            get;
            set;
        }
        public int invite
        {
            get;
            set;
        }
        public int pass
        {
            get;
            set;
        }
        public int fail
        {
            get;
            set;
        }
        public int autoFilter
        {
            get;
            set;
        }
    }
    public class HttpCollectionCount
    {
        public int total
        {
            get;
            set;
        }
    }

    public class HttpRecommendRed
    {
        public bool viewJobReverse { get; set; }

        public bool collectJobReverse { get; set; }

    }

    public class HttpMobileRoot
    {
        /// <summary>
        /// Mobile
        /// </summary>
        public HttpMobile mobile { get; set; }
    }
    public class HttpMobile
    {
        /// <summary>
        /// 13671237527
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// 986159139@qq.com
        /// </summary>
        public string email { get; set; }
    }

    public class HttpPhoneRoot
    {
        /// <summary>
        /// 15
        /// </summary>
        public string freePhoneID { get; set; }
        /// <summary>
        /// Contact
        /// </summary>
        public HttpMobile contact { get; set; }
    }

}



