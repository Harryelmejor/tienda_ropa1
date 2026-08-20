import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { ApiService } from "../services/api.service";

@Component({
    selector: "app-clientes",
    standalone: true,
    imports: [CommonModule],
    template: `
        <h1>Clientes</h1>
        <table *ngIf="clientes.length">
            <thead><tr><th>Nombre</th><th>Email</th><th>Telefono</th><th style="text-align:center">Ventas</th></tr></thead>
            <tbody>
                <tr *ngFor="let c of clientes">
                    <td><strong>{{ c.nombreCompleto }}</strong></td>
                    <td>{{ c.email ?? '—' }}</td>
                    <td>{{ c.telefono ?? '—' }}</td>
                    <td style="text-align:center">{{ c.cantidadVentas }}</td>
                </tr>
            </tbody>
        </table>
        <p *ngIf="!clientes.length" style="color:#888">No hay clientes.</p>
    `
})
export class ClientesComponent implements OnInit {
    clientes: any[] = [];
    constructor(private api: ApiService) {}
    ngOnInit() { this.api.getClientes().subscribe(c => this.clientes = c); }
}
