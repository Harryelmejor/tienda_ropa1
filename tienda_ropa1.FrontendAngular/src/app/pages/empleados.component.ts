import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { ApiService } from "../services/api.service";

@Component({
    selector: "app-empleados",
    standalone: true,
    imports: [CommonModule],
    template: `
        <h1>Empleados</h1>
        <table *ngIf="empleados.length">
            <thead><tr><th>Nombre</th><th>Email</th><th>Cargo</th><th style="text-align:center">Ventas</th></tr></thead>
            <tbody>
                <tr *ngFor="let e of empleados">
                    <td><strong>{{ e.nombreCompleto }}</strong></td>
                    <td>{{ e.email ?? '—' }}</td>
                    <td>{{ e.cargo ?? '—' }}</td>
                    <td style="text-align:center">{{ e.cantidadVentas }}</td>
                </tr>
            </tbody>
        </table>
        <p *ngIf="!empleados.length" style="color:#888">No hay empleados.</p>
    `
})
export class EmpleadosComponent implements OnInit {
    empleados: any[] = [];
    constructor(private api: ApiService) {}
    ngOnInit() { this.api.getEmpleados().subscribe(e => this.empleados = e); }
}
