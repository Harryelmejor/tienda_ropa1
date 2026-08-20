import type { Metadata } from "next";
import "./globals.css";

export const metadata: Metadata = {
    title: "Tienda Ropa - React + Next.js",
    description: "Sistema de Control de Ventas",
};

export default function RootLayout({ children }: { children: React.ReactNode }) {
    return (
        <html lang="es">
        <body>
            <header className="header">
                <h1 className="logo">STORE</h1>
                <nav className="nav">
                    <a href="/">Inicio</a>
                    <a href="/prendas">Prendas</a>
                    <a href="/clientes">Clientes</a>
                    <a href="/empleados">Empleados</a>
                    <a href="/ventas">Ventas</a>
                </nav>
            </header>
            <main className="container">{children}</main>
        </body>
        </html>
    );
}
