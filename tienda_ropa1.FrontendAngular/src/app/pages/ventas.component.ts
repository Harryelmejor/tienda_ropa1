import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { ApiService } from "../services/api.service";

@Component({
    selector: "app-ventas",
    standalone: true,
    imports: [CommonModule],
    template: `
        <h1>Ventas</h1>
        <table *ngIf="ventas.length">
            <thead><tr><th>Factura</th><th>Cliente</th><th>Empleado</th><th>Fecha</th><th style="text-align:right">Total</th></tr></thead>
            <tbody>
                <tr *ngFor="let v of ventas">
                    <td><strong>{{ v.numeroFactura }}</strong></td>
                    <td>{{ v.clienteNombre ?? 'Sin cliente' }}</td>
                    <td>{{ v.empleadoNombre }}</td>
                    <td>{{ v.fecha | date:'dd/MM/yyyy HH:mm' }}</td>
                    <td style="text-align:right;color:#070;font-weight:600">\${{ v.total.toFixed(2) }}</td>
                </tr>
            </tbody>
        </table>
        <p *ngIf="!ventas.length" style="color:#888">No hay ventas.</p>
    `
})
export class VentasComponent implements OnInit {
    ventas: any[] = [];
    constructor(private api: ApiService) {}
    ngOnInit() { this.api.getVentas().subscribe(v => this.ventas = v); }
}
