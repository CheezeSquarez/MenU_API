CREATE DATABASE MenU;
GO
USE MenU;
GO

--Account Table
CREATE TABLE Account(
    AccountID INT IDENTITY(1,1) NOT NULL,
    Username NVARCHAR(255) NOT NULL,
    FirstName NVARCHAR(255) NOT NULL,
    LastName NVARCHAR(255) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    DateOfBirth DATE NOT NULL,
    AccountType INT NOT NULL DEFAULT 1,
    ProfilePicture NVARCHAR(255) NOT NULL DEFAULT '/imgs/default_user_pfp.png',
    Pass NVARCHAR(255) NOT NULL,
    AccountStatus INT NOT NULL DEFAULT 1,
    Salt NVARCHAR(255) NOT NULL,
    Iterations INT NOT NULL
);
ALTER TABLE
    Account ADD CONSTRAINT PK_Account_AccountID PRIMARY KEY(AccountID);

CREATE UNIQUE INDEX UK_Account_Username ON
    Account(Username);

CREATE INDEX I_Account_FirstName ON
    Account(FirstName);

CREATE INDEX I_Account_LastName ON
    Account(LastName);

CREATE UNIQUE INDEX UK_Account_Email ON
    Account(Email);

CREATE INDEX I_Account_DateOfBirth ON
    Account(DateOfBirth);
--

--Restaurant Table
CREATE TABLE Restaurant(
    RestaurantID INT IDENTITY(1,1) NOT NULL,
    RestaurantName NVARCHAR(255) NOT NULL,
    StreetName NVARCHAR(255) NOT NULL,
    OwnerID INT NOT NULL,
    City NVARCHAR(255) NOT NULL,
    RestaurantPicture NVARCHAR(255) NOT NULL DEFAULT '/imgs/default_restaurant.png',
    StreetNumber NVARCHAR(255) NOT NULL,
	RestaurantStatus INT NOT NULL DEFAULT 4,
);

ALTER TABLE
    Restaurant ADD CONSTRAINT PK_Restaurant_RestaurantID PRIMARY KEY(RestaurantID);

CREATE INDEX I_Restaurant_RestaurantName ON
    Restaurant(RestaurantName);

CREATE INDEX I_Restaurant_StreetName ON
    Restaurant(StreetName);

CREATE INDEX I_Restaurant_City ON
    Restaurant(City);
--

--Review Table
CREATE TABLE Review(
    ReviewID INT IDENTITY(1,1) NOT NULL,
    PostDate DATE NOT NULL DEFAULT GETDATE(),
    Dish INT NOT NULL,
	ReviewTitle NVARCHAR(50) NOT NULL,
	ReviewBody NVARCHAR(255) NOT NULL,
    Rating INT NOT NULL,
    Reviewer INT NOT NULL,
    ReviewStatus INT NOT NULL DEFAULT 1,
    HasPicture BIT NOT NULL,
	IsLiked BIT NOT NULL
);
ALTER TABLE
    Review ADD CONSTRAINT PK_Review_ReviewID PRIMARY KEY(ReviewID);
--

--Dish Table
CREATE TABLE Dish(
    DishID INT IDENTITY(1,1) NOT NULL,
    DishName NVARCHAR(255) NOT NULL,
    DishDescription NVARCHAR(255) NOT NULL,
    Restaurant INT NOT NULL,
    DishStatus INT NOT NULL DEFAULT 1,
    DishPicture NVARCHAR(255) NOT NULL DEFAULT '/imgs/default_dish.png'
);

ALTER TABLE
    Dish ADD CONSTRAINT PK_Dish_DishID PRIMARY KEY(DishID);

CREATE INDEX I_Dish_DishName ON
    Dish(DishName);
--

--AccountType Table
CREATE TABLE AccountType(
    TypeID INT IDENTITY(1,1) NOT NULL,
    TypeName NVARCHAR(255) NOT NULL
);
ALTER TABLE
    AccountType ADD CONSTRAINT PK_AccountType_TypeID PRIMARY KEY(TypeID);
--

--Tag Table
CREATE TABLE Tag(
    TagID INT IDENTITY(1,1) NOT NULL,
    TagName NVARCHAR(255) NOT NULL
);

ALTER TABLE
    Tag ADD CONSTRAINT PK_Tag_TagID PRIMARY KEY(TagID);
--

--DishTag Table
CREATE TABLE DishTag(
    DishID INT NOT NULL,
    TagID INT NOT NULL
);
ALTER TABLE
    DishTag ADD CONSTRAINT PK_DishTag_DishID_TagID PRIMARY KEY(DishID, TagID);
--

--ObjectStatus Table
CREATE TABLE ObjectStatus(
    StatusID INT IDENTITY(1,1) NOT NULL,
    StatusName NVARCHAR(255) NOT NULL
);
ALTER TABLE
    ObjectStatus ADD CONSTRAINT PK_Status_StatusID PRIMARY KEY(StatusID);
--

--Allergen Table
CREATE TABLE Allergen(
    AllergenID INT IDENTITY(1,1) NOT NULL,
    Allergen NVARCHAR(255) NOT NULL
);
ALTER TABLE
    Allergen ADD CONSTRAINT PK_Allergen_AllergenID PRIMARY KEY(AllergenID);
--

--AllergenInDish
CREATE TABLE AllergenInDish(
    AllergenID INT NOT NULL,
    DishID INT NOT NULL
);
ALTER TABLE
    AllergenInDish ADD CONSTRAINT PK_AllergenInDish_AllergenID_DishID PRIMARY KEY(AllergenID, DishID);
--

--AccountTag Table
CREATE TABLE AccountTag(
    AccountID INT NOT NULL,
    TagID INT NOT NULL,
    PickedNum INT NOT NULL
);

ALTER TABLE
    AccountTag ADD CONSTRAINT PK_AccountTag_AccountID_TagID PRIMARY KEY(AccountID, TagID);
--

--AccountAuthToken Table
CREATE TABLE AccountAuthToken(
    AccountID INT NOT NULL,
    AuthToken NVARCHAR(255) NOT NULL
);
ALTER TABLE
    AccountAuthToken ADD CONSTRAINT PK_AccountAuthToken_AuthToken PRIMARY KEY(AuthToken);
--

--RestaurantTag Table
CREATE TABLE RestaurantTag(
    TagID INT NOT NULL,
    RestaurantID INT NOT NULL
);
ALTER TABLE
    RestaurantTag ADD CONSTRAINT PK_RestaurantTag_TagID_RestaurantID PRIMARY KEY(TagID, RestaurantID);
--

--Adding Foreign Keys
ALTER TABLE
    Review ADD CONSTRAINT FK_Review_Dish FOREIGN KEY(Dish) REFERENCES Dish(DishID);
ALTER TABLE
    Restaurant ADD CONSTRAINT FK_Restaurant_Owner FOREIGN KEY(OwnerID) REFERENCES Account(AccountID);
ALTER TABLE
	Restaurant ADD CONSTRAINT FK_Restaurant_RestaurantStatus FOREIGN KEY(RestaurantStatus) REFERENCES ObjectStatus(StatusID);
ALTER TABLE
    Review ADD CONSTRAINT FK_Review_Reviewer FOREIGN KEY(Reviewer) REFERENCES Account(AccountID);
ALTER TABLE
    Account ADD CONSTRAINT FK_Account_AccountType FOREIGN KEY(AccountType) REFERENCES AccountType(TypeID);
ALTER TABLE
    Dish ADD CONSTRAINT FK_Dish_Restaurant FOREIGN KEY(Restaurant) REFERENCES Restaurant(RestaurantID);
ALTER TABLE
    Review ADD CONSTRAINT FK_Review_ReviewStatus FOREIGN KEY(ReviewStatus) REFERENCES ObjectStatus(StatusID);
ALTER TABLE
    Dish ADD CONSTRAINT FK_Dish_DishStatus FOREIGN KEY(DishStatus) REFERENCES ObjectStatus(StatusID);
ALTER TABLE
    Account ADD CONSTRAINT FK_Account_AccountStatus FOREIGN KEY(AccountStatus) REFERENCES ObjectStatus(StatusID);
ALTER TABLE
    AccountAuthToken ADD CONSTRAINT FK_AccountAuthToken_AccountID FOREIGN KEY(AccountID) REFERENCES Account(AccountID);
ALTER TABLE
	AllergenInDish ADD CONSTRAINT FK_AllergenInDish_AllergenID FOREIGN KEY(AllergenID) REFERENCES Allergen(AllergenID);
ALTER TABLE
	AllergenInDish ADD CONSTRAINT FK_AllergenInDish_DishID FOREIGN KEY(DishID) REFERENCES Dish(DishID);
ALTER TABLE
	RestaurantTag ADD CONSTRAINT FK_RestaurantTag_TagID FOREIGN KEY(TagID) REFERENCES Tag(TagID);
ALTER TABLE
	DishTag ADD CONSTRAINT FK_Dish_TagID FOREIGN KEY(TagID) REFERENCES Tag(TagID);
ALTER TABLE
	AccountTag ADD CONSTRAINT FK_AccountTag_TagID FOREIGN KEY(TagID) REFERENCES Tag(TagID);
ALTER TABLE
	RestaurantTag ADD CONSTRAINT FK_RestaurantTag_RestaurantID FOREIGN KEY(RestaurantID) REFERENCES Restaurant(RestaurantID);
ALTER TABLE
	DishTag ADD CONSTRAINT FK_DishTag_DishID FOREIGN KEY(DishID) REFERENCES Dish(DishID);
ALTER TABLE
	AccountTag ADD CONSTRAINT FK_AccountTag_AccountID FOREIGN KEY(AccountID) REFERENCES Account(AccountID);
