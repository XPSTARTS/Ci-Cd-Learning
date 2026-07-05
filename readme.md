# Library Manager API

![CI Pipeline](https://github.com/XPSTARTS/Ci-Cd-Learning/actions/workflows/ci.yml/badge.svg)

A simple Library Management API built with ASP.NET Core 9.

## What It Does

- CRUD operations for books and categories
- Business logic with proper validation
- Unit tests and integration tests (xUnit + Moq)
- CI pipeline with GitHub Actions

## Run It

```bash
git clone https://github.com/XPSTARTS/Ci-Cd-Learning.git
cd LibraryManager
dotnet restore
dotnet build
dotnet test
dotnet run --project LibraryManager.Api