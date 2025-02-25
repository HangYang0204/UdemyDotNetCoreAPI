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

