﻿using System.Globalization;

namespace Common {
    public class PayslipMonthAlreadyExistException : Exception {
        public PayslipMonthAlreadyExistException() { }

        public PayslipMonthAlreadyExistException(int month)
            : base(String.Format("Payslip for {0} already exist",
                CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month))) {
        }
    }
    public class UserAlreadyExistException : Exception {
        public UserAlreadyExistException() { }

        public UserAlreadyExistException(string user)
            : base(String.Format("User Name {0} already exist", user)) {
        }
    }
    public class InvalidUserException : Exception {
        public InvalidUserException()
            : base("Invalid AD User") {
        }
    }
    public class RecordVersionException : Exception {
        public RecordVersionException()
            : base("Someone Changed the Record. Please retry!")
        {
        }
    }
    public class CookieSizeExceedLimitException : Exception {
        public CookieSizeExceedLimitException()
            : base("Cookie Size Exceed Limit") {
        }
    }
    public class SystemParameterAlreadyExistException : Exception {
        public SystemParameterAlreadyExistException() { }

        public SystemParameterAlreadyExistException(string systemParameter)
            : base(String.Format("System Parameter {0} already exist", systemParameter)) {
        }
    }
}
