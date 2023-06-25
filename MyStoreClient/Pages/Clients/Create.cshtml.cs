using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyStoreClient.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage = "";
        public String successMessage = "";

        public void OnGet()
        {
        }
        public void OnPost() {
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];
            if (clientInfo.name.Length == 0 || clientInfo.email.Length == 0 ||
                clientInfo.phone.Length == 0 || clientInfo.address.Length == 0)
            {
                errorMessage = " All fields are required!";
                return;
            }
            //save the new client to database

            try
            {
                String connectionString = "Data Source=.\\sqlexpress01;Initial Catalog=mystoredb;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO Clients" +
                        "(name, email, phone, address) VALUES " +
                        "(@name, @email, @phone, @address) ";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);

                        command.ExecuteNonQuery();
                    }
                }

            }catch (Exception ex) {
            errorMessage = ex.Message;
                return;
            
            }
            clientInfo.name = "";
            clientInfo.email = "";
            clientInfo.phone = "";
            clientInfo.address = "";
            successMessage = "New Client Added Successfully!";

            //redirect the user to the list page after successful data input
            Response.Redirect("/Clients/Index");
        }
    }
}
