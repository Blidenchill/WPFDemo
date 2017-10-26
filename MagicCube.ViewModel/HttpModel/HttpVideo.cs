using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.ViewModel
{
    public class HttpVideoSig
    {
        public string sig
        {
            get;
            set;
        }
    }

    public class HttpVideoInvite
    {
        public string roomID
        {
            get;
            set;
        }
    }

    public class HttpVideoInviteParams
    {
        public long fromUserID
        {
            get;
            set;
        }
        public long fromUserIdentity
        {
            get;
            set;
        }
        public long toUserID
        {
            get;
            set;
        }
        public long toUserIdentity
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
}
