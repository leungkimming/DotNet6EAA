using Microsoft.Extensions.DependencyInjection;
using Business;
using System.Reflection;
using Common;

namespace P6.StoryTest {
    [Binding]
    public class CommonStepDefinitions : StepDefinitionBase {
        public static ServiceProvider _provider { get; set; }
        public CommonStepDefinitions(
          ScenarioContext context) : base(context) {
            _provider = TestHelper.provider;
            TestHelper.context = context;
            TestHelper.client = client;
        }

        [Given(@"InitDB")]
        public void GivenInitDB() {
            // prepare an empty database for auto test
            using (var scope = _provider.CreateScope()) {
                var db = scope.ServiceProvider.GetRequiredService<Data.EFContext>();
                //string sql = "EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL' EXEC sp_MSForEachTable 'DELETE FROM ?' EXEC sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL'";
                //db.Database.ExecuteSqlRaw(sql);
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                db.SaveChanges();
            }
        }

        [Given(@"I logon as ""([^""]*)""")]
        public void GivenILogonAs(string LogonId) {
            SetLogonId(LogonId);
            SetAuthorization("Specflow");
            TestHelper.LogonId = LogonId;
        }

        [Given(@"have the following access codes")]
        public void GivenHaveTheFollowingAccessCodes(Table table) {
            TestHelper.SetClaims(table);
        }

        [Given(@"I have the ""([^""]*)"" table with audit ""([^""]*)"" save as ""([^""]*)""")]
        public void GivenIHaveTheTableWithAudit(string entityName, string @true, string varName, Table table) {
            Assembly assem = typeof(RootEntity).Assembly;
            Type entity = assem.GetType(entityName);
            bool audit = Boolean.Parse(@true);

            typeof(TestHelper).GetMethod(nameof(TestHelper.SetTable))
                .MakeGenericMethod(entity).Invoke(null, new object[] { table, audit, varName });

        }

        [When(@"I have the following ""([^""]*)"" DTO save as ""([^""]*)""")]
        [Given(@"I have the following ""([^""]*)"" DTO save as ""([^""]*)""")]
        public void GivenIHaveTheFollowingDTOSaveAs(string dtoName, string varName, Table table) {
            Assembly assem = typeof(DTObase).Assembly;
            Type entity = assem.GetType(dtoName);

            var r = typeof(TestHelper).GetMethod(nameof(TestHelper.SetDTO))
                .MakeGenericMethod(entity).Invoke(null, new object[] { table, varName });
        }

        [When(@"I post DTO ""([^""]*)"" to API ""([^""]*)"" with status code (.*) and response save as ""([^""]*)""")]
        public async Task WhenIPostDTOToAPIWithStatusCodeAndResponseSaveAs(string vNameDTO, string apiRoute, int statusCode, string vNameResponse) {
            context.TryGetValue(vNameDTO, out var dto);
            Type t = dto.GetType();
            var task = (Task)typeof(TestHelper).GetMethod(nameof(TestHelper.PostAPI))
                .MakeGenericMethod(t).Invoke(null, new object[] { vNameDTO, apiRoute, vNameResponse });
            await task.ConfigureAwait(false);
            HttpResponseMessage response = context.Get<HttpResponseMessage>(vNameResponse);
            Assert.AreEqual(statusCode, ((int)response.StatusCode));
        }

        [When(@"Response ""([^""]*)"" contains the ""([^""]*)"" DTO save as ""([^""]*)""")]
        [Then(@"Response ""([^""]*)"" contains the ""([^""]*)"" DTO save as ""([^""]*)""")]
        public async Task ThenResponseContainsTheDTOSaveAs(string vNameResponse, string dtoName, string vNameDTO) {
            context.TryGetValue(vNameResponse, out HttpResponseMessage response);
            var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            Assembly assem = typeof(DTObase).Assembly;
            Type t = assem.GetType(dtoName);

            typeof(TestHelper).GetMethod(nameof(TestHelper.SetDTOJson))
                .MakeGenericMethod(t).Invoke(null, new object[] { responseBody, vNameDTO });
        }

        [Then(@"DTO ""([^""]*)"" matches the following table")]
        public void ThenDTOMatchesTheFollowingTable(string vNameDTO, Table table) {
            context.TryGetValue(vNameDTO, out var dto);
            Type t = dto.GetType();
            bool match = (bool)typeof(TestHelper).GetMethod(nameof(TestHelper.Compare))
                .MakeGenericMethod(t).Invoke(null, new object[] { table, dto });
            Assert.IsTrue(match);
        }

        [When(@"I get API ""([^""]*)"" with status code (.*) and response DTO ""([^""]*)"" save as ""([^""]*)""")]
        public async Task WhenIGetAPIWithStatusCodeAndResponseSaveAs(string apiRoute, int statusCode, string dtoName, string vNameResponse) {
            Assembly assem = typeof(DTObase).Assembly;
            Type t = assem.GetType(dtoName);

            var task = (Task<int>)typeof(TestHelper).GetMethod(nameof(TestHelper.GetAPI))
                .MakeGenericMethod(t).Invoke(null, new object[] { apiRoute, vNameResponse });
            await task.ConfigureAwait(false);

            Assert.AreEqual(statusCode, (task.Result));
        }

        [Given(@"DTO ""([^""]*)"" should contain a record save as ""([^""]*)"" that matches the following table")]
        [When(@"DTO ""([^""]*)"" should contain a record save as ""([^""]*)"" that matches the following table")]
        [Then(@"DTO ""([^""]*)"" should contain a record save as ""([^""]*)"" that matches the following table")]
        public void ThenDTOShouldContainARecordThatMatchesTheFollowingTable(string vNameDTO, string varName, Table table) {
            context.TryGetValue(vNameDTO, out var dto);
            Type type = dto.GetType();
            if (type.IsArray) {
                Type innerType = type.GetElementType();
                bool match = (bool)typeof(TestHelper).GetMethod(nameof(TestHelper.CompareList))
                    .MakeGenericMethod(new Type[] { type, innerType }).Invoke(null, new object[] { table, dto, varName });
                Assert.IsTrue(match);
            } else {
                Assert.IsTrue(false);
            }
        }
    }
}
