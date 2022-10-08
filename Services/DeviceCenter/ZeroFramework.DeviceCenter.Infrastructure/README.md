
## Create DbContext Migrations

Add-Migration InitialCreate -Context DeviceCenterDbContext -Project ZeroFramework.DeviceCenter.Infrastructure -StartupProject ZeroFramework.DeviceCenter.Infrastructure

Update-Database -Context DeviceCenterDbContext -Project ZeroFramework.DeviceCenter.Infrastructure -StartupProject ZeroFramework.DeviceCenter.Infrastructure

