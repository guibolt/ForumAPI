using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Migrations
{
    public partial class primeira : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DataCadastro = table.Column<DateTime>(nullable: false),
                    Nome = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Senha = table.Column<string>(nullable: true),
                    ConfirmaSenha = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DataCadastro = table.Column<DateTime>(nullable: false),
                    Usuario = table.Column<Guid>(nullable: true),
                    AutorID = table.Column<Guid>(nullable: false),
                    Titulo = table.Column<string>(maxLength: 250, nullable: true),
                    Texto = table.Column<string>(nullable: true),
                    Tipo = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    MediaVotos = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Usuarios_Usuario",
                        column: x => x.Usuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comentarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DataCadastro = table.Column<DateTime>(nullable: false),
                    PublicacaoId = table.Column<Guid>(nullable: false),
                    ComentarioId = table.Column<Guid>(nullable: true),
                    CitacaoId = table.Column<Guid>(nullable: true),
                    AutorId = table.Column<Guid>(nullable: false),
                    Nota = table.Column<double>(nullable: true),
                    Msg = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comentarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comentarios_Usuarios_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comentarios_Comentarios_CitacaoId",
                        column: x => x.CitacaoId,
                        principalTable: "Comentarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comentarios_Comentarios_ComentarioId",
                        column: x => x.ComentarioId,
                        principalTable: "Comentarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comentarios_Posts_PublicacaoId",
                        column: x => x.PublicacaoId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_AutorId",
                table: "Comentarios",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_CitacaoId",
                table: "Comentarios",
                column: "CitacaoId",
                unique: true,
                filter: "[CitacaoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_ComentarioId",
                table: "Comentarios",
                column: "ComentarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_PublicacaoId",
                table: "Comentarios",
                column: "PublicacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Usuario",
                table: "Posts",
                column: "Usuario");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comentarios");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
