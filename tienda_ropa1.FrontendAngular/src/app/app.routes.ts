import { Routes } from "@angular/router";
import { DashboardComponent } from "./pages/dashboard.component";
import { PrendasComponent } from "./pages/prendas.component";
import { ClientesComponent } from "./pages/clientes.component";
import { EmpleadosComponent } from "./pages/empleados.component";
import { VentasComponent } from "./pages/ventas.component";

export const routes: Routes = [
    { path: "", component: DashboardComponent },
    { path: "prendas", component: PrendasComponent },
    { path: "clientes", component: ClientesComponent },
    { path: "empleados", component: EmpleadosComponent },
    { path: "ventas", component: VentasComponent },
];
