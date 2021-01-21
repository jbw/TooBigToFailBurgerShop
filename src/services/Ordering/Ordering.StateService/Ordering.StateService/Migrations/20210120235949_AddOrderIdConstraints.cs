using Microsoft.EntityFrameworkCore.Migrations;

namespace Ordering.StateService.Migrations
{
    public partial class AddOrderIdConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BurgerOrderStateInstance_BurgerOrderId",
                table: "BurgerOrderStateInstance",
                column: "BurgerOrderId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BurgerOrderStateInstance_BurgerOrderId",
                table: "BurgerOrderStateInstance");
        }
    }
}
