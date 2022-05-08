using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhatsYourIdea.Infrastructure.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "avatar_file_path",
                table: "userprofiles",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "avatar_file_path",
                table: "userprofiles");
        }
    }
}
