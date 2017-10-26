
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace MagicCube.Common
{
    public static class SQLiteHelper
    {
        public static bool ExecuteNoQuery(string UserName, string sql)
        {

            SQLiteConnectionStringBuilder sqlitestring = new SQLiteConnectionStringBuilder();
            sqlitestring.DataSource = getMsgDBPath(UserName);
            sqlitestring.Pooling = true;
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(sqlitestring.ToString()))
                {
                    SQLiteCommand cmd = new SQLiteCommand();
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Connection = conn;
                    cmd.ExecuteNonQuery();
                    sqlitestring.Clear();
                    cmd.Connection = null;
                    conn.Close();
                }
            }
            catch 
            {
               
                return false;
            }
            return true;
        }


        public static DataTable ExecuteQuery(string UserName, string sql)
        {
            DataTable dt = new DataTable();
            SQLiteConnectionStringBuilder sqlitestring = new SQLiteConnectionStringBuilder();
            sqlitestring.DataSource = getMsgDBPath(UserName);
            sqlitestring.Pooling = true;
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(sqlitestring.ToString()))
                {
                    SQLiteCommand cmd = new SQLiteCommand();
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Connection = conn;
                    SQLiteDataReader sdr = cmd.ExecuteReader();
                    dt.Load(sdr);
                    sqlitestring.Clear();
                    cmd.Connection = null;
                    conn.Close();

                }
            }
            catch 
            {
               
            }
            return dt;
        }

        public static string getMsgDBPath(string UserName)
        {
            string dbPath = DAL.ConfUtil.LocalHomePath + UserName + "/" + "Msg.db";
            return dbPath;

        }

        public static void CreatTable(string UserName)
        {
            if (!Directory.Exists(DAL.ConfUtil.LocalHomePath + UserName + "/"))
            {
                // Create the directory it does not exist.
                Directory.CreateDirectory(DAL.ConfUtil.LocalHomePath + UserName + "/");
            }
            //创建.db文件
            string dbPath = DAL.ConfUtil.LocalHomePath + UserName + "/" + "Msg.db";
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
                string sql = "create table IF NOT EXISTS msg(contactRecordId INTEGER, msg TEXT,SRType INTEGER,time DATETIME);";
                sql += "create table IF NOT EXISTS unreadmsg(contactRecordId INTEGER, msg TEXT,time DATETIME);";
                sql += "create table IF NOT EXISTS recent(contactRecordId INTEGER,id INTEGER,jobId INTEGER,jobName TEXT,name TEXT,avatarUrl TEXT,age TEXT,workingExp TEXT,degree TEXT,LastTalkTime DATETIME);";
                ExecuteNoQuery(UserName, sql);
            }
        }


    }

}
