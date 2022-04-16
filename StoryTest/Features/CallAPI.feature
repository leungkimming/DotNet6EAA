Feature: CallAPI
@mytag
Scenario: CallAPIAddNew
	Given I have the following new user:
		| Field              | Value                     |
		| userName           | Micl                      |
		| firstName          | Leung                     |
		| lastName           | Michael                   |
		| address            | Kwong Chiu Terrace        |
		| birthDate          | 1961-03-04T00:00:00+08:00 |
		| departmentName     | IT                        |
		| CoefficientsSalary | 7500                      |
	And InitDB
	When I post this request to the "users" operation
	Then the result is a 200 ("OK") response
	And the response contains username ("Micl") and Department ("IT")

Scenario: CallAPIAddPayslip
	Given I can retrieve user ("Micl")
	And I have the following Payslip
		| Field       | Value                     |
		| date        | 2022-04-09T00:00:00+08:00 |
		| workingDays | 10                        |
		| bonus       | 100                       |
		| isPaid      | true                      |
	When I post this request to the AddPayslip API
	Then the result is a 200 ("OK") response
	And the response contains TotalSalary (75100)
