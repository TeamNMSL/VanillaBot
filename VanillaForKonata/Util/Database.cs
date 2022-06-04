using System.Data.SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.Common;
using System.Collections;

namespace VanillaForKonata.Util
{
    public class Database
    {
        
        SQLiteConnection con;
        public void Open()
        {
            if (!File.Exists(path))
            {
                SQLiteConnection.CreateFile(path);
            }
            con = new SQLiteConnection("Data Source=" + path + "");

            SQLiteCommand com = new SQLiteCommand();
            com.Connection = con;
            com.CommandType = CommandType.Text;
            con.Open();

        }
        public SQLiteCommand com;
        public string path;
        public void setcmd(string cmdstr)
        {
            com.CommandText = cmdstr;

        }

        public Database(string path)
        {
            this.path = path;
            this.com = new SQLiteCommand();
            Open();
        }
        public string addParameters(string ParaName, string value)
        {
            SQLiteParameter para = new SQLiteParameter();
            para = new SQLiteParameter($"@{ParaName}", value);
            com.Parameters.Add(para);
            return "@"+ParaName;
        }
        public DataTable execute()
        {

            using (con = new SQLiteConnection("Data Source=" + path + ""))
            {
                com.Connection = con;
                com.CommandType = CommandType.Text;
                con.Open();
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(com);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                adapter.Dispose();
                return dataTable;
            }

        }

        public DataTable Insert(string TableName,KeyValuePair<string,string>[] keyValue) {
            List<string> Key = new ();
            List<string> Value = new ();
            foreach (var item in keyValue)
            {
                addParameters(item.Key,item.Value );
                Key.Add(item.Key);
                Value.Add($"@{item.Key}");
            }
            string cmd= $"INSERT INTO {TableName} ({string.Join(",", (Key.ToArray()))})  VALUES({string.Join(",", (Value.ToArray()))})";
            Console.WriteLine(cmd);
            setcmd(cmd);
            return execute();
        }
        public DataTable Select(string TableName,string Conditions) {
            
            string cmd = $"SELECT * from {TableName} WHERE {Conditions}";
            setcmd(cmd);
            return execute();
        }
        public DataTable Select(string TableName)
        {

            string cmd = $"SELECT * from {TableName}";
            setcmd(cmd);
            return execute();
        }
        public DataTable Create(string TableName,params string[] key) {
            List<string> keylist=new();
            foreach (var item in key)
            {
                keylist.Add($"'{item}'TEXT");
            }
            
            string cmd = $"CREATE TABLE '{TableName}' ({string.Join(",",keylist.ToArray() )});";
            setcmd(cmd);
            return execute();

        }
        public DataTable Delete(string TableName, string Conditions)
        {

            string cmd = $"delete from {TableName} WHERE {Conditions}";
            setcmd(cmd);
            return execute();
        }

    }
}



    

   