using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaryApp
{
    public class DataGridInfo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Importance { get; set; }
        public string Date { get; set; }
        public string Body { get; set; }
        public string Location { get; set; }
        public string Signature { get; set; }
        public override string ToString()
        {
            return $"{Importance} - {Date} - {Body} - {Signature} - {Location}";
        }
    }
    
}
