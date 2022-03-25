namespace API {
    public class TestDefinition : RequirementDefinitionBase {
        private HashSet<string> _definitionList;

        public TestDefinition() {
            _definitionList = new HashSet<string>();
            InitializeDefinitions();
        }

        public override HashSet<string> DefinitionList => _definitionList;

        protected override void InitializeDefinitions() {
            _definitionList.Clear();
            _definitionList.Add("Test1");
        }
    }
}
