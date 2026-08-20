<template>
    <div>
        <h1>Empleados</h1>
        <table v-if="empleados.length">
            <thead><tr><th>Nombre</th><th>Email</th><th>Cargo</th><th style="text-align:center">Ventas</th></tr></thead>
            <tbody>
                <tr v-for="e in empleados" :key="e.id">
                    <td><strong>{{ e.nombreCompleto }}</strong></td>
                    <td>{{ e.email ?? '—' }}</td>
                    <td>{{ e.cargo ?? '—' }}</td>
                    <td style="text-align:center">{{ e.cantidadVentas }}</td>
                </tr>
            </tbody>
        </table>
        <p v-else style="color:#888">No hay empleados.</p>
    </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
const empleados = ref<any[]>([]);
onMounted(async () => {
    const res = await fetch("http://localhost:5000/api/empleados");
    empleados.value = await res.json();
});
</script>
