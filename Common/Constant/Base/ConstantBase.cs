using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common {
    public abstract class ConstantBase<T> : IConstantBase where T : IConstantBase {
        public string Code { get; }
        public string Description { get; }

        protected ConstantBase(string code, string description) {
            Code = code;
            Description = description;

            ConstantHelper.Subscribe(this);
        }
    }
}
