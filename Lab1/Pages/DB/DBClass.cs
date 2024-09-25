

using Lab1.Pages.DataClasses;
using System.Data.SqlClient;


namespace Lab1.Pages.DB
{
    public class DBClass
    {
        // Connection Object
        public static SqlConnection Lab1DBConnection = new SqlConnection();

        // Connection String - How to find and connect to DB
        private static readonly string Lab1DBConnString =
            "Server=Localhost;Database=Lab1;Trusted_Connection=True";

        // Method to Open Connection
        public static void OpenConnection()
        {
            if (Lab1DBConnection.State == System.Data.ConnectionState.Closed)
            {
                Lab1DBConnection.ConnectionString = Lab1DBConnString;
                Lab1DBConnection.Open();
            }
        }

        // Method to Close Connection
        public static void CloseConnection()
        {
            if (Lab1DBConnection.State == System.Data.ConnectionState.Open)
            {
                Lab1DBConnection.Close();
            }
        }

        
    }
}
