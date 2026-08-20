import { Component } from "@angular/core";
import { RouterLink, RouterLinkActive, RouterOutlet } from "@angular/router";

@Component({
    selector: "app-root",
    standalone: true,
    imports: [RouterOutlet, RouterLink, RouterLinkActive],
    template: `
        <header class="header">
            <h1 class="logo">STORE</h1>
            <nav class="nav">
                <a routerLink="/" routerLinkActive="active" [routerLinkActiveOptions]="{exact:true}">Inicio</a>
                <a routerLink="/prendas" routerLinkActive="active">Prendas</a>
                <a routerLink="/clientes" routerLinkActive="active">Clientes</a>
                <a routerLink="/empleados" routerLinkActive="active">Empleados</a>
                <a routerLink="/ventas" routerLinkActive="active">Ventas</a>
            </nav>
        </header>
        <main class="container">
            <router-outlet />
        </main>
    `
})
export class AppComponent {}
