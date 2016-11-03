WHAT IS IT ?

A demonstration project based on Angular JS and ASPNET CORE MVC API

- AngularJS for client side interactions: views, ajax requests, some business logic
- ASP.net core REST API
- ASP.net identity + Entity Framework Core for user data storage and cookie based authentication
- Entity Framework Core data persistence in SQL Server

USER GUIDE

- Install .NET Core 1.0 SDK and CLI (http://dot.net)
- Check connectionstring in appsettings.json
- Deploy migration using "dotnet ef database update"
- Start Kestrel server from command line by typing "dotnet run"
- Check app on http://localhost:5000

CREATE A DELIVERY PACKAGE
- Restore npm packages from packages.json using "npm install"
- Type "grunt package"
- Package is located under "build" folder

Project was migrated from RC1 to 1.0 on 07/07/16

.NET VERSION: .NET Core 1.0