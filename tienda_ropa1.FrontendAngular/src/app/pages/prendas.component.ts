import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { ApiService } from "../services/api.service";

@Component({
    selector: "app-prendas",
    standalone: true,
    imports: [CommonModule],
    template: `
        <h1>Prendas</h1>
        <table *ngIf="prendas.length">
            <thead><tr><th>Nombre</th><th>Categoria</th><th>Talla</th><th>Color</th><th style="text-align:right">Precio</th><th style="text-align:center">Stock</th></tr></thead>
            <tbody>
                <tr *ngFor="let p of prendas">
                    <td><strong>{{ p.nombre }}</strong></td>
                    <td>{{ p.categoriaNombre }}</td>
                    <td>{{ p.talla ?? '—' }}</td>
                    <td>{{ p.color ?? '—' }}</td>
                    <td style="text-align:right">\${{ p.precio.toFixed(2) }}</td>
                    <td style="text-align:center">
                        <span class="badge" [ngClass]="p.stock<=5?'badge-danger':p.stock<=10?'badge-warning':'badge-success'">{{ p.stock }}</span>
                    </td>
                </tr>
            </tbody>
        </table>
        <p *ngIf="!prendas.length" style="color:#888">No hay prendas.</p>
    `
})
export class PrendasComponent implements OnInit {
    prendas: any[] = [];
    constructor(private api: ApiService) {}
    ngOnInit() { this.api.getPrendas().subscribe(p => this.prendas = p); }
}
