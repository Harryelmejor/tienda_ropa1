import { createApp } from "vue";
import { createRouter, createWebHistory } from "vue-router";
import App from "./App.vue";
import Dashboard from "./views/Dashboard.vue";
import Prendas from "./views/Prendas.vue";
import Clientes from "./views/Clientes.vue";
import Empleados from "./views/Empleados.vue";
import Ventas from "./views/Ventas.vue";

const router = createRouter({
    history: createWebHistory(),
    routes: [
        { path: "/", component: Dashboard },
        { path: "/prendas", component: Prendas },
        { path: "/clientes", component: Clientes },
        { path: "/empleados", component: Empleados },
        { path: "/ventas", component: Ventas },
    ]
});

createApp(App).use(router).mount("#app");
