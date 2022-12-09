namespace Common {
    public class ErrorDetail {
        public string Code { get; set; }
    }

    public class ValidationError : ErrorDetail {
        public ValidationError() { }

        public ValidationError(ValidationError inst) {
            this.RelatedMasterData.AddRange(inst.RelatedMasterData);
            this.Code = inst.Code;
            if (inst.Extra != null)
                this.Extra = new Dictionary<string, object>(inst.Extra);
        }

        public List<string> RelatedMasterData { get; set; } = new List<string>();
        public Dictionary<string, Object>? Extra { get; set; }
    }
}
