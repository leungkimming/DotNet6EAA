using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utilities {
    public static class ServiceHelper {
        public static CustomFaultException DeriveCustomFaultException(Exception ex, string userID) {
            if (ex is NotImplementedException) {
                return new CustomFaultException(ex, ErrorRegistry.E9000);
            }

            if (ex.InnerException != null) {
                return DeriveCustomFaultException(ex.InnerException, userID);
            }

            //if (ex is FaultException && ex.Message == "Access is denied.")
            //{
            //    return new CustomFaultException(ex, ErrorRegistry.E2001, userID);
            //}
            //else 
            if (ex is SqlException) {
                switch (((SqlException)ex).Number) {
                    case -2:// SqlCommand Timeout
                        return new CustomFaultException(ex, ErrorRegistry.E2317, userID);
                    case 547: // constraint conflict
                        return new CustomFaultException(ex, ErrorRegistry.E2305, userID, ex.Message);
                    case 1205: // deadlock conflict
                        return new CustomFaultException(ex, ErrorRegistry.E2306, userID);
                    case 4060: // CannotOpenDB
                    default:
                        return new CustomFaultException(ex, ErrorRegistry.X2303, userID);
                }
            }
            //else if (ex is OptimisticConcurrencyException)
            //{
            //    return new CustomFaultException(ex, ErrorRegistry.E2039, userID);
            //}
            //else if (ex is CustomFaultException)
            //{
            //    return (CustomFaultException)ex;
            //}
            else if (ex is CustomException) {
                return new CustomFaultException((CustomException)ex, userID);
            } else {
                return new CustomFaultException(ex, ErrorRegistry.X2301, userID);
            }
        }


        public static void PrepareAndWriteErrorLog(Exception ex, string errorCode, string errorMessage, string userId) {
            string message = "";
            string stackTrace = "";

            if (!string.IsNullOrEmpty(errorMessage)) {
                message = errorMessage + LogHelper.MessageSeparator;
            }
            ConcatenateExceptionMessagesAndStackTraces(ex, 0, ref message, ref stackTrace);
            LogHelper.WriteErrorLog(errorCode, message, stackTrace, userId);
        }

        public static void ConcatenateExceptionMessagesAndStackTraces(Exception ex, int level, ref string message, ref string stackTrace) {
            message += string.Format("Level {0}: {1} {2}", level, ex.Message, LogHelper.MessageSeparator);
            stackTrace += string.Format("Level {0}: {1} {2}", level, ex.StackTrace, LogHelper.MessageSeparator);

            if (ex.InnerException != null) {
                ConcatenateExceptionMessagesAndStackTraces(ex.InnerException, ++level, ref message, ref stackTrace);
            }
        }
    }
}
