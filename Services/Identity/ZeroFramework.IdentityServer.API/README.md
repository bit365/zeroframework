
## Identity Server Document

https://identitymodel.readthedocs.io

https://Duende.IdentityServer.readthedocs.io

https://github.com/IdentityServer/Duende.IdentityServer.Quickstart.UI

https://github.com/domaindrivendev/Swashbuckle.AspNetCore/tree/master/test/WebSites/OAuth2Integration

https://github.com/skoruba/Duende.IdentityServer.Admin

https://github.com/dotnet/aspnetcore/tree/master/src/Identity

## 30+ Best Login Template Bootstrap 

https://www.bootstrapdash.com/bootstrap-login-template
https://github.com/nauvalazhar/bootstrap-4-login-page
https://www.bootstrapdash.com/demo/star-admin-free/jquery/src/pages/samples/login.html
https://www.bootstrapdash.com/demo/stellar-admin-free/jquery/pages/samples/login.html

## Create DbContext Migrations

Add-Migration InitialCreate -c PersistedGrantDbContext -o Migrations/PersistedGrantMigrations -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API
Add-Migration InitialCreate -c ConfigurationDbContext -o Migrations/ConfigurationMigrations -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API
Add-Migration InitialCreate -c ApplicationDbContext -o Migrations/ApplicationMigrations -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API

Update-Database -Context PersistedGrantDbContext -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API
Update-Database -Context ConfigurationDbContext -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API
Update-Database -Context ApplicationDbContext -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API

## Creates SSL Certificate

makecert.exe -r -n "CN=idsrvtest" -pe -sv idsrvtest.pvk -a sha1 -len 2048 -b 11/11/2020 -e 11/11/2088 idsrvtest.cer
pvk2pfx.exe -pvk idsrvtest.pvk -spc idsrvtest.cer -pfx idsrvtest.pfx  -pi idsrvtest