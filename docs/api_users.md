# User API Documentation

## Overview
This document provides detailed information about the User API, including endpoints, request/response formats, and usage examples.

## Endpoints

### Create User
- **URL**: `/api/users`
- **Method**: `POST`
- **Description**: Creates a new user.
- **Request Body**:
  ```json
  {
    "username": "string",
    "creationDate": "datetime",
    "createdBy": "number",
    "updateDate": "datetime",
    "updatedBy": "number"
  }
  ```
- **Response**: Returns the created user object.

### Update User
- **URL**: `/api/users/{userId}`
- **Method**: `PUT`
- **Description**: Updates an existing user.
- **Request Body**:
  ```json
  {
    "username": "string",
    "updateDate": "datetime",
    "updatedBy": "number"
  }
  ```
- **Response**: Returns the updated user object.

### Get User
- **URL**: `/api/users/{userId}`
- **Method**: `GET`
- **Description**: Retrieves a user by their ID.
- **Response**: Returns the user object.

### Get All Users
- **URL**: `/api/users`
- **Method**: `GET`
- **Description**: Retrieves all users.
- **Response**: Returns a list of user objects.

### Delete User
- **URL**: `/api/users/{userId}`
- **Method**: `DELETE`
- **Description**: Deletes a user by their ID.
- **Response**: No content.

## Usage Examples

### Create User
```csharp
var user = new User { Username = "NewUser", CreationDate = DateTime.Now, CreatedBy = 1, UpdateDate = DateTime.Now, UpdatedBy = 1 };
var createdUser = await userService.CreateUserAsync(user);
```

### Update User
```csharp
var user = await userService.GetUserByIdAsync(1);
user.Username = "UpdatedUser";
var updatedUser = await userService.UpdateUserAsync(user);
```

### Get User
```csharp
var user = await userService.GetUserByIdAsync(1);
```

### Get All Users
```csharp
var users = await userService.GetAllUsersAsync();
```

### Delete User
```csharp
await userService.DeleteUserAsync(1);
```

## Notes
- Ensure all required fields are provided in the request body.
- Use appropriate error handling to manage API responses. 