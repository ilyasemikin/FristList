using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FristList.Service.PublicApi.Migrations
{
    public partial class AddCurrentActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityCategory_Activities_ActivityId",
                table: "ActivityCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityCategory_Categories_CategoryId",
                table: "ActivityCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActivityCategory",
                table: "ActivityCategory");

            migrationBuilder.RenameTable(
                name: "ActivityCategory",
                newName: "ActivityCategories");

            migrationBuilder.RenameIndex(
                name: "IX_ActivityCategory_CategoryId",
                table: "ActivityCategories",
                newName: "IX_ActivityCategories_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActivityCategories",
                table: "ActivityCategories",
                columns: new[] { "ActivityId", "CategoryId" });

            migrationBuilder.CreateTable(
                name: "CurrentActivities",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    BeginAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentActivities", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_CurrentActivities_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CurrentActivityCategories",
                columns: table => new
                {
                    CategoriesId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrentActivitiesUserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentActivityCategories", x => new { x.CategoriesId, x.CurrentActivitiesUserId });
                    table.ForeignKey(
                        name: "FK_CurrentActivityCategories_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CurrentActivityCategories_CurrentActivities_CurrentActiviti~",
                        column: x => x.CurrentActivitiesUserId,
                        principalTable: "CurrentActivities",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrentActivityCategories_CurrentActivitiesUserId",
                table: "CurrentActivityCategories",
                column: "CurrentActivitiesUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityCategories_Activities_ActivityId",
                table: "ActivityCategories",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityCategories_Categories_CategoryId",
                table: "ActivityCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityCategories_Activities_ActivityId",
                table: "ActivityCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityCategories_Categories_CategoryId",
                table: "ActivityCategories");

            migrationBuilder.DropTable(
                name: "CurrentActivityCategories");

            migrationBuilder.DropTable(
                name: "CurrentActivities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActivityCategories",
                table: "ActivityCategories");

            migrationBuilder.RenameTable(
                name: "ActivityCategories",
                newName: "ActivityCategory");

            migrationBuilder.RenameIndex(
                name: "IX_ActivityCategories_CategoryId",
                table: "ActivityCategory",
                newName: "IX_ActivityCategory_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActivityCategory",
                table: "ActivityCategory",
                columns: new[] { "ActivityId", "CategoryId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityCategory_Activities_ActivityId",
                table: "ActivityCategory",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityCategory_Categories_CategoryId",
                table: "ActivityCategory",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
