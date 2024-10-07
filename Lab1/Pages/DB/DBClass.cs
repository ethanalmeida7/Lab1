

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
            "Server=Localhost;Database=Lab2;Trusted_Connection=True";

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


        public static int LoginQuery(string loginQuery)
        {
            // This method expects to receive an SQL SELECT
            // query that uses the COUNT command.

            SqlCommand cmdLogin = new SqlCommand();
            cmdLogin.Connection = Lab1DBConnection;
            cmdLogin.Connection.ConnectionString = Lab1DBConnString;
            cmdLogin.CommandText = loginQuery;
            cmdLogin.Connection.Open();

            // ExecuteScalar() returns back data type Object
            // Use a typecast to convert this to an int.
            // Method returns first column of first row.
            int rowCount = (int)cmdLogin.ExecuteScalar();

            return rowCount;

            
        }


    }
}
