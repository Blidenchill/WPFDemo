using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SuperSocket.ProtoBase;

namespace MagicCube.DAL
{
    public class PushPackageInfo : IPackageInfo
    {
        public PushPackageInfo(byte[] header, byte[] bodyBuffer, byte[] allBuffer)
        {
            Header = header;
            Data = bodyBuffer;
            AllData = allBuffer;
        }

        /// <summary>
        /// 服务器返回的字节数据头部
        /// </summary>
        public byte[] Header { get; set; }
       
        /// <summary>
        /// 服务器返回的字节数据
        /// </summary>
        public byte[] Data { get; set; }
        /// <summary>
        /// 服务器返回的整个数据包
        /// </summary>
        public byte[] AllData { get; set; }
        /// <summary>
        /// 服务器返回的数据长度
        /// </summary>
        public int DataLength
        {
            get
            {
                if (Data != null)
                {
                    return Data.Length;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// 服务器返回的整个数据包长度
        /// </summary>
        public int AllLength
        {
            get
            {
                if (Header != null && Data != null)
                {
                    return Header.Length + Data.Length;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// 服务器返回的字符串数据
        /// </summary>
        public string Body
        {
            get
            {
                return Encoding.UTF8.GetString(Data);
            }
        }

        public string AllDataJson
        {
            get
            {
                return Encoding.UTF8.GetString(AllData);
            }
        }

       
    }
}
