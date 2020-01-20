# Warehouse
The current Repository includes two Visual Studio 2019 solutions:
  -Warehouse solution: It's a .NET Core 3.1 Web API
  -WarehouseFE solution: It's a .NET Core 3.1 Razor/jQuey front-end that requieres the Web API to work

During the development of the solutions the following tools has been used:
  -Visual Studio 2019 Community Edition 16.4.2
  -SQL Server Express 2019
  -Postman 7.16.0
  
To be able to test the solutions I provided two options to restore the database: 
  -Option 1: Restore db backup with the file WarehouseDB.bak
  -Option 2: Run the scripts WarehouseDBDumpScript.sql to generate the DB objects and then run the script OriginData.sql to populate the database.
  
For testing the API I provided an exported Postman collection v2.1 so you could import it to Postman and test easier. The file: Warehouse.postman_collection.json

Important: The API utilizes JSON Web Token (JWT) to Authentication and Authorization so it's required to start by generating a token using the Login method (/api/Login/Auth). Once you got a token using the credentials of one of the Users in the database you can use that token as Authorization of type Bearer Token in any of the other methods.
