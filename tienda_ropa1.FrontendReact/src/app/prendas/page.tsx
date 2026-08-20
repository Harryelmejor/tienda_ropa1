import { api } from "@/lib/api";

export default async function PrendasPage() {
    const prendas = await api.getPrendas();

    return (
        <>
            <h1>Prendas</h1>
            {prendas.length > 0 ? (
                <table>
                    <thead>
                        <tr><th>Nombre</th><th>Categoria</th><th>Talla</th><th>Color</th><th style={{textAlign:"right"}}>Precio</th><th style={{textAlign:"center"}}>Stock</th></tr>
                    </thead>
                    <tbody>
                        {prendas.map((p: any) => (
                            <tr key={p.id}>
                                <td><strong>{p.nombre}</strong></td>
                                <td>{p.categoriaNombre ?? ""}</td>
                                <td>{p.talla ?? "—"}</td>
                                <td>{p.color ?? "—"}</td>
                                <td style={{textAlign:"right"}}>${p.precio.toFixed(2)}</td>
                                <td style={{textAlign:"center"}}>
                                    <span className={`badge ${p.stock <= 5 ? "badge-danger" : p.stock <= 10 ? "badge-warning" : "badge-success"}`}>
                                        {p.stock}
                                    </span>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            ) : (
                <p style={{color:"#888"}}>No hay prendas.</p>
            )}
        </>
    );
}
