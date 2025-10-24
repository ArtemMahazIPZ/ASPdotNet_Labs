using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsPortal.Migrations
{
    /// <inheritdoc />
    public partial class AddCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.AddColumn<long>(
                name: "CategoryId",
                table: "Articles",
                type: "bigint",
                nullable: true);

            migrationBuilder.Sql("INSERT INTO Categories (Name) SELECT DISTINCT Category FROM Articles WHERE Category IS NOT NULL AND LTRIM(RTRIM(Category)) <> '';");
            migrationBuilder.Sql("UPDATE a SET CategoryId = c.CategoryId FROM Articles a INNER JOIN Categories c ON a.Category = c.Name;");
            migrationBuilder.Sql(@"IF EXISTS (SELECT 1 FROM Articles WHERE CategoryId IS NULL)
BEGIN
    INSERT INTO Categories (Name) VALUES ('Uncategorized');
    DECLARE @newId bigint = SCOPE_IDENTITY();
    UPDATE Articles SET CategoryId = @newId WHERE CategoryId IS NULL;
END");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Articles");

            migrationBuilder.AlterColumn<long>(
                name: "CategoryId",
                table: "Articles",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CategoryId",
                table: "Articles",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Categories_CategoryId",
                table: "Articles",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Categories_CategoryId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_CategoryId",
                table: "Articles");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Articles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.Sql("UPDATE a SET Category = c.Name FROM Articles a INNER JOIN Categories c ON a.CategoryId = c.CategoryId;");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Articles");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
