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
    // write your specific step definitions that the common generic libraries do not support
    }
}
