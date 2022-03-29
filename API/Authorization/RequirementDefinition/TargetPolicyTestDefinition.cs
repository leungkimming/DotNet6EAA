namespace API {
    /// <summary>
    /// This is a <b>PolicyTestDefinition</b> for how to define the <b>Access Codes</b>.
    /// Just <b>Define</b> what you want to authorize in the inherited class like this class.
    /// Basically, this is the target access codes that actions or controllers needed.
    /// </summary>
    public class TargetPolicyTestDefinition : DefinitionBase<PolicyTestRequirement> {
        public TargetPolicyTestDefinition() {
            InitializeDefinitions();
        }

        protected override void InitializeDefinitions() {
            DefinitionList.Clear();
            DefinitionList.Add("Test1");
        }
    }
}
