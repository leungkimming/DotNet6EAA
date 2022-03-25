namespace API {
    public class AccessCodesDefinition : RequirementDefinitionBase {

        private HashSet<string> _definitionList;

        public AccessCodesDefinition() {
            _definitionList = new HashSet<string>();
            InitializeDefinitions();
        }

        public override HashSet<string> DefinitionList => _definitionList;

        protected override void InitializeDefinitions() {
            _definitionList.Clear();
            _definitionList.Add("CSMTA_WOD00");
            _definitionList.Add("CSMTA_SES00");
        }
    }
}
