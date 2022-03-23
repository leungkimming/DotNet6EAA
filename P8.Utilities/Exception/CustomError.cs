using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities {
    public class CustomError {
        #region Static methods

        public static CustomError CreateNormalError(string code, string message) {
            return new CustomError(code, message, ErrorType.Normal);
        }

        public static CustomError CreateSystemError(string code, string message) {
            return new CustomError(code, message, ErrorType.System);
        }

        public static string[] Split(string ruleViolationErrorMessage) {
            if (ruleViolationErrorMessage.Contains(CustomError.Separator)) {
                return ruleViolationErrorMessage.Split(new string[] { CustomError.Separator }, 2, StringSplitOptions.None);
            } else {
                return new string[] { "0", ruleViolationErrorMessage };
            }
        }

        public static string FormatErrorCodeAndMessage(string ruleViolationErrorMessage) {
            return string.Join(CustomError.Connector, Split(ruleViolationErrorMessage));
        }

        #endregion // Static methods

        public const string Separator = @"||";
        public const string Connector = @" - ";

        public string Code { get; private set; }

        public string Message { get; private set; }

        public ErrorType ErrorType { get; private set; }

        #region Constructors

        public CustomError(string ruleViolationErrorMessage)
            : this(Split(ruleViolationErrorMessage)[0], Split(ruleViolationErrorMessage)[1], ErrorType.Normal) {
        }

        public CustomError(string code, string message, ErrorType errorType) {
            this.Code = code;
            this.Message = message;
            this.ErrorType = errorType;
        }

        #endregion // Constructors

        public override string ToString() {
            return this.ErrorType.ToString() + " " + this.Code + Connector + this.Message;
        }
        public string ToString(params object[] args) {
            return this.ErrorType.ToString() + " " + this.Code + Connector + string.Format(this.Message, args);
        }

        public string ToRuleViolationMessage() {
            return this.Code + Separator + this.Message;
        }
        public string ToRuleViolationMessage(params object[] args) {
            return this.Code + Separator + string.Format(this.Message, args);
        }
    }

    public enum ErrorType {
        Normal,
        System
    }
}
