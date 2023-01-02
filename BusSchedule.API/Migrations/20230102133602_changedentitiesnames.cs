using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusSchedule.API.Migrations
{
    /// <inheritdoc />
    public partial class changedentitiesnames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeTableId",
                table: "TimeTables",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "StopId",
                table: "Stops",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "StopOrderId",
                table: "StopOrders",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "RouteId",
                table: "Routes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "BusId",
                table: "Buses",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "TimeTables",
                newName: "TimeTableId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Stops",
                newName: "StopId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "StopOrders",
                newName: "StopOrderId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Routes",
                newName: "RouteId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Buses",
                newName: "BusId");
        }
    }
}
