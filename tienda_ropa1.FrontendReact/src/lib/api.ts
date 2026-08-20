const API_URL = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5000/api";

export interface Prenda { id: number; nombre: string; descripcion?: string; precio: number; stock: number; talla?: string; color?: string; categoriaId: number; categoriaNombre?: string; }
export interface Cliente { id: number; nombre: string; apellido: string; nombreCompleto: string; telefono?: string; email?: string; direccion?: string; cantidadVentas: number; }
export interface Empleado { id: number; nombre: string; apellido: string; nombreCompleto: string; telefono?: string; email?: string; cargo?: string; cantidadVentas: number; }
export interface Venta { id: number; numeroFactura: string; clienteNombre?: string; empleadoNombre: string; fecha: string; total: number; detalles: any[]; }
export interface Categoria { id: number; nombre: string; descripcion?: string; }

async function apiFetch<T>(path: string): Promise<T> {
    const res = await fetch(`${API_URL}${path}`, { cache: "no-store" });
    if (!res.ok) throw new Error(`API error: ${res.status}`);
    return res.json();
}

export const api = {
    getDashboard: () => apiFetch<any>("/dashboard"),
    getPrendas: (buscar?: string) => apiFetch<Prenda[]>(`/prendas${buscar ? `?buscar=${buscar}` : ""}`),
    getClientes: (buscar?: string) => apiFetch<Cliente[]>(`/clientes${buscar ? `?buscar=${buscar}` : ""}`),
    getEmpleados: (buscar?: string) => apiFetch<Empleado[]>(`/empleados${buscar ? `?buscar=${buscar}` : ""}`),
    getVentas: () => apiFetch<Venta[]>("/ventas"),
    getCategorias: () => apiFetch<Categoria[]>("/categorias"),
};
