INSERT INTO Customer (FirstName, LastName) VALUES ('Sherlock','Holmes'), ('Rowland','Hale'), ('Eli','Marlow'), ('Roland','Elwin'), ('Morton','Chadwick');

SELECT * FROM Customer;

INSERT INTO Location (Name) VALUES ('Walmart'),('Target'),('Starbucks');

SELECT * FROM Location;

INSERT INTO Product (Name, Description, Price, OrderLimit) VALUES
	('Calculator', 'Magical Adding Device', 149.99, 2),('Towel', 'Do you know where your towel is?', 14.99, 10),('Boots', 'You know, For walking in', 74.99, 5),
	('Blender', 'Will it Blend?', 49.99, 7),('Mop', 'To clean up the mess left by your blender', 7.99, 50);

SELECT * FROM PRODUCT;

INSERT INTO "Order" (CustomerId, LocationId, Date, OrderTotal) VALUES (1, 10002, '01/11/2018', 449.88), (3, 10003, '02/14/2018', 49.99), (3, 10001, '02/15/2018', 15.98);

SELECT * FROM "Order";

INSERT INTO Sale (OrderNumber, ProductId, ProductName, PurchasePrice, Quantity) VALUES 
	(1, 5001, 'Calulator',149.99, 2),
	(1, 5002, 'Towel',14.99, 10),
	(2, 5004, 'Blender',49.99, 1),
	(3, 5005, 'Mop',7.99, 2)

SELECT * FROM Sale;

INSERT INTO Inventory (LocationId, ProductId, Quantity) VALUES
	(10001, 5001, 2), (10001, 5002, 100), (10001, 5003, 20), (10001, 5004, 45), (10001, 5005, 1000),
	(10002, 5001, 20), (10002, 5002, 1000), (10002, 5003, 15), (10002, 5004, 100), (10002, 5005, 20),
	(10003, 5001, 4), (10003, 5002, 20), (10003, 5003, 5), (10003, 5004, 200), (10003, 5005, 800)

SELECT * FROM Inventory;