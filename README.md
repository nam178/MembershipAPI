# Introduction

To satisfy the requirements, the API was implemented with OAuth authentication, in which: 

- Each tenant application (The OAuth clients) is assigned with an **OAuth ID** and an **OAuth Secret**.
- Tenant applications can create new user by calling `POST /api/register`, using the ID and Secret above to authenticate.
- To make other API calls (update password, activate user, etc.) on the user's behalf, tenant applications must first request access token `GET /api/auth`, passing its ID and secret (using HTTP Basic Authentication) along with the **username** and **password** (as query string) they collect from the user.
- The application should be deployed over HTTPS, however for the simplicy of this demo HTTP is used.

# Build, run and test the API

## Build

You can build the docker image from this source code, or pull it down from Docker Hub.

`docker build -t membership-api .` Or  `docker pull namdocker/membership-api`

## Run 

`docker run -p 8080:8080 namdocker/membership-api`

The API should now available in your localhost on port 8080 for testing.

## Browse and test

### Swagger documentation is located at

`http://localhost:8080/swagger/v1/swagger.json`

### Sample Credentials

Tenant Application 1 OAuth ID: `esiStOMporitandRogYRaTRacstigHAnTERStATH`

Tenant Application 1 OAuth Secret: `$lGh6QlOIPRgd87DWeOUo5Y!H!%GOPsI0YdFxDN1`

Tenant Application 2 OAuth ID: `iONUMbRovELoPHOmFoRGouNDRUstrAtERBactOLd`

Tenant Application 2 OAuth Secret: `lGsxh03Qw^qXb@8ot*oa5NCS5qtjZIU^$gK&T0!B`

Tenant Application 3 OAuth ID: `diChoRtMAsterNaMartYphinGTaCeTOlDEnArdER`

Tenant Application 3 OAuth Secret: `l^W!Q$P!oCe%RX5R4k8Ejbk6DdW#wiIPSIGH*Uq*`


### Test step 1: Create account for Tenant Application 1
Using the tenant application 1's ID and secret above, we create a new user for testing:

```
curl -X POST \
  http://localhost:8080/api/register \
  -H 'Content-Type: application/json' \
  -H 'Authorization: Basic ZXNpU3RPTXBvcml0YW5kUm9nWVJhVFJhY3N0aWdIQW5URVJTdEFUSDokbEdoNlFsT0lQUmdkODdEV2VPVW81WSFIISVHT1BzSTBZZEZ4RE4x' \
  -H 'cache-control: no-cache' \
  -d '{"firstName": "Nam", "lastName": "Duong", "plainPassword": "12345678", "username": "nam-admin", "isAdmin": true }'
```

### Test step 2: Login as the user we've created

```
curl -X GET \
  'http://localhost:8080/api/auth?grant_type=password&username=nam-admin&password=12345678' \
  -H 'Authorization: Basic ZXNpU3RPTXBvcml0YW5kUm9nWVJhVFJhY3N0aWdIQW5URVJTdEFUSDokbEdoNlFsT0lQUmdkODdEV2VPVW81WSFIISVHT1BzSTBZZEZ4RE4x'
```

We now receive a token that can be used to make user-related requests

### Test step 3: Try changing my password

Using the token received from the previous step, we can change the password for the user associated with that token:

```
curl -X PUT \
  http://localhost:8080/api/password \
  -H 'Content-Type: application/json' \
  -H 'Authorization: Bearer TOKEN_FROM_PREVIOUS_STEP' \
  -H 'cache-control: no-cache' \
  -d '{ "oldPassword": "12345678", "newPassword": "123456" }'
``` 

### Test step 4: Try de-activating other users

Because the current user is the admin of application 1, and it has accquired token under application 1, it can activate/deactivate any users (for application 1 only):

```
curl -X PUT \
  'http://localhost:8080/api/deactivate?userId=dbd3346d-b52c-4f8a-a957-aee814700483' \
  -H 'Content-Length: 0' \
  -H 'Authorization: Bearer TOKEN_FROM_PREVIOUS_STEP'
 
```

