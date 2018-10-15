# Introduction

To satisfy the requirements, the API was implemented with OAuth authentication, in which: 

- Each tenant applications (The OAuth clients) is assigned with a **OAuth ID** and **OAuth Secret**.
- To make API calls (update password, activate user, etc.) on the user's behalf, first tenant applications must request access token `GET /api/auth`, passing its ID and secret (using HTTP Basic Authentication) along with **username** and **password** (as query string) they collect from the user.
- Tenant applications can also create user by calling the endpoint `POST /api/member`

# Build, run and test the API

## Build

You can build the docker image from this source code, or pull it down from Docker Hub.

`docker build -t membership-api .` Or  `docker pull namdocker/membership-api`

## Run 

`docker run -p 8080:8080 namdocker/membership-api`

The API should now available in your localhost on port 8080 for testing.

## Browse and test

Swagger documentation is located at

`http://localhost/swagger/v1/swagger.json`
