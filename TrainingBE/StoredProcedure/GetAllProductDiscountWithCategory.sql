﻿"-- ============================================= "
"-- Author:		<Author,,Name> "
"-- Create date: <Create Date,,> "
"-- Description:	<Description,,> "
"-- ============================================= "
"CREATE PROCEDURE [dbo].[GetAllProductDiscountWithCategory] "
"	-- Add the parameters for the stored procedure here "
"	 "
"AS "
"BEGIN "
"	-- SET NOCOUNT ON added to prevent extra result sets from "
"	-- interfering with SELECT statements. "
"	SET NOCOUNT ON; "
" "
"    -- Insert statements for procedure here "
"	SELECT  "
"        pd.Id , "
"        pd.ProductId, "
"		p.Name AS ProductName, "
"        p.Price AS ProductPrice,  "
"        p.CategoryID AS ProductCategoryId, "
"        pd.DiscountId, "
"        d.Percentage AS DiscountPercentage, "
"        d.StartDate AS DiscountStartDate, "
"        d.EndDate AS DiscountEndDate "
" "
"    FROM Products_Discount pd "
"    LEFT JOIN Products p ON pd.ProductId = p.Id "
"    LEFT JOIN Discounts d ON pd.DiscountId = d.Id "
"END "
