
namespace Common {
    public abstract class DTObase {
        public byte[]? RowVersion { get; set; }
        protected DTObase() {
            RowVersion = null;
        }
    }
}
