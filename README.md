# DomainModelEditor
The domain model editor makes the visualisation and modeling of the application domain easier for business users by prividing a dynamic way to add attributes/properties to entities as well as being able to align, order and group different entities for the Business Users  

Technologies & Tools  used:
- ASP .Net Core 5
- .Net Standard (V 2.0)=>All Librairies
- .Net Core DI & Container =>('.NET Core Microsoft.Extensions.DependencyInjection')
- Entity Framework Core (v 3.1) with 'Microsoft.EntityFrameworkCore.Sqlite provider'.
- Microsoft.EntityFrameworkCore.InMemory(For Testing)
- Moq =>Moq is the most popular and friendly mocking framework for .NET(for testing)
- MSTest.TestFramework.


Architecture :

- Entity-Attribute-Value (For Database Modeling)
- REST API supports paging, sorting, data shaping, HATEOAS, advanced content negotation, custom media types,
- WPF (MVVM) for Desktop users
## Paging, Searching, and Sorting
We don’t want to return a collection of all resources when querying our API. That can cause performance issues and it’s in no way optimized for public or private APIs. It can cause massive slowdowns and even application crashes in severe cases. So, implementing paging, searching, and sorting will allow our users to easily find and navigate through returned results, but it will also narrow down the resulting scope,  which can speed up the process for sure.

### Paging:
- Page by default to avoid performance issues when collection grows.
- 	Page all the way through the datastore, thanks to deferred execution which enables to us to build a query first before execution 
- It is best practice to page all collection resources by default to avoid unintended impact on performance
- Return pagination metadata so that the consumer knows how to navigate to previous and next pages and how many records in the database.
- Parameters are passed through the query string (http:host/api.authors?pagenumber=1&pagesize=5).
- Page size should be limited.


### Data Shaping:
- Allows the consumer of the API to choose the resource fields
- It has a good impact on performance as you return only what you need (ex: you have 50 properties and you only interested in only two) (http:host/api.authors?fields=id,name).


### HATEOAS:
Hypermedia as the Engine of Application State (HATEOAS) is a component of the REST application architecture that distinguishes it from other network application architectures.
With HATEOAS, a client interacts with a network application whose application servers provide information dynamically through hypermedia. A REST client needs little to no prior knowledge about how to interact with an application or server beyond a generic understanding of hypermedia.
By contrast, clients and servers in CORBA interact through a fixed interface shared through documentation or an interface description language (IDL).
The restrictions imposed by HATEOAS decouple client and server. This enables server functionality to evolve independently.

-	Hypermedia, like links drive how to consume and use the API and the functionality of the consuming application: its state.
- Allows the API to truly evolve without breaking the consuming application, in the end resulting in self-discoverable APIs.
- Reduce the need for API knowledge.
- Even functionality and business rules change, client application won’t break, as he needs to inspect the links he get back from the Uri response body. 
- The restrictions imposed by HATEOAS decouple client and server. This enables server functionality to evolve independently.
  Content Negotiation in REST APIs
- Generally, resources can have multiple presentations, mostly because there may be multiple different clients expecting different representations. Asking for a suitable      presentation by a client, is referred as content negotiation.
- Use Vendor-specific media types to differentiate between resources with and without HATEOAS links.
- It also leads to better evolvability and reliability
