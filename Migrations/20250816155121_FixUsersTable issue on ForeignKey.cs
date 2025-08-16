using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FullstackDevTS.Migrations
{
    /// <inheritdoc />
    public partial class FixUsersTableissueonForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_users_credential_CredentialModelId",
                table: "users");

            migrationBuilder.DropForeignKey(
                name: "FK_users_users_information_InformationModelId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_CredentialModelId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_InformationModelId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "CredentialModelId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "InformationModelId",
                table: "users");

            migrationBuilder.CreateIndex(
                name: "IX_users_CredentialId",
                table: "users",
                column: "CredentialId");

            migrationBuilder.CreateIndex(
                name: "IX_users_InformationId",
                table: "users",
                column: "InformationId");

            migrationBuilder.AddForeignKey(
                name: "FK_users_users_credential_CredentialId",
                table: "users",
                column: "CredentialId",
                principalTable: "users_credential",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_users_users_information_InformationId",
                table: "users",
                column: "InformationId",
                principalTable: "users_information",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_users_credential_CredentialId",
                table: "users");

            migrationBuilder.DropForeignKey(
                name: "FK_users_users_information_InformationId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_CredentialId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_InformationId",
                table: "users");

            migrationBuilder.AddColumn<Guid>(
                name: "CredentialModelId",
                table: "users",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "InformationModelId",
                table: "users",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_users_CredentialModelId",
                table: "users",
                column: "CredentialModelId");

            migrationBuilder.CreateIndex(
                name: "IX_users_InformationModelId",
                table: "users",
                column: "InformationModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_users_users_credential_CredentialModelId",
                table: "users",
                column: "CredentialModelId",
                principalTable: "users_credential",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_users_users_information_InformationModelId",
                table: "users",
                column: "InformationModelId",
                principalTable: "users_information",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
