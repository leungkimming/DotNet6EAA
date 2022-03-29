namespace API {
    /// <summary>
    /// This abstract class defines the RequirementDefinition that host the access fields.
    /// Must override the method and set all <b>Needed Access Codes or any others</b>
    /// </summary>
    public abstract class RequirementDefinitionBase {
        public virtual HashSet<string> DefinitionList { get; }

        public RequirementDefinitionBase() {
            DefinitionList = new HashSet<string>();
        }

        protected virtual void InitializeDefinitions() { }
    }
}
