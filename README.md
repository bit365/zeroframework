
 ♥ 项目基本介绍

水乙方框架是一套基于微服务和领域模型驱动设计的企业级快速开发框架，基于微软 .NET 7+ 最新技术栈构建，容器化微服务最佳实践，水乙方框架的搭建以开发简单，多屏体验，前后端分离，灵活部署，最少依赖，最新框架为原则，以物联网平台管理系统为业务模型，参考诸多优秀开源框架，采用主流稳定的技术栈，从零开始搭建企业级架构。

后端技术：Visual Studio 2022 + C# 11.0 + .NET 7.0 + ASP.NET Core + EF Core

前端技术：Visual Studio Code + Node.js + TypeScript + React + ANTD

技术架构：https://www.xcode.me/business/we-framework

视频教程：https://www.xcode.me/video/list/1000225681

演示地址：https://cloud.syifang.com

演示账号：用户名 admin 密码 guest


 ♥ 在 Visual Studio 2022 中运行后端微服务

1、首先使用 EF Code First 使用代码生成数据库，为了简化操作，以下脚本可完成数据库删除创建迁移并重建数据库，在 Visual Studio 选择「工具」->「NuGet包管理器」->「程序包管理控制台」中执行以下命令即可：

Drop-Database -Context DeviceCenterDbContext -Project ZeroFramework.DeviceCenter.Infrastructure -StartupProject ZeroFramework.DeviceCenter.Infrastructure -Confirm:$false
Drop-Database -Context ApplicationDbContext -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API -Confirm:$false

Remove-Migration -Context PersistedGrantDbContext -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API
Remove-Migration -Context ConfigurationDbContext -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API
Remove-Migration -Context ApplicationDbContext -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API
Remove-Migration -Context DeviceCenterDbContext -Project ZeroFramework.DeviceCenter.Infrastructure -StartupProject ZeroFramework.DeviceCenter.Infrastructure

Add-Migration InitialCreate -c PersistedGrantDbContext -o Migrations/PersistedGrantMigrations -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API
Add-Migration InitialCreate -c ConfigurationDbContext -o Migrations/ConfigurationMigrations -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API
Add-Migration InitialCreate -c ApplicationDbContext -o Migrations/ApplicationMigrations -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API
Add-Migration InitialCreate -Context DeviceCenterDbContext -Project ZeroFramework.DeviceCenter.Infrastructure -StartupProject ZeroFramework.DeviceCenter.Infrastructure

Update-Database -Context PersistedGrantDbContext -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API
Update-Database -Context ConfigurationDbContext -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API
Update-Database -Context ApplicationDbContext -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API
Update-Database -Context DeviceCenterDbContext -Project ZeroFramework.DeviceCenter.Infrastructure -StartupProject ZeroFramework.DeviceCenter.Infrastructure

2、设备数据通过「分桶模式」存储在 Mongdb 数据库中，因此你需要安装并启动 Mongdb 数据库，并在配置文件中修改连接字符串。

3、依次启动 ZeroFramework.IdentityServer.API 和 ZeroFramework.DeviceCenter.API 项目，容器、网关和聚合可暂时不配置，后期根据需要进行配置。


 ♥ 在 Visual Studio Code 中运行前端站点

1、首先确保系统已安装 Visual Studio Code 工具和 Node.js 环境，并进入 ZeroFramework.DeviceCenter.Web 目录。

2、在目录启动命令行并运行：「npm install --global yarn」 和 「yarn install」 即可安装前端所需的 NPM 包，如果失败，可重试几次。

3、在目录启动命令行并运行：「npm run dev」即可启动编译，使用 「npm run build 」编译会生成 dist 目录，该目录可直接部署到生产平台。


 ♥ 项目更新记录

+ 所有项目框架及其用到的包已经升级到 .NET 7 最新版，并消除了很多警告和建议。

+ 在配置文件中添加了 「"UseDemoLaunchMode": false」以表示以演示模式运行，演示模式使用 EF Core 拦截器禁用了编辑和删除操作。


 ♥ 项目目录结构说明

zeroframework「项目总目录」
├── ApiGateways「网关和聚合」
│   └── ZeroFramework.ReverseProxy「网关与反向代理」
│       ├── Program.cs
│       ├── Properties
│       ├── ZeroFramework.ReverseProxy.csproj
│       ├── appsettings.Development.json
│       ├── appsettings.json
│       ├── bin
│       └── obj
├── BuildingBlocks「公共中间件」
│   └── EventBus
│       ├── ZeroFramework.EventBus「事件总线抽象」
│       ├── ZeroFramework.EventBus.MemoryQueue「基于内存的队列」
│       └── ZeroFramework.EventBus.RabbitMQ「分布式队列」
├── Services「微服务」
│   ├── DeviceCenter「基于领域驱动的设备中心微服务」
│   │   ├── ZeroFramework.DeviceCenter.API「开放接口」
│   │   ├── ZeroFramework.DeviceCenter.Application「应用层」
│   │   ├── ZeroFramework.DeviceCenter.BackgroundTasks「生成演示」
│   │   ├── ZeroFramework.DeviceCenter.Domain「领域层」
│   │   └── ZeroFramework.DeviceCenter.Infrastructure「基础设施层」
│   └── Identity「认证和授权微服务」
│       └── ZeroFramework.IdentityServer.API「OAuth2.0开放接口」
├── Web
│   └── ZeroFramework.DeviceCenter.Web「基于 ANTD 的前端站点」
│       ├── README.md
│       ├── ZeroFramework.DeviceCenter.Web.esproj
│       ├── config
│       ├── jest.config.js
│       ├── jsconfig.json
│       ├── mock
│       ├── nuget.config
│       ├── package.json
│       ├── public
│       ├── src
│       ├── tests
│       └── tsconfig.json
└── ZeroFramework.sln「解决方案」

♥ 前端学习技术资料整理

TypeScript：http://www.patrickzhong.com/TypeScript
React：https://react.docschina.org/docs/getting-started.html
ECMAScript：https://es6.ruanyifeng.com
ANTD：https://ant.design/docs/react/introduce-cn
ANTD PRO：https://pro.ant.design/zh-CN/docs/overview