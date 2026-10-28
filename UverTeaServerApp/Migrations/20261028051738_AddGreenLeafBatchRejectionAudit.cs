using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UverTeaServerApp.Migrations
{
    /// <inheritdoc />
    public partial class AddGreenLeafBatchRejectionAudit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "rejected_at",
                schema: "uvateafactory",
                table: "greenleafbatch",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "rejected_by_user_id",
                schema: "uvateafactory",
                table: "greenleafbatch",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rejection_reason",
                schema: "uvateafactory",
                table: "greenleafbatch",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_greenleafbatch_rejected_by_user_id",
                schema: "uvateafactory",
                table: "greenleafbatch",
                column: "rejected_by_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_greenleafbatch_user_rejected_by_user_id",
                schema: "uvateafactory",
                table: "greenleafbatch",
                column: "rejected_by_user_id",
                principalSchema: "uvateafactory",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_greenleafbatch_user_rejected_by_user_id",
                schema: "uvateafactory",
                table: "greenleafbatch");

            migrationBuilder.DropIndex(
                name: "IX_greenleafbatch_rejected_by_user_id",
                schema: "uvateafactory",
                table: "greenleafbatch");

            migrationBuilder.DropColumn(
                name: "rejected_at",
                schema: "uvateafactory",
                table: "greenleafbatch");

            migrationBuilder.DropColumn(
                name: "rejected_by_user_id",
                schema: "uvateafactory",
                table: "greenleafbatch");

            migrationBuilder.DropColumn(
                name: "rejection_reason",
                schema: "uvateafactory",
                table: "greenleafbatch");
        }
    }
}
