using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.SqlConnection;

namespace KWLCodes_HMSProject.Maui
{
    internal class sql
    {
        public void ConnectToDatabase()
        {
            //Replace with your connection string
            string connectionString = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;";

            //Create a new connection to the database
            SqlConnection connection = new SqlConnection(connectionString);

            // Open the connection
            connection.Open();

            // Do some work with the connection// ...// Close the connection
            connection.Close();
        }
    }
}
