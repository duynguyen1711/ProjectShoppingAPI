﻿"-- ============================================= "
"-- Author:		<Author,,Name> "
"-- Create date: <Create Date,,> "
"-- Description:	<Description,,> "
"-- ============================================= "
"CREATE PROCEDURE GetDiscountByDateRange "
"	-- Add the parameters for the stored procedure here "
"	@StartDate DATETIME, "
"    @EndDate DATETIME "
"AS "
"BEGIN "
"	-- SET NOCOUNT ON added to prevent extra result sets from "
"	-- interfering with SELECT statements. "
"	SET NOCOUNT ON; "
" "
"    -- Insert statements for procedure here "
"	SELECT * "
"    FROM Discounts "
"    WHERE StartDate <= @EndDate AND EndDate >= @StartDate "
"END "
