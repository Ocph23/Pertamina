using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.Migrations
{
    public partial class CreateIdentitySchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Karyawan",
                columns: table => new
                {
                    IdKaryawan = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Kodekaryawan = table.Column<string>(nullable: true),
                    Namakaryawan = table.Column<string>(nullable: true),
                    Alamat = table.Column<string>(nullable: true),
                    Kontak = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    Photo = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Karyawan", x => x.IdKaryawan);
                });

            migrationBuilder.CreateTable(
                name: "Level",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LevelName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Level", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pengaduan",
                columns: table => new
                {
                    IdPengaduan = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    JenisPengaduan = table.Column<string>(nullable: true),
                    IdDetail = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pengaduan", x => x.IdPengaduan);
                });

            migrationBuilder.CreateTable(
                name: "Periode",
                columns: table => new
                {
                    IdPeriode = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Mulai = table.Column<DateTime>(nullable: false),
                    Selesai = table.Column<DateTime>(nullable: false),
                    Undian = table.Column<DateTime>(nullable: false),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Periode", x => x.IdPeriode);
                });

            migrationBuilder.CreateTable(
                name: "Perusahaan",
                columns: table => new
                {
                    IdPerusahaan = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    namaperusahaan = table.Column<string>(nullable: true),
                    alamat = table.Column<string>(nullable: true),
                    direktur = table.Column<string>(nullable: true),
                    kontakdirektur = table.Column<string>(nullable: true),
                    emaildirektur = table.Column<string>(nullable: true),
                    logo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perusahaan", x => x.IdPerusahaan);
                });

            migrationBuilder.CreateTable(
                name: "Terlapor",
                columns: table => new
                {
                    IdTerlapor = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdPengadian = table.Column<int>(nullable: false),
                    IdKaryawan = table.Column<int>(nullable: false),
                    IdJenisPelanggaran = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Terlapor", x => x.IdTerlapor);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Absen",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AbsenType = table.Column<int>(nullable: false),
                    Masuk = table.Column<DateTime>(nullable: true),
                    Pulang = table.Column<DateTime>(nullable: true),
                    KaryawanIdKaryawan = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Absen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Absen_Karyawan_KaryawanIdKaryawan",
                        column: x => x.KaryawanIdKaryawan,
                        principalTable: "Karyawan",
                        principalColumn: "IdKaryawan",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JenisPelanggaran",
                columns: table => new
                {
                    idjenispelanggaran = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    jenispelanggaran = table.Column<string>(nullable: true),
                    pengurangankaryawan = table.Column<double>(nullable: false),
                    penguranganperusahaan = table.Column<double>(nullable: false),
                    penambahanpoint = table.Column<double>(nullable: false),
                    LevelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JenisPelanggaran", x => x.idjenispelanggaran);
                    table.ForeignKey(
                        name: "FK_JenisPelanggaran_Level_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Level",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pemenang",
                columns: table => new
                {
                    IdPemenang = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdPeriode = table.Column<int>(nullable: false),
                    PeriodeIdPeriode = table.Column<int>(nullable: true),
                    IdKaryawan = table.Column<int>(nullable: false),
                    KaryawanIdKaryawan = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pemenang", x => x.IdPemenang);
                    table.ForeignKey(
                        name: "FK_Pemenang_Karyawan_KaryawanIdKaryawan",
                        column: x => x.KaryawanIdKaryawan,
                        principalTable: "Karyawan",
                        principalColumn: "IdKaryawan",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pemenang_Periode_PeriodeIdPeriode",
                        column: x => x.PeriodeIdPeriode,
                        principalTable: "Periode",
                        principalColumn: "IdPeriode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PerusahaanKaryawan",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Jabatan = table.Column<string>(nullable: true),
                    MulaiKerja = table.Column<DateTime>(nullable: false),
                    SelesaiKerja = table.Column<DateTime>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    IdKaryawan = table.Column<int>(nullable: false),
                    KaryawanIdKaryawan = table.Column<int>(nullable: true),
                    IdPerusahaan = table.Column<int>(nullable: false),
                    PerusahaanIdPerusahaan = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerusahaanKaryawan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerusahaanKaryawan_Karyawan_KaryawanIdKaryawan",
                        column: x => x.KaryawanIdKaryawan,
                        principalTable: "Karyawan",
                        principalColumn: "IdKaryawan",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PerusahaanKaryawan_Perusahaan_PerusahaanIdPerusahaan",
                        column: x => x.PerusahaanIdPerusahaan,
                        principalTable: "Perusahaan",
                        principalColumn: "IdPerusahaan",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pelanggaran",
                columns: table => new
                {
                    IdPelanggaran = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NilaiKaryawan = table.Column<double>(nullable: false),
                    NilaiPerusahaan = table.Column<double>(nullable: false),
                    Tanggal = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    idjenispelanggaran = table.Column<int>(nullable: false),
                    Jenispelanggaranidjenispelanggaran = table.Column<int>(nullable: true),
                    idkaryawan = table.Column<int>(nullable: false),
                    DataKaryawanIdKaryawan = table.Column<int>(nullable: true),
                    idperusahaan = table.Column<int>(nullable: false),
                    DataPerusahaanIdPerusahaan = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pelanggaran", x => x.IdPelanggaran);
                    table.ForeignKey(
                        name: "FK_Pelanggaran_Karyawan_DataKaryawanIdKaryawan",
                        column: x => x.DataKaryawanIdKaryawan,
                        principalTable: "Karyawan",
                        principalColumn: "IdKaryawan",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pelanggaran_Perusahaan_DataPerusahaanIdPerusahaan",
                        column: x => x.DataPerusahaanIdPerusahaan,
                        principalTable: "Perusahaan",
                        principalColumn: "IdPerusahaan",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pelanggaran_JenisPelanggaran_Jenispelanggaranidjenispelangga~",
                        column: x => x.Jenispelanggaranidjenispelanggaran,
                        principalTable: "JenisPelanggaran",
                        principalColumn: "idjenispelanggaran",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BuktiPelanggaran",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdPelanggaran = table.Column<int>(nullable: false),
                    FileType = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    Thumb = table.Column<string>(nullable: true),
                    PelanggaranIdPelanggaran = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuktiPelanggaran", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuktiPelanggaran_Pelanggaran_PelanggaranIdPelanggaran",
                        column: x => x.PelanggaranIdPelanggaran,
                        principalTable: "Pelanggaran",
                        principalColumn: "IdPelanggaran",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Absen_KaryawanIdKaryawan",
                table: "Absen",
                column: "KaryawanIdKaryawan");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BuktiPelanggaran_PelanggaranIdPelanggaran",
                table: "BuktiPelanggaran",
                column: "PelanggaranIdPelanggaran");

            migrationBuilder.CreateIndex(
                name: "IX_JenisPelanggaran_LevelId",
                table: "JenisPelanggaran",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Pelanggaran_DataKaryawanIdKaryawan",
                table: "Pelanggaran",
                column: "DataKaryawanIdKaryawan");

            migrationBuilder.CreateIndex(
                name: "IX_Pelanggaran_DataPerusahaanIdPerusahaan",
                table: "Pelanggaran",
                column: "DataPerusahaanIdPerusahaan");

            migrationBuilder.CreateIndex(
                name: "IX_Pelanggaran_Jenispelanggaranidjenispelanggaran",
                table: "Pelanggaran",
                column: "Jenispelanggaranidjenispelanggaran");

            migrationBuilder.CreateIndex(
                name: "IX_Pemenang_KaryawanIdKaryawan",
                table: "Pemenang",
                column: "KaryawanIdKaryawan");

            migrationBuilder.CreateIndex(
                name: "IX_Pemenang_PeriodeIdPeriode",
                table: "Pemenang",
                column: "PeriodeIdPeriode");

            migrationBuilder.CreateIndex(
                name: "IX_PerusahaanKaryawan_KaryawanIdKaryawan",
                table: "PerusahaanKaryawan",
                column: "KaryawanIdKaryawan");

            migrationBuilder.CreateIndex(
                name: "IX_PerusahaanKaryawan_PerusahaanIdPerusahaan",
                table: "PerusahaanKaryawan",
                column: "PerusahaanIdPerusahaan");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Absen");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BuktiPelanggaran");

            migrationBuilder.DropTable(
                name: "Pemenang");

            migrationBuilder.DropTable(
                name: "Pengaduan");

            migrationBuilder.DropTable(
                name: "PerusahaanKaryawan");

            migrationBuilder.DropTable(
                name: "Terlapor");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Pelanggaran");

            migrationBuilder.DropTable(
                name: "Periode");

            migrationBuilder.DropTable(
                name: "Karyawan");

            migrationBuilder.DropTable(
                name: "Perusahaan");

            migrationBuilder.DropTable(
                name: "JenisPelanggaran");

            migrationBuilder.DropTable(
                name: "Level");
        }
    }
}
