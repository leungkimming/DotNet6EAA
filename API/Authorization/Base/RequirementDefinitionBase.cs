namespace API {
    public abstract class RequirementDefinitionBase {
        public virtual HashSet<string> DefinitionList { get; }

        public RequirementDefinitionBase() {
            DefinitionList = new HashSet<string>();
        }

        protected virtual void InitializeDefinitions() { }
    }
}
