# User Management API - Submission Summary

## Rubric Coverage (25 points)

### 1) GitHub repository created (5 pts)

- Status: Pending final push from local machine.
- Local project is ready for upload.
- Suggested first commit message: `Initial commit`
- Add your repository URL here after publishing: `<PASTE_YOUR_GITHUB_URL_HERE>`

### 2) CRUD endpoints implemented (5 pts)

- `GET /api/users`
- `GET /api/users/{id}`
- `POST /api/users`
- `PUT /api/users/{id}`
- `DELETE /api/users/{id}`

### 3) Copilot-assisted debugging completed (5 pts)

- Identified and fixed multiple reliability issues:
  - Missing/weak input validation
  - Non-existent user handling improvements
  - Unhandled exception crash protection
  - Safer service-layer operations

### 4) Validation for valid user data (5 pts)

- Added model validation attributes:
  - Required `Name`
  - Name length constraints
  - Required and valid `Email` format
- Controller now returns validation errors and defensive bad-request checks.
- Duplicate email protection added in service logic.

### 5) Middleware implemented (5 pts)

- Logging middleware in pipeline.
- Middleware now handles unexpected exceptions and returns a safe `500` JSON response.
- Request method/path/status/duration are logged.

## Edge-case testing

Use `UserManagementAPI.http` to verify:

- Invalid input payloads
- Non-existent IDs for GET and DELETE
- Normal create/update/delete flow

## Notes

- This API uses in-memory storage for users, suitable for course activities and debugging practice.
- For production use, replace with a persistent database and add authentication/authorization.
