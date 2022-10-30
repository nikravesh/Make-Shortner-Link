using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Interview.DAL.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UriShorteners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrginalUri = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ShortenerUri = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UsedUriCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UriShorteners", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UriShorteners");
        }
    }
}
