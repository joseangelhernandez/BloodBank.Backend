using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BloodBank.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RequireBloodRequestLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Las solicitudes viejas sin ubicación toman la del usuario que las creó;
            // si él tampoco tiene, "Unknown". Así la columna puede pasar a NOT NULL.
            migrationBuilder.Sql("""
                UPDATE r
                SET r.Location = COALESCE(NULLIF(LTRIM(RTRIM(u.Location)), ''), 'Unknown')
                FROM BloodRequests r
                JOIN Users u ON u.Id = r.RequesterId
                WHERE r.Location IS NULL OR LTRIM(RTRIM(r.Location)) = '';
                """);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "BloodRequests",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "BloodRequests",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);
        }
    }
}
