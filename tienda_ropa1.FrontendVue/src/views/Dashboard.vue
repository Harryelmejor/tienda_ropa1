<template>
    <div>
        <h1>Dashboard</h1>
        <div class="stats" v-if="dashboard">
            <div class="stat-card"><div class="stat-value">{{ dashboard.totalPrendas }}</div><div class="stat-label">Prendas</div></div>
            <div class="stat-card"><div class="stat-value">{{ dashboard.totalClientes }}</div><div class="stat-label">Clientes</div></div>
            <div class="stat-card"><div class="stat-value">{{ dashboard.totalVentas }}</div><div class="stat-label">Ventas</div></div>
            <div class="stat-card"><div class="stat-value">{{ dashboard.inventarioTotal }}</div><div class="stat-label">En Inventario</div></div>
        </div>
        <h2>Ventas recientes</h2>
        <table v-if="ventas.length">
            <thead><tr><th>Factura</th><th>Cliente</th><th>Fecha</th><th style="text-align:right">Total</th></tr></thead>
            <tbody>
                <tr v-for="v in ventas.slice(0,5)" :key="v.id">
                    <td><strong>{{ v.numeroFactura }}</strong></td>
                    <td>{{ v.clienteNombre ?? 'Sin cliente' }}</td>
                    <td>{{ new Date(v.fecha).toLocaleDateString('es-DO') }}</td>
                    <td style="text-align:right">${{ v.total.toFixed(2) }}</td>
                </tr>
            </tbody>
        </table>
        <p v-else style="color:#888">No hay ventas registradas.</p>
    </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";

const dashboard = ref<any>(null);
const ventas = ref<any[]>([]);

const API = "http://localhost:5000/api";

onMounted(async () => {
    const [pRes, cRes, vRes] = await Promise.all([
        fetch(`${API}/prendas`), fetch(`${API}/clientes`), fetch(`${API}/ventas`)
    ]);
    const prendas = await pRes.json();
    const clientes = await cRes.json();
    ventas.value = await vRes.json();
    dashboard.value = {
        totalPrendas: prendas.length,
        totalClientes: clientes.length,
        totalVentas: ventas.value.length,
        inventarioTotal: prendas.reduce((s: number, p: any) => s + p.stock, 0)
    };
});
</script>
