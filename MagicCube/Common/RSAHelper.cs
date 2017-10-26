using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MagicCube.Common
{
    public class RSAHelper
    {
        public static string DESEncrypt(string message)
        {
            DES des = new DESCryptoServiceProvider();
            des.Mode = CipherMode.ECB;
            des.Key = Encoding.UTF8.GetBytes("leedongj");
            des.IV = Encoding.UTF8.GetBytes("leedongj");

            byte[] bytes = Encoding.UTF8.GetBytes(message);
            byte[] resultBytes = des.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
            return Convert.ToBase64String(resultBytes);
        } 
    }
}
