using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebAPI.Migrations
{
    public partial class VoucherUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientCPFOwner",
                table: "Voucher",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientNameOwner",
                table: "Voucher",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientOrderId",
                table: "Voucher",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientTicketId",
                table: "Voucher",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientCPFOwner",
                table: "Voucher");

            migrationBuilder.DropColumn(
                name: "ClientNameOwner",
                table: "Voucher");

            migrationBuilder.DropColumn(
                name: "ClientOrderId",
                table: "Voucher");

            migrationBuilder.DropColumn(
                name: "ClientTicketId",
                table: "Voucher");
        }
    }
}
