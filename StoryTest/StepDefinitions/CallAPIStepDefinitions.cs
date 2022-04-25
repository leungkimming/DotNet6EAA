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

        [Given(@"I have the following new user:")]
        public void GivenIHaveTheFollowingNewUser(Table table) {
            AddUserRequest newuser = table.CreateInstance<AddUserRequest>();
            newuser.Refresh(System.Security.Principal.WindowsIdentity.GetCurrent().Name, DateTime.Now);
            context.Set<AddUserRequest>(newuser, "newuserRequest");
        }

        [When(@"I post this request to the ""([^""]*)"" operation")]
        public async Task WhenIPostThisRequestToTheOperation(string users) {
            SetAuthorization("HEH");
            SetLogonId("41776");
            var newuserRequest = context.Get<AddUserRequest>("newuserRequest");
            var response = await client.PostAsJsonAsync("users", newuserRequest);
            context.Set(response.StatusCode, "ResponseStatusCode");
            context.Set(response.ReasonPhrase, "ResponseReasonPhrase");
            var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            context.Set(responseBody, "ResponseBody");
        }

        [Then(@"the result is a (.*) \(""([^""]*)""\) response")]
        public void ThenTheResultIsAResponse(int statusCode, string ResponseStatusCode) {
            Assert.AreEqual(statusCode, (int)context.Get<HttpStatusCode>("ResponseStatusCode"));
            Assert.AreEqual(ResponseStatusCode, context.Get<string>("ResponseReasonPhrase"));
        }

        [Then(@"the response contains username \(""([^""]*)""\) and Department \(""([^""]*)""\)")]
        public void ThenTheResponseContainsUsernameAndDepartment(string micl, string iT) {
            AddUserResponse result = JsonConvert.DeserializeObject<AddUserResponse>(context.Get<string>("ResponseBody"));
            Assert.AreEqual(result.UserName, micl);
            Assert.AreEqual(result.DepartmentName, iT);
        }

        [Then(@"the response contains TotalSalary \((.*)\)")]
        public async Task ThenTheResponseContainsTotalSalary(int p0) {
            HttpResponseMessage result = context.Get<HttpResponseMessage>("addpayslipresponse");
            AddPayslipResponse response = await result.Content.ReadFromJsonAsync<AddPayslipResponse>();
            Assert.AreEqual(response.TotalSalary, p0);
        }
        [Given(@"I can retrieve user \(""([^""]*)""\)")]
        public async Task GivenICanRetrieveUser(string micl) {
            GetUserRequest search = new GetUserRequest() {
                Search = micl,
                RecordsPerPage = 10,
                PageNo = 1,
            };
            var result = await client.PostAsJsonAsync("users/getuserlist", search);
            GetAllDatasResponse<UserInfoDTO> users = await result.Content.ReadFromJsonAsync<GetAllDatasResponse<UserInfoDTO>>();
            Assert.AreEqual(users.TotalCount, 1);
            context.Set(users.Datas[0], "AddPayslipUser");
        }

        [Given(@"I have the following Payslip")]
        public void GivenIHaveTheFollowingPayslip(Table table) {
            AddPayslipRequest addPayslipRequest = table.CreateInstance<AddPayslipRequest>();
            addPayslipRequest.Refresh(System.Security.Principal.WindowsIdentity.GetCurrent().Name, DateTime.Now);
            addPayslipRequest.UserDTO = context.Get<UserInfoDTO>("AddPayslipUser");
            context.Set<AddPayslipRequest>(addPayslipRequest, "AddPayslipRequest");
        }
        [When(@"I post this request to the AddPayslip API")]
        public async Task WhenIPostThisRequestToTheAddPayslipAPI() {
            var newpayslip = context.Get<AddPayslipRequest>("AddPayslipRequest");
            var result = await client.PostAsJsonAsync("users/AddPayslip", newpayslip);
            Assert.IsTrue(result.IsSuccessStatusCode);
            context.Set<HttpResponseMessage>(result, "addpayslipresponse");
            context.Set(result.StatusCode, "ResponseStatusCode");
            context.Set(result.ReasonPhrase, "ResponseReasonPhrase");
        }
    }
}
