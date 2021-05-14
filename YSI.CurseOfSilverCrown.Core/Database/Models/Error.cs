using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YSI.CurseOfSilverCrown.Core.Database.Models
{
    public class Error
    {
        public int Id { get; set; }
        public string RequestId { get; set; }
        public string TypeFullName { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}
