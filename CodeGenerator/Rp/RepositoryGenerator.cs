using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeGenerator.Rp
{
    public class RepositoryGenerator
    {
        //private string InterfacePath = @"F:\projects\Metro\codes\MICS\DMS系统\Das.MICS.Domain";
        //private string ImplenmentPath = @"F:\projects\Metro\codes\MICS\DMS系统\Das.MICS.Domain.Repositories";
        private const string preDir = @"E:\work\das\Metro";
        private string InterfacePath = preDir + @"\MICS\DMS系统\Das.MICS.Domain";
        private string ImplenmentPath = preDir+@"\MICS\DMS系统\Das.MICS.Domain.Repositories";

        bool IsRepositoryFile(string filePath)
        {
           var fn= Path.GetFileName(filePath);
          return  fn.Contains("Repository");
        }
        List<string> GetRepositoryFiles(string path)
        {
            var files = Directory.GetFiles(path);
            var list = new List<string>();
            foreach (var f in files)
            {
                if (IsRepositoryFile(f))
                {
                    list.Add(f);
                }
            }

            var paths = Directory.GetDirectories(path);
            foreach (var p in paths)
            {
                list.AddRange(GetRepositoryFiles(p));
            }

            return list;
        }
    
        List<string> GetToGenerateFiles()
        {
            List<string> impList = GetRepositoryFiles(ImplenmentPath);
            List<string> interfacList = GetRepositoryFiles(InterfacePath);
            for (var i = interfacList.Count - 1; i >= 0; i--)
            {
                var fileName = Path.GetFileName(interfacList[i]);
                var implName = fileName.Substring(1);
                if (impList.Any(o => Path.GetFileName(o)== implName))
                {
                    interfacList.RemoveAt(i);
                }
            }

            return interfacList;
        }

        public void GenerateCodes()
        {
            var list = GetToGenerateFiles();
            foreach (var item in list)
                GenerateImplFile(item);
        }

        void GenerateImplFile(string interfacePath)
        {
            string content = string.Empty;
            using (StreamReader sr = new StreamReader(new FileStream(interfacePath, FileMode.Open, FileAccess.Read)))
            {
                content   = sr.ReadToEnd();
            }
           var match= Regex.Match(content, "interface\\s*I(\\w*)\\s*:\\s*IRepository(<\\w*\\s*[,]*\\s*\\w*>)");
            StringBuilder sb=new StringBuilder(1000);
            var className = match.Groups[1].Value;
            var tplStr = match.Groups[2].Value;
            sb.AppendLine("//auto generate");
            sb.Append("//");
            sb.AppendLine(Path.GetFileName(interfacePath));
            sb.AppendLine("namespace Das.MICS.Domain.Repositories{\n");
            sb.AppendLine(" /// <summary>");
            sb.AppendLine($"  ///     名称： {className}");
            sb.AppendLine($"  ///     描述：  {className}");
            sb.AppendLine(@" ///     创建人：zhongwd
            ///     创建时间：2017-8-23
            ///     版权所有 (C) :DAS
            /// </summary>\n");

            sb.Append("public class ");

            sb.Append(className);
            sb.Append(":EfRepositoryBase");
            sb.Append(tplStr);
            sb.Append(",I");
            sb.AppendLine(className);
            sb.AppendLine("{\n}\n}");
            Console.WriteLine(match.Groups[1].Value);
            Console.WriteLine(match.Groups[2].Value);
            string filePath = Path.Combine(ImplenmentPath, $"generates/{className}.cs");
            using (StreamWriter sw = new StreamWriter(new FileStream(filePath, FileMode.Create, FileAccess.Write)))
            {
                sw.WriteLine(sb.ToString());
            }
        }
    }
}
