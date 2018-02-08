using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;

namespace CodeGenerator
{
    static class ConnectionManager
    {
        public const string MySqlConnectionString = "server=localhost;port=3306;user id=root;password=zt123;database=tzzx_dev; Allow Zero Datetime=True;";
        public const string ConnectionString = "Server=WIN-TPOTA8LLU3T;database=C3Metro0302;uid=sa;pwd=das123";
    }

  
    static class TableService
    {

        public static List<TableInfo> GetMySqlTableInfoes(string likeStr)
        {
            string sql = $" select Table_Name as TableName ,CREATE_TIME as CreateTime,Table_Name as ObjectId  from information_schema.tables t where t.table_schema='tzzx' and Table_Name like '{likeStr}%'";
            using (IDbConnection connection = new MySqlConnection(ConnectionManager.MySqlConnectionString))
            {
                var tables = connection.Query<TableInfo>(sql);
                List<TableInfo> tableInfos = new List<TableInfo>();
                foreach (var t in tables)
                {
                    var columns = connection.Query<ColumnInfo>(
                          $"select COLUMN_NAME as ColumnName from information_schema.columns  where table_name='{t.ObjectId}'");
                    t.Columns = columns.ToList();
                    tableInfos.Add(t);
                }

                return tableInfos;
            }
        }
        public static List<TableInfo> GetTableInfoes(string likeStr)
        {
            string sql = $"select name as TableName,create_date as CreateTime,object_id as ObjectId From sys.objects tb where name like '{likeStr}%' and (type='u' or type='v')";
            using (IDbConnection connection = new SqlConnection(ConnectionManager.ConnectionString))
            {
              var tables=  connection.Query<TableInfo>(sql);
                List<TableInfo> tableInfos = new List<TableInfo>();
                foreach (var t in tables)
                {
                  var columns=  connection.Query<ColumnInfo>(
                        $"select name as ColumnName from Sys.columns where object_id={t.ObjectId}");
                    t.Columns = columns.ToList();
                    tableInfos.Add(t);
                }

                return tableInfos;
            }
        }

       public static DataTable GetTables(string likeStr)
       {
           string sql = $"select name as TableName,create_date as CreateTime From sys.objects tb where name like '{likeStr}%' and type='u'";
           using (SqlConnection connection = new SqlConnection(ConnectionManager.ConnectionString))
           {
               using (var cmd = connection.CreateCommand())
               {
                   cmd.CommandText = sql;
                   cmd.CommandType = CommandType.Text;
                   connection.Open();
                   using (var datareader = cmd.ExecuteReader())
                   {
                       DataTable dt = new DataTable();
                       dt.Load(datareader);
                       return dt;
                   }
               }
           }

       }
    }
}
