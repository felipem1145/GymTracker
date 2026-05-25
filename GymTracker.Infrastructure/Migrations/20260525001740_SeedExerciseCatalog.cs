using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GymTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedExerciseCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "exercises",
                columns: new[] { "id", "deleted_at", "is_deleted", "name", "target_muscle" },
                values: new object[,]
                {
                    { new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100001"), null, false, "Bench Press", "Chest" },
                    { new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100002"), null, false, "Incline Dumbbell Press", "Chest" },
                    { new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100003"), null, false, "Decline Bench Press", "Chest" },
                    { new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100004"), null, false, "Cable Fly", "Chest" },
                    { new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100005"), null, false, "Pull-Ups", "Back" },
                    { new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100006"), null, false, "Lat Pulldown", "Back" },
                    { new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100007"), null, false, "Barbell Row", "Back" },
                    { new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100008"), null, false, "Seated Cable Row", "Back" },
                    { new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100009"), null, false, "Face Pull", "Back" },
                    { new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100010"), null, false, "Back Squat", "Legs" },
                    { new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100011"), null, false, "Leg Press", "Legs" },
                    { new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100012"), null, false, "Romanian Deadlift", "Legs" },
                    { new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100013"), null, false, "Leg Curl", "Legs" },
                    { new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100014"), null, false, "Leg Extension", "Legs" },
                    { new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100015"), null, false, "Hip Thrust", "Legs" },
                    { new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100016"), null, false, "Calf Raises", "Legs" },
                    { new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100017"), null, false, "Overhead Press", "Shoulders" },
                    { new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100018"), null, false, "Lateral Raise", "Shoulders" },
                    { new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100019"), null, false, "Rear Delt Fly", "Shoulders" },
                    { new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100020"), null, false, "Barbell Curl", "Arms" },
                    { new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100021"), null, false, "Hammer Curl", "Arms" },
                    { new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100022"), null, false, "Tricep Pushdown", "Arms" },
                    { new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100023"), null, false, "Skull Crusher", "Arms" },
                    { new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100024"), null, false, "Plank", "Core" },
                    { new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100025"), null, false, "Hanging Leg Raise", "Core" },
                    { new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100026"), null, false, "Cable Crunch", "Core" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100001"));

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100002"));

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100003"));

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100004"));

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100005"));

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100006"));

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100007"));

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100008"));

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100009"));

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100010"));

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100011"));

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100012"));

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100013"));

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100014"));

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100015"));

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100016"));

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100017"));

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100018"));

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100019"));

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100020"));

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100021"));

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100022"));

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100023"));

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100024"));

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100025"));

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("a2d9f111-3d77-4b13-8f14-0a3e6f100026"));
        }
    }
}
