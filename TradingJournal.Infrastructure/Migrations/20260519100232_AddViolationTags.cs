using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingJournal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddViolationTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "ViolationTags",
                table: "Trades",
                type: "text[]",
                nullable: false,
                defaultValueSql: "'{}'");

            migrationBuilder.DropColumn(name: "EntryQuality", table: "Trades");
            migrationBuilder.DropColumn(name: "ExitQuality", table: "Trades");
            migrationBuilder.DropColumn(name: "RiskManagement", table: "Trades");
            migrationBuilder.DropColumn(name: "PlanAdherence", table: "Trades");
            migrationBuilder.DropColumn(name: "TickedScore", table: "Trades");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "ViolationTags", table: "Trades");

            migrationBuilder.AddColumn<int>(name: "EntryQuality", table: "Trades", nullable: false, defaultValue: 0);
            migrationBuilder.AddColumn<int>(name: "ExitQuality", table: "Trades", nullable: false, defaultValue: 0);
            migrationBuilder.AddColumn<int>(name: "RiskManagement", table: "Trades", nullable: false, defaultValue: 0);
            migrationBuilder.AddColumn<int>(name: "PlanAdherence", table: "Trades", nullable: false, defaultValue: 0);
        }
    }
}
