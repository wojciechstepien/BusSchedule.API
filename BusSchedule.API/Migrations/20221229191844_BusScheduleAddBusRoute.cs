using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusSchedule.API.Migrations
{
    /// <inheritdoc />
    public partial class BusScheduleAddBusRoute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stops_Buses_BusId",
                table: "Stops");

            migrationBuilder.DropIndex(
                name: "IX_Stops_BusId",
                table: "Stops");

            migrationBuilder.DropColumn(
                name: "BusId",
                table: "Stops");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BusId",
                table: "Stops",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stops_BusId",
                table: "Stops",
                column: "BusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stops_Buses_BusId",
                table: "Stops",
                column: "BusId",
                principalTable: "Buses",
                principalColumn: "Id");
        }
    }
}
