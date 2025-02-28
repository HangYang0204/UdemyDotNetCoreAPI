
## Week 1
we create endpint (controllers) and defines action methonds (Get Put Post Delete...)
to create a property fast, type prop tab tab..
to create a constuctor using ctor tab
in the constructor parameter clrt + . select Create field...
Controllers <--> DBContext <--> Database

Client --- DTO --- API --- Domain Model --- Database
	Advantagers of DTOS
	1. Speraration of Concerns
	2. Performance
	3. Security
	4. Versioning
Mapping between DTOs to Domain Models

We have learned so far is how to create data models, dtos, endpoints and action methods (GET, POST, PUT, DELETE)

why async (so that long runing operations (such as db queries) won't block the main thread allowing the program to continue responding to
user requests.)

Repository Pattern
DP to separate the data access layer from the apllication

Controller --- Repo(hide the dbContext) --- Database

Decoupling
Consistency (using whatever db you want)
Performance

IRegionRepository.cs
SQLRegionRepository.cs

AutoMapper

## Week 2
Model state Validations Endpoints -> to block all the bad data from the endpont
1. Annotations for DTO Properties;
2. Check in action methods ModelState.IsValid;
3. If not valid, should return BadRequest(); \
Some common Annotations are [Required] [MaxLength(100)][Range(0,50)]

The alternative way of doing this is using Custome Actin Filters. This is done by override the OnActionExecuting where you can accesse the ModelState. Instead of writting the validation explictly, you can just annotate the action method with the new Attribute

In this section we will learn Filtering, Sorting and Pagination
__Filtering__ is used for retriving subset of the data. (add query string to controllers) ---GET [FromQuery] specify domain /dto property to filter on and the value to filter.
> /api/walks?filterOn=Name&filterQuery=Track
__Sorting__ is similar to Filtering, it is just addtional Query string/Parameters 
>/api/walks?filterOn=Name&FilterQuery=park&sortBy=LenthInKm&isAscending=true

__Pagination__ Skip(skipResult).Take(pageSize) where skipResult = (pageNumber - 1) * pageSize. 
>/api/walks?filterOn=Name&FilterQuery=park&sortBy=LenthInKm&isAscending=true&pageNumber=1&pageSize=10


Authentication and Authorization JWT Token

1. User -> website (login with username and pwed) -> API 
2. API sends back JWT token 
3. User uses JWT Token to make calls and get databack

Some Nuget packages 
1. Microsoft.AspNetCore.Authentication.JtwBearer
2. Microsoft.IdentityMOdel.Tokens
3. System.IdentityModel.Tokens.JWT
4. Microsoft.AspNetCore.Identity.EntityFrameworkCore
   
Inject Authentication service and use Authencation midleware.
Then use Attribute [Authorize] in controller to response 401.

Now we are going to Register some logins. \
Roles:
1. Reader -- GET
2. Writer --POST PUT DELETE


Run EF Migration with multiple DB context should specify the Context name with -Context 
> Add-Migraiton "Create Auth DB" -Context "NZWalksAuthDbContext" \
> Update-Database -Context "NZWalksAuthDbContext"