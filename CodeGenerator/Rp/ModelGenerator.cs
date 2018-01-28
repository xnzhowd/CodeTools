/**************************************************************
*项目名称：
*项目描述：
*类 名 称：ModelGenerator
*版 本 号：v1.0.0.0
*作    者：xnzhw
*创建时间：2018/1/21 10:33:51
***************************************************************
* Copyright @ xnzhw 2018. All rights reserved.
***************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Rp
{
    /// <summary>
    /// 作者：zhongwd
    /// 时间：2018-01-20
    /// 描述：ModelGenerator
    /// </summary>
     class ModelGenerator
    {
      static   string ResolveColName(string colName)
        {
            var ns = colName.Split('_');
            var name = "";
            foreach(var item in ns)
            {
                name += item[0].ToString().ToUpper() + item.Substring(1).ToLower();
            }
            return name;
        }
        public static void GenerateModel(TableInfo tableInfo,string path=null)
        {
            StringBuilder sb = new StringBuilder(1000);
            var className = tableInfo.TableName;
          //  var tplStr = match.Groups[2].Value;
            sb.AppendLine("//auto generate");
            sb.Append("//");
           // sb.AppendLine(Path.GetFileName(interfacePath));
            sb.AppendLine("namespace Das.MICS.Domain.Repositories{\n");
            sb.AppendLine(" /// <summary>");
            sb.AppendLine($"  ///     名称： {className}");
            sb.AppendLine($"  ///     描述：  {className}");
            sb.AppendLine(@" ///     创建人：zhongwd
            ///     创建时间："+DateTime.Now.ToString("yyyy-MM-dd")+ @"
            ///     版权所有 (C) :DAS
            /// </summary>\n");

            sb.Append("public class ");

            sb.Append(className);
            sb.Append("\n{\n\t");
            foreach(var col in tableInfo.Columns)
            {
                var name = ResolveColName(col.ColumnName);
                sb.Append($"public string {name}"+"{get;set;}\n\t");
            }
            sb.Append("}");
            string filePath =  $"../../codes/{className}.cs";
            using (StreamWriter sw = new StreamWriter(new FileStream(filePath, FileMode.Create, FileAccess.Write)))
            {
                sw.WriteLine(sb.ToString());
            }
        }
    }
}
