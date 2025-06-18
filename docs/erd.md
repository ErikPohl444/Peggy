# Entity Relationship Diagram (ERD)

## Overview
This document provides a visual representation of the database structure and relationships between entities in the Peggy system. The system supports both individual project patronage and collection-based patronage through a hierarchical project structure.

## Diagram
```mermaid
erDiagram
    User ||--o{ Project : creates
    User ||--o{ Patronage : makes
    Project ||--o{ Patronage : has
    Patronage ||--o{ PatronagePayment : receives
    Project ||--o{ Project : contains

    User {
        int UserId PK
        string Username
        string Email
        string PasswordHash
        DateTime CreatedAt
    }

    Project {
        int ProjectId PK
        string Name
        string Description
        int OwnerUserId FK
        int? ProjectParentId FK
        DateTime CreatedAt
    }

    Patronage {
        int PatronageId PK
        int UserId FK
        int ProjectId FK
        decimal Amount
        DateTime CreatedAt
    }

    PatronagePayment {
        int PaymentId PK
        int PatronageId FK
        decimal Amount
        DateTime PaymentDate
        string Status
    }
```

## Entity Descriptions

### User
- Primary entity representing system users
- Can create multiple projects
- Can make multiple patronages
- Can be either a project creator or a patron

### Project
- Represents creative projects in the system
- Belongs to a user (owner)
- Can have multiple patronages
- Can have a parent project (optional)
- Can have multiple child projects
- Can be either:
  - A parent project (collection)
  - A child project (individual work)
  - A standalone project

### Patronage
- Represents a user's patronage of a project
- Links a user to a project
- Can have multiple payments
- Tracks the total patronage amount
- Can support either:
  - Individual projects
  - Collection projects (parent projects)

### PatronagePayment
- Represents individual payments made to a patronage
- Belongs to a single patronage
- Tracks payment amount and status
- Records payment date

## Relationships

1. **User to Project**
   - One-to-Many
   - A user can create multiple projects
   - Each project has one owner

2. **User to Patronage**
   - One-to-Many
   - A user can make multiple patronages
   - Each patronage is made by one user

3. **Project to Patronage**
   - One-to-Many
   - A project can have multiple patronages
   - Each patronage is for one project
   - Supports both individual and collection patronage

4. **Patronage to PatronagePayment**
   - One-to-Many
   - A patronage can have multiple payments
   - Each payment belongs to one patronage

5. **Project to Project (Self-referencing)**
   - One-to-Many
   - A project can have one parent project
   - A project can have multiple child projects
   - Enables collection-based project organization
   - Allows for hierarchical project structures

## Project Hierarchy Examples

1. **Collection-based Structure**:
   ```
   Photography Series (Parent Project)
   ├── Landscape Collection
   │   ├── Mountain Photos
   │   ├── Ocean Photos
   │   └── Forest Photos
   └── Portrait Collection
       ├── Studio Portraits
       └── Outdoor Portraits
   ```

2. **Series-based Structure**:
   ```
   Novel Series (Parent Project)
   ├── Book 1
   ├── Book 2
   └── Book 3
   ```

3. **Category-based Structure**:
   ```
   Art Portfolio (Parent Project)
   ├── Paintings
   ├── Drawings
   └── Digital Art
   ```

## Notes
- All entities include creation timestamps for auditing
- Foreign keys are properly indexed for performance
- Soft delete is implemented where appropriate
- All monetary values use decimal type for precision
- Project hierarchy is optional (ProjectParentId is nullable)
- Patrons can support either individual projects or entire collections
- Collection support automatically includes all child projects 