Feature: CallAPI
@mytag
Scenario: CallAPIAddNew
	Given I have the following request body:
		"""
		{
		"userName": "Micl",
		"firstName": "Leung",
		"lastName": "Michael",
		"address": "Kwong Chiu Terrace",
		"birthDate": "1961-03-04T00:00:00.000Z",
		"departmentId": 1,
		"CoefficientsSalary": 7500
		}
		"""
	And InitDB
	When I post this request to the "users" operation
	Then the result is a 200 ("OK") response
	And the response contains username ("Micl") and ID (1) and Department ("IT")

Scenario: CallAPIAddPayslip
	Given I can retrieve the newly inserted user
	And I have the following Payslip
		| Field       | Value                     |
		| date        | 2022-04-09T00:00:00+08:00 |
		| workingDays | 10                        |
		| bonus       | 100                       |
		| isPaid      | true                      |
	When I post this request to the AddPayslip API
	Then the result is a 200 ("OK") response
	And the response contains UserId (1) and TotalSalary (75100)
