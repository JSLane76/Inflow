using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Data.Entities
{
    public class LogEntry
    {
        public string Action { get; set; } = default!;
        public DateTime CreatedDate { get; set; } = default!;
        public string FieldValues { get; set; } = default!;
        public long UserID { get; set; }

    }
}
