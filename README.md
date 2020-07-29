# AirportDistance
A REST service to measure distance in miles between two airports. Airports are identified by 3-letter IATA code.

## Solution structure
| Directory | Description |
| ------ | ------------------------------------------------------------ |
| sources | Directory with source code of Application |
| sources\core | Contains business logic layer |
| sources\infrastructure | Infrastructure layer (including dataaccess) |
| sources\presentation | API application |
| tests | Directory with test projects |


### GET@/airports/distance?from=LED&to=AMS
Returns distance between airports
{
    "from": {
        "country": "Netherlands",
        "city": "Amsterdam",
        "iata": "AMS"
    },
    "to": {
        "country": "Russian Federation",
        "city": null,
        "iata": "LED"
    },
    "distance": 1116.804973511147
}


### GET@/airports/AMS/
Returns information about airport
{
    "country": "Netherlands",
    "city": "Amsterdam",
    "hubs": 7,
    "iata": "AMS",
    "location": {
        "longitude": 4.763385,
        "latitude": 52.309069
    }
}
