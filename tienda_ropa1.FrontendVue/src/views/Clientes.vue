<template>
    <div>
        <h1>Clientes</h1>
        <table v-if="clientes.length">
            <thead><tr><th>Nombre</th><th>Email</th><th>Telefono</th><th style="text-align:center">Ventas</th></tr></thead>
            <tbody>
                <tr v-for="c in clientes" :key="c.id">
                    <td><strong>{{ c.nombreCompleto }}</strong></td>
                    <td>{{ c.email ?? '—' }}</td>
                    <td>{{ c.telefono ?? '—' }}</td>
                    <td style="text-align:center">{{ c.cantidadVentas }}</td>
                </tr>
            </tbody>
        </table>
        <p v-else style="color:#888">No hay clientes.</p>
    </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
const clientes = ref<any[]>([]);
onMounted(async () => {
    const res = await fetch("http://localhost:5000/api/clientes");
    clientes.value = await res.json();
});
</script>
