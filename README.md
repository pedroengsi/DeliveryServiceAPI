# DeliveryServiceAPI
This project currently works in debug mode because before going to production needs to update the DbContext to use some SQL Server. At the moment it's using an InMemory DataBase and some Dummy Data.

It was developed using C# and consists of an ASP.Net Core Web application with [Swagger](https://swagger.io/) to facilitate the use and test of the Rest API, redirects to HTTPS and has the concept of versionning and authentication via _Bearer Token_.

## Steps to use the RestApi:
* Open the solution with Visual Studio 2017;
* Start debugging the solution;
* It should open the Windows default browser:
![Browser](ReadmeImages/Browser.png?raw=true "Browser")

    * The initial page is [https://localhost:44320/swagger/](https://localhost:44320/swagger/)

* Just use Swagger UI to test each of the available methods:
    * Click on the _HTTP verb_ to expand the corresponding operation:
![Expand Operation](ReadmeImages/Swagger_01.png?raw=true "Expand Operation")

    * Fill the required parameters if needed:
![Fill Parameters](ReadmeImages/Swagger_02.png?raw=true "Fill Parameters")

    * Click the _Try it out!_ button:
![Try it out!](ReadmeImages/Swagger_03.png?raw=true "Try it out!")

    * Check the results:
![Results](ReadmeImages/Swagger_04.png?raw=true "Results")   

##### Note
For every operation there is a description for it and for every parameter and the possible Unsuccessfull error codes available.


## Available methods
The available methods will be described in the order I believe they should be checked.

### Root
#### GET /
This operation list the URLs for all the availabe points, routes and the URL to the login.
![Root](ReadmeImages/Root.png?raw=true "Root") 


### Login
#### POST /Accounts/LoginUser
This operation allows the user to login in the rest api and in return retrieves a _Bearer Token_ to include in the header of every request.
In Development the dummy data adds two roles *AdminRole* and *UserRole* and two user:

| UserName | Email | Password | Role |
| -------- | ----- | -------- | ---- |
| localadmin | admin@local.com | Supersecret123!! | AdminRole |
| localuser  | user@local.com  | Password!123 | UserRole |

With a successful login a _Bearer Token_ is generated:
![Bearer Token](ReadmeImages/BearerToken.png?raw=true "Bearer Token") 

It's necessary to copy it and tell _Swagger_ to use it in every request. For this just move to the top of the page and click on the _Authorize_ button:
With a successful login a _Bearer Token_ is generated:
![Authorize](ReadmeImages/Login_02.png?raw=true "Authorize") 

and copy to the newly openned window.
![Authorize](ReadmeImages/Login_03.png?raw=true "Authorize") 

From this moment the user is logged in the system, but even so only users with _AdminRole_ can perform the CRUD operations.


#### GET /Accounts/VerifyLoginStatus
With a successful login and after a adding the _Bearer Token_ in the Authorize section of the Swagger UI it's possible to execute this operation. This operation returns some info about the logged in user:
![Logged in user info](ReadmeImages/LoggedInUser.png?raw=true "Logged in user info") 


#### POST /Accounts/CreateNewUserAsync
Only a user with AdminRole can create new users and even the created users belong to the UserRole.


### Points
This section is version ready, but currently only supports version *1* and contains all the operations available for the points.

#### GET /appi/v{version}/Points
Retrieves a collection with all the available points in the system.

#### POST /appi/v{version}/Points
Allows an Admin user to add new points.

#### DELETE /appi/v{version}/Points/{pointId}
Allows an Admin user to delete a given point.

#### GET /appi/v{version}/Points/{pointId}
Allows any user to retrieve a given point.

#### PUT /appi/v{version}/Points/{pointId}
Allows an Admin user to update an existing point.


### Routes
This section is version ready, but currently only supports version *1* and contains all the operations available for the routes.

#### GET /appi/v{version}/Routes
Retrieves a collection with all the available routes in the system as well as their:
* Route Name;
* Origin;
* Destination;
* Cost;
* Time.

#### POST /appi/v{version}/Routes
Allows an Admin user to add new routes.

#### DELETE /appi/v{version}/Routes/{routeId}
Allows an Admin user to delete a given route.

#### GET /appi/v{version}/Routes/{routeId}
Allows any user to retrieve a given route.

#### PUT /appi/v{version}/Routes/{routeId}
Allows an Admin user to update an existing route.

#### GET /api/v{version}/Routes/{originPointId}/{destinationPointId}
This operation is used to find the shortest path, in term of cost or time, between two points
![Route Search](ReadmeImages/RouteSearch.png?raw=true "Route Search") 

The parameters required to execute this operation are:
* originPointId
    * The Id of the point to be considered the origin of the path;
* destinationPointId 
    * The Id of the point to be considered the destination of the path;
* SearchOptions
    * Define if the search will be made in terms of cost of time of each route;
* SearchAlgorithm
    * Possibility to define the search algorithm to be used in finding the shortest path between two points.
        * Currently only the _Dijkstra_ algorithm is implemented, but the solution is ready to implement more, like shown with the _AStar_;
* SearchOptionPath
    * Allow the user to define if the shortest path to find can be:
        * Direct - which means that the destination can be a neighbor of the origin;
        * At Least one Hop - which means that the between the origin and the destination has to be at least another point.
* version
    * Currently 1

##### Note
The dummy data added is the same as the exercise except in the _route_ from _Point A_ to _Point E_ where the cost is _2_ instead of _5_. 
This solution also contains some initial Unit Tests to ensure the proper functioning of the Service.
