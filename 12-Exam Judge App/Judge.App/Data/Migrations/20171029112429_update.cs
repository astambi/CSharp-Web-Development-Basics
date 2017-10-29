namespace Judge.App.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contests_Users_OwnerId",
                table: "Contests");

            migrationBuilder.DropIndex(
                name: "IX_Contests_OwnerId",
                table: "Contests");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Contests");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Contests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Contests_UserId",
                table: "Contests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contests_Users_UserId",
                table: "Contests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contests_Users_UserId",
                table: "Contests");

            migrationBuilder.DropIndex(
                name: "IX_Contests_UserId",
                table: "Contests");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Contests");

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Contests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Contests_OwnerId",
                table: "Contests",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contests_Users_OwnerId",
                table: "Contests",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
