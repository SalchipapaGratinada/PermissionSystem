using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PermissionSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNotificationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HierarchyNodeId",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_HierarchyNodeId",
                table: "Notifications",
                column: "HierarchyNodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_HierarchyNodes_HierarchyNodeId",
                table: "Notifications",
                column: "HierarchyNodeId",
                principalTable: "HierarchyNodes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_HierarchyNodes_HierarchyNodeId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_HierarchyNodeId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "HierarchyNodeId",
                table: "Notifications");
        }
    }
}
