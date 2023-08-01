﻿"-- ============================================= "
"-- Author:		<Author,,Name> "
"-- Create date: <Create Date,,> "
"-- Description:	<Description,,> "
"-- ============================================= "
"CREATE PROCEDURE usp_SearchProducts "
"	-- Add the parameters for the stored procedure here "
"	@Keyword NVARCHAR(100) "
"AS "
"BEGIN "
"	-- SET NOCOUNT ON added to prevent extra result sets from "
"	-- interfering with SELECT statements. "
"	SET NOCOUNT ON; "
" "
"    -- Insert statements for procedure here "
"	SELECT  "
"        p.Id, "
"        p.Name, "
"        p.Price, "
"        p.CategoryID, "
"        c.Name AS CategoryName, "
"        pd.DiscountId, "
"        d.Percentage AS DiscountPercentage, "
"        d.StartDate AS DiscountStartDate, "
"        d.EndDate AS DiscountEndDate "
"    FROM Products p "
"    LEFT JOIN Categories c ON p.CategoryID = c.Id "
"    LEFT JOIN Products_Discount pd ON p.Id = pd.ProductId "
"    LEFT JOIN Discounts d ON pd.DiscountId = d.Id "
"    WHERE p.Name LIKE '%' + @Keyword + '%'; "
"END "
