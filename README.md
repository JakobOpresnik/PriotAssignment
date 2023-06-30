# PRIOT Backend Interview Assignment

## Introduction

In 2022 Priot d.o.o. was tasked with implementing
a [Smart Cross-Country Tracks](https://priot.io/project/smart-cross-country-tracks/) system in Planica in order to:

- Implement ticketing control,
- Issue tickets and,
- Provide an easy to use personal timing system.

Cross-Country
skiers buy seasonal or daily tickets containing RFID tags. When running along the tracks, they are intercepted
by RFID antennas that send time measurements to the server. Users can scan a QR code on their ticket to view their
results, status, and all time records.

## Assignment Overview

In this assignment please implement a simplified Smart Tracks system where only 1 measuring station
is used.
Skiers run in laps and an RFID reading happens once per lap.

Implement two API endpoints in `TimingController` explained below. The controller endpoints should send/query data
to/from `MeasurementService` and `FastestLapService`.

### Measurement `POST api/v1/timing/measurement/`

A measuring station sends the reading data to the endpoint with the following payload:

```
{
  "rfid": "E00000001",
  "date": "2023-01-01T09:00:00.000Z"
}
```

**RFID** tags are uniquely represented by their Electronic Product Code (EPC) as a string. In our system the first
character of a valid EPC is `E`, followed by 8 hexadecimal digits.

Save the measurements in memory in `MeasurementService`. The endpoint returns status code `200` if the post was
successful. If the EPC field is invalid (see above), the endpoint returns status code `400` with an error response.

### Fastest Laps `GET api/v1/timing/fastest_laps/`

There is a screen next to the measuring station that displays the all time fastest laps. Implement a Fastest Laps
endpoint that returns a list of laps calculated by `FastestLapService`. The laps are ordered by duration.

**Lap duration** is defined as the time difference between two consecutive measurements with the same RFID.

```
[
  {
    "rfid": "string",
    "duration": "00:05:00",
    "measurements": [
      {
        "rfid": "string",
        "date": "2023-01-01T09:00:00.000Z"
      },
      {
        "rfid": "string",
        "date": "2023-01-01T09:05:00.000Z"
      }
    ]
  },
  ...
]
```

The endpoint returns status `200` if it was successful.

Add a new Unit Tests project in the solution. Implement unit tests for resolving fastest laps from a measurements list
to make sure the implementation is correct.

## Bonus points

Implement additional features into the assignment to showcase your skills. Upgrades can be content or tech oriented (authentication, connecting to a database, including an external API). Do your best :)


## Discussion

Please discuss and comment the following questions:

1. How could we adjust the API to show user's personal results?
2. What's the time complexity of finding fastest laps? Could it be improved?
3. Is the project code appropriate for use with a database (instead of in-memory saving)? If not, what would we have to
   change to allow calculations saving and querying on database level? (imagine we have 1M entries in the database, what would be your approach?)
4. Suggest possible upgrades and changes to improve the project (both content and technical).

Answers to the questions above:

1. We could add another endpoint which would return all laps (including their durations) of a particular user, based on the user's id or username.
2. Time complexity of finding the fastest lap in my case is O(n^2) because I used 2 for loops
   (outer loop - iterating over different RFIDs, inner loop - iterating over consecutive measurements of each RFID, to calculate duration of each lap).
   Time complexity could perhaps be improved if we used different data structures like dictionaries or hashmaps for faster lookups,
   which could bring the time complexity down to linear.
3. To make the project code appropriate for use with a database we would need to:
   - setup a database system and connect it to the IDE we are developing in,
   - clearly define database entities and then create corresponding classes in C# to properly match the database entities,
   - use/implement some sort of ORM (Object Relational Mapping) to convert between the C# objects and database entities,
   - update the code so we are no longer saving objects in the memory and use database operations like querying,
   - implement some sort of optimization, caching and batch queries to improve performance (in case of large amounts of entries),
   - update unit tests so they are compatible with database usage
4. I would add some more controllers and endpoints for handling:
   - user authentication (as I did - perhaps using JWT token authentication),
   - user result display,
   - different track locations,
   - teams of users for competing in groups

## Submission

Please create and share a private GitHub repository with [ZigaByte](https://github.com/ZigaByte) and notify us by email.
