using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL_DOTNETCORE.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Time = table.Column<string>(type: "varchar(50)", nullable: true),
                    SenderWalletNumber = table.Column<string>(type: "varchar(50)", nullable: true),
                    ReceiverWalletNumber = table.Column<string>(type: "varchar(50)", nullable: true),
                    Category = table.Column<string>(type: "varchar(50)", nullable: true),
                    Sum = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstName = table.Column<string>(type: "varchar(50)", nullable: true),
                    lastName = table.Column<string>(type: "varchar(50)", nullable: true),
                    phoneNumber = table.Column<string>(type: "varchar(50)", nullable: true),
                    tin = table.Column<string>(type: "varchar(50)", nullable: true),
                    passportId = table.Column<string>(type: "varchar(50)", nullable: true),
                    country = table.Column<string>(type: "varchar(50)", nullable: true),
                    city = table.Column<string>(type: "varchar(50)", nullable: true),
                    passwordHash = table.Column<string>(type: "varchar(50)", nullable: true),
                    email = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    walletName = table.Column<string>(type: "varchar(50)", nullable: true),
                    walletNumber = table.Column<string>(type: "varchar(50)", nullable: true),
                    walletBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    walletOwnerPassId = table.Column<string>(type: "varchar(50)", nullable: true),
                    walletOwnerTIN = table.Column<string>(type: "varchar(50)", nullable: true),
                    walletDate = table.Column<string>(type: "varchar(50)", nullable: true),
                    walletCVV = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Wallets");
        }
    }
}
