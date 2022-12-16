using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business {
    public partial class RequestLog : BaseEntity<int> {
        public DateTime TimeStamp { get; set; }
        public string Route { get; set; }
        public string Method { get; set; }
        public string Parameters { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
    }
}
