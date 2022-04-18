

namespace Business {
    public partial class Department : BaseEntity<short> {
        public string Name { get; internal set; }
        public string Description { get; internal set; }
        public string Manager { get; internal set; }
        public virtual ICollection<User> Users { get; internal set; }
    }
}