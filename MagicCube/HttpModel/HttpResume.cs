using MagicCube.Common;
using MagicCube.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MagicCube.HttpModel
{

    public class ResumeInfo
    {

        public  HttpContactRecord contactRecord
        {
            get;
            set;
        }
        public HttpResume resume
        {
            get;
            set;
        }

        public string jobName
        {
            get;
            set;
        }

        public string minSalary
        {
            get;
            set;
        }
        public string maxSalary
        {
            get;
            set;
        }

    }

    public class HttpResume: NotifyBaseModel
    {
        public List<HttpCareer> career
        {
            get;
            set;
        }
        public List<HttpEducation> education
        {
            get;
            set;
        }

        public List<HttpProjects> projects
        {
            get;
            set;
        }
        public System.Windows.Visibility careerVis
        {
            get
            {
                if(career==null)
                {
                    return System.Windows.Visibility.Collapsed;
                }
                if (career.Count==0)
                {
                    return System.Windows.Visibility.Collapsed;
                }
                return System.Windows.Visibility.Visible;
            }
            set
            {
               
            }
        }
        public System.Windows.Visibility educationVis
        {
            get
            {
                if (education == null)
                {
                    return System.Windows.Visibility.Collapsed;
                }
                if (education.Count == 0)
                {
                    return System.Windows.Visibility.Collapsed;
                }
                return System.Windows.Visibility.Visible;
            }
            set
            {

            }
        }

        public System.Windows.Visibility projectsVis
        {
            get
            {
                if (projects == null)
                {
                    return System.Windows.Visibility.Collapsed;
                }
                if (projects.Count == 0)
                {
                    return System.Windows.Visibility.Collapsed;
                }
                return System.Windows.Visibility.Visible;
            }
            set
            {

            }
        }
        private string _imName;
        public string imName
        {
            get 
            {
                return _imName; 
            }
            set
            {
                _imName = value;
                NotifyPropertyChange(() => imName);
            }
        }

        private string _moblie;
        public string mobile
        {
            get 
            {
                return _moblie; 
            }
            set
            {
                _moblie = value;
                NotifyPropertyChange(() => mobile);
            }
        }

         private string _email;
         public string email
        {
           get 
            {
                return _email; 
            }
            set
            {
                _email = value;
                NotifyPropertyChange(() => email);
            }
        }

         public bool _isRecord;
        public bool isRecord
        {
           get 
            {
                return _isRecord; 
            }
            set
            {
                showContactInformation = value;
                _isRecord = value;
                NotifyPropertyChange(() => isRecord);
            }
        }
        public bool _showContactInformation;
        public bool showContactInformation
        {
           get 
            {
                return _showContactInformation; 
            }
            set
            {
                _showContactInformation = value;
                NotifyPropertyChange(() => showContactInformation);
            }
        }
        
        public List<string> targetJobTypes
        {
            get;
            set;
        }

        private string _targetSalary;

        public string targetSalary
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_targetSalary))
                    return "不限";
                return _targetSalary;
            }
            set
            {
                _targetSalary = value;
            }
        }

        private string _targetWorkLocation;
        public string targetWorkLocation
        {
            get
            {
                if (postoperationTargetWorkLocations!=null)
                {
                    if(postoperationTargetWorkLocations.Count()>0)
                    {
                        string t = postoperationTargetWorkLocations[0];
                        for (int i = 1; i < postoperationTargetWorkLocations.Count; i++)
                        {
                            t += "/" + postoperationTargetWorkLocations[i];
                        }
                        return t;
                    }
                    else
                    {
                        return _targetWorkLocation;
                    }
                }
                    else
                    {
                         return _targetWorkLocation;
                    }
             
               
            }
            set
            {
                _targetWorkLocation = value;
            }
        }
        public List<string>  postoperationTargetWorkLocations
        {
            get;
            set;
        }
        public List<string> targetPosition
        {
            get;
            set;
        }



        public string integrity
        {
            get;
            set;
        }

        public int id
        {
            get;
            set;
        }
        public bool hasResume
        {
            get;
            set;
        }
        public string workingExp
        {
            get;
            set;
        }

        public string avatarUrl
        {
            get;
            set;
        }

        public string source
        {
            get;
            set;
        }

        public string skills
        {
            get;
            set;
        }
        public string location
        {
            get;
            set;
        }
        public string degree
        {
            get;
            set;
        }

        public string _degreeschool;
        public string degreeschool
        {
            get
            {
                if (education != null)
                {
                    HttpEducation pHttpEducation = education.FirstOrDefault(x => x.degree == degree);
                    if (pHttpEducation != null)
                        return degree + "(" + pHttpEducation.schoolName + ")";
                    else
                        return degree + "(" + education.Last().schoolName + ")";
                }
                else
                {
                    return degree;
                }
            }
            set
            {
                _degreeschool = value;
                NotifyPropertyChange(() => degreeschool);
            }
        }

        public bool deleted
        {
            get;
            set;
        }

        public bool searchEnabled
        {
            get;
            set;
        }

        public string qq
        {
            get;
            set;
        }

        public string updatedTime
        {
            get;
            set;
        }
        private string _name;
        public string name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_name))
                {
                    return  VTHelper.Phone(mobile);
                }
                else
                {
                    return _name;
                }
                
            }
            set
            {
                _name = value;

            }
        }
        public string gender
        {
            get;
            set;
        }

        public string _age;
        public string age
        {
            get
            {
               return TimeHelper.getAge(dob);
            }
            set
            {
                _age = value;

            }
        }

        public string dob
        {
            get;
            set;
        }
        public string selfIntroduction
        {
            get;
            set;
        }

        public string createdTime
        {
            get;
            set;
        }

        public ObservableCollection<TagV> tagVs { get; set; }

        public string _flag;
        public string flag
        {
            get
            {
                return _flag;
            }
            set
            {
                _flag = value;
                NotifyPropertyChange(() => flag);
            }
        }

        public string sendTime
        {
            get;
            set;
        }

        public int recordId
        {
            get;
            set;
        }
    }

    public class HttpCareer
    {
        public string start
        {
            get;
            set;
        }
        public string jobName
        {
            get;
            set;
        }
        public string end
        {
            get;
            set;
        }
        public string jobDescription
        {
            get;
            set;
        }
        public string companyName
        {
            get;
            set;
        }

    }

    public class HttpEducation
    {
        public string start
        {
            get;
            set;
        }
        public string major
        {
            get;
            set;
        }
        public string end
        {
            get;
            set;
        }
        public string degree
        {
            get;
            set;
        }
        public string schoolName
        {
            get;
            set;
        }
    }

    public class HttpProjects
    {
        public string projectDescription
        {
            get;
            set;
        }

        public string end
        {
            get;
            set;
        }

        public string companyName
        {
            get;
            set;
        }

        public string projectName
        {
            get;
            set;
        }

        public string start
        {
            get;
            set;
        }

        public string responsibility
        {
            get;
            set;
        }
    }

    public class Httpinterview
    {
        public int id
        {
            get;
            set;
        }


        public string createdTime
        {
            get;
            set;
        }

        public string interviewEvaluation
        {
            get;
            set;
        }

        public string interviewResult
        {
            get;
            set;
        }
    }
    public class HttpInterviewResume
    {
        public HttpResume resume { get; set; }
    }

}
