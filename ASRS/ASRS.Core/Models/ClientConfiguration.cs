using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASRS.Core.Models
{
    public class ClientConfiguration
    {
        public ExcelConfiguration ExcelConfiguration { get; set; }
        public SqlConfiguration SqlConfiguration { get; set; }
    }

    public class ExcelConfiguration
    {
        public string ConnectionString { get; set; }
    }

    public class SqlConfiguration
    {
        public string ConnectionString { get; set; }
    }
}
