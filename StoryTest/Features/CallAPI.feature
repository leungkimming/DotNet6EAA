Feature: CallAPI
@mytag
Scenario: 1_Initialize
	Given InitDB
	And I logon as "41776"
	And have the following access codes
		| Field | Value |
		| 1     | AA01  |
		| 2     | AB01  |
		| 3     | AC01  |
	And I have the "Business.Department" table with audit "true"
		| Name | Description | Manager |
		| CS   | CS          | Raymond |
		| PA   | PA          | Mimmi   |

Scenario: 2_AddNewUser
	Given I have the following "Common.AddUserRequest" DTO save as "U1"
		| Field              | Value                     |
		| userName           | Micl                      |
		| firstName          | Leung                     |
		| lastName           | Michael                   |
		| address            | Kwong Chiu Terrace        |
		| birthDate          | 1961-03-04T00:00:00+08:00 |
		| departmentName     | IT                        |
		| CoefficientsSalary | 7500                      |
	When I post DTO "U1" to API "users" with status code 200 and response save as "R1"
	Then Response "R1" contains the "Common.AddUserResponse" DTO save as "R1DTO"
	And DTO "R1DTO" matches the following table
		| Field          | Value  |
		| UserName       | Micl   |
		| DepartmentName | IT     |

Scenario: 3_AddPayslip
	Given I have the following "Common.AddPayslipRequest" DTO save as "P1"
			| Field       | Value           |
			| date        | CURRENT_DATE-5M |
			| workingDays | 10              |
			| bonus       | 100             |
			| isPaid      | true            |
	And I have the following "Common.GetUserRequest" DTO save as "U2"
			| Field          | Value |
			| Search         | Micl  |
			| RecordsPerPage | 10    |
			| PageNo         | 1     |
	When I post DTO "U2" to API "users/getuserlist" with status code 200 and response save as "R2"
	Then Response "R2" contains the "Common.GetAllDatasResponse`1[Common.UserInfoDTO]" DTO save as "R2DTO"
	And I locate user "Micl" in DTO "R2DTO" and update to DTO "P1"
	When I post DTO "P1" to API "users/AddPayslip" with status code 200 and response save as "R3"
	Then Response "R3" contains the "Common.AddPayslipResponse" DTO save as "R3DTO"
	And DTO "R3DTO" matches the following table
		| Field          | Value        |
		| TotalSalary    | 75100        |
		| LetterSentDate | CURRENT_DATE+0D |

