
## RabbitMQ For Docker

docker pull rabbitmq:management
docker run -d -p 5672:5672 -p 15672:15672 --name rabbitmq rabbitmq:management

## Microservices Endpoints

IdentityServer : https://localhost:5001
DeviceCenterAPI : https://localhost:6001

## Develop ASP.NET Core apps using a file watcher

dotnet watch --project .\Services\DeviceCenter\ZeroFramework.DeviceCenter.API run
dotnet watch --project .\Services\Identity\ZeroFramework.IdentityServer.API run

## Create DbContext Migrations

Add-Migration InitialCreate -c PersistedGrantDbContext -o Migrations/PersistedGrantMigrations -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API
Add-Migration InitialCreate -c ConfigurationDbContext -o Migrations/ConfigurationMigrations -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API
Add-Migration InitialCreate -c ApplicationDbContext -o Migrations/ApplicationMigrations -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API
Add-Migration InitialCreate -Context DeviceCenterDbContext -Project ZeroFramework.DeviceCenter.Infrastructure -StartupProject ZeroFramework.DeviceCenter.Infrastructure

Update-Database -Context PersistedGrantDbContext -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API
Update-Database -Context ConfigurationDbContext -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API
Update-Database -Context ApplicationDbContext -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API
Update-Database -Context DeviceCenterDbContext -Project ZeroFramework.DeviceCenter.Infrastructure -StartupProject ZeroFramework.DeviceCenter.Infrastructure

Drop-Database -Context DeviceCenterDbContext -Project ZeroFramework.DeviceCenter.Infrastructure -StartupProject ZeroFramework.DeviceCenter.Infrastructure
Drop-Database -Context ApplicationDbContext -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API
