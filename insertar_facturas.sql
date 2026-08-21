-- ============================================
-- Script para insertar facturas (Ventas)
-- Base de datos: TiendaRopaDB
-- ============================================

-- Ver datos existentes primero
-- SELECT ClienteID, Nombre, Apellido FROM Clientes;
-- SELECT EmpleadoID, Nombre, Apellido FROM Empleados;
-- SELECT ProductoID, Nombre, Precio, Stock FROM Productos;

-- ============================================
-- FACTURA 1: Venta con 2 prendas, con cliente
-- ============================================
INSERT INTO Ventas (ClienteID, EmpleadoID, Fecha, Total)
VALUES (1, 1, '2025-01-15 10:30:00', 0);

DECLARE @Venta1 INT = SCOPE_IDENTITY();

INSERT INTO DetalleVenta (VentaID, ProductoID, Cantidad, PrecioUnitario)
VALUES
    (@Venta1, 1, 2, (SELECT Precio FROM Productos WHERE ProductoID = 1)),
    (@Venta1, 3, 1, (SELECT Precio FROM Productos WHERE ProductoID = 3));

UPDATE Ventas
SET Total = (SELECT ISNULL(SUM(Cantidad * PrecioUnitario), 0) FROM DetalleVenta WHERE VentaID = @Venta1)
WHERE VentaID = @Venta1;

-- ============================================
-- FACTURA 2: Venta con 3 prendas, con cliente
-- ============================================
INSERT INTO Ventas (ClienteID, EmpleadoID, Fecha, Total)
VALUES (2, 2, '2025-01-20 14:15:00', 0);

DECLARE @Venta2 INT = SCOPE_IDENTITY();

INSERT INTO DetalleVenta (VentaID, ProductoID, Cantidad, PrecioUnitario)
VALUES
    (@Venta2, 2, 1, (SELECT Precio FROM Productos WHERE ProductoID = 2)),
    (@Venta2, 4, 2, (SELECT Precio FROM Productos WHERE ProductoID = 4)),
    (@Venta2, 5, 1, (SELECT Precio FROM Productos WHERE ProductoID = 5));

UPDATE Ventas
SET Total = (SELECT ISNULL(SUM(Cantidad * PrecioUnitario), 0) FROM DetalleVenta WHERE VentaID = @Venta2)
WHERE VentaID = @Venta2;

-- ============================================
-- FACTURA 3: Venta sin cliente, 1 prenda
-- ============================================
INSERT INTO Ventas (ClienteID, EmpleadoID, Fecha, Total)
VALUES (NULL, 1, '2025-02-01 09:45:00', 0);

DECLARE @Venta3 INT = SCOPE_IDENTITY();

INSERT INTO DetalleVenta (VentaID, ProductoID, Cantidad, PrecioUnitario)
VALUES
    (@Venta3, 1, 3, (SELECT Precio FROM Productos WHERE ProductoID = 1));

UPDATE Ventas
SET Total = (SELECT ISNULL(SUM(Cantidad * PrecioUnitario), 0) FROM DetalleVenta WHERE VentaID = @Venta3)
WHERE VentaID = @Venta3;

-- ============================================
-- FACTURA 4: Venta grande con 4 prendas
-- ============================================
INSERT INTO Ventas (ClienteID, EmpleadoID, Fecha, Total)
VALUES (3, 3, '2025-02-10 16:00:00', 0);

DECLARE @Venta4 INT = SCOPE_IDENTITY();

INSERT INTO DetalleVenta (VentaID, ProductoID, Cantidad, PrecioUnitario)
VALUES
    (@Venta4, 1, 1, (SELECT Precio FROM Productos WHERE ProductoID = 1)),
    (@Venta4, 2, 2, (SELECT Precio FROM Productos WHERE ProductoID = 2)),
    (@Venta4, 6, 1, (SELECT Precio FROM Productos WHERE ProductoID = 6)),
    (@Venta4, 8, 3, (SELECT Precio FROM Productos WHERE ProductoID = 8));

UPDATE Ventas
SET Total = (SELECT ISNULL(SUM(Cantidad * PrecioUnitario), 0) FROM DetalleVenta WHERE VentaID = @Venta4)
WHERE VentaID = @Venta4;

-- ============================================
-- FACTURA 5: Venta con cliente, 2 prendas
-- ============================================
INSERT INTO Ventas (ClienteID, EmpleadoID, Fecha, Total)
VALUES (1, 2, '2025-02-15 11:20:00', 0);

DECLARE @Venta5 INT = SCOPE_IDENTITY();

INSERT INTO DetalleVenta (VentaID, ProductoID, Cantidad, PrecioUnitario)
VALUES
    (@Venta5, 5, 2, (SELECT Precio FROM Productos WHERE ProductoID = 5)),
    (@Venta5, 7, 1, (SELECT Precio FROM Productos WHERE ProductoID = 7));

UPDATE Ventas
SET Total = (SELECT ISNULL(SUM(Cantidad * PrecioUnitario), 0) FROM DetalleVenta WHERE VentaID = @Venta5)
WHERE VentaID = @Venta5;

-- ============================================
-- FACTURA 6: Venta sin cliente, 2 prendas
-- ============================================
INSERT INTO Ventas (ClienteID, EmpleadoID, Fecha, Total)
VALUES (NULL, 1, '2025-03-01 13:00:00', 0);

DECLARE @Venta6 INT = SCOPE_IDENTITY();

INSERT INTO DetalleVenta (VentaID, ProductoID, Cantidad, PrecioUnitario)
VALUES
    (@Venta6, 3, 2, (SELECT Precio FROM Productos WHERE ProductoID = 3)),
    (@Venta6, 4, 1, (SELECT Precio FROM Productos WHERE ProductoID = 4));

UPDATE Ventas
SET Total = (SELECT ISNULL(SUM(Cantidad * PrecioUnitario), 0) FROM DetalleVenta WHERE VentaID = @Venta6)
WHERE VentaID = @Venta6;

-- ============================================
-- FACTURA 7: Venta con cliente, 3 prendas
-- ============================================
INSERT INTO Ventas (ClienteID, EmpleadoID, Fecha, Total)
VALUES (4, 3, '2025-03-05 17:30:00', 0);

DECLARE @Venta7 INT = SCOPE_IDENTITY();

INSERT INTO DetalleVenta (VentaID, ProductoID, Cantidad, PrecioUnitario)
VALUES
    (@Venta7, 1, 1, (SELECT Precio FROM Productos WHERE ProductoID = 1)),
    (@Venta7, 6, 2, (SELECT Precio FROM Productos WHERE ProductoID = 6)),
    (@Venta7, 9, 1, (SELECT Precio FROM Productos WHERE ProductoID = 9));

UPDATE Ventas
SET Total = (SELECT ISNULL(SUM(Cantidad * PrecioUnitario), 0) FROM DetalleVenta WHERE VentaID = @Venta7)
WHERE VentaID = @Venta7;

-- ============================================
-- FACTURA 8: Venta grande, con cliente
-- ============================================
INSERT INTO Ventas (ClienteID, EmpleadoID, Fecha, Total)
VALUES (2, 1, '2025-03-10 10:45:00', 0);

DECLARE @Venta8 INT = SCOPE_IDENTITY();

INSERT INTO DetalleVenta (VentaID, ProductoID, Cantidad, PrecioUnitario)
VALUES
    (@Venta8, 2, 3, (SELECT Precio FROM Productos WHERE ProductoID = 2)),
    (@Venta8, 5, 2, (SELECT Precio FROM Productos WHERE ProductoID = 5)),
    (@Venta8, 8, 1, (SELECT Precio FROM Productos WHERE ProductoID = 8)),
    (@Venta8, 10, 4, (SELECT Precio FROM Productos WHERE ProductoID = 10));

UPDATE Ventas
SET Total = (SELECT ISNULL(SUM(Cantidad * PrecioUnitario), 0) FROM DetalleVenta WHERE VentaID = @Venta8)
WHERE VentaID = @Venta8;

-- ============================================
-- FACTURA 9: Venta pequeña, sin cliente
-- ============================================
INSERT INTO Ventas (ClienteID, EmpleadoID, Fecha, Total)
VALUES (NULL, 2, '2025-03-15 15:10:00', 0);

DECLARE @Venta9 INT = SCOPE_IDENTITY();

INSERT INTO DetalleVenta (VentaID, ProductoID, Cantidad, PrecioUnitario)
VALUES
    (@Venta9, 1, 1, (SELECT Precio FROM Productos WHERE ProductoID = 1));

UPDATE Ventas
SET Total = (SELECT ISNULL(SUM(Cantidad * PrecioUnitario), 0) FROM DetalleVenta WHERE VentaID = @Venta9)
WHERE VentaID = @Venta9;

-- ============================================
-- FACTURA 10: Venta final, con cliente
-- ============================================
INSERT INTO Ventas (ClienteID, EmpleadoID, Fecha, Total)
VALUES (5, 3, '2025-03-20 12:00:00', 0);

DECLARE @Venta10 INT = SCOPE_IDENTITY();

INSERT INTO DetalleVenta (VentaID, ProductoID, Cantidad, PrecioUnitario)
VALUES
    (@Venta10, 3, 1, (SELECT Precio FROM Productos WHERE ProductoID = 3)),
    (@Venta10, 4, 2, (SELECT Precio FROM Productos WHERE ProductoID = 4)),
    (@Venta10, 7, 3, (SELECT Precio FROM Productos WHERE ProductoID = 7));

UPDATE Ventas
SET Total = (SELECT ISNULL(SUM(Cantidad * PrecioUnitario), 0) FROM DetalleVenta WHERE VentaID = @Venta10)
WHERE VentaID = @Venta10;

-- ============================================
-- VERIFICAR RESULTADOS
-- ============================================
SELECT
    v.VentaID,
    'VTA-' + RIGHT('00000' + CAST(v.VentaID AS VARCHAR), 5) AS NumeroFactura,
    ISNULL(c.Nombre + ' ' + c.Apellido, 'Público General') AS Cliente,
    e.Nombre + ' ' + e.Apellido AS Empleado,
    v.Fecha,
    v.Total
FROM Ventas v
LEFT JOIN Clientes c ON v.ClienteID = c.ClienteID
INNER JOIN Empleados e ON v.EmpleadoID = e.EmpleadoID
ORDER BY v.VentaID;

SELECT
    v.VentaID,
    p.Nombre AS Prenda,
    d.Cantidad,
    d.PrecioUnitario,
    (d.Cantidad * d.PrecioUnitario) AS Subtotal
FROM DetalleVenta d
INNER JOIN Ventas v ON d.VentaID = v.VentaID
INNER JOIN Productos p ON d.ProductoID = p.ProductoID
ORDER BY v.VentaID;
