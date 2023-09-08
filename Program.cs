using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

public class BikeRentalSystem
{
    private readonly string connectionString;

    public BikeRentalSystem(string dbFilePath)
    {
        connectionString = $"Data Source={dbFilePath};Version=3;";
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            
            string createClientsTableQuery = @"
                CREATE TABLE IF NOT EXISTS Clients (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL UNIQUE,
                    Address TEXT NOT NULL,
                    Phone TEXT NOT NULL,
                    Birthday DATE NOT NULL
                );";

            using (SQLiteCommand command = new SQLiteCommand(createClientsTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            
            string createRentalsTableQuery = @"
                CREATE TABLE IF NOT EXISTS Rentals (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ClientId INTEGER NOT NULL,
                    RentalDate DATE NOT NULL,
                    ReturnDate DATE,
                    RentalPrice DECIMAL(10, 2) NOT NULL,
                    FOREIGN KEY(ClientId) REFERENCES Clients(Id)
                );";

            using (SQLiteCommand command = new SQLiteCommand(createRentalsTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }

    public void AddClient(Client client)
    {
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string insertClientQuery = @"
                INSERT INTO Clients (Name, Address, Phone, Birthday)
                VALUES (@Name, @Address, @Phone, @Birthday);";

            using (SQLiteCommand command = new SQLiteCommand(insertClientQuery, connection))
            {
                command.Parameters.AddWithValue("@Name", client.Name);
                command.Parameters.AddWithValue("@Address", client.Address);
                command.Parameters.AddWithValue("@Phone", client.Phone);
                command.Parameters.AddWithValue("@Birthday", client.Birthday);

                command.ExecuteNonQuery();
            }
        }
    }

 

    public void RentBike(Rental rental)
    {
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string insertRentalQuery = @"
                INSERT INTO Rentals (ClientId, RentalDate, ReturnDate, RentalPrice)
                VALUES (@ClientId, @RentalDate, @ReturnDate, @RentalPrice);";

            using (SQLiteCommand command = new SQLiteCommand(insertRentalQuery, connection))
            {
                command.Parameters.AddWithValue("@ClientId", rental.ClientId);
                command.Parameters.AddWithValue("@RentalDate", rental.RentalDate);
                command.Parameters.AddWithValue("@ReturnDate", rental.ReturnDate ?? DBNull.Value);
                command.Parameters.AddWithValue("@RentalPrice", rental.RentalPrice);

                command.ExecuteNonQuery();
            }
        }
    }

    
}

public class Client
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public DateTime Birthday { get; set; }
}

public class Rental
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public DateTime RentalDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public decimal RentalPrice { get; set; }
}