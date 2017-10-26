using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.HttpModel
{

    public class HttpJobMsg
    {

        public List<string> workingExp
        {
            get;
            set;
        }
        public List<string> degree
        {
            get;
            set;
        }

    }

    public class HttpTypeCount
    {
        public string untreated
        {
            get;
            set;
        }

        public string pass
        {
            get;
            set;
        }

        public string reserveFail
        {
            get;
            set;
        }

        public string fail
        {
            get;
            set;
        }
    }

    public class HttpResumeCount
    {

        private int _reserve_fail;
        public int reserve_fail
        {
            get
            {
                return initiative_reserve_fail + invitation_reserve_fail;
            }
            set
            {
                _reserve_fail = value ;
            }
        }

        private int _untreated;
        public int untreated
        {
            get
            {
                return invitation_untreated + initiative_untreated;
            }
            set
            {
                _untreated = value ;
            }
        }

        private int _pass;
        public int pass
        {
            get
            {
                return initiative_pass + invitation_pass;
            }
            set
            {
                _pass = value; ;
            }
        }

        private int _fail;
        public int fail
        {
            get
            {
                return invitation_fail + initiative_fail;
            }
            set
            {
                _fail = value; ;
            }
        }
        private int _total;
        public int total
        {
            get
            {
                return invitation + initiative;
            }
            set
            {
                _total = value ;
            }
        }
        /// <summary>
        /// Invitation_reserve_fail
        /// </summary>
        public int invitation_reserve_fail { get; set; }
        /// <summary>
        /// Initiative_reserve_fail
        /// </summary>
        public int initiative_reserve_fail { get; set; }
        /// <summary>
        /// Invitation_untreated
        /// </summary>
        public int invitation_untreated { get; set; }
        /// <summary>
        /// Initiative_untreated
        /// </summary>
        public int initiative_untreated { get; set; }
        /// <summary>
        /// Initiative_pass
        /// </summary>
        public int initiative_pass { get; set; }
        /// <summary>
        /// Invitation_pass
        /// </summary>
        public int invitation_pass { get; set; }
        /// <summary>
        /// Invitation_fail
        /// </summary>
        public int invitation_fail { get; set; }
        /// <summary>
        /// Initiative_fail
        /// </summary>
        public int initiative_fail { get; set; }

        /// <summary>
        /// Recommend_VIEW_JOB
        /// </summary>
        public int recommend_VIEW_JOB { get; set; }
        /// <summary>
        /// Invitation
        /// </summary>
        public int invitation { get; set; }
        /// <summary>
        /// Initiative
        /// </summary>
        public int initiative { get; set; }
        /// <summary>
        /// Recommend_FAVORITE_JOB
        /// </summary>
        public int recommend_FAVORITE_JOB { get; set; }
        /// <summary>
        /// Recommend_recommendation
        /// </summary>
        public int recommend_recommendation { get; set; }

        /// <summary>
        /// Recommend
        /// </summary>
        public int recommend { get; set; }

    }

    public class HttpJobCount
    {
        public string _interviewedJobs;
        public string cinterviewedJobs
        {
            get
            {
                if (interviewedJobs != null)
                    return interviewedJobs.Count().ToString();
                else
                    return string.Empty;
            }
            set
            {
                _interviewedJobs = value;
            }
        }
        public string _interviewingJobs;
        public string cinterviewingJobs
        {
            get
            {
                if (interviewingJobs != null)
                    return interviewingJobs.Count().ToString();
                else
                    return string.Empty;
            }
            set
            {
                _interviewingJobs = value;
            }
        }

        public List<int> interviewedJobs { get; set; }
        public List<int> allJobs { get; set; }
        public List<int> interviewingJobs { get; set; }
    }
}
