-- Create a stored prcedure with all possible queries for the application
CREATE PROCEDURE [ProductsSPROC]
	@Action varchar(max),
   @ProductID INT = NULL,
   @CategoryID INT = NULL,
   @Title nvarchar(64) = NULL,
   @ShortDescription nvarchar(64)  = NULL,
   @LongDescription nvarchar(128)  = NULL,
   @ImageUrl nvarchar(128)  = NULL,
   @Price money = 0.00
AS
BEGIN
   SET NOCOUNT ON;

   -- Display everything except the product id from the products table
	IF @Action = 'SELECT'
      BEGIN
         SELECT ProductID, CategoryID, Title, ShortDescription, LongDescription, ImageUrl, Price
         FROM Product
      END

   -- Select everything from the products table whose category id is passed in from the calling method
   IF @Action = 'SELECT-CATEGORY'
      BEGIN
         SELECT *
         FROM Product
         WHERE CategoryID = @CategoryID
      END

   -- Insert all values passed in from the insert method and place them into the product table
   IF @Action = 'INSERT'
      BEGIN
         INSERT INTO Product(CategoryID, Title, ShortDescription, LongDescription, ImageUrl, Price)
         VALUES (@CategoryId, @Title, @ShortDescription, @LongDescription, @ImageUrl, @Price);
      END

   -- Update the product table's specific product by it's id, using the values passed in from the update method
   IF @Action = 'UPDATE'
      BEGIN
         UPDATE Product
         SET CategoryID = @CategoryId, Title = @Title, ShortDescription = @ShortDescription, LongDescription = @LongDescription, ImageUrl = @ImageUrl, Price = @Price
         WHERE ProductID = @ProductID
      END
 
   -- Delete the specified product id from the product table using the passed in product id parameter
   IF @Action = 'DELETE'
      BEGIN
         DELETE FROM Product
         WHERE ProductID = @ProductID
      END

    -- Delete an entire category from the category using the passed in category it, by first deleting all 
    -- it's dependant products whose product id's are suppplied via the calling method
    IF @Action = 'DELETE-CATEGORY'
      BEGIN
         DELETE FROM Product
         WHERE CategoryID = @CategoryID
         DELETE FROM Category
         WHERE CategoryID = @CategoryID
      END
END