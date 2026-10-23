using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UverTeaServerApp.Migrations
{
    /// <inheritdoc />
    public partial class AddGreenLeafProcessingRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "containertype",
                schema: "uvateafactory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_containertype", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "leafbatchstatus",
                schema: "uvateafactory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_leafbatchstatus", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "leafcondition",
                schema: "uvateafactory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_leafcondition", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "weathercondition",
                schema: "uvateafactory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weathercondition", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "greenleafbatch",
                schema: "uvateafactory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    batchnumber = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    area_id = table.Column<int>(type: "int", nullable: false),
                    containertype_id = table.Column<int>(type: "int", nullable: false),
                    weathercondition_id = table.Column<int>(type: "int", nullable: false),
                    leafcondition_id = table.Column<int>(type: "int", nullable: false),
                    leafbatch_status_id = table.Column<int>(type: "int", nullable: false),
                    pluckingdatetime = table.Column<DateTime>(type: "datetime", nullable: false),
                    collectiondatetime = table.Column<DateTime>(type: "datetime", nullable: false),
                    arrivaldatetime = table.Column<DateTime>(type: "datetime", nullable: false),
                    weight = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    remarks = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_greenleafbatch", x => x.id);
                    table.ForeignKey(
                        name: "FK_greenleafbatch_area_area_id",
                        column: x => x.area_id,
                        principalSchema: "uvateafactory",
                        principalTable: "area",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_greenleafbatch_containertype_containertype_id",
                        column: x => x.containertype_id,
                        principalSchema: "uvateafactory",
                        principalTable: "containertype",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_greenleafbatch_leafbatchstatus_leafbatch_status_id",
                        column: x => x.leafbatch_status_id,
                        principalSchema: "uvateafactory",
                        principalTable: "leafbatchstatus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_greenleafbatch_leafcondition_leafcondition_id",
                        column: x => x.leafcondition_id,
                        principalSchema: "uvateafactory",
                        principalTable: "leafcondition",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_greenleafbatch_weathercondition_weathercondition_id",
                        column: x => x.weathercondition_id,
                        principalSchema: "uvateafactory",
                        principalTable: "weathercondition",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "greenleafprocessingrecord",
                schema: "uvateafactory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GreenLeafBatchId = table.Column<int>(type: "int", nullable: false),
                    ProcessedWeight = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    ProcessingNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_greenleafprocessingrecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_greenleafprocessingrecord_greenleafbatch_GreenLeafBatchId",
                        column: x => x.GreenLeafBatchId,
                        principalSchema: "uvateafactory",
                        principalTable: "greenleafbatch",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "greenleafriskassessment",
                schema: "uvateafactory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    greenleafbatch_id = table.Column<int>(type: "int", nullable: false),
                    riskscore = table.Column<int>(type: "int", nullable: false),
                    risklevel = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    riskreason = table.Column<string>(type: "text", nullable: true),
                    assessed_at = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_greenleafriskassessment", x => x.id);
                    table.ForeignKey(
                        name: "FK_greenleafriskassessment_greenleafbatch_greenleafbatch_id",
                        column: x => x.greenleafbatch_id,
                        principalSchema: "uvateafactory",
                        principalTable: "greenleafbatch",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_greenleafbatch_area_id",
                schema: "uvateafactory",
                table: "greenleafbatch",
                column: "area_id");

            migrationBuilder.CreateIndex(
                name: "IX_greenleafbatch_batchnumber",
                schema: "uvateafactory",
                table: "greenleafbatch",
                column: "batchnumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_greenleafbatch_containertype_id",
                schema: "uvateafactory",
                table: "greenleafbatch",
                column: "containertype_id");

            migrationBuilder.CreateIndex(
                name: "IX_greenleafbatch_leafbatch_status_id",
                schema: "uvateafactory",
                table: "greenleafbatch",
                column: "leafbatch_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_greenleafbatch_leafcondition_id",
                schema: "uvateafactory",
                table: "greenleafbatch",
                column: "leafcondition_id");

            migrationBuilder.CreateIndex(
                name: "IX_greenleafbatch_weathercondition_id",
                schema: "uvateafactory",
                table: "greenleafbatch",
                column: "weathercondition_id");

            migrationBuilder.CreateIndex(
                name: "IX_greenleafprocessingrecord_GreenLeafBatchId",
                schema: "uvateafactory",
                table: "greenleafprocessingrecord",
                column: "GreenLeafBatchId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_greenleafriskassessment_greenleafbatch_id",
                schema: "uvateafactory",
                table: "greenleafriskassessment",
                column: "greenleafbatch_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "greenleafprocessingrecord",
                schema: "uvateafactory");

            migrationBuilder.DropTable(
                name: "greenleafriskassessment",
                schema: "uvateafactory");

            migrationBuilder.DropTable(
                name: "greenleafbatch",
                schema: "uvateafactory");

            migrationBuilder.DropTable(
                name: "containertype",
                schema: "uvateafactory");

            migrationBuilder.DropTable(
                name: "leafbatchstatus",
                schema: "uvateafactory");

            migrationBuilder.DropTable(
                name: "leafcondition",
                schema: "uvateafactory");

            migrationBuilder.DropTable(
                name: "weathercondition",
                schema: "uvateafactory");
        }
    }
}
