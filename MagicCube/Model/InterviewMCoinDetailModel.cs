using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.Model
{
    public class InterviewMCoinDetailModel : NotifyBaseModel
    {
        private string mCoinRemain = string.Empty;
        /// <summary>
        /// M币余额
        /// </summary>
        public string MCoinRemain
        {
            get { return mCoinRemain; }
            set
            {
                this.mCoinRemain = value;
                this.NotifyPropertyChange(() => MCoinRemain);
            }
        }

        private string mCoinEnableUse = string.Empty;
        public string MCoinEnableUse
        {
            get { return mCoinEnableUse; }
            set
            {
                this.mCoinEnableUse = value;
                this.NotifyPropertyChange(() => MCoinEnableUse);
            }
        }

        private string mCoinFreeze = string.Empty;
        public string MCoinFreez
        {
            get { return mCoinFreeze; }
            set
            {
                this.mCoinFreeze = value;
                this.NotifyPropertyChange(() => MCoinFreez);
            }
        }
        
    }
    public class MCoinDetailListModel : NotifyBaseModel
    {
        private string operationName = string.Empty;
        public string OperationName
        {
            get { return operationName; }

            set
            {
                this.operationName = value;
                this.NotifyPropertyChange(() => OperationName);
            }
        }
        private string operationContent = string.Empty;
        public string OperationContent
        {
            get { return operationContent; }
            set
            {
                this.operationContent = value;
                this.NotifyPropertyChange(() => OperationContent);
            }
        }
        private string operationTime = string.Empty;
        public string OperationTime
        {
            get { return operationTime; }   
            set
            {
                this.operationTime = value;
                this.NotifyPropertyChange(() => OperationTime);
            }
        }
        private string mCoin = string.Empty;
        public string MCoin
        {
            get { return mCoin; }
            set
            {
                this.mCoin = value;
                this.NotifyPropertyChange(() => MCoin);
            }
        }

        private string operationLogoUrl = string.Empty;
        public string OperationLogoUrl
        {
            get { return operationLogoUrl; }
            set
            {
                this.operationLogoUrl = value;
                this.NotifyPropertyChange(() => OperationLogoUrl);
            }
        }
    }

}
