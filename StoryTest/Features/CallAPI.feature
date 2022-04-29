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
	And I have the "Business.Department" table with audit "true" save as "D1"
		| Name | Description | Manager |
		| CS   | CS          | Raymond |
		| PA   | PA          | Mimmi   |
	And DTO "D1" should contain a record save as "D2" that matches the following table
		| Field | Value |
		| Name  | CS    |
	And I have the "Business.User" table with audit "true" save as "U0"
		| UserName | FirstName | LastName | Address | BirthDate        | CoefficientsSalary | Department |
		| Mary     | Mary      | Wong     | Central | CURRENT_DATE-30Y | 45.00              | @{D2}      |
		| 41776    | Tommy     | Leung    | KLN     | CURRENT_DATE-25Y | 55.50              | @{D2}      |

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
	When I get API "users?Search=Micl" with status code 200 and response DTO "Common.UserInfoDTO[]" save as "R9"
	Then DTO "R9" should contain a record save as "R10" that matches the following table
		| Field          | Value |
		| UserName       | Micl  |
		| DepartmentName | IT    |
	Given I have the following "Common.AddPayslipRequest" DTO save as "P1"
			| Field       | Value           |
			| date        | CURRENT_DATE-5M |
			| workingDays | 10              |
			| bonus       | 100             |
			| isPaid      | true            |
			| UserDTO     | @{R10}          |
	When I post DTO "P1" to API "users/AddPayslip" with status code 200 and response save as "R3"
	Then Response "R3" contains the "Common.AddPayslipResponse" DTO save as "R3DTO"
	And DTO "R3DTO" matches the following table
		| Field          | Value           |
		| TotalSalary    | 75100           |
		| LetterSentDate | CURRENT_DATE+0D |

