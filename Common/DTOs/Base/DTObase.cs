
namespace Common {
    public abstract class DTObase {
        public int Id { get; set; }
        public string? CreateBy { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public virtual void Refresh(string currentLoginID, DateTime now) {
            if (this.CreateTime == DateTime.MinValue || this.CreateTime == null) {
                this.CreateBy = currentLoginID;
                this.CreateTime = now;
            }

            this.UpdateBy = currentLoginID;
            this.UpdateTime = now;
        }
        public byte[]? RowVersion { get; set; }
        protected DTObase() {
            RowVersion = null;
        }
    }
}
