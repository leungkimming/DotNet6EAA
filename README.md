# A SOLID+DDD+Specflow based .NET 6 framework [GitHub link](https://github.com/leungkimming/DotNet6EAA)
* Fork from EnLabSoftware's HRManagement
## Ref: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/
## Ref: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/
## Introduction
I have a SOLID+DDD+Specflow based .net framework and would like to migrate to .NET 6. I selected EnLabSoftware's HRManagement template as the base for migration. I refactor the solution to align with the above Microsoft references and add EF Migration & SpecFlow test project.
### Domain Driven Design (DDD)
* Define bounded Contexts with validated interfaces among Contexts
* Distill Domain Logics and implement in Business Layer
* Business and Common layers should only depends on .NET6 and MediatR
* When Business Logics has side effects, create Domain Event (MediatR) to handle in Servce Layer
* Supplement with Command Query Request Segregation (CQRS)
* Supplement with Event Service Bus
![Solution Architecture](https://github.com/leungkimming/DotNet6EAA/wiki/images/EAAa.PNG)
### SOLID principles
* S - Single-responsiblity Principle
* O - Open-closed Principle
* L - Liskov Substitution Principle
* I - Interface Segregation Principle
* D - Dependency Inversion Principle
### behavior-driven development (BDD) - SpecFlow
* It is the most accurate requirements - as of QC pass
* Only for most CRICITAL business processes, failing which will make the whole system down!
* Must pass before merging to UAT
## Steps to run
* First, View, SQL Server Object Explorer and note down your SQL server instance name e.g. <br>(localdb)\\ProjectModels
* Replace the SQL server instance in API project's appsettings.json file e.g. <br> "DDDConnectionString": "Server=(localdb)\\ProjectModels;Database=DDDSample;Trusted_Connection=True;MultipleActiveResultSets=true"
* Add package source https://itdtfs397/DefaultCollection/_packaging/PackageSources_dotnet/nuget/v3/index.json
* Build the project, ensure all 6 success
* Debug Run Express IIS
## How to test API using Swagger UI
* On home page, click the "API Swagger UI"" link
* press F12 and monitor network
* First, execute Login API
    * Copy the antifogery token in F12 response header
    * Copy the JWE string in the Swagger UI
* in the API you wish to test, paste the JWE token string onto the X-UserRoles field
* paste the antiforgery token string onto the X-CSRF-TOKEN-HEADER field
* fill in API parameters as appropriate and execute

# Please visit this project's wiki pages for more topics
*  Blazor Client WebAssembly with Telerik UI Added
*  Low Code SpecFlow Story Test
*  DDD Rich Domain Design
*  Domain Event Pattern
*  Global Exception Handling
*  Negotiate Authentication and Policy-based Authorization
*  Add Data Migration Project
*  Concurrency Control
*  CQRS Pattern
*  Hosting .NET6 Blazor in subsite
*  Logging
*  Validation
*  Repository and Eager Loading
*  Set Parameters in Pipelines
*  Antiforgery Token Validation
*  Telerik Report
*  Telerik Document & Zip
*  Integration Event Pattern using Service Bus
