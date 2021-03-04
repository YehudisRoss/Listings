using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Listings.Data
{
    public class Listing
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }

    }
    public class ListingsDb
    {
        private readonly string _connectionString;
        public ListingsDb(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void AddListing(Listing listing)
        {
            var connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Listings " +
                "(DateCreated, Text, Name, Phone) " +
                "VALUES(@DateCreated, @Text, @Name, @Phone) SELECT SCOPE_IDENTITY()";
            cmd.Parameters.AddWithValue("@DateCreated", listing.DateCreated);
            cmd.Parameters.AddWithValue("@Text", listing.Text);
            object name = listing.Name;
            if (name == null)
            {
                name = DBNull.Value;
            }
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Phone", listing.Phone);
            connection.Open();
            listing.Id = (int)(decimal)cmd.ExecuteScalar();
        }
        public void DeleteListing(int id)
        {
            var connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE from Listings WHERE Id = @Id";
            cmd.Parameters.AddWithValue("@Id", id);
            connection.Open();
            cmd.ExecuteNonQuery();
        }
        public List<Listing> GetListings()
        {
            var connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * From Listings ORDER BY DateCreated Desc";
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Listing> listings = new List<Listing>();
                        while (reader.Read())
            {
                listings.Add(new Listing
                {
                    Id = (int)reader["Id"],
                    DateCreated = (DateTime)reader["DateCreated"],
                    Text = (string)reader["Text"],
                    Name = getString(reader["Name"]),
                    Phone = (string)reader["Phone"]
                 });;
            }
            return listings;
        }
        private static String getString(Object o)
        {
            if (o == DBNull.Value) return null;
            return (String)o;
        }
    }
}
