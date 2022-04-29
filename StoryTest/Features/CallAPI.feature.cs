﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (https://www.specflow.org/).
//      SpecFlow Version:3.9.0.0
//      SpecFlow Generator Version:3.9.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace P6.StoryTest.Features
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class CallAPIFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Microsoft.VisualStudio.TestTools.UnitTesting.TestContext _testContext;
        
        private string[] _featureTags = ((string[])(null));
        
#line 1 "CallAPI.feature"
#line hidden
        
        public virtual Microsoft.VisualStudio.TestTools.UnitTesting.TestContext TestContext
        {
            get
            {
                return this._testContext;
            }
            set
            {
                this._testContext = value;
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features", "CallAPI", null, ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute()]
        public virtual void TestInitialize()
        {
            if (((testRunner.FeatureContext != null) 
                        && (testRunner.FeatureContext.FeatureInfo.Title != "CallAPI")))
            {
                global::P6.StoryTest.Features.CallAPIFeature.FeatureSetup(null);
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
        public virtual void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Microsoft.VisualStudio.TestTools.UnitTesting.TestContext>(_testContext);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("1_Initialize")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "CallAPI")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("mytag")]
        public virtual void _1_Initialize()
        {
            string[] tagsOfScenario = new string[] {
                    "mytag"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("1_Initialize", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 3
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 4
 testRunner.Given("InitDB", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 5
 testRunner.And("I logon as \"41776\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
                TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                            "Field",
                            "Value"});
                table1.AddRow(new string[] {
                            "1",
                            "AA01"});
                table1.AddRow(new string[] {
                            "2",
                            "AB01"});
                table1.AddRow(new string[] {
                            "3",
                            "AC01"});
#line 6
 testRunner.And("have the following access codes", ((string)(null)), table1, "And ");
#line hidden
                TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                            "Name",
                            "Description",
                            "Manager"});
                table2.AddRow(new string[] {
                            "CS",
                            "CS",
                            "Raymond"});
                table2.AddRow(new string[] {
                            "PA",
                            "PA",
                            "Mimmi"});
#line 11
 testRunner.And("I have the \"Business.Department\" table with audit \"true\" save as \"D1\"", ((string)(null)), table2, "And ");
#line hidden
                TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                            "Field",
                            "Value"});
                table3.AddRow(new string[] {
                            "Name",
                            "CS"});
#line 15
 testRunner.And("DTO \"D1\" should contain a record save as \"D2\" that matches the following table", ((string)(null)), table3, "And ");
#line hidden
                TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                            "UserName",
                            "FirstName",
                            "LastName",
                            "Address",
                            "BirthDate",
                            "CoefficientsSalary",
                            "Department"});
                table4.AddRow(new string[] {
                            "Mary",
                            "Mary",
                            "Wong",
                            "Central",
                            "CURRENT_DATE-30Y",
                            "45.00",
                            "@{D2}"});
                table4.AddRow(new string[] {
                            "41776",
                            "Tommy",
                            "Leung",
                            "KLN",
                            "CURRENT_DATE-25Y",
                            "55.50",
                            "@{D2}"});
#line 18
 testRunner.And("I have the \"Business.User\" table with audit \"true\" save as \"U0\"", ((string)(null)), table4, "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("2_AddNewUser")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "CallAPI")]
        public virtual void _2_AddNewUser()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("2_AddNewUser", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 23
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
                TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                            "Field",
                            "Value"});
                table5.AddRow(new string[] {
                            "userName",
                            "Micl"});
                table5.AddRow(new string[] {
                            "firstName",
                            "Leung"});
                table5.AddRow(new string[] {
                            "lastName",
                            "Michael"});
                table5.AddRow(new string[] {
                            "address",
                            "Kwong Chiu Terrace"});
                table5.AddRow(new string[] {
                            "birthDate",
                            "1961-03-04T00:00:00+08:00"});
                table5.AddRow(new string[] {
                            "departmentName",
                            "IT"});
                table5.AddRow(new string[] {
                            "CoefficientsSalary",
                            "7500"});
#line 24
 testRunner.Given("I have the following \"Common.AddUserRequest\" DTO save as \"U1\"", ((string)(null)), table5, "Given ");
#line hidden
#line 33
 testRunner.When("I post DTO \"U1\" to API \"users\" with status code 200 and response save as \"R1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 34
 testRunner.Then("Response \"R1\" contains the \"Common.AddUserResponse\" DTO save as \"R1DTO\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
                TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                            "Field",
                            "Value"});
                table6.AddRow(new string[] {
                            "UserName",
                            "Micl"});
                table6.AddRow(new string[] {
                            "DepartmentName",
                            "IT"});
#line 35
 testRunner.And("DTO \"R1DTO\" matches the following table", ((string)(null)), table6, "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("3_AddPayslip")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "CallAPI")]
        public virtual void _3_AddPayslip()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("3_AddPayslip", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 40
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 41
 testRunner.When("I get API \"users?Search=Micl\" with status code 200 and response DTO \"Common.UserI" +
                        "nfoDTO[]\" save as \"R9\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
                TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                            "Field",
                            "Value"});
                table7.AddRow(new string[] {
                            "UserName",
                            "Micl"});
                table7.AddRow(new string[] {
                            "DepartmentName",
                            "IT"});
#line 42
 testRunner.Then("DTO \"R9\" should contain a record save as \"R10\" that matches the following table", ((string)(null)), table7, "Then ");
#line hidden
                TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                            "Field",
                            "Value"});
                table8.AddRow(new string[] {
                            "date",
                            "CURRENT_DATE-5M"});
                table8.AddRow(new string[] {
                            "workingDays",
                            "10"});
                table8.AddRow(new string[] {
                            "bonus",
                            "100"});
                table8.AddRow(new string[] {
                            "isPaid",
                            "true"});
                table8.AddRow(new string[] {
                            "UserDTO",
                            "@{R10}"});
#line 46
 testRunner.Given("I have the following \"Common.AddPayslipRequest\" DTO save as \"P1\"", ((string)(null)), table8, "Given ");
#line hidden
#line 53
 testRunner.When("I post DTO \"P1\" to API \"users/AddPayslip\" with status code 200 and response save " +
                        "as \"R3\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 54
 testRunner.Then("Response \"R3\" contains the \"Common.AddPayslipResponse\" DTO save as \"R3DTO\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
                TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                            "Field",
                            "Value"});
                table9.AddRow(new string[] {
                            "TotalSalary",
                            "75100"});
                table9.AddRow(new string[] {
                            "LetterSentDate",
                            "CURRENT_DATE+0D"});
#line 55
 testRunner.And("DTO \"R3DTO\" matches the following table", ((string)(null)), table9, "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
