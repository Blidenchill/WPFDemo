using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Resources;

namespace MagicCube.Common
{
    public class ResXResourcHelper
    {
        public static void WriteResourceFile(string filePath, List<string> key, List<string> value)
        {
            System.Resources.ResXResourceWriter writer = new System.Resources.ResXResourceWriter(filePath);
            for(int i = 0; i < key.Count(); i++)
            {
                writer.AddResource(key[i], value[i]);
            }
          
            writer.Generate();
            writer.Close();
        }

        public static List<string> ReadResourceKeyList(string filePath)
        {
            List<string> temp = new List<string>();
            using (System.Resources.ResXResourceReader resxReader = new System.Resources.ResXResourceReader(filePath))
            {
                foreach (DictionaryEntry entry in resxReader)
                {
                    temp.Add(entry.Key.ToString());
                }
            }
            return temp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseName">MagicCube.文件名（不加扩展名）</param>
        /// <param name="key">查询键值</param>
        /// <returns></returns>
        public static string GetResourceValue(string baseName ,string key)
        {
            try
            {
                ResourceManager rs = new ResourceManager(baseName, System.Reflection.Assembly.GetExecutingAssembly());
                return rs.GetObject(key).ToString();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}
