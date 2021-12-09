namespace Mitekat.Persistence.Migrations;

using System;
using Microsoft.EntityFrameworkCore.Migrations;

public partial class AddUsersAndRefreshTokens : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "users",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                username = table.Column<string>(type: "text", nullable: true),
                display_name = table.Column<string>(type: "text", nullable: true),
                password = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table => table.PrimaryKey("pk_users", x => x.id));

        migrationBuilder.CreateTable(
            name: "refresh_tokens",
            columns: table => new
            {
                token_id = table.Column<Guid>(type: "uuid", nullable: false),
                user_id = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_refresh_tokens", x => x.token_id);
                table.ForeignKey(
                    name: "fk_refresh_tokens_users",
                    column: x => x.user_id,
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "ix_refresh_tokens_user_id",
            table: "refresh_tokens",
            column: "user_id");

        migrationBuilder.CreateIndex(
            name: "uix_users_username",
            table: "users",
            column: "username",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable("refresh_tokens");
        migrationBuilder.DropTable("users");
    }
}
