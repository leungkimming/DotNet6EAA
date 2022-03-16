using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs {
    public class EntityCodeDTO : IDTO {

        public EntityCodeDTO() {
            Code = string.Empty;
            RecordVersion = null;
        }

        public string Code { get; set; }
        public byte[]? RecordVersion { get; set; }
    }
}
