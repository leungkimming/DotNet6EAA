using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common {
    public partial class ErrorConstant : ConstantBase<ErrorConstant> {

        public static readonly ErrorConstant AuthorizationError = new ErrorConstant("Auth-403","User is not has permisson");
        private ErrorConstant(string code, string description)
         : base(code, description) {
        }
    }
}
