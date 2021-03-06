using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using TechTalk.SpecFlow.Assist;
using TechTalk.SpecFlow.Assist.ValueRetrievers;
using System.Text.RegularExpressions;

namespace P6.StoryTest {
    [Binding]
    public static class Hooks {

        [BeforeTestRun]
        public static void BeforeTestRun(ITestRunnerManager testRunnerManager, ITestRunner testRunner) {
            var projectDir = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(projectDir, "appsettings.test.json");
            var config = new ConfigurationBuilder().AddJsonFile(configPath).Build();

            TestHelper.provider = new ServiceCollection()
                .AddDbContext<Data.EFContext>(options =>
                     options.UseSqlServer(config.GetConnectionString("DDDConnectionString")))
                .AddScoped<Data.EFContext>()
                .BuildServiceProvider();

            TechTalk.SpecFlow.Assist.Service.Instance.ValueRetrievers.Register(new NullValueRetriever("<null>"));
            TechTalk.SpecFlow.Assist.Service.Instance.ValueRetrievers.Register(new DateTimeValueRetrieverEx());
            TechTalk.SpecFlow.Assist.Service.Instance.ValueRetrievers.Register(new ChildObjectValueRetriever(
                testRunner.ScenarioContext));
        }
    }
    public class DateTimeValueRetrieverEx : IValueRetriever {
        public virtual DateTime GetValue(string value) {
            var returnValue = DateTime.MinValue;
            string match = value;
            int adjust = 0;
            string unit = "";

            Match list = Regex.Match(value, @"(?<key>[a-zA-Z_]{12})(?<adj>[0-9+-]{2,})(?<unit>[A-Z]{1})");
            if (list.Success) {
                match = list.Groups["key"].Value;
                adjust = int.Parse(list.Groups["adj"].Value);
                unit = list.Groups["unit"].Value;
            }

            switch (match) {
                case "CURRENT_DATE":
                    switch (unit) {
                        case "D":
                            returnValue = DateTime.Now.Date.AddDays(adjust);
                            break;
                        case "M":
                            returnValue = DateTime.Now.Date.AddMonths(adjust);
                            break;
                        case "Y":
                            returnValue = DateTime.Now.Date.AddYears(adjust);
                            break;
                    }
                    break;

                case "CURRENT_TIME":
                    switch (unit) {
                        case "H":
                            returnValue = DateTime.Now.AddHours(adjust);
                            break;
                        case "M":
                            returnValue = DateTime.Now.AddMinutes(adjust);
                            break;
                        case "S":
                            returnValue = DateTime.Now.AddSeconds(adjust);
                            break;
                    }
                    break;

                default:
                    DateTime.TryParse(value, out returnValue);
                    break;
            }
            return returnValue;
        }

        public object Retrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType) {
            return GetValue(keyValuePair.Value);
        }

        public bool CanRetrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType) {
            return (propertyType == typeof(DateTime) ||
                    propertyType == typeof(Nullable<DateTime>));
        }
    }
    public class ChildObjectValueRetriever : IValueRetriever {
        private readonly ScenarioContext scenarioContext;
        private string vname = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="ChildObjectValueRetriever"/> class.
        /// </summary>
        /// <param name="scenarioContext">The ambient scenario context.</param>
        public ChildObjectValueRetriever(ScenarioContext scenarioContext) {
            this.scenarioContext = scenarioContext;
        }

        /// <inheritdoc/>
        public bool CanRetrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType) {
            Match list = Regex.Match(keyValuePair.Value, @"(@{)(?<vname>[0-9a-zA-Z_]{1,})}");
            if (list.Success) {
                vname = list.Groups["vname"].Value;
                return true;
            } else {
                return false;
            }
        }

        /// <inheritdoc/>
        public object Retrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType) {
            var obj = TestHelper.context.Get<object>(vname);
            return TestHelper.context.Get<object>(vname);
        }
    }
}
