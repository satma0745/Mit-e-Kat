namespace Mitekat.Discovery.Application.Persistence.Migrations;

using System;
using Microsoft.EntityFrameworkCore.Migrations;

public partial class FixSignedUpUsersPK : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "fk_meetups_signed_up_users",
            table: "signed_up_users");

        migrationBuilder.DropPrimaryKey(
            name: "pk_signed_up_users",
            table: "signed_up_users");

        migrationBuilder.AlterColumn<Guid>(
            name: "meetup_id",
            table: "signed_up_users",
            type: "uuid",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldNullable: true);

        migrationBuilder.AddPrimaryKey(
            name: "pk_signed_up_users",
            table: "signed_up_users",
            columns: new[] { "id", "meetup_id" });

        migrationBuilder.AddForeignKey(
            name: "fk_meetups_signed_up_users",
            table: "signed_up_users",
            column: "meetup_id",
            principalTable: "meetups",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "fk_meetups_signed_up_users",
            table: "signed_up_users");

        migrationBuilder.DropPrimaryKey(
            name: "pk_signed_up_users",
            table: "signed_up_users");

        migrationBuilder.AlterColumn<Guid>(
            name: "meetup_id",
            table: "signed_up_users",
            type: "uuid",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uuid");

        migrationBuilder.AddPrimaryKey(
            name: "pk_signed_up_users",
            table: "signed_up_users",
            column: "id");

        migrationBuilder.AddForeignKey(
            name: "fk_meetups_signed_up_users",
            table: "signed_up_users",
            column: "meetup_id",
            principalTable: "meetups",
            principalColumn: "id");
    }
}
