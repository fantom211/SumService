using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SumService.Migrations
{
    /// <inheritdoc />
    public partial class m2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalInterest",
                table: "Credits");

            migrationBuilder.DropColumn(
                name: "TotalRepaymentAmount",
                table: "Credits");

            migrationBuilder.RenameColumn(
                name: "TermInMonth",
                table: "Credits",
                newName: "TermInMonths");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TermInMonths",
                table: "Credits",
                newName: "TermInMonth");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalInterest",
                table: "Credits",
                type: "numeric(20,2)",
                precision: 20,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalRepaymentAmount",
                table: "Credits",
                type: "numeric(20,2)",
                precision: 20,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
