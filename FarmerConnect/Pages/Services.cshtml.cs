using FarmerConnect.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace FarmerConnect.Pages
{
    public class ServicesModel : PageModel
    {
        private readonly string stgcon = "Data Source=SQL5112.site4now.net;Initial Catalog=db_aa2c0d_farmerconnect;User Id=db_aa2c0d_farmerconnect_admin;Password=Dreamon22";
        public string message = "";
        Services Services = new Services();
        
        public void OnGet()
        {
        }
        public void OnPost()
        {
            try
            {

                Services.ProviderId = int.Parse(Request.Form["user_id"]);
                Services.ServiceType = Request.Form["type"];
                Services.Description = Request.Form["description"];
                Services.Price = int.Parse(Request.Form["price"]);
    

            }catch (Exception ex)
            {
                message = "Problem: " + ex.Message;
            }
            using (SqlConnection con = new SqlConnection(stgcon))
            {
                string query = "INSERT INTO Services(provider_id, service_type, description, price) VALUES(@provider_id, @service_type, @description, @price)";
                try
                {
                    con.Open();
                    using(SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@provider_id", Services.ProviderId);
                        cmd.Parameters.AddWithValue("@service_type", Services.ServiceType);
                        cmd.Parameters.AddWithValue("@description", Services.Description);
                        cmd.Parameters.AddWithValue("@price", Services.Price);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            message = "Service Saved";
                            Services = new Services();
                        }
                        else
                        {
                            message = "Failed to save";
                        }
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    message = "Problem" + ex.Message;
                }
            }
        }
    }
}
