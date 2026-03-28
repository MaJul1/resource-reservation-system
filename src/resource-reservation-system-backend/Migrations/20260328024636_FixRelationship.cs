using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace resource_reservation_system_backend.Migrations
{
    /// <inheritdoc />
    public partial class FixRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resources_Reservations_ReservationId",
                table: "Resources");

            migrationBuilder.DropIndex(
                name: "IX_Resources_ReservationId",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "Resources");

            migrationBuilder.AddColumn<int>(
                name: "ResourceId",
                table: "Reservations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ResourceId",
                table: "Reservations",
                column: "ResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Resources_ResourceId",
                table: "Reservations",
                column: "ResourceId",
                principalTable: "Resources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Resources_ResourceId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_ResourceId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ResourceId",
                table: "Reservations");

            migrationBuilder.AddColumn<int>(
                name: "ReservationId",
                table: "Resources",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Resources_ReservationId",
                table: "Resources",
                column: "ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_Reservations_ReservationId",
                table: "Resources",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
