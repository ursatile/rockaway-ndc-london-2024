using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Rockaway.WebApp.Migrations {
	/// <inheritdoc />
	public partial class TicketTypesAndVenueCultures : Migration {

		private readonly Dictionary<string, string> countryCodesToCultureNames = new() {
			{ "GB", "en-GB" }, // English (Great Britain)
			{ "FR", "fr-FR" }, // French (France)
			{ "DE", "de-DE" }, // Germany (Germany)
			{ "PT", "pt-PT" }, // Portuguese (Portugal)
			{ "GR", "el-GR" }, // Greek (Greece)
			{ "NO", "nn-NO" }, // Norwegian (Norway)
			{ "SE", "sv-SE" }, // Swedish (Sweden)
			{ "DK", "dk-DK" }, // Danish (Denmark)
			{ "AU", "en-AU" }, // Australia
			{ "CH", "fr-CH" }, // Switzerland
			{ "CL", "es-CL" }, // Spanish / Chile
			{ "CA", "fr-CA" }
		};


		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder) {

			migrationBuilder.AddColumn<string>(
				name: "CultureName",
				table: "Venue",
				type: "nvarchar(max)",
				nullable: false,
				defaultValue: "");

			// HERE:
			foreach (var (countryCode, cultureName) in countryCodesToCultureNames) {
				var sql = $@"
			        UPDATE Venue
			        SET CultureName = '{cultureName}'
			        WHERE CountryCode = '{countryCode}'";
				migrationBuilder.Sql(sql);
			}

			migrationBuilder.DropColumn(
				name: "CountryCode",
				table: "Venue");


			migrationBuilder.CreateTable(
				name: "TicketType",
				columns: table => new {
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					ShowVenueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					ShowDate = table.Column<DateOnly>(type: "date", nullable: false),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Price = table.Column<decimal>(type: "money", nullable: false),
					Limit = table.Column<int>(type: "int", nullable: true)
				},
				constraints: table => {
					table.PrimaryKey("PK_TicketType", x => x.Id);
					table.ForeignKey(
						name: "FK_TicketType_Show_ShowVenueId_ShowDate",
						columns: x => new { x.ShowVenueId, x.ShowDate },
						principalTable: "Show",
						principalColumns: new[] { "VenueId", "Date" },
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.UpdateData(
				table: "AspNetUsers",
				keyColumn: "Id",
				keyValue: "rockaway-sample-admin-user",
				columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
				values: new object[] { "5a50763f-f8b1-4889-878a-719b56b44e05", "AQAAAAIAAYagAAAAEKjJYGnGJ2fA7FNj9TD4MvsQb2BADqb5Qobj2Eo4HAXns0vnOMrmkfaoQtR4dnHesA==", "5d6e7212-d082-4714-97c8-d4246e7e442b" });

			migrationBuilder.InsertData(
				table: "TicketType",
				columns: new[] { "Id", "Limit", "Name", "Price", "ShowDate", "ShowVenueId" },
				values: new object[,]
				{
					{ new Guid("cccccccc-cccc-cccc-cccc-cccccccccc10"), null, "General Admission", 350m, new DateOnly(2024, 5, 22), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb5") },
					{ new Guid("cccccccc-cccc-cccc-cccc-cccccccccc11"), null, "VIP Meet & Greet", 750m, new DateOnly(2024, 5, 22), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb5") },
					{ new Guid("cccccccc-cccc-cccc-cccc-cccccccccc12"), null, "General Admission", 300m, new DateOnly(2024, 5, 23), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb8") },
					{ new Guid("cccccccc-cccc-cccc-cccc-cccccccccc13"), null, "VIP Meet & Greet", 720m, new DateOnly(2024, 5, 23), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb8") },
					{ new Guid("cccccccc-cccc-cccc-cccc-cccccccccc14"), null, "General Admission", 25m, new DateOnly(2024, 5, 25), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb4") },
					{ new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc1"), null, "Upstairs unallocated seating", 25m, new DateOnly(2024, 5, 17), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb7") },
					{ new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc2"), null, "Downstairs standing", 25m, new DateOnly(2024, 5, 17), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb7") },
					{ new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc3"), null, "Cabaret table (4 people)", 120m, new DateOnly(2024, 5, 17), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb7") },
					{ new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc4"), null, "General Admission", 35m, new DateOnly(2024, 5, 18), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3") },
					{ new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc5"), null, "VIP Meet & Greet", 75m, new DateOnly(2024, 5, 18), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3") },
					{ new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc6"), null, "General Admission", 35m, new DateOnly(2024, 5, 19), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2") },
					{ new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc7"), null, "VIP Meet & Greet", 75m, new DateOnly(2024, 5, 19), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2") },
					{ new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc8"), null, "General Admission", 25m, new DateOnly(2024, 5, 20), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb9") },
					{ new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc9"), null, "VIP Meet & Greet", 55m, new DateOnly(2024, 5, 20), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb9") }
				});

			migrationBuilder.UpdateData(
				table: "Venue",
				keyColumn: "Id",
				keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"),
				column: "CultureName",
				value: "en-GB");

			migrationBuilder.UpdateData(
				table: "Venue",
				keyColumn: "Id",
				keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"),
				column: "CultureName",
				value: "fr-FR");

			migrationBuilder.UpdateData(
				table: "Venue",
				keyColumn: "Id",
				keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"),
				column: "CultureName",
				value: "de-DE");

			migrationBuilder.UpdateData(
				table: "Venue",
				keyColumn: "Id",
				keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb4"),
				column: "CultureName",
				value: "el-GR");

			migrationBuilder.UpdateData(
				table: "Venue",
				keyColumn: "Id",
				keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb5"),
				column: "CultureName",
				value: "nn-NO");

			migrationBuilder.UpdateData(
				table: "Venue",
				keyColumn: "Id",
				keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb6"),
				column: "CultureName",
				value: "da-DK");

			migrationBuilder.UpdateData(
				table: "Venue",
				keyColumn: "Id",
				keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb7"),
				column: "CultureName",
				value: "pt-PT");

			migrationBuilder.UpdateData(
				table: "Venue",
				keyColumn: "Id",
				keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb8"),
				column: "CultureName",
				value: "sv-SE");

			migrationBuilder.UpdateData(
				table: "Venue",
				keyColumn: "Id",
				keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb9"),
				column: "CultureName",
				value: "en-GB");

			migrationBuilder.CreateIndex(
				name: "IX_TicketType_ShowVenueId_ShowDate",
				table: "TicketType",
				columns: new[] { "ShowVenueId", "ShowDate" });
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder) {
			migrationBuilder.DropTable(
				name: "TicketType");

			migrationBuilder.DropColumn(
				name: "CultureName",
				table: "Venue");

			migrationBuilder.AddColumn<string>(
				name: "CountryCode",
				table: "Venue",
				type: "varchar(2)",
				unicode: false,
				maxLength: 2,
				nullable: false,
				defaultValue: "");

			migrationBuilder.UpdateData(
				table: "AspNetUsers",
				keyColumn: "Id",
				keyValue: "rockaway-sample-admin-user",
				columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
				values: new object[] { "4d53d9b0-33c7-4688-92e4-d5ce6a2f989c", "AQAAAAIAAYagAAAAEBYgCg8f86AANtxZXr4WEunmIFTPQpBQ44vSF1EbHSYWvMGr79evEg9TX0C3vrPPPA==", "f097ebe2-1fde-49dc-945c-064ace03286f" });

			migrationBuilder.UpdateData(
				table: "Venue",
				keyColumn: "Id",
				keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"),
				column: "CountryCode",
				value: "GB");

			migrationBuilder.UpdateData(
				table: "Venue",
				keyColumn: "Id",
				keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"),
				column: "CountryCode",
				value: "FR");

			migrationBuilder.UpdateData(
				table: "Venue",
				keyColumn: "Id",
				keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"),
				column: "CountryCode",
				value: "DE");

			migrationBuilder.UpdateData(
				table: "Venue",
				keyColumn: "Id",
				keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb4"),
				column: "CountryCode",
				value: "GR");

			migrationBuilder.UpdateData(
				table: "Venue",
				keyColumn: "Id",
				keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb5"),
				column: "CountryCode",
				value: "NO");

			migrationBuilder.UpdateData(
				table: "Venue",
				keyColumn: "Id",
				keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb6"),
				column: "CountryCode",
				value: "DK");

			migrationBuilder.UpdateData(
				table: "Venue",
				keyColumn: "Id",
				keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb7"),
				column: "CountryCode",
				value: "PT");

			migrationBuilder.UpdateData(
				table: "Venue",
				keyColumn: "Id",
				keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb8"),
				column: "CountryCode",
				value: "SE");

			migrationBuilder.UpdateData(
				table: "Venue",
				keyColumn: "Id",
				keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb9"),
				column: "CountryCode",
				value: "GB");
		}
	}
}