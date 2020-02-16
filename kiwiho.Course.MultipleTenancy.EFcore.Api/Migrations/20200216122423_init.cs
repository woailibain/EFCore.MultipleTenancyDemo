using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace kiwiho.Course.MultipleTenancy.EFcore.Api.Migrations
{
    public partial class init : Migration
    {
        private readonly string prefix;
        public init(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                throw new System.ArgumentNullException($"{nameof(prefix)} argument is null");
            }
            this.prefix = prefix;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: prefix + "_Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Category = table.Column<string>(maxLength: 50, nullable: true),
                    Price = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Products", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: prefix + "_Products");
        }
    }
}
