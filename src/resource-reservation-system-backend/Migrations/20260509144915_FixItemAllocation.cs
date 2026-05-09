using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace resource_reservation_system_backend.Migrations
{
    /// <inheritdoc />
    public partial class FixItemAllocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemAllocations_Facilities_FacilityId",
                table: "ItemAllocations");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemAllocations_Reservations_ReservationId",
                table: "ItemAllocations");

            migrationBuilder.DropIndex(
                name: "IX_ItemAllocations_ReservationId",
                table: "ItemAllocations");

            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "ItemAllocations");

            migrationBuilder.AlterColumn<int>(
                name: "FacilityId",
                table: "ItemAllocations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemAllocations_Facilities_FacilityId",
                table: "ItemAllocations",
                column: "FacilityId",
                principalTable: "Facilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemAllocations_Facilities_FacilityId",
                table: "ItemAllocations");

            migrationBuilder.AlterColumn<int>(
                name: "FacilityId",
                table: "ItemAllocations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ReservationId",
                table: "ItemAllocations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ItemAllocations_ReservationId",
                table: "ItemAllocations",
                column: "ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemAllocations_Facilities_FacilityId",
                table: "ItemAllocations",
                column: "FacilityId",
                principalTable: "Facilities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemAllocations_Reservations_ReservationId",
                table: "ItemAllocations",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
