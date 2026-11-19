CREATE TABLE "Clients" (
    "ClientId" SERIAL PRIMARY KEY,
    "Name"     VARCHAR(100) NOT NULL,
    "Email"    VARCHAR(100) NOT NULL
);

CREATE TABLE "Products" (
    "ProductId"   SERIAL PRIMARY KEY,
    "Name"        VARCHAR(100) NOT NULL,
    "Description" VARCHAR(100),
    "Price"       NUMERIC(10,2) NOT NULL
);

CREATE TABLE "Orders" (
    "OrderId"   SERIAL PRIMARY KEY,
    "ClientId"  INTEGER NOT NULL REFERENCES "Clients"("ClientId"),
    "OrderDate" TIMESTAMP WITHOUT TIME ZONE NOT NULL
);

CREATE TABLE "OrderDetails" (
    "OrderDetailId" SERIAL PRIMARY KEY,
    "OrderId"       INTEGER NOT NULL REFERENCES "Orders"("OrderId"),
    "ProductId"     INTEGER NOT NULL REFERENCES "Products"("ProductId"),
    "Quantity"      INTEGER NOT NULL
);

INSERT INTO "Clients" ("ClientId", "Name", "Email") VALUES
    (1, 'Juan Pérez', 'juan.perez@example.com'),
    (2, 'Ana Gómez', 'ana.gomez@example.com'),
    (3, 'Carlos Díaz', 'carlos.diaz@example.com'),
    (4, 'Lucía Martínez', 'lucia.martinez@example.com');

INSERT INTO "Products" ("ProductId", "Name", "Description", "Price") VALUES
    (1, 'Producto A', 'This product is cheap', 10.50),
    (2, 'Producto B', 'This product is pretty', 25.00),
    (3, 'Producto C', 'This product is awesome', 15.75),
    (4, 'Producto D', NULL, 30.20);

INSERT INTO "Orders" ("OrderId", "ClientId", "OrderDate") VALUES
    (1, 1, '2025-05-01'),
    (2, 1, '2025-05-02'),
    (3, 2, '2025-05-03'),
    (4, 2, '2025-05-04'),
    (5, 3, '2025-05-05'),
    (6, 3, '2025-05-06'),
    (7, 4, '2025-05-07');

INSERT INTO "OrderDetails" ("OrderDetailId", "OrderId", "ProductId", "Quantity") VALUES
    (1, 1, 2, 2),
    (2, 1, 1, 1),
    (3, 2, 3, 1),
    (4, 2, 4, 1),
    (5, 3, 1, 3),
    (6, 3, 2, 2),
    (7, 4, 3, 1);

SELECT setval('"Clients_ClientId_seq"', (SELECT MAX("ClientId") FROM "Clients"));
SELECT setval('"Products_ProductId_seq"', (SELECT MAX("ProductId") FROM "Products"));
SELECT setval('"Orders_OrderId_seq"', (SELECT MAX("OrderId") FROM "Orders"));
SELECT setval('"OrderDetails_OrderDetailId_seq"', (SELECT MAX("OrderDetailId") FROM "OrderDetails"));
