namespace Common {
    public class ErrorPayloadResponse<T> where T : ErrorDetail {
        public List<T> Details { get; set; } = new List<T>();

        public ErrorPayloadResponse() {

        }

        public ErrorPayloadResponse(T errorObject) {
            this.Append(errorObject);
        }

        public void Append(T errorObject) {
            this.Details.Add(errorObject);
        }

        public void AppendList(List<T> errorObjects) {
            this.Details.AddRange(errorObjects);
        }

    }
}
