# 🚀 User Management API - TechHive Solutions

This project is a **User Management API** built with **ASP.NET Core Minimal API**. It was developed as part of a technical activity to demonstrate the ability to scaffold, debug, and enhance a back-end project using **Microsoft Copilot**.

## ✨ Features implemented

### 🛠 1. Core CRUD Operations
Full implementation of User management endpoints:
* `GET /users` - Retrieve all users.
* `GET /users/{id}` - Retrieve a specific user by ID.
* `POST /users` - Add a new user to the system.
* `PUT /users/{id}` - Update existing user details.
* `DELETE /users/{id}` - Remove a user from the system.

### 🛡 2. Middleware Pipeline (In Order)
To ensure compliance with corporate policies, the following middleware were implemented:
1.  **Error Handling Middleware**: Catches unhandled exceptions and returns standardized JSON error responses. 🛑
2.  **Authentication Middleware**: Validates the presence of a token in the request headers to secure the API. 🔑
3.  **Logging Middleware**: Logs the HTTP method, request path, and response status code to the console for auditing. 📝

### ✅ 3. Data Validation & Debugging
* Implemented input validation for the `POST` and `PUT` methods (checking for empty fields and valid email formats).
* Added robust error handling using `try-catch` blocks to prevent API crashes.
* Handled edge cases like searching for non-existent user IDs.

## 🛠 Tech Stack
* **Framework:** .NET 8.0 (ASP.NET Core Minimal API)
* **Language:** C#
* **Tooling:** Microsoft Copilot, Visual Studio, Swagger/OpenAPI

## 🚀 How to Run
1. Clone the repository:
   ```bash
   git clone [https://github.com/YousefNady/TechHive.git](https://github.com/YousefNady/TechHive.git)
