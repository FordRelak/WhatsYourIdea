using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhatsYourIdea.Infrastructure.Migrations
{
    public partial class Init3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "main_image_path",
                table: "ideas",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "main_image_path",
                table: "ideas");
        }
    }
}
