using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace apis.Migrations
{
    public partial class RecetteCreatorId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Commentaire_User_UserId", table: "Commentaire");
            migrationBuilder.DropForeignKey(name: "FK_IngredientRecette_Ingredient_IngredientId", table: "IngredientRecette");
            migrationBuilder.DropForeignKey(name: "FK_IngredientRecette_Recette_RecetteId", table: "IngredientRecette");
            migrationBuilder.DropForeignKey(name: "FK_IdentityRoleClaim<int>_IdentityRole<int>_RoleId", table: "AspNetRoleClaims");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserClaim<int>_User_UserId", table: "AspNetUserClaims");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserLogin<int>_User_UserId", table: "AspNetUserLogins");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserRole<int>_IdentityRole<int>_RoleId", table: "AspNetUserRoles");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserRole<int>_User_UserId", table: "AspNetUserRoles");
            migrationBuilder.AlterColumn<int>(
                name: "Level",
                table: "AspNetUsers",
                nullable: false);
            migrationBuilder.AlterColumn<short>(
                name: "BirthYear",
                table: "AspNetUsers",
                nullable: false);
            migrationBuilder.AddColumn<int>(
                name: "JsonId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "Recette",
                nullable: false,
                defaultValue: 0);
            migrationBuilder.AddForeignKey(
                name: "FK_Commentaire_User_UserId",
                table: "Commentaire",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_IngredientRecette_Ingredient_IngredientId",
                table: "IngredientRecette",
                column: "IngredientId",
                principalTable: "Ingredient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_IngredientRecette_Recette_RecetteId",
                table: "IngredientRecette",
                column: "RecetteId",
                principalTable: "Recette",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityRoleClaim<int>_IdentityRole<int>_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserClaim<int>_User_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserLogin<int>_User_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserRole<int>_IdentityRole<int>_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserRole<int>_User_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Commentaire_User_UserId", table: "Commentaire");
            migrationBuilder.DropForeignKey(name: "FK_IngredientRecette_Ingredient_IngredientId", table: "IngredientRecette");
            migrationBuilder.DropForeignKey(name: "FK_IngredientRecette_Recette_RecetteId", table: "IngredientRecette");
            migrationBuilder.DropForeignKey(name: "FK_IdentityRoleClaim<int>_IdentityRole<int>_RoleId", table: "AspNetRoleClaims");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserClaim<int>_User_UserId", table: "AspNetUserClaims");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserLogin<int>_User_UserId", table: "AspNetUserLogins");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserRole<int>_IdentityRole<int>_RoleId", table: "AspNetUserRoles");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserRole<int>_User_UserId", table: "AspNetUserRoles");
            migrationBuilder.DropColumn(name: "JsonId", table: "AspNetUsers");
            migrationBuilder.DropColumn(name: "CreatorId", table: "Recette");
            migrationBuilder.AlterColumn<ushort>(
                name: "Level",
                table: "AspNetUsers",
                nullable: false);
            migrationBuilder.AlterColumn<ushort>(
                name: "BirthYear",
                table: "AspNetUsers",
                nullable: false);
            migrationBuilder.AddForeignKey(
                name: "FK_Commentaire_User_UserId",
                table: "Commentaire",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_IngredientRecette_Ingredient_IngredientId",
                table: "IngredientRecette",
                column: "IngredientId",
                principalTable: "Ingredient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_IngredientRecette_Recette_RecetteId",
                table: "IngredientRecette",
                column: "RecetteId",
                principalTable: "Recette",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityRoleClaim<int>_IdentityRole<int>_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserClaim<int>_User_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserLogin<int>_User_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserRole<int>_IdentityRole<int>_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserRole<int>_User_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
