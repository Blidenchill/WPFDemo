using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicCube.DAL
{
    public class PushReceiveFilter :FixedHeaderReceiveFilter<PushPackageInfo>
    {
        private int dataLen = 0;
        public PushReceiveFilter()
        : base(4)
        {
            //回复格式：地址 功能码 字节数 数值……
            //01 03 02 00 01   地址:01 功能码:03 字节数:02 数据:00 01
        }

        protected override int GetBodyLengthFromHeader(IBufferStream bufferStream, int length)
        {
            ArraySegment<byte> buffers = bufferStream.Buffers[0];
            byte[] array = buffers.Array;
            byte[] temp = new byte[length];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = array[length -1 - i];
            }
            int len = BitConverter.ToInt32(temp, 0);
            this.dataLen = len;
            return len;
        }
        public override PushPackageInfo ResolvePackage(IBufferStream bufferStream)
        {

            byte[] header = new byte[bufferStream.Buffers[0].Count];
            for(int i = 0; i < header.Length; i++)
            {
                header[i] = bufferStream.Buffers[0].Array[i + bufferStream.Buffers[0].Offset];
            }

            //body
            byte[] bodyBuffer = new byte[this.dataLen];
           
            int len = 0;
            for(int j = 1; j < bufferStream.Buffers.Count; j++)
            {
                int tag = 0;
                for (int i = bufferStream.Buffers[j].Offset; i < bufferStream.Buffers[j].Offset+bufferStream.Buffers[j].Count; i++)
                {
                    bodyBuffer[len + tag] = bufferStream.Buffers[j].Array[i];
                    tag++;
                }
                len += bufferStream.Buffers[j].Count;
            }
            return new PushPackageInfo(header, bodyBuffer, null);
        }

    }
}
