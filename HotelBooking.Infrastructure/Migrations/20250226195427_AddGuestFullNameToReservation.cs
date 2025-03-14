﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGuestFullNameToReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmergencyContactName",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "EmergencyContactPhone",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "GuestEmail",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "GuestFullName",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Guests");

            migrationBuilder.DropColumn(
                name: "DocumentNumber",
                table: "Guests");

            migrationBuilder.DropColumn(
                name: "DocumentType",
                table: "Guests");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Guests");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Guests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactName",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactPhone",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GuestEmail",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GuestFullName",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "Guests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DocumentNumber",
                table: "Guests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DocumentType",
                table: "Guests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Guests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Guests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
