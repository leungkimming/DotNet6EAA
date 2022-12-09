namespace Common {
    public class ConcurrencyUpdateError : ValidationError {
        public ConcurrencyUpdateError() { }
        public ConcurrencyUpdateError(ConcurrencyUpdateError inst) : base(inst) { }
    }
}
