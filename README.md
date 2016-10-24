WHAT IS IT ?

A demonstration project based on Angular JS and ASPNET CORE MVC API

- AngularJS for client side interactions: views, ajax requests, some business logic
- ASP.net core REST API
- ASP.net identity for user data storage and cookie based authentication
- Entity framework 7 used for interacting with SQL Server

USER GUIDE

- Install .NET Core 1.0 SDK and CLI
- Check connectionstring in appsettings.json
- Deploy migration using "dotnet ef database update"
- Start Kestrel server from command line by typing "dotnet run"

Project was migrated from RC1 to 1.0 on 07/07/16

TODO

- Ajouter GRUNT pour la minification + création du package de livraison
- Quelques bugs identifiés
- J'ai peur qu'il faille refactoriser le NgContext car compliqué de mocker les DBSET. Il faudra peut-être passer par le pattern repository, qui lui-même utilisera le NgContext
  Cela consistera à passer par des méthodes plus spécialisés (genre GetRecetteById) plutôt que par du LINQ directement dans le contrôleur

.NET VERSION

.NET version Core RC1

