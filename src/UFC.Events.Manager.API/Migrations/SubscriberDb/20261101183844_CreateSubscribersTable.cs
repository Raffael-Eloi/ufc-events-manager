using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UFC.Events.Manager.API.Migrations.SubscriberDb
{
    /// <inheritdoc />
    public partial class CreateSubscribersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "subscribers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subscribers", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_subscribers_email",
                table: "subscribers",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "subscribers");
        }
    }
}
