using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseSellingBot.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Houses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WebPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PicturePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoomsNumber = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<float>(type: "real", nullable: true),
                    Footage = table.Column<float>(type: "real", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Houses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseDistrict = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseRoomsNumbe = table.Column<int>(type: "int", nullable: true),
                    LowerPrice = table.Column<float>(type: "real", nullable: true),
                    HightPrice = table.Column<float>(type: "real", nullable: true),
                    LowerFootage = table.Column<float>(type: "real", nullable: true),
                    HightFootage = table.Column<float>(type: "real", nullable: true),
                    HouseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HouseUser",
                columns: table => new
                {
                    FavoriteHousesId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseUser", x => new { x.FavoriteHousesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_HouseUser_Houses_FavoriteHousesId",
                        column: x => x.FavoriteHousesId,
                        principalTable: "Houses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HouseUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HouseUser_UsersId",
                table: "HouseUser",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HouseUser");

            migrationBuilder.DropTable(
                name: "Houses");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
