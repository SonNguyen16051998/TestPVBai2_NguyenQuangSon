using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestPVBai2.Migrations
{
    public partial class m1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OTPs",
                columns: table => new
                {
                    UserEmail = table.Column<string>(type: "varchar(30)", nullable: false),
                    Code_OTP = table.Column<string>(type: "varchar(6)", nullable: true),
                    isUse = table.Column<bool>(type: "bit", nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OTPs", x => x.UserEmail);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserFullName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(40)", nullable: false),
                    UserPhone = table.Column<string>(type: "varchar(15)", nullable: false),
                    UserBirthday = table.Column<DateTime>(type: "date", nullable: false),
                    UserGender = table.Column<bool>(type: "bit", nullable: false),
                    UserPassword = table.Column<string>(type: "varchar(50)", nullable: false),
                    UserCreatedAt = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OTPs");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
