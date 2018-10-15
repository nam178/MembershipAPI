# Quick Start
.NET Core WebAPI excerise.

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
