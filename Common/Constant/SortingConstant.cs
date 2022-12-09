namespace Common {
    public class SortingConstant : ConstantBase<SortingConstant> {
        //Common sorting constant
        public static readonly SortingConstant MINUS_SYMBOL = new SortingConstant("-", "-");
        public static readonly SortingConstant ASC = new SortingConstant("asc", " asc");
        public static readonly SortingConstant DESC = new SortingConstant("desc", " desc");
        public static readonly SortingConstant NULL_LAST = new SortingConstant("== null", " null last");
        public static readonly SortingConstant SORTING_FIRST = new SortingConstant("0", "first");
        public static readonly SortingConstant SORTING_LAST = new SortingConstant("1", "larger");
        public static readonly SortingConstant DEFAULT_SORT_FIELD_NAME = new SortingConstant("Id", "default sort by id when no sorting criteria is provided");

        private SortingConstant(string code, string description)
            : base(code, description) {
        }
    }
}
