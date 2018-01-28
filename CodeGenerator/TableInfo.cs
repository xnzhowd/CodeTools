using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator
{
    class TableInfo
    {
        public string TableName { get; set; }
        public DateTime CreateTime { get; set; }

        public string ObjectId { get; set; }

        public List<ColumnInfo> Columns { get; set; }
    }
}
