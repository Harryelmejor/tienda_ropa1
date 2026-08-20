<template>
    <div>
        <h1>Prendas</h1>
        <table v-if="prendas.length">
            <thead><tr><th>Nombre</th><th>Categoria</th><th>Talla</th><th>Color</th><th style="text-align:right">Precio</th><th style="text-align:center">Stock</th></tr></thead>
            <tbody>
                <tr v-for="p in prendas" :key="p.id">
                    <td><strong>{{ p.nombre }}</strong></td>
                    <td>{{ p.categoriaNombre ?? '' }}</td>
                    <td>{{ p.talla ?? '—' }}</td>
                    <td>{{ p.color ?? '—' }}</td>
                    <td style="text-align:right">${{ p.precio.toFixed(2) }}</td>
                    <td style="text-align:center">
                        <span class="badge" :class="p.stock <= 5 ? 'badge-danger' : p.stock <= 10 ? 'badge-warning' : 'badge-success'">{{ p.stock }}</span>
                    </td>
                </tr>
            </tbody>
        </table>
        <p v-else style="color:#888">No hay prendas.</p>
    </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
const prendas = ref<any[]>([]);
onMounted(async () => {
    const res = await fetch("http://localhost:5000/api/prendas");
    prendas.value = await res.json();
});
</script>
