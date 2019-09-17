﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Model;

namespace Model.Migrations
{
    [DbContext(typeof(ForumContext))]
    [Migration("20190917185058_primeira")]
    partial class primeira
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Model.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AutorId");

                    b.Property<Guid?>("CitacaoId");

                    b.Property<Guid?>("ComentarioId");

                    b.Property<DateTime>("DataCadastro");

                    b.Property<string>("Msg")
                        .HasMaxLength(500);

                    b.Property<double?>("Nota");

                    b.Property<Guid>("PostId");

                    b.HasKey("Id");

                    b.HasIndex("AutorId");

                    b.ToTable("Comentarios");
                });

            modelBuilder.Entity("Model.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AutorID");

                    b.Property<DateTime>("DataCadastro");

                    b.Property<double?>("MediaVotos");

                    b.Property<string>("Status");

                    b.Property<string>("Texto");

                    b.Property<string>("Tipo");

                    b.Property<string>("Titulo")
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.HasIndex("AutorID");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Model.Usuario", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConfirmaSenha");

                    b.Property<DateTime>("DataCadastro");

                    b.Property<string>("Email");

                    b.Property<string>("Nome");

                    b.Property<string>("Senha");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("Model.Comment", b =>
                {
                    b.HasOne("Model.Usuario", "Autor")
                        .WithMany()
                        .HasForeignKey("AutorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Model.Post", b =>
                {
                    b.HasOne("Model.Usuario", "Autor")
                        .WithMany()
                        .HasForeignKey("AutorID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}