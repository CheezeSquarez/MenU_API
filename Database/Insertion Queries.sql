Use MenU;
Go
INSERT INTO AccountType (TypeName) Values ('DefaultUser');
INSERT INTO AccountType (TypeName) Values ('RestaurantOwner');
INSERT INTO AccountType (TypeName) Values ('Admin');

INSERT INTO ObjectStatus (StatusName) Values ('Active');
INSERT INTO ObjectStatus (StatusName) Values ('Hidden');
INSERT INTO ObjectStatus (StatusName) Values ('Disabled');
INSERT INTO ObjectStatus (StatusName) Values ('Pending');

INSERT INTO Allergen (Allergen) Values ('Egg');
INSERT INTO Allergen (Allergen) Values ('Fish');
INSERT INTO Allergen (Allergen) Values ('Fruit');
INSERT INTO Allergen (Allergen) Values ('Garlic');
INSERT INTO Allergen (Allergen) Values ('Milk');
INSERT INTO Allergen (Allergen) Values ('Oats');
INSERT INTO Allergen (Allergen) Values ('Peanut');
INSERT INTO Allergen (Allergen) Values ('Red meat');
INSERT INTO Allergen (Allergen) Values ('Poultry meat');
INSERT INTO Allergen (Allergen) Values ('Rice');
INSERT INTO Allergen (Allergen) Values ('Seasame');
INSERT INTO Allergen (Allergen) Values ('Soy');
INSERT INTO Allergen (Allergen) Values ('Tree nut');
INSERT INTO Allergen (Allergen) Values ('Wheat');
INSERT INTO Allergen (Allergen) Values ('Penicillin');
INSERT INTO Allergen (Allergen) Values ('Mushroom');
INSERT INTO Allergen (Allergen) Values ('Mustard');
INSERT INTO Allergen (Allergen) Values ('Balsam of Peru');
INSERT INTO Allergen (Allergen) Values ('Buckwheat');
INSERT INTO Allergen (Allergen) Values ('Gluten');
INSERT INTO Allergen (Allergen) Values ('Celery');

INSERT INTO Tag (TagName) Values ('Greek');
INSERT INTO Tag (TagName) Values ('Italian');
INSERT INTO Tag (TagName) Values ('Mediterranean');
INSERT INTO Tag (TagName) Values ('Indian');
INSERT INTO Tag (TagName) Values ('Thai');
INSERT INTO Tag (TagName) Values ('Farsi');
INSERT INTO Tag (TagName) Values ('Asian');
INSERT INTO Tag (TagName) Values ('Hawaiian');
INSERT INTO Tag (TagName) Values ('Breakfast');
INSERT INTO Tag (TagName) Values ('Lucnh');
INSERT INTO Tag (TagName) Values ('Brunch');
INSERT INTO Tag (TagName) Values ('Dinner');
INSERT INTO Tag (TagName) Values ('Global Cuisine');
INSERT INTO Tag (TagName) Values ('British');
INSERT INTO Tag (TagName) Values ('Fusion Cuisine');
INSERT INTO Tag (TagName) Values ('Middle Eastern');
INSERT INTO Tag (TagName) Values ('Street Food');
INSERT INTO Tag (TagName) Values ('French');
INSERT INTO Tag (TagName) Values ('Soul food');
INSERT INTO Tag (TagName) Values ('Mexican');
INSERT INTO Tag (TagName) Values ('European');
INSERT INTO Tag (TagName) Values ('American');
INSERT INTO Tag (TagName) Values ('Argentinian');
INSERT INTO Tag (TagName) Values ('Japanese');
INSERT INTO Tag (TagName) Values ('Spanish');
INSERT INTO Tag (TagName) Values ('Sushi');
INSERT INTO Tag (TagName) Values ('Spicy');
INSERT INTO Tag (TagName) Values ('Fast Food');
INSERT INTO Tag (TagName) Values ('Casual Dining');
INSERT INTO Tag (TagName) Values ('Family Style');
INSERT INTO Tag (TagName) Values ('Fine Dining');
INSERT INTO Tag (TagName) Values ('Pet Friendly');
INSERT INTO Tag (TagName) Values ('Cafe');
INSERT INTO Tag (TagName) Values ('Buffet');
INSERT INTO Tag (TagName) Values ('Pub');
INSERT INTO Tag (TagName) Values ('Vegtables');
INSERT INTO Tag (TagName) Values ('Soup');
INSERT INTO Tag (TagName) Values ('Meat');
INSERT INTO Tag (TagName) Values ('Dairy');
INSERT INTO Tag (TagName) Values ('BBQ');
INSERT INTO Tag (TagName) Values ('Deli');
INSERT INTO Tag (TagName) Values ('Pastry');
INSERT INTO Tag (TagName) Values ('Chefs Choice');
INSERT INTO Tag (TagName) Values ('Gluten Free');
INSERT INTO Tag (TagName) Values ('Pizza');
INSERT INTO Tag (TagName) Values ('Pasta');
INSERT INTO Tag (TagName) Values ('Vegan Friendly');
INSERT INTO Tag (TagName) Values ('Seafood');
INSERT INTO Tag (TagName) Values ('Vegitarian');
INSERT INTO Tag (TagName) Values ('Kosher');
INSERT INTO Tag (TagName) Values ('Salad');
INSERT INTO Tag (TagName) Values ('Rice Bowl');

--Demo Users
INSERT INTO Account (Username, FirstName, LastName, Email, DateOfBirth, AccountType, Pass, AccountStatus, Salt, Iterations) VALUES ('IdoSadeh123', 'Ido', 'Sadeh', 'idosadeh@rosenfeld.com',GETDATE(), 1, 'aafeeba6959ebeeb96519d5dcf0bcc069f81e4bb56c246d04872db92666e6d4b', 1, '12345678', 14000);
INSERT INTO Account (Username, FirstName, LastName, Email, DateOfBirth, AccountType, Pass, AccountStatus, Salt, Iterations) VALUES ('MitziHaHatool', 'Mitzi', 'HaHatool', 'mitzi@hahatool.com',GETDATE(), 1, 'ec81882d4a11906cd6e58e576fbcbb1a4e7893bd18cb050660bd55ed6dad25fa', 1, 'x4fqvc669', 14000);
--Demo Restaurant
INSERT INTO [dbo].[Restaurant] ([RestaurantName],[StreetName],[OwnerID],[City],[StreetNumber],[RestaurantStatus])
     VALUES
('Cat Meat Express','Kfar Yonah',2,'Givat Lavi',8,1)


