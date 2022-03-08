## Environment:  
- .NET version: 3.0

## Read-Only Files:   
- RoomsService.Tests/IntegrationTests.cs

## Data:  
Example of a room data JSON object:
```
{
    id: 1,
    category: "Luxe",
    number: 122,
    floor: 3,
    isAvailable: true,
    addedDate: 1573843210
}    
```

## Requirements:

A company is launching a room service for managing rooms. The service should be a web API layer using .NET Core 3.0. You already have a prepared infrastructure and need to implement the Web API Controller `RoomsController`. Use the Middleware mechanism from .Net Core to protect the API service from third-party requests.



The following API calls are implemented:

1. Creating rooms - a POST request to endpoint `api/rooms` adds the room to the database. The HTTP response code is 200.
2. Getting all rooms - a GET request to endpoint `api/rooms` returns the entire list of rooms. The HTTP response code is 200.
3. Getting room by id - a GET request to endpoint `api/rooms/{id}` returns the details of the room for `{id}`. If there is no room with `{id}`, status code 404 is returned. On success, status code 200 is returned.
4. Getting all rooms filtered by the Floors property- a GET request to endpoint `api/rooms?Floors={floor1}&Floors={floor2}` returns the entire list of rooms for floor1 and floor2. The HTTP response code is 200.



Complete the Middleware of the project in the following way:

- Implement Middleware that checks if the request contains the header **passwordKey** with the value **passwordKey123456789**. If the request contains this header, allow the request. If not, return HTTP response code 403 Forbidden.



Definition of the Room model:
+ id - The ID of the room. [INTEGER]
+ category - The category of the room. [STRING]
+ number - The number of the room. [INTEGER]
+ floor - The floor of the room. [INTEGER]
+ isAvailable - The flag that shows if the room is available. [BOOLEAN]
+ addedDate - The date when the room was added in UTC (GMT + 0). [EPOCH INTEGER]


## Example requests and responses with headers


**Request 1:**

POST request to `api/rooms`

The request contains the header passwordKey with the value passwordKey123456789:
```
{
    id: 1,
    category: "Luxe",
    number: 122,
    floor: 3,
    isAvailable: true,
    addedDate: 1573843210
}    
```

The response code will be 200 because the document was created.


**Request 2:**

GET request to `api/rooms/1`

The request doesn't contain the header passwordKey with the value passwordKey123456789. Therefore, the response code will be 403.



**Request 3:**

GET request to `api/rooms/1`

The request contains the header passwordKey with the value passwordKey123456789. The response code will be 200 with the room's details:
```
{
    id: 1,
    category: "Luxe",
    number: 122,
    floor: 3,
    isAvailable: true,
    addedDate: 1573843210
}    
```
