WHAT IS IT ?

A demonstration project based on Angular JS and ASPNET CORE MVC API

- AngularJS for client side interactions: views, ajax requests, some business logic
- ASP.net core REST API
- ASP.net identity for user data storage and cookie based authentication
- Entity framework 7 used for interacting with SQL Server

USER GUIDE

Select coreclr x64 as the CLR "dnvm use 1.0.0-rc1-update1 -arch x64 -r coreclr"
Check connectionstring in appsettings.json
Deploy migration using "ef database update"
Start Kestrel server from command line by typing "dnx web"

TODO
Error messages handling on recipe add, login
Recipe's image handling on new recipe