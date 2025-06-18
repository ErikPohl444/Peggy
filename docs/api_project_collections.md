# Project Collections API Documentation

This document provides detailed information about the Project Collections API endpoints, including request/response formats, status codes, and examples.

## Base URL

All API endpoints are relative to the base URL: `/api/projectcollection`

## Endpoints

### Create Collection

Creates a new project collection.

- **URL**: `/`
- **Method**: `POST`
- **Auth Required**: Yes
- **Request Body**:
  ```json
  {
    "name": "string",
    "description": "string",
    "ownerUserId": "integer"
  }
  ```
- **Success Response**:
  - **Code**: 201 Created
  - **Content**:
    ```json
    {
      "collectionId": "integer",
      "name": "string",
      "description": "string",
      "ownerUserId": "integer",
      "creationDate": "datetime",
      "projects": []
    }
    ```
- **Error Response**:
  - **Code**: 500 Internal Server Error
  - **Content**: `{ "message": "An error occurred while creating the collection" }`

### Get Collection

Retrieves a specific project collection by ID.

- **URL**: `/{id}`
- **Method**: `GET`
- **Auth Required**: Yes
- **URL Parameters**:
  - `id`: The ID of the collection to retrieve
- **Success Response**:
  - **Code**: 200 OK
  - **Content**:
    ```json
    {
      "collectionId": "integer",
      "name": "string",
      "description": "string",
      "ownerUserId": "integer",
      "creationDate": "datetime",
      "projects": [
        {
          "projectId": "integer",
          "name": "string",
          "description": "string",
          "ownerUserId": "integer"
        }
      ]
    }
    ```
- **Error Response**:
  - **Code**: 404 Not Found
  - **Content**: `{ "message": "Collection not found" }`
  - **Code**: 500 Internal Server Error
  - **Content**: `{ "message": "An error occurred while retrieving the collection" }`

### Get All Collections

Retrieves all project collections.

- **URL**: `/`
- **Method**: `GET`
- **Auth Required**: Yes
- **Success Response**:
  - **Code**: 200 OK
  - **Content**:
    ```json
    [
      {
        "collectionId": "integer",
        "name": "string",
        "description": "string",
        "ownerUserId": "integer",
        "creationDate": "datetime",
        "projects": []
      }
    ]
    ```
- **Error Response**:
  - **Code**: 500 Internal Server Error
  - **Content**: `{ "message": "An error occurred while retrieving collections" }`

### Update Collection

Updates an existing project collection.

- **URL**: `/{id}`
- **Method**: `PUT`
- **Auth Required**: Yes
- **URL Parameters**:
  - `id`: The ID of the collection to update
- **Request Body**:
  ```json
  {
    "collectionId": "integer",
    "name": "string",
    "description": "string",
    "ownerUserId": "integer"
  }
  ```
- **Success Response**:
  - **Code**: 200 OK
  - **Content**:
    ```json
    {
      "collectionId": "integer",
      "name": "string",
      "description": "string",
      "ownerUserId": "integer",
      "creationDate": "datetime",
      "projects": []
    }
    ```
- **Error Response**:
  - **Code**: 400 Bad Request
  - **Content**: `{ "message": "Collection ID mismatch" }`
  - **Code**: 500 Internal Server Error
  - **Content**: `{ "message": "An error occurred while updating the collection" }`

### Delete Collection

Deletes a project collection.

- **URL**: `/{id}`
- **Method**: `DELETE`
- **Auth Required**: Yes
- **URL Parameters**:
  - `id`: The ID of the collection to delete
- **Success Response**:
  - **Code**: 204 No Content
- **Error Response**:
  - **Code**: 500 Internal Server Error
  - **Content**: `{ "message": "An error occurred while deleting the collection" }`

### Add Project to Collection

Adds a project to a collection.

- **URL**: `/{collectionId}/projects/{projectId}`
- **Method**: `POST`
- **Auth Required**: Yes
- **URL Parameters**:
  - `collectionId`: The ID of the collection
  - `projectId`: The ID of the project to add
- **Success Response**:
  - **Code**: 204 No Content
- **Error Response**:
  - **Code**: 500 Internal Server Error
  - **Content**: `{ "message": "An error occurred while adding the project to the collection" }`

### Remove Project from Collection

Removes a project from a collection.

- **URL**: `/{collectionId}/projects/{projectId}`
- **Method**: `DELETE`
- **Auth Required**: Yes
- **URL Parameters**:
  - `collectionId`: The ID of the collection
  - `projectId`: The ID of the project to remove
- **Success Response**:
  - **Code**: 204 No Content
- **Error Response**:
  - **Code**: 500 Internal Server Error
  - **Content**: `{ "message": "An error occurred while removing the project from the collection" }`

## Data Models

### ProjectCollection

```json
{
  "collectionId": "integer",
  "name": "string",
  "description": "string",
  "ownerUserId": "integer",
  "creationDate": "datetime",
  "projects": [
    {
      "projectId": "integer",
      "name": "string",
      "description": "string",
      "ownerUserId": "integer"
    }
  ]
}
```

## Error Handling

All endpoints follow a consistent error handling pattern:

1. **400 Bad Request**: Invalid input data
2. **404 Not Found**: Resource not found
3. **500 Internal Server Error**: Server-side error

Error responses include a message explaining the error.

## Rate Limiting

The API implements rate limiting to prevent abuse. The current limits are:
- 100 requests per minute per IP address
- 1000 requests per hour per IP address

## Authentication

All endpoints require authentication using JWT tokens. Include the token in the Authorization header:
```
Authorization: Bearer <your-token>
```

## Examples

### Creating a Collection

```http
POST /api/projectcollection
Authorization: Bearer <your-token>
Content-Type: application/json

{
  "name": "My Art Collection",
  "description": "A collection of my favorite art projects",
  "ownerUserId": 1
}
```

### Adding a Project to a Collection

```http
POST /api/projectcollection/1/projects/2
Authorization: Bearer <your-token>
```

### Getting a Collection with Projects

```http
GET /api/projectcollection/1
Authorization: Bearer <your-token>
``` 