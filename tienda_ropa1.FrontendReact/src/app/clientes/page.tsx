import { api } from "@/lib/api";

export default async function ClientesPage() {
    const clientes = await api.getClientes();

    return (
        <>
            <h1>Clientes</h1>
            {clientes.length > 0 ? (
                <table>
                    <thead>
                        <tr><th>Nombre</th><th>Email</th><th>Telefono</th><th style={{textAlign:"center"}}>Ventas</th></tr>
                    </thead>
                    <tbody>
                        {clientes.map((c: any) => (
                            <tr key={c.id}>
                                <td><strong>{c.nombreCompleto}</strong></td>
                                <td>{c.email ?? "—"}</td>
                                <td>{c.telefono ?? "—"}</td>
                                <td style={{textAlign:"center"}}>{c.cantidadVentas}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            ) : (
                <p style={{color:"#888"}}>No hay clientes.</p>
            )}
        </>
    );
}
