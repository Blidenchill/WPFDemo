using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MagicCube.ViewModel
{
    public class CodeTable
    {
        private static CodeTable instance;
        private static object objLock = new object();
        public static CodeTable Instance
        {
            get
            {
                if(instance == null)
                {
                    lock (objLock)
                    {
                        if (instance == null)
                            instance = new CodeTable();
                    }
                }
                return instance;
            
            }
        } 
        public List<CodeTableModel> educationList = new List<CodeTableModel>();

        private CodeTable()
        {
            this.InitialEducationList();
        }

        private void InitialEducationList()
        {
            educationList.Add(new CodeTableModel() { name = "不限", code = 1 });
            educationList.Add(new CodeTableModel() { name = "高中/中专及以下", code = 2 });
            educationList.Add(new CodeTableModel() { name = "大专", code = 3 });
            educationList.Add(new CodeTableModel() { name = "本科", code = 4 });
            educationList.Add(new CodeTableModel() { name = "硕士", code = 5 });
            educationList.Add(new CodeTableModel() { name = "博士", code = 6 });

        }
        public  string GetEducation(int code)
        {
            
           
            foreach(var item in educationList)
            {
                if (item.code == code)
                    return item.name;
            }
            return string.Empty;

        }
    }

    public class CodeTableModel
    {
        public string name { get; set; }
        public int code { get; set; }
    }
}
