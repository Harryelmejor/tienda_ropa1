const API_URL = "http://localhost:5000/api";

interface Prenda { id: number; nombre: string; descripcion?: string; precio: number; stock: number; talla?: string; color?: string; categoriaId: number; categoriaNombre?: string; }
interface Cliente { id: number; nombre: string; apellido: string; nombreCompleto: string; telefono?: string; email?: string; direccion?: string; cantidadVentas: number; }
interface Empleado { id: number; nombre: string; apellido: string; nombreCompleto: string; telefono?: string; email?: string; cargo?: string; cantidadVentas: number; }
interface Venta { id: number; numeroFactura: string; clienteNombre?: string; empleadoNombre: string; fecha: string; total: number; detalles: any[]; }
interface Categoria { id: number; nombre: string; descripcion?: string; }
interface Dashboard { totalPrendas: number; totalClientes: number; totalEmpleados: number; totalVentas: number; inventarioTotal: number; ventasMes: number; ventasRecientes: Venta[]; stockBajo: Prenda[]; }

async function api<T>(path: string): Promise<T> {
    const res = await fetch(`${API_URL}${path}`);
    if (!res.ok) throw new Error(`API error: ${res.status}`);
    return res.json();
}

function showToast(message: string): void {
    const toast = document.createElement("div");
    toast.className = "toast";
    toast.textContent = message;
    document.body.appendChild(toast);
    setTimeout(() => toast.remove(), 3000);
}

function badgeClass(stock: number): string {
    if (stock <= 5) return "badge-danger";
    if (stock <= 10) return "badge-warning";
    return "badge-success";
}

async function loadSection(section: string): Promise<void> {
    document.querySelectorAll(".nav a").forEach(a => a.classList.remove("active"));
    document.querySelector(`.nav a[href="#${section}"]`)?.classList.add("active");
    const app = document.getElementById("app")!;

    switch (section) {
        case "dashboard": await loadDashboard(app); break;
        case "prendas": await loadPrendas(app); break;
        case "clientes": await loadClientes(app); break;
        case "empleados": await loadEmpleados(app); break;
        case "ventas": await loadVentas(app); break;
    }
}

async function loadDashboard(el: HTMLElement): Promise<void> {
    const prendas = await api<Prenda[]>("/prendas");
    const clientes = await api<Cliente[]>("/clientes");
    const ventas = await api<Venta[]>("/ventas");

    el.innerHTML = `
        <div class="stats">
            <div class="stat-card"><div class="stat-value">${prendas.length}</div><div class="stat-label">Prendas</div></div>
            <div class="stat-card"><div class="stat-value">${clientes.length}</div><div class="stat-label">Clientes</div></div>
            <div class="stat-card"><div class="stat-value">${ventas.length}</div><div class="stat-label">Ventas</div></div>
            <div class="stat-card"><div class="stat-value">${prendas.reduce((s, p) => s + p.stock, 0)}</div><div class="stat-label">En Inventario</div></div>
        </div>
        <h2 style="font-family:'Oswald'; margin-bottom:1rem;">Ventas recientes</h2>
        ${ventas.length ? `<table>
            <thead><tr><th>Factura</th><th>Cliente</th><th>Fecha</th><th style="text-align:right">Total</th></tr></thead>
            <tbody>${ventas.slice(0, 5).map(v => `<tr><td><strong>${v.numeroFactura}</strong></td><td>${v.clienteNombre ?? "Sin cliente"}</td><td>${new Date(v.fecha).toLocaleDateString("es-DO")}</td><td style="text-align:right">$${v.total.toFixed(2)}</td></tr>`).join("")}</tbody>
        </table>` : '<div class="empty-state">No hay ventas registradas.</div>'}
    `;
}

async function loadPrendas(el: HTMLElement): Promise<void> {
    const prendas = await api<Prenda[]>("/prendas");
    el.innerHTML = `
        <div class="page-actions"><h1>Prendas</h1></div>
        ${prendas.length ? `<table>
            <thead><tr><th>Nombre</th><th>Categoria</th><th>Talla</th><th>Color</th><th style="text-align:right">Precio</th><th style="text-align:center">Stock</th></tr></thead>
            <tbody>${prendas.map(p => `<tr><td><strong>${p.nombre}</strong></td><td>${p.categoriaNombre ?? ""}</td><td>${p.talla ?? "—"}</td><td>${p.color ?? "—"}</td><td style="text-align:right">$${p.precio.toFixed(2)}</td><td style="text-align:center"><span class="badge ${badgeClass(p.stock)}">${p.stock}</span></td></tr>`).join("")}</tbody>
        </table>` : '<div class="empty-state">No hay prendas.</div>'}
    `;
}

async function loadClientes(el: HTMLElement): Promise<void> {
    const clientes = await api<Cliente[]>("/clientes");
    el.innerHTML = `
        <div class="page-actions"><h1>Clientes</h1></div>
        ${clientes.length ? `<table>
            <thead><tr><th>Nombre</th><th>Email</th><th>Telefono</th><th style="text-align:center">Ventas</th></tr></thead>
            <tbody>${clientes.map(c => `<tr><td><strong>${c.nombreCompleto}</strong></td><td>${c.email ?? "—"}</td><td>${c.telefono ?? "—"}</td><td style="text-align:center">${c.cantidadVentas}</td></tr>`).join("")}</tbody>
        </table>` : '<div class="empty-state">No hay clientes.</div>'}
    `;
}

async function loadEmpleados(el: HTMLElement): Promise<void> {
    const empleados = await api<Empleado[]>("/empleados");
    el.innerHTML = `
        <div class="page-actions"><h1>Empleados</h1></div>
        ${empleados.length ? `<table>
            <thead><tr><th>Nombre</th><th>Email</th><th>Cargo</th><th style="text-align:center">Ventas</th></tr></thead>
            <tbody>${empleados.map(e => `<tr><td><strong>${e.nombreCompleto}</strong></td><td>${e.email ?? "—"}</td><td>${e.cargo ?? "—"}</td><td style="text-align:center">${e.cantidadVentas}</td></tr>`).join("")}</tbody>
        </table>` : '<div class="empty-state">No hay empleados.</div>'}
    `;
}

async function loadVentas(el: HTMLElement): Promise<void> {
    const ventas = await api<Venta[]>("/ventas");
    el.innerHTML = `
        <div class="page-actions"><h1>Ventas</h1></div>
        ${ventas.length ? `<table>
            <thead><tr><th>Factura</th><th>Cliente</th><th>Empleado</th><th>Fecha</th><th style="text-align:right">Total</th></tr></thead>
            <tbody>${ventas.map(v => `<tr><td><strong>${v.numeroFactura}</strong></td><td>${v.clienteNombre ?? "Sin cliente"}</td><td>${v.empleadoNombre}</td><td>${new Date(v.fecha).toLocaleDateString("es-DO")}</td><td style="text-align:right;color:#070;font-weight:600">$${v.total.toFixed(2)}</td></tr>`).join("")}</tbody>
        </table>` : '<div class="empty-state">No hay ventas.</div>'}
    `;
}

document.addEventListener("DOMContentLoaded", () => loadSection("dashboard"));
(window as any).loadSection = loadSection;
