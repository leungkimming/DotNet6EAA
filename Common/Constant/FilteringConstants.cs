namespace Common {
    public static class FilteringConstants {
        public static class ExpressionMethod {
            public const string Equal = "Equals";
            public const string Like = "Like";
            public const string Contain = "Contains";
            public const string CompareTo = "CompareTo";
        }

        public static class LikeExpressionType {
            public const string Contain = "Contain";
            public const string StartWith = "StartWith";
            public const string EndWith = "EndWith";
        }

        public const string Separator = ".";
    }
}
