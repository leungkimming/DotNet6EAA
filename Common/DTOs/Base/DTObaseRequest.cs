using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common {
    public abstract class DTObaseRequest : DTObase {
        public string? CreateBy { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public virtual void Refresh(string currentLoginID, DateTime now) {
            if (this.CreateTime == DateTime.MinValue || this.CreateTime == null) {
                this.CreateBy = currentLoginID;
                this.CreateTime = now;
            }

            this.UpdateBy = currentLoginID;
            this.UpdateTime = now;
        }
    }
}
