
 ## 框架介绍

零度框架是一套基于微服务和领域模型驱动设计的企业级快速开发框架，基于微软 .NET 7+ 最新技术栈构建，容器化微服务最佳实践，零度框架的搭建以开发简单，多屏体验，前后端分离，灵活部署，最少依赖，最新框架为原则，以物联网平台管理系统为业务模型，参考诸多优秀开源框架，采用主流稳定的技术栈，从零开始搭建企业级架构。

## 相关技术

后端技术：Visual Studio 2022 + C# 12.0 + .NET 8.0 + ASP.NET Core + EF Core

前端技术：Visual Studio Code + Node.js + TypeScript + React + ANTD

## 项目演示

演示地址：https://cloud.helloworldnet.com

演示账号：用户名 admin 密码 guest

## 学习教程

视频教程：https://www.xcode.me/Training/Module/250

零度课堂：https://www.xcode.me


## 项目部署

### 部署 Identity Server 微服务

#### 数据库迁移

修改 「appsettings.Development.json」中的数据库连接字符串，在 Visual Studio 选择「工具」->「NuGet包管理器」->「程序包管理控制台」执行以下命令。

1、删除已有数据库

```shell

Drop-Database -Context ApplicationDbContext -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API -Confirm:$false

```

2、删除迁移文件

```shell


Remove-Migration -Context PersistedGrantDbContext -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API
Remove-Migration -Context ConfigurationDbContext -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API
Remove-Migration -Context ApplicationDbContext -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API

```

3、添加新的迁移文件

```shell

Add-Migration InitialCreate -c PersistedGrantDbContext -o Migrations/PersistedGrantMigrations -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API
Add-Migration InitialCreate -c ConfigurationDbContext -o Migrations/ConfigurationMigrations -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API
Add-Migration InitialCreate -c ApplicationDbContext -o Migrations/ApplicationMigrations -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API

```

3、执行迁移并自动生成数据库

```shell

Update-Database -Context PersistedGrantDbContext -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API
Update-Database -Context ConfigurationDbContext -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API
Update-Database -Context ApplicationDbContext -Project ZeroFramework.IdentityServer.API -StartupProject ZeroFramework.IdentityServer.API
```

4、启动 Identity Server 微服务

在 Visual Studio 中启动 ZeroFramework.IdentityServer.API 项目，如果启动失败，可尝试重新生成解决方案。

### 部署 Device Center 微服务

#### 数据库迁移

修改 「appsettings.Development.json」中的数据库连接字符串，在 Visual Studio 选择「工具」->「NuGet包管理器」->「程序包管理控制台」执行以下命令。

1、删除已有数据库

```shell

Drop-Database -Context DeviceCenterDbContext -Project ZeroFramework.DeviceCenter.Infrastructure -StartupProject ZeroFramework.DeviceCenter.Infrastructure -Confirm:$false

```

2、删除迁移文件

```shell

Remove-Migration -Context DeviceCenterDbContext -Project ZeroFramework.DeviceCenter.Infrastructure -StartupProject ZeroFramework.DeviceCenter.Infrastructure
    
```

3、添加新的迁移文件

```shell

Add-Migration InitialCreate -Context DeviceCenterDbContext -Project ZeroFramework.DeviceCenter.Infrastructure -StartupProject ZeroFramework.DeviceCenter.Infrastructure
```

4、执行迁移并自动生成数据库
    
```shell
Update-Database -Context DeviceCenterDbContext -Project ZeroFramework.DeviceCenter.Infrastructure -StartupProject ZeroFramework.DeviceCenter.Infrastructure

```

5、由于该微服务是物联网数据管理平台，设备数据使用分桶模式存储在 MongoDB 数据库中， 因此需要你安装 MongoDB 数据库，并修改 「appsettings.Development.json」中的 MongoDB 连接字符串。


6、启动 Device Center 微服务

在 Visual Studio 中启动 ZeroFramework.DeviceCenter.API 项目，如果启动失败，可尝试重新生成解决方案。


## 其它服务配置

可根据项目情况在 ZeroFramework.ReverseProxy 微服务中自行配置网关和聚合

## 前端 React 项目运行  

### 使用 Visual Studio Code 运行前端项目

首先确保系统已安装 Visual Studio Code 工具和 Node.js 环境，并打开 ZeroFramework.DeviceCenter.Web 所在目录。

### 安装依赖 Yarn 前端包管理工具

在 Visual Studio Code 中打开终端，运行以下命令：

```shell
npm install --global yarn
````
### 安装依赖 NPM 包
在 Visual Studio Code 中打开终端，运行以下命令：
    
    ```shell
    yarn install
    ```     
### 启动并运行项目

在 Visual Studio Code 中打开终端，运行以下命令：

```shell
npm run dev
```

### 部署前端项目

在 Visual Studio Code 中打开终端，运行以下命令：

```shell
npm run build
```

命令运行成功，生成 DIST 目录，将 DIST 目录下的文件部署到服务器即可。
