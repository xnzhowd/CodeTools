
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.SqlSugars
{

    class MappingHelper
    {
        static string ResolveColName(string colName)
        {
            var ns = colName.Split('_');
            var name = "";
            foreach (var item in ns)
            {
                name += item[0].ToString().ToUpper() + item.Substring(1).ToLower();
            }
            return name;
        }
        static string GenerateMapperCode(string colName)
        {
            var pn = ResolveColName(colName);
            return $" Property(p => p.{pn}).HasColumnName(Field.{colName});";
        }
        static StringBuilder GenerateFieldClassCode(List<ColumnInfo> cols)
        {
            StringBuilder sb = new StringBuilder(500);
            sb.AppendLine(" private class Field{");
            foreach (var c in cols)
            {
                sb.AppendLine($" public const string {c.ColumnName} =\"{c.ColumnName}\";");
            }

            sb.AppendLine("}\n}");
            return sb;
        }
        public static  StringBuilder GenerateClassCode(TableInfo t, string className, string modeClassName)
        {
            StringBuilder sb = new StringBuilder(600);
            sb.AppendLine("namespace Das.MICS.Domain.Repositories.Mappers\n{");

            sb.AppendLine(
                 "    /// <summary>\n///     名称： \n///     描述： \n///     创建人：zhongwd" +
                 "\n///     创建时间：2017-8-26\n///     版权所有 (C) :DAS\n/// </summary>");
            sb.AppendLine($"  public class {className} : MappingColumnList");
            sb.AppendLine("{");
            //sb.AppendLine($"public {className}()");
            sb.AppendLine($"public const string EntityName= \"{modeClassName}\";");
            sb.AppendLine($"public const string DbTableName= \"{t.TableName}\";");
            sb.AppendLine("this.MappingTables.Add(new MappingTable() { EntityName = "+ className + ".EntityName, DbTableName =  " + className + ".DbTableName});");
            //sb.AppendLine("this.MappingTables.Add(new MappingTable(){EntityName=\""+
            //    modeClassName+ "\",DbTableName=\""+ t.TableName+"\"});");

            sb.AppendLine($"public {className}()");
            sb.AppendLine("{");
            sb.AppendLine($"string entityName=EntityName;");
            //  sb.AppendLine($" ToTable(\"{t.TableName}\");");
            foreach (var c in t.Columns)
            {
                var pn = ResolveColName(c.ColumnName);
                sb.AppendLine("this.Add(new MappingColumn(){ EntityName =entityName,PropertyName =\"" + pn + "\",DbColumnName = Field." + c.ColumnName + "});");
              //  sb.AppendLine(GenerateMapperCode(c.ColumnName));
            }
            // sb.AppendLine("Ignore(p => p.Id);");//ef
         //   sb.AppendLine("builder.Ignore(p => p.Id);");//ef core
            sb.AppendLine("}");
            sb.Append(GenerateFieldClassCode(t.Columns));
            sb.AppendLine("}");
            return sb;
        }
    }
}
