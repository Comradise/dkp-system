using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dkp_system_back_front.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddUserMemberConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_InternalUser_InternalUserId",
                table: "Members");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_InternalUser_UserId",
                table: "Members");

            migrationBuilder.DropTable(
                name: "InternalUser");

            migrationBuilder.DropIndex(
                name: "IX_Members_InternalUserId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "InternalUserId",
                table: "Members");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Members",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_AspNetUsers_UserId",
                table: "Members",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_AspNetUsers_UserId",
                table: "Members");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Members",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "InternalUserId",
                table: "Members",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InternalUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalUser", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Members_InternalUserId",
                table: "Members",
                column: "InternalUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_InternalUser_InternalUserId",
                table: "Members",
                column: "InternalUserId",
                principalTable: "InternalUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_InternalUser_UserId",
                table: "Members",
                column: "UserId",
                principalTable: "InternalUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
