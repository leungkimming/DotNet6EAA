using System.Net.Http.Headers;
using System.Net;
using TechTalk.SpecFlow;
using Microsoft.AspNetCore.Mvc.Testing;
using API;
using Service;
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

        [Given(@"I have the following request body:")]
        public void GivenIHaveTheFollowingRequestBody(string multilineText) {
            context.Set(multilineText, "Request");
        }

        [When(@"I post this request to the ""([^""]*)"" operation")]
        public async Task WhenIPostThisRequestToTheOperation(string users) {
            var requestBody = context.Get<string>("Request");
            // set up Http Request Message
            var request = new HttpRequestMessage(HttpMethod.Post, $"/{users}") {
                Content = new StringContent(requestBody) {
                    Headers = {
                      ContentType = new MediaTypeHeaderValue("application/json")
                    }
                }
            };
            SetAuthorization("HEH");
            SetLogonId("41776");
            // let's post
            var response = await client.SendAsync(request).ConfigureAwait(false);
            try {
                context.Set(response.StatusCode, "ResponseStatusCode");
                context.Set(response.ReasonPhrase, "ResponseReasonPhrase");
                var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                context.Set(responseBody, "ResponseBody");
            } finally {
                // move along, move along
            }
        }

        [Then(@"the result is a (.*) \(""([^""]*)""\) response")]
        public void ThenTheResultIsAResponse(int statusCode, string ResponseStatusCode) {
            Assert.AreEqual(statusCode, (int)context.Get<HttpStatusCode>("ResponseStatusCode"));
            Assert.AreEqual(ResponseStatusCode, context.Get<string>("ResponseReasonPhrase"));
        }

        [Then(@"the response contains username \(""([^""]*)""\) and ID \((.*)\) and Department \(""([^""]*)""\)")]
        public void ThenTheResponseContainsUsernameAndIDAndDepartment(string micl, int p1, string iT) {
            AddUserResponse result = JsonConvert.DeserializeObject<AddUserResponse>(context.Get<string>("ResponseBody"));
            Assert.AreEqual(result.UserName, micl);
            Assert.AreEqual(result.Id, p1);
            Assert.AreEqual(result.DepartmentName, iT);
        }

        [Then(@"the response contains UserId \((.*)\) and TotalSalary \((.*)\)")]
        public async Task ThenTheResponseContainsUserIdAndTotalSalary(int p0, int p1) {
            HttpResponseMessage result = context.Get<HttpResponseMessage>("addpayslipresponse");
            AddPayslipResponse response = await result.Content.ReadFromJsonAsync<AddPayslipResponse>();
            Assert.AreEqual(response.UserId, p0);
            Assert.AreEqual(response.TotalSalary, p1);
        }

        [Given(@"I can retrieve the newly inserted user")]
        public async Task GivenICanRetrieveTheNewlyInsertedUser() {
            UserInfoDTO[]? users = await client.GetFromJsonAsync<UserInfoDTO[]>("users?Search=Micl");
            Assert.AreEqual(users.Count(), 1);
            context.Set<UserInfoDTO>(users[0], "AddPayslipUser");
        }

        [Given(@"I have the following Payslip")]
        public void GivenIHaveTheFollowingPayslip(Table table) {
            AddPayslipRequest addPayslipRequest = table.CreateInstance<AddPayslipRequest>();
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

        //[Then(@"the response contains UserId \((.*)\) and TotalSalary \((.*)\) and lettersentdate \(""([^""]*)""\) and letter start with \(""([^""]*)""\)")]
        //public void ThenTheResponseContainsUserIdAndTotalSalaryAndLettersentdateAndLetterStartWith(int p0, int p1, string today, string p3)
        //{
        //    AddPayslipResponse result = JsonConvert.DeserializeObject<AddPayslipResponse>(context.Get<string>("ResponseBody"));
        //    Assert.AreEqual(result.UserId, p0);
        //    Assert.AreEqual(result.TotalSalary, p1);
        //    Assert.AreEqual(result.LetterSentDate.Value.Date.ToString(), DateTime.Now.Date.ToString());
        //    Assert.IsTrue(result.Letter.StartsWith(p3.Replace("\\n","\n")));
        //}
    }
}
