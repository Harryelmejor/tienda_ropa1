<template>
    <div>
        <h1>Ventas</h1>
        <table v-if="ventas.length">
            <thead><tr><th>Factura</th><th>Cliente</th><th>Empleado</th><th>Fecha</th><th style="text-align:right">Total</th></tr></thead>
            <tbody>
                <tr v-for="v in ventas" :key="v.id">
                    <td><strong>{{ v.numeroFactura }}</strong></td>
                    <td>{{ v.clienteNombre ?? 'Sin cliente' }}</td>
                    <td>{{ v.empleadoNombre }}</td>
                    <td>{{ new Date(v.fecha).toLocaleDateString('es-DO') }}</td>
                    <td style="text-align:right;color:#070;font-weight:600">${{ v.total.toFixed(2) }}</td>
                </tr>
            </tbody>
        </table>
        <p v-else style="color:#888">No hay ventas.</p>
    </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
const ventas = ref<any[]>([]);
onMounted(async () => {
    const res = await fetch("http://localhost:5000/api/ventas");
    ventas.value = await res.json();
});
</script>
