
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

## Week 3
__Logging__

Add Dependency Logging with Serilog and we are going to inject to Constroller later
```c#
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/My_log.txt", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Information()
    .CreateLogger();

builder.Logging.ClearProviders(); //cleaer current provider so we can add SeriLog
builder.Logging.AddSerilog(logger); //Add SeriLog
```

Here we both log to Console and to File (to directory app/Logs/My_log.txt with new log file every Day)

In the Controller, we can inject as below:
```c#
        private readonly IMyRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public MyController(IMyRepository myRepository, IMapper mapper, ILogger<MyController> logger)
        {
            this.regionRepository = myRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

```

Add Middleware `ExceptionHandlerMiddleware.cs` for global exception handling ( we are going to log the exception message to logs and show customized messages to
users to hide application secrets.), here is how we use it
```c#
	//Consume our middleware
	app.UseMiddleware<ExceptionHandlerMiddleware>();
```
we leverage the `RequestDelegate` a function to process HTTP request and returns a task shows process completion

__API versioning Techniques__

1. URL-based versioning
2. Query parameter-based versioning
3. Header-based versioning

If you manually do it, you can implement it by haveing multiple resources and change routing rules like below:
```shell
    HTTP GET https://localhost:8080/api/my/v1/GetAll
    HTTP GET https://localhost:8080/api/my/v2/GetAll
```
So you can have multiple controller with same name in V1 folder as well as V2 folder. It is important to have versioning in mind when design APIs

The second option is using Nuget package named `Microsoft.AspNetCore.Mvc.Versioning` add services like below:
```csharp
    builder.Services.AddApiVersioning(options =>
    {
        options.AssumeDefaultVersionWhenUnspecified = true;
    });
```
and in controller (you can have only one but different version of action method) and annotate with:
```csharp
[Route(api/[controller])]
[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]

public class MyController : ControllerBase
{
    [MapToApiVersion("1.0")]
    [HttpGet]
    public IactionResult GetV1()
    {
        //TODO Code goes here
    }

    [MapToApiVersion("2.0")]
    [HttpGet]
    public IactionResult GetV2()
    {
        //TODO different Code goes here
    }
}

```
And you should use the URL to specify the version you are using as below:
```Shell
    HTTP GET https://localhost:8080/api/my?api.version=2.0
```

Ok, how do we mimic the same URI request like we did manually versioning? we can modify routing rules as below:
```csharp
[Route(api/v{version:apiVersion}/[controller])]
[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
```
and now you can call different version like below:
```shell
    HTTP GET https://localhost:8080/api/my/v1/GetAll
    HTTP GET https://localhost:8080/api/my/v2/GetAll

```

__Consuming API__
If using ASP.NET project, the front end code would use `HttpClient`, once again, we start with inject the services in Program.cs
```c#
    builder.Services.AddHttpClient();

```
and in the controller code you can add the interface of `IHttpClientFactor`
```Csharp
{ //inside the controller body
    private readonly IHttpClinetFactory httpClientFactory;
    
    public myController(IHttpClinetFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory
    }

    {//inside your routing page Index() etc...

        List<MyDto> response = new List<MyDto>();

        var endpoint = "https://localhost:8080/api/my";
        var client = httpClientFactory.CreateClient();
        var httpReponseMessage = awati client.GetAsync(endpoint);

        httpResponseMessage.EnsureSucessStatusCode();

        reponse.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<MyDto>>());

        return View(response);

    }
}
    
```