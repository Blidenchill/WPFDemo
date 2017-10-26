using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SuperSocket.ProtoBase;

namespace MagicCube.DAL
{
    public class NoReceiveFilter : IReceiveFilter<PushPackageInfo>
    {
        public IReceiveFilter<PushPackageInfo> NextReceiveFilter { get; set; }

        public FilterState State { get; set; }

        public PushPackageInfo Filter(BufferList data, out int rest)
        {
            rest = 0;
            return new PushPackageInfo(null, null, data[0].Array);
        }

        public void Reset()
        {

        }
    }
}
