using MagicCube.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicCube.Common
{
    public static class O2OHelper
    {
        public static async Task<ViewModel.BaseHttpModel> VisitConfirm(string userID, string jobID, string sessionRecordID,string state)
        {
            string std1 = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "jobID", "sessionRecordID", "state" },
                 new string[] { userID, jobID, sessionRecordID, state });
            string presult1 = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrInterviewVisitConfirm, MagicGlobal.UserInfo.Version, std1));
            return DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel>(presult1);
        }
        public static async Task<ViewModel.BaseHttpModel> RefuseConfirm(string userID, string jobID, string sessionRecordID, string state,string mobile,string name)
        {
            string std1 = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "jobID", "sessionRecordID", "state", "mobile","name" },
                 new string[] { userID, jobID, sessionRecordID, state, mobile,name });
            string presult1 = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrInterviewRefuseConfirm, MagicGlobal.UserInfo.Version, std1));
            return DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel>(presult1);
        }
        public static async Task<ViewModel.BaseHttpModel> ConfirmArrive(string userID, string jobID, string sessionRecordID)
        {
            return await VisitConfirm(userID, jobID, sessionRecordID, "10");
        }
        public static async Task<ViewModel.BaseHttpModel> UnConfirmArrive(string userID, string jobID, string sessionRecordID)
        {
            return await VisitConfirm(userID, jobID, sessionRecordID, "-10");
        }
        public static async Task<ViewModel.BaseHttpModel> RefuseArrive(string userID, string jobID, string sessionRecordID,string mobile,string name)
        {
            return await RefuseConfirm(userID, jobID, sessionRecordID, "-30", mobile,name);
        }

        public static async Task<InterviewQuestion> getQuestion(string userID, string jobID)
        {
            InterviewQuestion pInterviewQuestion = new InterviewQuestion();
            pInterviewQuestion.question = new List<InterviewQuestionItem>();
            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "jobID" },
                 new string[] { userID, jobID });
            string presult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrInterviewAnswer, MagicGlobal.UserInfo.Version, std));
            ViewModel.BaseHttpModel<ViewModel.HttpQuesion> model= DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel<ViewModel.HttpQuesion>>(presult);
            if(model!=null)
            {
                if(model.code==200)
                {
                    foreach(ViewModel.HttpQuestionList iquestionList in model.data.questionList)
                    {
                        if(iquestionList.questionType == "ESSAY")
                        {
                            InterviewQuestionItem pInterviewQuestionItem = new InterviewQuestionItem();
                            pInterviewQuestionItem.question = iquestionList.question;
                            pInterviewQuestionItem.answer = new List<string>();
                            pInterviewQuestionItem.answer.Add(iquestionList.essay);
                            pInterviewQuestion.question.Add(pInterviewQuestionItem);
                        }
                        else
                        {
                            InterviewQuestionItem pInterviewQuestionItem = new InterviewQuestionItem();
                            pInterviewQuestionItem.question = iquestionList.question;
                            pInterviewQuestionItem.answer = new List<string>();
                            foreach(ViewModel.HttpQuestionItemList iHttpQuestionItemList in iquestionList.questionItemList)
                            {
                                if(iHttpQuestionItemList.selected == true)
                                    pInterviewQuestionItem.answer.Add(iHttpQuestionItemList.option);
                            }
                            pInterviewQuestion.question.Add(pInterviewQuestionItem);

                        }
                    }
                }
            }

            return pInterviewQuestion;

        }
    }
}
