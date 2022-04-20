using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Business {
    public partial class SystemParameters : IAggregateRoot {
        public SystemParameters() { 
        
        }
        public void Update(string code,string? description,
            string? parameterType
           , string dataType
           , string? value_Text
           , DateTime? value_Datetime
           , decimal? value_Decimal
           , int? value_Integer) {
            Code = code;
            Description = description;
            ParameterTypeCode = parameterType;
            DataTypeCode = dataType;
            Value_Datetime = value_Datetime;
            Value_Decimal = value_Decimal;
            Value_Integer = value_Integer;
            Value_Text = value_Text;
        }
        
    }
}
