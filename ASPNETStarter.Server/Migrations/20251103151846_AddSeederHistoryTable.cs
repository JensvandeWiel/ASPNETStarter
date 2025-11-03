using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPNETStarter.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddSeederHistoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SeederHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeederName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LastSeededAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SeedPriority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeederHistories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SeederHistories_SeederName",
                table: "SeederHistories",
                column: "SeederName");

            migrationBuilder.CreateIndex(
                name: "IX_SeederHistories_SeedPriority",
                table: "SeederHistories",
                column: "SeedPriority");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeederHistories");
        }
    }
}
