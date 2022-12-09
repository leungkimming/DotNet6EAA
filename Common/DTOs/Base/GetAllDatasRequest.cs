using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common {
    public class GetAllDatasRequest : DTObaseRequest {
        public int PageNo { get; set; }
        public bool GetItemCountOnly { get; set; }

        public int RecordsPerPage { get; set; }
        public List<SortingParameter>? Sort { get; set; }
    }
    public class SortingParameter {
        public string Parameter { get; set; }
        public bool IsAscending { get; set; }
    }
}
