
namespace Business {
    public abstract class RootEntity {
        private List<BaseDomainEvent> _events;
        public IReadOnlyList<BaseDomainEvent> Events => _events.AsReadOnly();

        protected RootEntity() {
            _events = new List<BaseDomainEvent>();
        }

        protected void AddEvent(BaseDomainEvent @event) {
            _events.Add(@event);
        }

        protected void RemoveEvent(BaseDomainEvent @event) {
            _events.Remove(@event);
        }

        public void ClearAllEvents() {
            _events = new List<BaseDomainEvent>();
        }
    }

    public abstract class BaseEntity<TKey> : RootEntity {

        public TKey Id { get; set; }
        public byte[] RowVersion { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public virtual void Refresh(string currentLoginID, DateTime now) {
            if (this.CreateTime == DateTime.MinValue) {
                this.CreateBy = currentLoginID;
                this.CreateTime = now;
            }

            this.UpdateBy = currentLoginID;
            this.UpdateTime = now;
        }

    }
}