using Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities {
    public class CustomFaultException {

        public CustomFaultException(CustomException ex) : this(ex, ex.CustomError, String.Empty) { }

        public CustomFaultException(CustomException ex, string userID) : this(ex, ex.CustomError, userID) { }

        public CustomFaultException(Exception exceptionDetail, CustomError customError) : this(exceptionDetail, customError, String.Empty) { }

        public CustomFaultException(Exception exceptionDetail, CustomError customError, string userID, params object[] args) {
            this.ExceptionDetail = exceptionDetail;
            this.UserId = String.IsNullOrEmpty(userID) ? "UNKNOWN" : userID;
            this.Fault = new CustomError(
                        customError.Code, string.Format(customError.Message, args), customError.ErrorType
                    );
        }

        public Exception ExceptionDetail { get; private set; }
        public string UserId { get; private set; }
        public CustomError Fault { get; private set; }

    }
}
