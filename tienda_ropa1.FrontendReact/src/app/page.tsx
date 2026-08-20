import { api } from "@/lib/api";

export default async function Home() {
    const prendas = await api.getPrendas();
    const clientes = await api.getClientes();
    const ventas = await api.getVentas();

    return (
        <>
            <h1>Dashboard</h1>
            <div className="stats">
                <div className="stat-card">
                    <div className="stat-value">{prendas.length}</div>
                    <div className="stat-label">Prendas</div>
                </div>
                <div className="stat-card">
                    <div className="stat-value">{clientes.length}</div>
                    <div className="stat-label">Clientes</div>
                </div>
                <div className="stat-card">
                    <div className="stat-value">{ventas.length}</div>
                    <div className="stat-label">Ventas</div>
                </div>
                <div className="stat-card">
                    <div className="stat-value">{prendas.reduce((s: number, p: any) => s + p.stock, 0)}</div>
                    <div className="stat-label">En Inventario</div>
                </div>
            </div>
            <h2>Ventas recientes</h2>
            {ventas.length > 0 ? (
                <table>
                    <thead>
                        <tr><th>Factura</th><th>Cliente</th><th>Fecha</th><th style={{textAlign:"right"}}>Total</th></tr>
                    </thead>
                    <tbody>
                        {ventas.slice(0, 5).map((v: any) => (
                            <tr key={v.id}>
                                <td><strong>{v.numeroFactura}</strong></td>
                                <td>{v.clienteNombre ?? "Sin cliente"}</td>
                                <td>{new Date(v.fecha).toLocaleDateString("es-DO")}</td>
                                <td style={{textAlign:"right"}}>${v.total.toFixed(2)}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            ) : (
                <p style={{color:"#888", padding:"1rem"}}>No hay ventas registradas.</p>
            )}
        </>
    );
}
