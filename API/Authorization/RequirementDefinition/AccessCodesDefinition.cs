namespace API {
    public class AccessCodesDefinition : DefinitionBase<AccessCodesRequirement> {

        public AccessCodesDefinition() {
            InitializeDefinitions();
        }

        protected override void InitializeDefinitions() {
            DefinitionList.Clear();
            DefinitionList.Add("CSMTA_WOD00");
            DefinitionList.Add("CSMTA_SES00");
        }
    }
}
