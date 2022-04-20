using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common {
    public class GetAllDatasResponse<T> where T : DTObaseResponse {
        public int TotalCount { get; set; }
        public List<T>? Datas { get; set; }
    }
}
