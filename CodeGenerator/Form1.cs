using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeGenerator.Rp;

namespace CodeGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // this.dataGridView1.DataSource = TableService.GetTableInfoes("V_ICS_DOOR_USG_INFO");
            this.dataGridView1.DataSource= TableService.GetMySqlTableInfoes("ak_order");
        }

         string ResolveColName(string colName)
        {
            var ns = colName.Split('_');
            var name = "";
            foreach (var item in ns)
            {
                name += item[0].ToString().ToUpper() + item.Substring(1).ToLower();
            }
            return name;
        }
        string ResolveString(string s)
        {
            return ResolveColName(s);
            var data = s.Split('_');
            string rlt = "";
            if (data.Length == 1)
            {
                return data[0];
            }
            foreach (var item in data)
            {

                rlt += item[0];
                rlt += item.Substring(1).ToLower();
            }

            return rlt;
        }

        //string GetMapperClassName(string tbName)
        //{
        //    var name = ResolveString(tbName);
        //    return $"{name}Mapper";
        //}

        string GenerateMapperCode(string colName)
        {
            var pn = ResolveString(colName);
            return $" Property(p => p.{pn}).HasColumnName(Field.{colName});";
        }
     
        //配置类
        StringBuilder GenerateClassCode(TableInfo t, string className, string modeClassName)
        {
            StringBuilder sb = new StringBuilder(600);
            sb.AppendLine("namespace Das.MICS.Domain.Repositories.Mappers\n{");

           sb.AppendLine(
                "    /// <summary>\n///     名称： \n///     描述： \n///     创建人：zhongwd" +
                "\n///     创建时间：2017-8-26\n///     版权所有 (C) :DAS\n/// </summary>");
            sb.AppendLine($"  public class {className} : EntityTypeConfiguration<{modeClassName}>");
            sb.AppendLine("{");
            sb.AppendLine($"public {className}()");
            sb.AppendLine("{");
            sb.AppendLine($" ToTable(\"{t.TableName}\");");
            foreach (var c in t.Columns)
            {
                sb.AppendLine(GenerateMapperCode(c.ColumnName));
            }
            // sb.AppendLine("Ignore(p => p.Id);");//ef
            sb.AppendLine("builder.Ignore(p => p.Id);");//ef core
            sb.AppendLine("}");
            sb.Append(GenerateFieldClassCode(t.Columns));
            sb.AppendLine("}");
            return sb;
        }

        StringBuilder GenerateFieldClassCode(List<ColumnInfo> cols)
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
        private void btnGenerate_Click(object sender, EventArgs e)
        {
           
            List<TableInfo> list = this.dataGridView1.DataSource as List<TableInfo>;
           
            StringBuilder addMapperCode =new StringBuilder(200);
            foreach (var t in list)
            {
                ModelGenerator.GenerateModel(t);
                var modeClassName = ResolveString(t.TableName);
                var className = $"{modeClassName}Mapper";
                var sb = GenerateClassCode(t, className, modeClassName);
                using (StreamWriter sw =
                    new StreamWriter(new FileStream("../../codes/" + className + ".cs", FileMode.Create,
                        FileAccess.Write)))
                {
                    sw.WriteLine(sb.ToString());
                }
                addMapperCode.AppendLine($"modelBuilder.Configurations.Add(new {className}());");
            }
            using (StreamWriter sw =
                new StreamWriter(new FileStream("../../codes/"  + "addMapperCode.txt", FileMode.Create,
                    FileAccess.Write)))
            {
                sw.WriteLine(addMapperCode.ToString());
            }
            MessageBox.Show("完成");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new RepositoryGenerator().GenerateCodes(); MessageBox.Show("完成");
        }
    }
}
