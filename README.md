# SchooProjectlApi

A **School Management System API** built with **.NET 8**, **Entity Framework Core**, and **SQL Server**.  
Supports JWT authentication, role-based authorization, and CRUD operations for Users, Courses, Assignments, Enrollments, and Grades.

---

## Features

- **Users**: Admin, Teacher, Student
- **Courses**: Create, Update, Delete, View
- **Assignments**: Add, Submit, Grade
- **Enrollments**: Admin assigns students to courses
- **Grades**: Track student performance
- **Authentication**: JWT-based
- **Authorization**: Role-based
- **Pagination & Filtering**: For list endpoints
- **Logging**: Serilog logs to file
- **Global Exception Handling**: Middleware catches unhandled exceptions

---

## Quick Start

1. **Clone the repository**:

```bash
git clone https://github.com/YOUR_USERNAME/SchooProjectlApi.git
cd SchooProjectlApi
