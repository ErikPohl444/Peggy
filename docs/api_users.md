# Peggy API Documentation

## Overview
The Peggy API provides endpoints for managing projects, patronages, payments, and system health. All endpoints are RESTful and return JSON responses.

## Base URL
```
https://api.peggy.com/v1
```

## Authentication
All endpoints require JWT authentication. Include the token in the Authorization header:
```
Authorization: Bearer <your_token>
```

## Health Check Endpoints

### Get System Health
```http
GET /health
```
Returns the health status of all services in the system.

**Response**
```json
{
    "status": "Healthy",
    "checks": [
        {
            "name": "database",
            "status": "Healthy",
            "description": "Database is healthy",
            "duration": "00:00:00.1234567"
        },
        {
            "name": "project-service",
            "status": "Healthy",
            "description": "ProjectService is healthy",
            "duration": "00:00:00.2345678",
            "data": {
                "TotalProjects": 42
            }
        },
        {
            "name": "patronage-service",
            "status": "Healthy",
            "description": "PatronageService is healthy",
            "duration": "00:00:00.3456789",
            "data": {
                "TotalPatronages": 156
            }
        },
        {
            "name": "payment-service",
            "status": "Healthy",
            "description": "PatronagePaymentService is healthy",
            "duration": "00:00:00.4567890",
            "data": {
                "TotalPayments": 89
            }
        }
    ]
}
```

### Get Service Health
```http
GET /health/{service}
```
Returns the health status of a specific service.

**Parameters**
- `service`: The name of the service to check (database, project-service, patronage-service, payment-service)

**Response**
```json
{
    "status": "Healthy",
    "checks": [
        {
            "name": "project-service",
            "status": "Healthy",
            "description": "ProjectService is healthy",
            "duration": "00:00:00.2345678",
            "data": {
                "TotalProjects": 42
            }
        }
    ]
}
```

### Health Check Dashboard
```http
GET /health-ui
```
Provides a visual dashboard for monitoring service health.

## Project Endpoints

### Create Project
```http
POST /api/project
```
Creates a new project.

**Request Body**
```json
{
    "name": "string",
    "description": "string",
    "projectParentId": "integer?",
    "creationDate": "datetime"
}
```

**Response**
```json
{
    "projectId": "integer",
    "name": "string",
    "description": "string",
    "projectParentId": "integer?",
    "creationDate": "datetime"
}
```

### Get Project
```http
GET /api/project/{id}
```
Retrieves a specific project by ID.

**Response**
```json
{
    "projectId": "integer",
    "name": "string",
    "description": "string",
    "projectParentId": "integer?",
    "creationDate": "datetime"
}
```

### Get All Projects
```http
GET /api/project
```
Retrieves all projects.

**Response**
```json
[
    {
        "projectId": "integer",
        "name": "string",
        "description": "string",
        "projectParentId": "integer?",
        "creationDate": "datetime"
    }
]
```

### Update Project
```http
PUT /api/project/{id}
```
Updates an existing project.

**Request Body**
```json
{
    "projectId": "integer",
    "name": "string",
    "description": "string",
    "projectParentId": "integer?",
    "creationDate": "datetime"
}
```

**Response**
```json
{
    "projectId": "integer",
    "name": "string",
    "description": "string",
    "projectParentId": "integer?",
    "creationDate": "datetime"
}
```

### Delete Project
```http
DELETE /api/project/{id}
```
Deletes a project.

**Response**
- 204 No Content on success
- 404 Not Found if project doesn't exist

## Patronage Endpoints

### Create Patronage
```http
POST /api/patronage
```
Creates a new patronage.

**Request Body**
```json
{
    "userId": "integer",
    "projectId": "integer",
    "amount": "decimal",
    "startDate": "datetime",
    "endDate": "datetime?"
}
```

**Response**
```json
{
    "patronageId": "integer",
    "userId": "integer",
    "projectId": "integer",
    "amount": "decimal",
    "startDate": "datetime",
    "endDate": "datetime?"
}
```

### Get Patronage
```http
GET /api/patronage/{id}
```
Retrieves a specific patronage by ID.

**Response**
```json
{
    "patronageId": "integer",
    "userId": "integer",
    "projectId": "integer",
    "amount": "decimal",
    "startDate": "datetime",
    "endDate": "datetime?"
}
```

### Get All Patronages
```http
GET /api/patronage
```
Retrieves all patronages.

**Response**
```json
[
    {
        "patronageId": "integer",
        "userId": "integer",
        "projectId": "integer",
        "amount": "decimal",
        "startDate": "datetime",
        "endDate": "datetime?"
    }
]
```

### Update Patronage
```http
PUT /api/patronage/{id}
```
Updates an existing patronage.

**Request Body**
```json
{
    "patronageId": "integer",
    "userId": "integer",
    "projectId": "integer",
    "amount": "decimal",
    "startDate": "datetime",
    "endDate": "datetime?"
}
```

**Response**
```json
{
    "patronageId": "integer",
    "userId": "integer",
    "projectId": "integer",
    "amount": "decimal",
    "startDate": "datetime",
    "endDate": "datetime?"
}
```

### Delete Patronage
```http
DELETE /api/patronage/{id}
```
Deletes a patronage.

**Response**
- 204 No Content on success
- 404 Not Found if patronage doesn't exist

## Payment Endpoints

### Create Payment
```http
POST /api/payment
```
Creates a new payment.

**Request Body**
```json
{
    "patronageId": "integer",
    "amount": "decimal",
    "paymentDate": "datetime",
    "status": "string"
}
```

**Response**
```json
{
    "paymentId": "integer",
    "patronageId": "integer",
    "amount": "decimal",
    "paymentDate": "datetime",
    "status": "string"
}
```

### Get Payment
```http
GET /api/payment/{id}
```
Retrieves a specific payment by ID.

**Response**
```json
{
    "paymentId": "integer",
    "patronageId": "integer",
    "amount": "decimal",
    "paymentDate": "datetime",
    "status": "string"
}
```

### Get All Payments
```http
GET /api/payment
```
Retrieves all payments.

**Response**
```json
[
    {
        "paymentId": "integer",
        "patronageId": "integer",
        "amount": "decimal",
        "paymentDate": "datetime",
        "status": "string"
    }
]
```

### Update Payment
```http
PUT /api/payment/{id}
```
Updates an existing payment.

**Request Body**
```json
{
    "paymentId": "integer",
    "patronageId": "integer",
    "amount": "decimal",
    "paymentDate": "datetime",
    "status": "string"
}
```

**Response**
```json
{
    "paymentId": "integer",
    "patronageId": "integer",
    "amount": "decimal",
    "paymentDate": "datetime",
    "status": "string"
}
```

### Delete Payment
```http
DELETE /api/payment/{id}
```
Deletes a payment.

**Response**
- 204 No Content on success
- 404 Not Found if payment doesn't exist

## Error Responses

All endpoints may return the following error responses:

### 400 Bad Request
```json
{
    "error": "string",
    "message": "string"
}
```

### 401 Unauthorized
```json
{
    "error": "Unauthorized",
    "message": "Invalid or missing authentication token"
}
```

### 404 Not Found
```json
{
    "error": "Not Found",
    "message": "The requested resource was not found"
}
```

### 500 Internal Server Error
```json
{
    "error": "Internal Server Error",
    "message": "An unexpected error occurred"
}
``` 