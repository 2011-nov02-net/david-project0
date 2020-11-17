DROP TABLE IF EXISTS Customer;

CREATE TABLE Customer (
	FirstName NVARCHAR(150) NOT NULL,
	LastName NVARCHAR(150) NOT NULL,
	DefaultLocationId INT NULL,
	Id INT IDENTITY,
	BirthDate DATE NULL,
	CONSTRAINT PK_CustId PRIMARY KEY (Id),
);

DROP TABLE IF EXISTS Location;

CREATE TABLE Location (
	Name NVARCHAR(150) NOT NULL,
	Id INT IDENTITY(10001,1),
	CONSTRAINT PK_LocationId PRIMARY KEY (Id)
);

DROP TABLE IF EXISTS Product;

CREATE TABLE Product (
	Name NVARCHAR(150) NOT NULL,
	Description TEXT NOT NULL,
	Id INT IDENTITY(5001,1),
	Price MONEY NOT NULL CHECK (Price >= 0.0),
	OrderLimit INT NOT NULL CHECK (OrderLimit > 0),
	CONSTRAINT PK_ProductId PRIMARY KEY (Id)
);

DROP TABLE IF EXISTS "Order";

CREATE TABLE "Order" (
	CustomerId INT NOT NULL,
	LocationId INT NOT NULL,
	Date DATE NOT NULL,
	OrderNumber INT IDENTITY,
	OrderTotal MONEY NOT NULL CHECK (OrderTotal > 0),
	CONSTRAINT PK_OrderNumber PRIMARY KEY (OrderNumber),
	CONSTRAINT FK_OrderCustomerId_CustomerId FOREIGN KEY (CustomerId) REFERENCES Customer(Id),
	CONSTRAINT FK_OrderLocationId_LocationId FOREIGN KEY (LocationId) REFERENCES Location(Id)
);

DROP TABLE IF EXISTS Inventory;

CREATE TABLE Inventory (
	LocationId INT NOT NULL,
	ProductId INT NOT NULL,
	Quantity INT NOT NULL CHECK (Quantity >= 0),
	CONSTRAINT PK_Inventory PRIMARY KEY (LocationId, ProductId),
	CONSTRAINT FK_InventoryLocationId_LocationId FOREIGN KEY (LocationId) REFERENCES Location(Id),
	CONSTRAINT FK_InventoryProductId_ProductId FOREIGN KEY (ProductId) REFERENCES Product(Id)

);

DROP TABLE IF EXISTS Sale;

CREATE TABLE Sale (
	OrderNumber INT NOT NULL,
	ProductId INT NOT NULL,
	ProductName NVARCHAR(150) NOT NULL,
	PurchasePrice MONEY NOT NULL CHECK (PurchasePrice >= 0),
	Quantity INT NOT NULL CHECK (Quantity > 0),
	CONSTRAINT PK_Sale PRIMARY KEY (OrderNumber, ProductId),
	CONSTRAINT FK_SaleOrderNumber_OrderOrderNumber FOREIGN KEY (OrderNumber) REFERENCES "Order"(OrderNumber),
	CONSTRAINT FK_SaleProductId_ProductId FOREIGN KEY (ProductId) REFERENCES Product(Id)
);

ALTER TABLE Customer ADD CONSTRAINT FK_DefaultLocationId_LocationId FOREIGN KEY (DefaultLocationId) REFERENCES Location(Id);