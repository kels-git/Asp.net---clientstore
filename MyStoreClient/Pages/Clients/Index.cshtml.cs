using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyStoreClient.Pages.Clients
{
    public class IndexModel : PageModel
    {
        //creating a list
        public List<ClientInfo> listClients = new List<ClientInfo>();
        public String databaseIsEmpty = "";
        public void OnGet()
        {
            try
            {

                String connectionString = "Data Source=.\\sqlexpress01;Initial Catalog=mystoredb;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM clients";
                     

                    // Retrieve the count of remaining rows in the clients table
                    String countSql = "SELECT COUNT(*) FROM clients";
                    using (SqlCommand countCommand = new SqlCommand(countSql, connection))
                    {
                        int rowCount = (int)countCommand.ExecuteScalar();

                        if (rowCount == 0)
                        {
                            // Display a notice if the database is empty
                            databaseIsEmpty = "The database is empty. Please add new Client with the button above.";
                        }
                    }


                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                       using(SqlDataReader reader = command.ExecuteReader()) {
                        while(reader.Read())
                            {
                                ClientInfo clientInfo = new ClientInfo();
                                clientInfo.id = "" + reader.GetInt32(0);
                                clientInfo.name = reader.GetString(1);
                                clientInfo.email = reader.GetString(2);
                                clientInfo.phone = reader.GetString(3);
                                clientInfo.address = reader.GetString(4);
                                clientInfo.created_at = reader.GetDateTime(5).ToString();

                                listClients.Add(clientInfo);
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                   Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }

    public class ClientInfo {
        public String id;
        public String name;
        public String email;
        public String phone;
        public String address;
        public String created_at;
    }

}
