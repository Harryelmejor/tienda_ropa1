import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

const API = "http://localhost:5000/api";

@Injectable({ providedIn: "root" })
export class ApiService {
    constructor(private http: HttpClient) {}

    getPrendas(): Observable<any[]> { return this.http.get<any[]>(`${API}/prendas`); }
    getClientes(): Observable<any[]> { return this.http.get<any[]>(`${API}/clientes`); }
    getEmpleados(): Observable<any[]> { return this.http.get<any[]>(`${API}/empleados`); }
    getVentas(): Observable<any[]> { return this.http.get<any[]>(`${API}/ventas`); }
    getCategorias(): Observable<any[]> { return this.http.get<any[]>(`${API}/categorias`); }
    getDashboard(): Observable<any> { return this.http.get<any>(`${API}/dashboard`); }
}
