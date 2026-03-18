CREATE DATABASE QuanLiBanSua;
GO
USE QuanLiBanSua;

SELECT * FROM Milk;
SELECT * FROM Users;
SELECT * FROM Cart;
SELECT * FROM CartItem;

CREATE TABLE Milk
(
    MilkId INT PRIMARY KEY IDENTITY(1,1),
    MilkName NVARCHAR(200) NOT NULL,
    Brand NVARCHAR(100) NOT NULL,
    Weight NVARCHAR(50) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    StockQuantity INT NOT NULL DEFAULT 0,
    ExpiryDate DATETIME NOT NULL,
    ImagePath NVARCHAR(255) DEFAULT '\image\\sua.jpg'
);

CREATE TABLE Users
(
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Email NVARCHAR(100) NOT NULL UNIQUE,
    UserName NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    Role NVARCHAR(20) DEFAULT 'customer'
);

CREATE TABLE Cart
(
    CartId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL UNIQUE,
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

CREATE TABLE CartItem
(
    CartItemId INT PRIMARY KEY IDENTITY(1,1),
    CartId INT NOT NULL,
    MilkId INT NOT NULL,
    Quantity INT NOT NULL DEFAULT 1,
    FOREIGN KEY (CartId) REFERENCES Cart(CartId),
    FOREIGN KEY (MilkId) REFERENCES Milk(MilkId)
);



INSERT INTO Milk (MilkName, Brand, Weight, Price, StockQuantity, ExpiryDate)
VALUES
(N'Sữa tươi có đường', N'Vinamilk', N'180ml', 8000, 200, '2026-05-20'),
(N'Sữa tươi không đường', N'Vinamilk', N'1L', 32000, 150, '2026-07-15'),
(N'Sữa socola', N'Vinamilk', N'180ml', 9500, 300, '2026-06-18'),
(N'Sữa dâu', N'Vinamilk', N'180ml', 9500, 280, '2026-06-20'),
(N'Sữa chua uống Probi', N'Vinamilk', N'65ml x 5', 26000, 350, '2026-04-18'),

(N'Sữa tươi ít đường', N'TH True Milk', N'1L', 33000, 120, '2026-08-10'),
(N'Sữa chua nha đam', N'TH True Milk', N'100g', 7000, 300, '2026-04-28'),
(N'Sữa tươi organic', N'TH True Milk', N'1L', 40000, 90, '2026-08-30'),

(N'Sữa dâu', N'Dutch Lady', N'180ml', 9500, 280, '2026-06-20'),
(N'Sữa ít béo', N'Dutch Lady', N'1L', 31000, 130, '2026-07-22'),

(N'Sữa cacao', N'Milo', N'180ml', 10000, 300, '2026-07-01'),

(N'Sữa đậu nành', N'Fami', N'200ml', 7000, 400, '2026-06-05'),
(N'Sữa đậu nành ít đường', N'Fami', N'200ml', 6500, 450, '2026-06-10'),

(N'Sữa hạt óc chó', N'137 Degrees', N'1L', 75000, 90, '2026-09-12'),
(N'Sữa hạt điều', N'137 Degrees', N'1L', 80000, 85, '2026-09-25'),
(N'Sữa hạt mix 5 loại', N'137 Degrees', N'1L', 85000, 65, '2026-10-15'),
(N'Sữa hạnh nhân', N'Almond Breeze', N'1L', 78000, 100, '2026-10-05'),

(N'Sữa bột trẻ em', N'Abbott', N'900g', 520000, 50, '2027-03-01'),
(N'Sữa bột người lớn', N'Ensure', N'850g', 480000, 60, '2027-06-10'),
(N'Sữa bột Friso', N'Friso', N'900g', 510000, 70, '2027-05-20'),

(N'Sữa đặc không đường', N'Ông Thọ', N'380g', 24000, 180, '2027-01-25'),
(N'Sữa đặc có đường', N'Ngôi Sao Phương Nam', N'380g', 23000, 200, '2027-02-01'),

(N'Sữa gạo', N'Vinasoy', N'200ml', 7000, 320, '2026-06-25'),
(N'Sữa dê', N'Vitamilk', N'180ml', 15000, 75, '2026-07-28'),
(N'Sữa tươi Mộc Châu', N'Mộc Châu', N'180ml', 9000, 220, '2026-05-30'),
(N'Sữa chua ăn', N'Vinamilk', N'100g', 6000, 500, '2026-04-25');


INSERT INTO Users (Email, UserName, Password, Role) VALUES
('admin@milk.com', 'admin', '123456', 'admin'),
('user1@gmail.com', 'user1', '123456', 'customer'),
('user2@gmail.com', 'user2', '123456', 'customer'),
('user3@gmail.com', 'user3', '123456', 'customer'),
('user4@gmail.com', 'user4', '123456', 'customer'),
('user5@gmail.com', 'user5', '123456', 'customer');


INSERT INTO Cart (UserId) VALUES (2),(3),(4),(5),(6);


INSERT INTO CartItem (CartId, MilkId, Quantity) VALUES
(1,1,2),(1,2,1),(1,5,3),
(2,3,2),(2,7,1),(2,10,2),
(3,4,1),(3,8,2),(3,12,1),
(4,6,3),(4,9,2),
(5,11,1),(5,13,2),(5,15,1);