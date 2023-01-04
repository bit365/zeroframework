using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeroFramework.DeviceCenter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateSequence(
                name: "EntityFrameworkHiLoSequence",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "Buyers",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buyers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceGroups",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreationTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceGroups_DeviceGroups_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "dbo",
                        principalTable: "DeviceGroups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "100000, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Coordinate = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastOnlineTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreationTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Idempotency",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Idempotency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationEventLogs",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TimesSent = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationEventLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MeasurementUnits",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasurementUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MonitoringFactors",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FactorCode = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ChineseName = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    EnglishName = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    Decimals = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitoringFactors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatus",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NodeType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NetType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ProtocolType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DataFormat = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Features = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreationTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreationTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResourceGroups",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CardNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    CardType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Expiration = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    BuyerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentMethods_Buyers_BuyerId",
                        column: x => x.BuyerId,
                        principalSchema: "dbo",
                        principalTable: "Buyers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceGroupings",
                schema: "dbo",
                columns: table => new
                {
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceGroupId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceGroupings", x => new { x.DeviceId, x.DeviceGroupId });
                    table.ForeignKey(
                        name: "FK_DeviceGroupings_DeviceGroups_DeviceGroupId",
                        column: x => x.DeviceGroupId,
                        principalSchema: "dbo",
                        principalTable: "DeviceGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeviceGroupings_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalSchema: "dbo",
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectGroups",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Sorting = table.Column<int>(type: "int", nullable: true),
                    CreationTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectGroups_ProjectGroups_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "dbo",
                        principalTable: "ProjectGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjectGroups_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "dbo",
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionGrants",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OperationName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ProviderName = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ResourceGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionGrants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionGrants_ResourceGroups_ResourceGroupId",
                        column: x => x.ResourceGroupId,
                        principalSchema: "dbo",
                        principalTable: "ResourceGroups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ResourceGroupings",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResourceType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ResourceId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ResourceGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceGroupings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResourceGroupings_ResourceGroups_ResourceGroupId",
                        column: x => x.ResourceGroupId,
                        principalSchema: "dbo",
                        principalTable: "ResourceGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressStreet = table.Column<string>(name: "Address_Street", type: "nvarchar(max)", nullable: false),
                    AddressCity = table.Column<string>(name: "Address_City", type: "nvarchar(max)", nullable: false),
                    AddressState = table.Column<string>(name: "Address_State", type: "nvarchar(max)", nullable: false),
                    AddressCountry = table.Column<string>(name: "Address_Country", type: "nvarchar(max)", nullable: false),
                    AddressZipCode = table.Column<string>(name: "Address_ZipCode", type: "nvarchar(max)", nullable: false),
                    CreationTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    BuyerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentMethodId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Buyers_BuyerId",
                        column: x => x.BuyerId,
                        principalSchema: "dbo",
                        principalTable: "Buyers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_PaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalSchema: "dbo",
                        principalTable: "PaymentMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Units = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "dbo",
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Buyers_UserId",
                schema: "dbo",
                table: "Buyers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceGroupings_DeviceGroupId",
                schema: "dbo",
                table: "DeviceGroupings",
                column: "DeviceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceGroups_ParentId",
                schema: "dbo",
                table: "DeviceGroups",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_MonitoringFactors_FactorCode",
                schema: "dbo",
                table: "MonitoringFactors",
                column: "FactorCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                schema: "dbo",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BuyerId",
                schema: "dbo",
                table: "Orders",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentMethodId",
                schema: "dbo",
                table: "Orders",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_BuyerId",
                schema: "dbo",
                table: "PaymentMethods",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGrants_OperationName_ProviderName_ProviderKey_ResourceGroupId_TenantId",
                schema: "dbo",
                table: "PermissionGrants",
                columns: new[] { "OperationName", "ProviderName", "ProviderKey", "ResourceGroupId", "TenantId" },
                unique: true,
                filter: "[ResourceGroupId] IS NOT NULL AND [TenantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGrants_ResourceGroupId",
                schema: "dbo",
                table: "PermissionGrants",
                column: "ResourceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectGroups_ParentId",
                schema: "dbo",
                table: "ProjectGroups",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectGroups_ProjectId",
                schema: "dbo",
                table: "ProjectGroups",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceGroupings_ResourceGroupId",
                schema: "dbo",
                table: "ResourceGroupings",
                column: "ResourceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceGroups_Name_TenantId",
                schema: "dbo",
                table: "ResourceGroups",
                columns: new[] { "Name", "TenantId" },
                unique: true,
                filter: "[TenantId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceGroupings",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Idempotency",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "IntegrationEventLogs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MeasurementUnits",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MonitoringFactors",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "OrderItems",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "OrderStatus",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PermissionGrants",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ProjectGroups",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ResourceGroupings",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "DeviceGroups",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Devices",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Orders",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Projects",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ResourceGroups",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PaymentMethods",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Buyers",
                schema: "dbo");

            migrationBuilder.DropSequence(
                name: "EntityFrameworkHiLoSequence");
        }
    }
}
