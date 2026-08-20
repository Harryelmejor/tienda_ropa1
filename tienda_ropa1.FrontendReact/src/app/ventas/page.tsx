import { api } from "@/lib/api";

export default async function VentasPage() {
    const ventas = await api.getVentas();

    return (
        <>
            <h1>Ventas</h1>
            {ventas.length > 0 ? (
                <table>
                    <thead>
                        <tr><th>Factura</th><th>Cliente</th><th>Empleado</th><th>Fecha</th><th style={{textAlign:"right"}}>Total</th></tr>
                    </thead>
                    <tbody>
                        {ventas.map((v: any) => (
                            <tr key={v.id}>
                                <td><strong>{v.numeroFactura}</strong></td>
                                <td>{v.clienteNombre ?? "Sin cliente"}</td>
                                <td>{v.empleadoNombre}</td>
                                <td>{new Date(v.fecha).toLocaleDateString("es-DO")}</td>
                                <td style={{textAlign:"right",color:"#070",fontWeight:600}}>${v.total.toFixed(2)}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            ) : (
                <p style={{color:"#888"}}>No hay ventas.</p>
            )}
        </>
    );
}
