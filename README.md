# Blogifier.SPA

Blogifier.SPA is multi-user, lightweight blog written in .NET Core with Angular front-end. For programmers, it is easy maintain and extend and for designers, it is simple to customize using familiar tools.

<!--- commented out until azure pipelines work with .net core 3.1
[![Build Status](https://dev.azure.com/rtur/Blogifier/_apis/build/status/blogifierdotnet.Blogifier)](https://dev.azure.com/rtur/Blogifier/_build/latest?definitionId=3)
-->

## System Requirements

* Windows, Mac or Linux
* ASP.NET Core 3.1
* Visual Studio 2019, VS Code or other code editor (Atom, Sublime etc)
* SQLite by default, MS SQL Server, Postgres and MySQL out of the box, EF compatible databases should work

## Getting Started

Blogifier is single blog supporting multiple authors. Administrator can manage common blog settings and create/remove regular users. Blog author can create and publish posts.

1. Clone or download source code
2. Run application in Visual Studio or using your code editor
3. Use admin/admin to log in as admininstrator
4. Use demo/demo to log in as regular user

## Other Projects

Blogifier.SPA relies on `Blogifier.API` and `Blogifier.Core` for the shared core functionality. Both projects 
are class libraries with source code hosted here under `github.com/blogifierdotnet` and deployed to `Nuget.org`.

![blogifier-spa-diagram](https://user-images.githubusercontent.com/1932785/78629955-1bfc4d00-785e-11ea-90b7-f3c8421c859e.png)

## Themes

Source code for themes distributed with Blogifier  is in the [themes](https://github.com/blogifierdotnet/themes) project. 
Themes are Angular CLI applications and developed independently.

> Blogifier requires write permissions on `wwwroot/themes` folder to be able to switch themes in the admin panel.

## Developer blog

Can also check out [developer's blog](http://rtur.net) for details.