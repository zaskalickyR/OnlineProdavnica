﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.DAL.Migrations.UserDb
{
    /// <inheritdoc />
    public partial class addimagetouser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Users");
        }
    }
}
