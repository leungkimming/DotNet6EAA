using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common {
    public class EditDataResponse : DTObaseResponse {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }
}
