import { Component, OnInit } from "@angular/core";
import { ApiService } from "../services/api.service";

@Component({
    selector: "app-dashboard",
    standalone: true,
    template: `
        <h1>Dashboard</h1>
        <div class="stats" *ngIf="prendas">
            <div class="stat-card"><div class="stat-value">{{ prendas.length }}</div><div class="stat-label">Prendas</div></div>
            <div class="stat-card"><div class="stat-value">{{ clientes.length }}</div><div class="stat-label">Clientes</div></div>
            <div class="stat-card"><div class="stat-value">{{ ventas.length }}</div><div class="stat-label">Ventas</div></div>
            <div class="stat-card"><div class="stat-value">{{ inventarioTotal }}</div><div class="stat-label">En Inventario</div></div>
        </div>
        <h2>Ventas recientes</h2>
        <table *ngIf="ventas.length">
            <thead><tr><th>Factura</th><th>Cliente</th><th>Fecha</th><th style="text-align:right">Total</th></tr></thead>
            <tbody>
                <tr *ngFor="let v of ventas.slice(0,5)">
                    <td><strong>{{ v.numeroFactura }}</strong></td>
                    <td>{{ v.clienteNombre ?? 'Sin cliente' }}</td>
                    <td>{{ v.fecha | date:'dd/MM/yyyy' }}</td>
                    <td style="text-align:right">\${{ v.total.toFixed(2) }}</td>
                </tr>
            </tbody>
        </table>
        <p *ngIf="!ventas.length" style="color:#888">No hay ventas registradas.</p>
    `
})
export class DashboardComponent implements OnInit {
    prendas: any[] = [];
    clientes: any[] = [];
    ventas: any[] = [];
    inventarioTotal = 0;

    constructor(private api: ApiService) {}

    ngOnInit() {
        this.api.getPrendas().subscribe(p => { this.prendas = p; this.inventarioTotal = p.reduce((s: number, x: any) => s + x.stock, 0); });
        this.api.getClientes().subscribe(c => this.clientes = c);
        this.api.getVentas().subscribe(v => this.ventas = v);
    }
}
