﻿"-- ============================================= "
"-- Author:		<Author,,Name> "
"-- Create date: <Create Date,,> "
"-- Description:	<Description,,> "
"-- ============================================= "
"CREATE PROCEDURE DeleteProduct "
"	-- Add the parameters for the stored procedure here "
"	@Id INT  "
"AS "
"BEGIN "
"	-- SET NOCOUNT ON added to prevent extra result sets from "
"	-- interfering with SELECT statements. "
"	SET NOCOUNT ON; "
" "
"    Delete From Products "
"	Where  Id = @Id "
"END "
