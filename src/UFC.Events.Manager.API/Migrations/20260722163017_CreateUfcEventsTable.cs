using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UFC.Events.Manager.API.Migrations
{
    /// <inheritdoc />
    public partial class CreateUfcEventsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "events",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    number = table.Column<int>(type: "integer", nullable: true),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    city = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    country = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    arena = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    preliminary_card_start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    main_card_start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_events", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_events_name",
                table: "events",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "events");
        }
    }
}
