using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteToCoreEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "exercises",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "exercises",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "routines",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "routines",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "exercises");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "exercises");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "routines");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "routines");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "users");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "users");
        }
    }
}
