using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UverTeaServerApp.Migrations
{
    /// <inheritdoc />
    public partial class CleanBaseline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "uvateafactory");

            migrationBuilder.CreateTable(
                name: "areacategory",
                schema: "uvateafactory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_areacategory", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "areastatus",
                schema: "uvateafactory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_areastatus", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "designation",
                schema: "uvateafactory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_designation", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "employeestatus",
                schema: "uvateafactory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employeestatus", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "gender",
                schema: "uvateafactory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gender", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "growthstage",
                schema: "uvateafactory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    displayorder = table.Column<int>(type: "int", nullable: false),
                    isactive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_growthstage", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Module",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Module", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "plantingconfiguration",
                schema: "uvateafactory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    rowspacingfeet = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    plantspacingfeet = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    isactive = table.Column<bool>(type: "bit", nullable: false),
                    createdat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_plantingconfiguration", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                schema: "uvateafactory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "userstatus",
                schema: "uvateafactory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userstatus", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "employee",
                schema: "uvateafactory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isdeleted = table.Column<bool>(type: "bit", nullable: false),
                    number = table.Column<string>(type: "nchar(4)", fixedLength: true, maxLength: 4, nullable: true),
                    fullname = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    callingname = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    gender_id = table.Column<int>(type: "int", nullable: false),
                    dobirth = table.Column<DateOnly>(type: "date", nullable: true),
                    nic = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    mobile = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    land = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    doassignment = table.Column<DateOnly>(type: "date", nullable: true),
                    designation_id = table.Column<int>(type: "int", nullable: false),
                    employeestatus_id = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    Createdat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updatedat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee", x => x.id);
                    table.ForeignKey(
                        name: "FK_employee_designation_designation_id",
                        column: x => x.designation_id,
                        principalSchema: "uvateafactory",
                        principalTable: "designation",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_employee_employeestatus_employeestatus_id",
                        column: x => x.employeestatus_id,
                        principalSchema: "uvateafactory",
                        principalTable: "employeestatus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_employee_gender_gender_id",
                        column: x => x.gender_id,
                        principalSchema: "uvateafactory",
                        principalTable: "gender",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Operation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModuleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operation_Module_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Module",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "user",
                schema: "uvateafactory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isdeleted = table.Column<bool>(type: "bit", nullable: false),
                    username = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: true),
                    password = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    docreated = table.Column<DateOnly>(type: "date", nullable: true),
                    tocreated = table.Column<TimeOnly>(type: "time", nullable: true),
                    userstatus_id = table.Column<int>(type: "int", nullable: false),
                    employee_id = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    role_id = table.Column<int>(type: "int", nullable: false),
                    createdat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_employee1",
                        column: x => x.employee_id,
                        principalSchema: "uvateafactory",
                        principalTable: "employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_user_role1",
                        column: x => x.role_id,
                        principalSchema: "uvateafactory",
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_user_userstatus1",
                        column: x => x.userstatus_id,
                        principalSchema: "uvateafactory",
                        principalTable: "userstatus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Privilage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    OperationId = table.Column<int>(type: "int", nullable: false),
                    Authority = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Privilage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Privilage_Module_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Privilage_Operation_OperationId",
                        column: x => x.OperationId,
                        principalTable: "Operation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Privilage_role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "uvateafactory",
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "area",
                schema: "uvateafactory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: true),
                    acres = table.Column<decimal>(type: "decimal(7,2)", nullable: true),
                    doattached = table.Column<DateOnly>(type: "date", nullable: true),
                    plantcount = table.Column<int>(type: "int", nullable: true),
                    doproofing = table.Column<DateOnly>(type: "date", nullable: true),
                    supervisor_id = table.Column<int>(type: "int", nullable: true),
                    areastatus_id = table.Column<int>(type: "int", nullable: false),
                    areacategory_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    growthstage_id = table.Column<int>(type: "int", nullable: true),
                    plantingconfiguration_id = table.Column<int>(type: "int", nullable: true),
                    rowversion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_area", x => x.id);
                    table.ForeignKey(
                        name: "FK_area_areacategory_areacategory_id",
                        column: x => x.areacategory_id,
                        principalSchema: "uvateafactory",
                        principalTable: "areacategory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_area_areastatus_areastatus_id",
                        column: x => x.areastatus_id,
                        principalSchema: "uvateafactory",
                        principalTable: "areastatus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_area_employee_supervisor_id",
                        column: x => x.supervisor_id,
                        principalSchema: "uvateafactory",
                        principalTable: "employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_area_growthstage_growthstage_id",
                        column: x => x.growthstage_id,
                        principalSchema: "uvateafactory",
                        principalTable: "growthstage",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_area_plantingconfiguration_plantingconfiguration_id",
                        column: x => x.plantingconfiguration_id,
                        principalSchema: "uvateafactory",
                        principalTable: "plantingconfiguration",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_area_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "uvateafactory",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "areagrowthstagehistory",
                schema: "uvateafactory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    area_id = table.Column<int>(type: "int", nullable: false),
                    from_growthstage_id = table.Column<int>(type: "int", nullable: false),
                    to_growthstage_id = table.Column<int>(type: "int", nullable: false),
                    changedat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    changedby = table.Column<int>(type: "int", nullable: false),
                    reason = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_areagrowthstagehistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_areagrowthstagehistory_area_area_id",
                        column: x => x.area_id,
                        principalSchema: "uvateafactory",
                        principalTable: "area",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_areagrowthstagehistory_growthstage_from_growthstage_id",
                        column: x => x.from_growthstage_id,
                        principalSchema: "uvateafactory",
                        principalTable: "growthstage",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_areagrowthstagehistory_growthstage_to_growthstage_id",
                        column: x => x.to_growthstage_id,
                        principalSchema: "uvateafactory",
                        principalTable: "growthstage",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_areagrowthstagehistory_user_changedby",
                        column: x => x.changedby,
                        principalSchema: "uvateafactory",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "areastatushistory",
                schema: "uvateafactory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    area_id = table.Column<int>(type: "int", nullable: false),
                    from_status_id = table.Column<int>(type: "int", nullable: false),
                    to_status_id = table.Column<int>(type: "int", nullable: false),
                    changedat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    changedby = table.Column<int>(type: "int", nullable: false),
                    reason = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    AreastatusId = table.Column<int>(type: "int", nullable: true),
                    AreastatusId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_areastatushistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_areastatushistory_area_area_id",
                        column: x => x.area_id,
                        principalSchema: "uvateafactory",
                        principalTable: "area",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_areastatushistory_areastatus_AreastatusId",
                        column: x => x.AreastatusId,
                        principalSchema: "uvateafactory",
                        principalTable: "areastatus",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_areastatushistory_areastatus_AreastatusId1",
                        column: x => x.AreastatusId1,
                        principalSchema: "uvateafactory",
                        principalTable: "areastatus",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_areastatushistory_areastatus_from_status_id",
                        column: x => x.from_status_id,
                        principalSchema: "uvateafactory",
                        principalTable: "areastatus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_areastatushistory_areastatus_to_status_id",
                        column: x => x.to_status_id,
                        principalSchema: "uvateafactory",
                        principalTable: "areastatus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_areastatushistory_user_changedby",
                        column: x => x.changedby,
                        principalSchema: "uvateafactory",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_area_areacategory_id",
                schema: "uvateafactory",
                table: "area",
                column: "areacategory_id");

            migrationBuilder.CreateIndex(
                name: "IX_area_areastatus_id",
                schema: "uvateafactory",
                table: "area",
                column: "areastatus_id");

            migrationBuilder.CreateIndex(
                name: "IX_area_growthstage_id",
                schema: "uvateafactory",
                table: "area",
                column: "growthstage_id");

            migrationBuilder.CreateIndex(
                name: "IX_area_plantingconfiguration_id",
                schema: "uvateafactory",
                table: "area",
                column: "plantingconfiguration_id");

            migrationBuilder.CreateIndex(
                name: "IX_area_supervisor_id",
                schema: "uvateafactory",
                table: "area",
                column: "supervisor_id");

            migrationBuilder.CreateIndex(
                name: "IX_area_user_id",
                schema: "uvateafactory",
                table: "area",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_areagrowthstagehistory_area_id",
                schema: "uvateafactory",
                table: "areagrowthstagehistory",
                column: "area_id");

            migrationBuilder.CreateIndex(
                name: "IX_areagrowthstagehistory_changedby",
                schema: "uvateafactory",
                table: "areagrowthstagehistory",
                column: "changedby");

            migrationBuilder.CreateIndex(
                name: "IX_areagrowthstagehistory_from_growthstage_id",
                schema: "uvateafactory",
                table: "areagrowthstagehistory",
                column: "from_growthstage_id");

            migrationBuilder.CreateIndex(
                name: "IX_areagrowthstagehistory_to_growthstage_id",
                schema: "uvateafactory",
                table: "areagrowthstagehistory",
                column: "to_growthstage_id");

            migrationBuilder.CreateIndex(
                name: "IX_areastatushistory_area_id",
                schema: "uvateafactory",
                table: "areastatushistory",
                column: "area_id");

            migrationBuilder.CreateIndex(
                name: "IX_areastatushistory_AreastatusId",
                schema: "uvateafactory",
                table: "areastatushistory",
                column: "AreastatusId");

            migrationBuilder.CreateIndex(
                name: "IX_areastatushistory_AreastatusId1",
                schema: "uvateafactory",
                table: "areastatushistory",
                column: "AreastatusId1");

            migrationBuilder.CreateIndex(
                name: "IX_areastatushistory_changedby",
                schema: "uvateafactory",
                table: "areastatushistory",
                column: "changedby");

            migrationBuilder.CreateIndex(
                name: "IX_areastatushistory_from_status_id",
                schema: "uvateafactory",
                table: "areastatushistory",
                column: "from_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_areastatushistory_to_status_id",
                schema: "uvateafactory",
                table: "areastatushistory",
                column: "to_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_employee_designation_id",
                schema: "uvateafactory",
                table: "employee",
                column: "designation_id");

            migrationBuilder.CreateIndex(
                name: "IX_employee_employeestatus_id",
                schema: "uvateafactory",
                table: "employee",
                column: "employeestatus_id");

            migrationBuilder.CreateIndex(
                name: "IX_employee_gender_id",
                schema: "uvateafactory",
                table: "employee",
                column: "gender_id");

            migrationBuilder.CreateIndex(
                name: "IX_Operation_ModuleId",
                table: "Operation",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_plantingconfiguration_code",
                schema: "uvateafactory",
                table: "plantingconfiguration",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_plantingconfiguration_name",
                schema: "uvateafactory",
                table: "plantingconfiguration",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Privilage_ModuleId",
                table: "Privilage",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Privilage_OperationId",
                table: "Privilage",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_Privilage_RoleId",
                table: "Privilage",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "fk_user_employee1_idx",
                schema: "uvateafactory",
                table: "user",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "fk_user_role1_idx",
                schema: "uvateafactory",
                table: "user",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "fk_user_userstatus1_idx",
                schema: "uvateafactory",
                table: "user",
                column: "userstatus_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "areagrowthstagehistory",
                schema: "uvateafactory");

            migrationBuilder.DropTable(
                name: "areastatushistory",
                schema: "uvateafactory");

            migrationBuilder.DropTable(
                name: "Privilage");

            migrationBuilder.DropTable(
                name: "area",
                schema: "uvateafactory");

            migrationBuilder.DropTable(
                name: "Operation");

            migrationBuilder.DropTable(
                name: "areacategory",
                schema: "uvateafactory");

            migrationBuilder.DropTable(
                name: "areastatus",
                schema: "uvateafactory");

            migrationBuilder.DropTable(
                name: "growthstage",
                schema: "uvateafactory");

            migrationBuilder.DropTable(
                name: "plantingconfiguration",
                schema: "uvateafactory");

            migrationBuilder.DropTable(
                name: "user",
                schema: "uvateafactory");

            migrationBuilder.DropTable(
                name: "Module");

            migrationBuilder.DropTable(
                name: "employee",
                schema: "uvateafactory");

            migrationBuilder.DropTable(
                name: "role",
                schema: "uvateafactory");

            migrationBuilder.DropTable(
                name: "userstatus",
                schema: "uvateafactory");

            migrationBuilder.DropTable(
                name: "designation",
                schema: "uvateafactory");

            migrationBuilder.DropTable(
                name: "employeestatus",
                schema: "uvateafactory");

            migrationBuilder.DropTable(
                name: "gender",
                schema: "uvateafactory");
        }
    }
}
