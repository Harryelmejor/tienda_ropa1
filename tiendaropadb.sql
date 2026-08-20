CREATE DATABASE TiendaRopaDB;
GO

USE TiendaRopaDB;
GO


CREATE TABLE Categorias (
    CategoriaID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(255)
);
GO


CREATE TABLE Productos (
    ProductoID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(150) NOT NULL,
    Descripcion VARCHAR(255),
    Precio DECIMAL(10,2) NOT NULL,
    Stock INT NOT NULL DEFAULT 0,
    Talla VARCHAR(20),
    Color VARCHAR(50),
    CategoriaID INT NOT NULL,

    CONSTRAINT FK_Productos_Categorias
        FOREIGN KEY (CategoriaID)
        REFERENCES Categorias(CategoriaID),

    CONSTRAINT CK_Productos_Precio
        CHECK (Precio >= 0),

    CONSTRAINT CK_Productos_Stock
        CHECK (Stock >= 0)
);
GO


CREATE TABLE Clientes (
    ClienteID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Apellido VARCHAR(100) NOT NULL,
    Telefono VARCHAR(20),
    Email VARCHAR(150),
    Direccion VARCHAR(250)
);
GO


CREATE TABLE Empleados (
    EmpleadoID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Apellido VARCHAR(100) NOT NULL,
    Telefono VARCHAR(20),
    Email VARCHAR(150),
    Cargo VARCHAR(100)
);
GO


CREATE TABLE Ventas (
    VentaID INT IDENTITY(1,1) PRIMARY KEY,
    ClienteID INT NULL,
    EmpleadoID INT NOT NULL,
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),
    Total DECIMAL(10,2) NOT NULL DEFAULT 0,

    CONSTRAINT FK_Ventas_Clientes
        FOREIGN KEY (ClienteID)
        REFERENCES Clientes(ClienteID),

    CONSTRAINT FK_Ventas_Empleados
        FOREIGN KEY (EmpleadoID)
        REFERENCES Empleados(EmpleadoID)
);
GO


CREATE TABLE DetalleVenta (
    DetalleVentaID INT IDENTITY(1,1) PRIMARY KEY,
    VentaID INT NOT NULL,
    ProductoID INT NOT NULL,
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(10,2) NOT NULL,
    Subtotal AS (Cantidad * PrecioUnitario) PERSISTED,

    CONSTRAINT FK_DetalleVenta_Ventas
        FOREIGN KEY (VentaID)
        REFERENCES Ventas(VentaID),

    CONSTRAINT FK_DetalleVenta_Productos
        FOREIGN KEY (ProductoID)
        REFERENCES Productos(ProductoID),

    CONSTRAINT CK_DetalleVenta_Cantidad
        CHECK (Cantidad > 0),

    CONSTRAINT CK_DetalleVenta_Precio
        CHECK (PrecioUnitario >= 0)
);
GO



INSERT INTO Categorias (Nombre, Descripcion)
VALUES
('Camisetas', 'Camisetas para hombres y mujeres'),
('Pantalones', 'Pantalones y jeans'),
('Zapatos', 'Calzado'),
('Accesorios', 'Gorras, cinturones y otros accesorios'),
('Chaquetas', 'Chaquetas y abrigos');
GO

INSERT INTO Productos
    (Nombre, Descripcion, Precio, Stock, Talla, Color, CategoriaID)
VALUES
('Camiseta básica', 'Camiseta de algodón', 750.00, 20, 'M', 'Negro', 1),
('Camiseta deportiva', 'Camiseta deportiva', 1200.00, 15, 'L', 'Azul', 1),
('Jean clásico', 'Pantalón jean clásico', 1800.00, 10, '32', 'Azul', 2),
('Pantalón negro', 'Pantalón casual negro', 1600.00, 12, '34', 'Negro', 2),
('Zapatillas deportivas', 'Zapatillas para uso diario', 2500.00, 8, '42', 'Blanco', 3),
('Gorra deportiva', 'Gorra ajustable', 600.00, 25, 'Única', 'Negro', 4),
('Chaqueta casual', 'Chaqueta para clima frío', 3000.00, 7, 'L', 'Gris', 5);
GO

INSERT INTO Clientes
    (Nombre, Apellido, Telefono, Email, Direccion)
VALUES
('Juan', 'Perez', '809-555-1001', 'juan@gmail.com', 'Santo Domingo'),
('Maria', 'Rodriguez', '809-555-1002', 'maria@gmail.com', 'Santo Domingo');
GO

INSERT INTO Empleados
    (Nombre, Apellido, Telefono, Email, Cargo)
VALUES
('Carlos', 'Martinez', '809-555-2001', 'carlos@tiendaropa.com', 'Vendedor'),
('Ana', 'Garcia', '809-555-2002', 'ana@tiendaropa.com', 'Administradora');
GO