using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppPharma.Migrations
{
    /// <inheritdoc />
    public partial class Nullsemployeestasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Cargos_IdCargo",
                table: "Empleados");

            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_EstadosdeEmpleados_IdEstadodeEmpleado",
                table: "Empleados");

            migrationBuilder.DropForeignKey(
                name: "FK_Tareas_EstadosdeTareas_IdEstadodeTarea",
                table: "Tareas");

            migrationBuilder.DropForeignKey(
                name: "FK_Tareas_Prioridades_IdPrioridad",
                table: "Tareas");

            migrationBuilder.AlterColumn<int>(
                name: "IdPrioridad",
                table: "Tareas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IdEstadodeTarea",
                table: "Tareas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IdEstadodeEmpleado",
                table: "Empleados",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IdCargo",
                table: "Empleados",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaNacimiento",
                table: "Empleados",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Cargos_IdCargo",
                table: "Empleados",
                column: "IdCargo",
                principalTable: "Cargos",
                principalColumn: "IdCargo");

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_EstadosdeEmpleados_IdEstadodeEmpleado",
                table: "Empleados",
                column: "IdEstadodeEmpleado",
                principalTable: "EstadosdeEmpleados",
                principalColumn: "IdEstadodeEmpleado");

            migrationBuilder.AddForeignKey(
                name: "FK_Tareas_EstadosdeTareas_IdEstadodeTarea",
                table: "Tareas",
                column: "IdEstadodeTarea",
                principalTable: "EstadosdeTareas",
                principalColumn: "IdEstadodeTarea");

            migrationBuilder.AddForeignKey(
                name: "FK_Tareas_Prioridades_IdPrioridad",
                table: "Tareas",
                column: "IdPrioridad",
                principalTable: "Prioridades",
                principalColumn: "IdPrioridad");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Cargos_IdCargo",
                table: "Empleados");

            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_EstadosdeEmpleados_IdEstadodeEmpleado",
                table: "Empleados");

            migrationBuilder.DropForeignKey(
                name: "FK_Tareas_EstadosdeTareas_IdEstadodeTarea",
                table: "Tareas");

            migrationBuilder.DropForeignKey(
                name: "FK_Tareas_Prioridades_IdPrioridad",
                table: "Tareas");

            migrationBuilder.AlterColumn<int>(
                name: "IdPrioridad",
                table: "Tareas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdEstadodeTarea",
                table: "Tareas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdEstadodeEmpleado",
                table: "Empleados",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdCargo",
                table: "Empleados",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaNacimiento",
                table: "Empleados",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Cargos_IdCargo",
                table: "Empleados",
                column: "IdCargo",
                principalTable: "Cargos",
                principalColumn: "IdCargo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_EstadosdeEmpleados_IdEstadodeEmpleado",
                table: "Empleados",
                column: "IdEstadodeEmpleado",
                principalTable: "EstadosdeEmpleados",
                principalColumn: "IdEstadodeEmpleado",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tareas_EstadosdeTareas_IdEstadodeTarea",
                table: "Tareas",
                column: "IdEstadodeTarea",
                principalTable: "EstadosdeTareas",
                principalColumn: "IdEstadodeTarea",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tareas_Prioridades_IdPrioridad",
                table: "Tareas",
                column: "IdPrioridad",
                principalTable: "Prioridades",
                principalColumn: "IdPrioridad",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
