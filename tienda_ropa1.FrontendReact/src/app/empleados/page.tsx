import { api } from "@/lib/api";

export default async function EmpleadosPage() {
    const empleados = await api.getEmpleados();

    return (
        <>
            <h1>Empleados</h1>
            {empleados.length > 0 ? (
                <table>
                    <thead>
                        <tr><th>Nombre</th><th>Email</th><th>Cargo</th><th style={{textAlign:"center"}}>Ventas</th></tr>
                    </thead>
                    <tbody>
                        {empleados.map((e: any) => (
                            <tr key={e.id}>
                                <td><strong>{e.nombreCompleto}</strong></td>
                                <td>{e.email ?? "—"}</td>
                                <td>{e.cargo ?? "—"}</td>
                                <td style={{textAlign:"center"}}>{e.cantidadVentas}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            ) : (
                <p style={{color:"#888"}}>No hay empleados.</p>
            )}
        </>
    );
}
