// <auto-generated />
using System;
using HouseSellingBot;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HouseSellingBot.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20220128114812_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HouseSellingBot.Models.House", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("District")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("Footage")
                        .HasColumnType("real");

                    b.Property<string>("Metro")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PicturePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("Price")
                        .HasColumnType("real");

                    b.Property<string>("RentType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RoomsNumber")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WebPath")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Houses");
                });

            modelBuilder.Entity("HouseSellingBot.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("ChatId")
                        .HasColumnType("bigint");

                    b.Property<float?>("HightFootage")
                        .HasColumnType("real");

                    b.Property<float?>("HightPrice")
                        .HasColumnType("real");

                    b.Property<string>("HouseDistrict")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HouseMetro")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HouseRentType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("HouseRoomsNumbe")
                        .HasColumnType("int");

                    b.Property<string>("HouseType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("LowerFootage")
                        .HasColumnType("real");

                    b.Property<float?>("LowerPrice")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("HouseUser", b =>
                {
                    b.Property<int>("FavoriteHousesId")
                        .HasColumnType("int");

                    b.Property<int>("UsersId")
                        .HasColumnType("int");

                    b.HasKey("FavoriteHousesId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("HouseUser");
                });

            modelBuilder.Entity("HouseUser", b =>
                {
                    b.HasOne("HouseSellingBot.Models.House", null)
                        .WithMany()
                        .HasForeignKey("FavoriteHousesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HouseSellingBot.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
