using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities {
    public class CustomException : Exception {
        #region Constructors

        public CustomException(string ruleViolationErrorMessage)
            : this(CustomError.Split(ruleViolationErrorMessage)[0], CustomError.Split(ruleViolationErrorMessage)[1], ErrorType.Normal) {
        }

        public CustomException(CustomError customError, params object[] args)
            : this(customError.Code, string.Format(customError.Message, args), customError.ErrorType) {
        }

        public CustomException(string code, string message, ErrorType errorType)
            : base(message) {
            this.CustomError = new CustomError(code, message, errorType);
        }

        #endregion // Constructors

        public CustomError CustomError { get; private set; }

        public override string ToString() {
            return this.CustomError.ToString();
        }
    }
}
