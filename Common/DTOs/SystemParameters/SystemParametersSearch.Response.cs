using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common {
    public class SystemParametersSearchResponse: DTObaseResponse {
        public string ParameterTypeCode { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string DataTypeCode { get; set; }
        public int? Value_Integer { get; set; }
        public decimal? Value_Decimal { get; set; }
        public DateTime? Value_Datetime { get; set; }
        public string Value_Text { get; set; }
    }
}
