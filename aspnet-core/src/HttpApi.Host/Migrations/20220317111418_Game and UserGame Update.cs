using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HttpApi.Host.Migrations
{
    public partial class GameandUserGameUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCorrect",
                table: "UserGames");

            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "UserGames");

            migrationBuilder.AddColumn<int>(
                name: "UserGameStatus",
                table: "UserGames",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<double>(
                name: "CorrectAnswer",
                table: "Games",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "Games",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserGameStatus",
                table: "UserGames");

            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "Games");

            migrationBuilder.AddColumn<bool>(
                name: "IsCorrect",
                table: "UserGames",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "UserGames",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "CorrectAnswer",
                table: "Games",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
