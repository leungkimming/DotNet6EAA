# A SOLID+DDD+Specflow based .NET 6 framework
* Fork from EnLabSoftware's HRManagement
## Ref: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/
## Ref: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/
### Author: Michael Leung from Hong Kong
## Intro
I have a SOLID+DDD+Specflow based .net framework and would like to migrate to .NET 6. I selected EnLabSoftware's HRManagement template as the base for migration. I refactor the solution to align with the above Microsoft references and add EF Migration & SpecFlow test project.

## Step to run
* First, View, SQL Server Object Explorer and note down your SQL server instance name e.g. <br>(localdb)\\ProjectsV13
* Replace the SQL server instance in API project's appsettings.json file e.g. <br> "DDDConnectionString": "Server=(localdb)\\ProjectsV13;Database=DDDSample;Trusted_Connection=True;MultipleActiveResultSets=true"
* Build the project, ensure all 6 success
* cd DOS to current folder "Migrator"
* Create first migration:<br>dotnet ef migrations add ver2 --startup-project ..\API\P1.API.csproj
* Create DB: <br>dotnet ef database update  --startup-project ..\API\P1.API.csproj

## How to test API using Swagger UI
* On home page, click the "API Swagger UI"" link
* press F12 and monitor network
* First, execute Login API
    * Copy the antifogery token in F12 response header
    * Copy the JWE string in the Swagger UI
* in the API you wish to test, paste the JWE token string onto the X-UserRoles field
* paste the antiforgery token string onto the X-CSRF-TOKEN-HEADER field
* fill in API parameters as appropriate and execute

## Tidy up Business
* convert Program.cs to .NET webApplication builder
* Move Share & DTO from Business to Common
* Move Interface from Business to Data

## Repository
* use Generic repository to manipulate individual Entity
```
RepositoryBase<T>(_dbContext);
```
* use custom repository with Eager loading for performance
```
   return _dbSet.Where(expression)
      .Include(User => User.Department)
      .ToListAsync();
```
# Please visit wiki pages for more topics
*  Use of Autofac DI & AutoMapper
*  Blazor Client WebAssembly Added
*  Add SpecFlow
*  Blazor webassembly client
*  Domain Event Pattern
*  Global Exception Handling
*  Negotiate Authentication and Policy-based Authorization
*  Add Data Migration Project
*  Concurrency Control
*  CQRS Pattern
*  Logging
*  Validation
*  Integration Event Pattern using Service Bus
