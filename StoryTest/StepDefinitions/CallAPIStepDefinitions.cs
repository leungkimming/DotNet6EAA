using System.Net;
using Common;
using Newtonsoft.Json;
using TechTalk.SpecFlow.Assist;
using System.Net.Http.Json;

namespace P6.StoryTest {
    [Binding]
    public class CallAPIStepDefinitions : StepDefinitionBase {
        public CallAPIStepDefinitions(
          ScenarioContext context) : base(context) {
        }

        [Then(@"I locate user ""([^""]*)"" in DTO ""([^""]*)"" and update to DTO ""([^""]*)""")]
        public void ThenILocateUserInDTOAndUpdateToDTO(string userName, string vNameDTO, string vNamePayslip) {
            GetAllDatasResponse<UserInfoDTO> users = context.Get<GetAllDatasResponse<UserInfoDTO>>(vNameDTO);
            AddPayslipRequest payslip = context.Get<AddPayslipRequest>(vNamePayslip);
            payslip.UserDTO = users.Datas.Where(x => x.UserName == userName).FirstOrDefault();
            context.Set<AddPayslipRequest>(payslip, vNamePayslip);
        }
    }
}
