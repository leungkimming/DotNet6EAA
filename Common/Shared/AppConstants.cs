﻿using System;

namespace Common.Shared {
    public static class AppConstants {
        public const string DateFormat = "dd/MM/yyyy";
        public const string DateMonthYearFormat = "MM/yyyy";
        public const string DateTimeFormat = "dd/MM/yyyy HH:mm";
        public const string TimeFormat = "HH:mm";
        public const int MaxErrorMessageLength = 10000;
        public const long MaxAttachmentUploadSizeInBytes = 10485760;
        public const int ErrorCodeLength = 6;
        public static readonly DateTime PeriodEndDate = new DateTime(2099, 12, 31);
        public const string NUM_ServChrgNilValue = "9999999.99";
        public const string STR_NilValue = "NIL";
        public const string SYSTEM_ID_EMAIL = "SYSTEM";
        public const string DummySN = "Dummy SN";

        public const string SYSTEM_NAME = "";
    }

    public static class DBNullConstants {
        public const int Integer = 0;
        public const long Long = 0;
        public const decimal Decimal = 0;
        public const double Double = 0;
        public const string String = "";
        public const bool Boolean = false;
        public static readonly DateTime Date = new DateTime(1900, 1, 1);
    }

}
