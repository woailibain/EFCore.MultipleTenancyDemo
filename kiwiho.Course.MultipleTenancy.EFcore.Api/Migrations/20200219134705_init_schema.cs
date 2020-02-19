using Microsoft.EntityFrameworkCore.Migrations;

namespace kiwiho.Course.MultipleTenancy.EFcore.Api.Migrations
{
    public partial class init_schema : Migration
    {
        private readonly string schema;
        public init_schema(string schema)
        {
            this.schema = schema;

        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo." + schema);

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "dbo." + schema,
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Category = table.Column<string>(maxLength: 50, nullable: true),
                    Price = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products",
                schema: "dbo." + schema);
        }
    }
}
