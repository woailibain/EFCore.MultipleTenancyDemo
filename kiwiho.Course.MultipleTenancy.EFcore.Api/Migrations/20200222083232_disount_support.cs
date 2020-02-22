using Microsoft.EntityFrameworkCore.Migrations;

namespace kiwiho.Course.MultipleTenancy.EFcore.Api.Migrations
{
    public partial class disount_support : Migration
    {
        private readonly string schema;
        public disount_support(string schema)
        {
            this.schema = schema;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Discount",
                schema: "dbo." + schema,
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                schema: "dbo." + schema,
                table: "Products");
        }
    }
}
