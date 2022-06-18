# Plant Database - The Foliage Folio
https://www.youtube.com/watch?v=RdAspJjqeQ0
A web application that allows users to manage a database of their plants and track when they water them.

# About Project
Written in C# using the ASP.NET MVC framework. Features the use of Code-First Migrations to create the database, and WebAPI and LINQ to perform CRUD operations.

# To Run
- Ensure there is an App_Data folder in the project (Right click solution > View in File Explorer)
- Tools > Nuget Package Manager > Package Manage Console > Update-Database
- Create the database using (View > SQL Server Object Explorer > MSSQLLocalDb > ..)
- Run API commands through CURL to add plants
