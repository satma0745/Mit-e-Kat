namespace Mitekat.Discovery.Application.Persistence.Migrations;

using System;
using Microsoft.EntityFrameworkCore.Migrations;

public partial class AddSignedUpUsers : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "signed_up_users",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                meetup_id = table.Column<Guid>(type: "uuid", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_signed_up_users", x => x.id);
                table.ForeignKey(
                    name: "fk_meetups_signed_up_users",
                    column: x => x.meetup_id,
                    principalTable: "meetups",
                    principalColumn: "id");
            });

        migrationBuilder.CreateIndex(
            name: "uix_signed_up_users_meetup_id",
            table: "signed_up_users",
            column: "meetup_id",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder) =>
        migrationBuilder.DropTable("signed_up_users");
}
