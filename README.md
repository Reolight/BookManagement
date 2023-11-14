# BookManagement

This is a ASP.NET Core Web API 7 web-application imitating work of library.

## Features overview

1. Authorization. To use all features of the application user must be authorized. To sign up user must enter email, name and simple password (minimal length is 4). Authentication is based on JWT tokens.
2. Librarian (authorized user) can add books, update or delete them.
3. Search by id, ISBN is available.
4. A book can be marked as borrowed with two dates: the first one when the book was borrowed *and* the second one when the book should be returned.
5. Librarian can get list of all books/borrowed books/all owed books.

**ISBN** must contain **17 characters**. **13** are **digits** and **4** are **delimiters** *(which are commonly hyphens)*

## How to run project
1. Clone/download source code from this repo
2. In appsettings.json change connection strings for identity database (stores user data) and for usual database hich stores data about books
3. Create databases:
From .net CLI
```
dotnet ef database update -s Host -p Infrastructure --context AppDbContext
```
```
dotnet ef database update -s Host -p Infrastructure --context IdentityContext
```
Or from PM Console
```
Update-Database -StartupProject Host -Project Infrastructure -Context AppDbContext
```
```
Update-Database -StartupProject Host -Project Infrastructure -Context IdentityContext
```

You may also use SQL scripts provided in migrations folder.
If you don't have dotnet-ef tools:
```
dotnet tool install --global dotnet-ef
```
or update it up to 7.0.13:
```
dotnet tool update --global dotnet-ef
```
4. Run the project.
   If you use .NET CLI, run it as
   ```
   dotnet run --project Host --launch-profile https
   ```

a. Register at *authorize/register* endpoint
b. login at *authorize/login* endpoint
c. Copy token from the response body of step b, press *Authorize* button, write down "bearer " *(with whitespace and without quotes)* and paste the token.
d. Now you have access to all *books/* enpoints.
