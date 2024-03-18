using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace project_kaupskra.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fasteignakaup",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FAERSLUNUMER = table.Column<int>(type: "integer", nullable: false),
                    EMNR = table.Column<int>(type: "integer", nullable: false),
                    SKJALANUMER = table.Column<string>(type: "text", nullable: true),
                    FASTNUM = table.Column<int>(type: "integer", nullable: false),
                    HEIMILISFANG = table.Column<string>(type: "text", nullable: true),
                    POSTNR = table.Column<int>(type: "integer", nullable: false),
                    HEINUM = table.Column<int>(type: "integer", nullable: false),
                    SVFN = table.Column<int>(type: "integer", nullable: false),
                    SVEITARFELAG = table.Column<string>(type: "text", nullable: true),
                    UTGDAG = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    THINGLYSTDAGS = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    KAUPVERD = table.Column<int>(type: "integer", nullable: false),
                    FASTEIGNAMAT = table.Column<int>(type: "integer", nullable: false),
                    FASTEIGNAMAT_GILDANDI = table.Column<string>(type: "text", nullable: true),
                    BRUNABOTAMAT_GILDANDI = table.Column<string>(type: "text", nullable: true),
                    BYGGAR = table.Column<string>(type: "text", nullable: true),
                    FEPILOG = table.Column<string>(type: "text", nullable: true),
                    EINFLM = table.Column<decimal>(type: "numeric", nullable: false),
                    LOD_FLM = table.Column<string>(type: "text", nullable: true),
                    LOD_FLMEIN = table.Column<string>(type: "text", nullable: true),
                    FJHERB = table.Column<string>(type: "text", nullable: true),
                    TEGUND = table.Column<string>(type: "text", nullable: true),
                    FULLBUID = table.Column<int>(type: "integer", nullable: false),
                    ONOTHAEFUR_SAMNINGUR = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fasteignakaup", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fasteignakaup");
        }
    }
}
