using FarmerConnect.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;  // Import List
using System.Data.SqlClient;

namespace FarmerConnect.Pages
{
    public class FarmerHomeModel : PageModel
    {
        private readonly string stgcon = "Data Source=SQL5112.site4now.net;Initial Catalog=db_aa2c0d_farmerconnect;User Id=db_aa2c0d_farmerconnect_admin;Password=Dreamon22";

        private readonly Farmers farmers = new Farmers();
        public string message = "";
        KnowledgeSharing knowledgesharing = new KnowledgeSharing();
        public List<KnowledgeSharing> knowledgesharingList = new List<KnowledgeSharing>();

        public List<Farmers> Farmerslist = new List<Farmers>();

        public void OnGet()
        {
            getknowledgesharing();
            getAllknowledgesharing();
        }

        public IActionResult getknowledgesharing()
        {
            try
            {
                int? userId = HttpContext.Session.GetInt32("UserId");

                if (userId.HasValue)
                {
                    farmers.userid = userId.Value;

                    using (SqlConnection con = new SqlConnection(stgcon))
                    {
                        string query = "SELECT * FROM Farmers WHERE user_id=@user_id";
                        con.Open();

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@user_id", farmers.userid);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (!reader.Read())
                                {
                                   
                                    return RedirectToPage("/FarmersInfo");
                                }
                            }
                        }

                        con.Close();
                    }
                }
                else
                {
                   message = "User ID is not available in the session.";
                   return NotFound();
                }

                return Page();
            }
            catch (Exception ex)
            {
                message = "Problem: " + ex.Message;
                // Handle this case as needed
                // For now, let's return a NotFound result
                return NotFound();
            }
        }

        public void getAllknowledgesharing()
        {
            try
            {

                using (SqlConnection con = new SqlConnection(stgcon))
                {
                    string query = "SELECT post_id, title, content, timestamp FROM KnowledgeSharing";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                KnowledgeSharing knowledgesharing = new KnowledgeSharing();

                                knowledgesharing.Id = reader.GetInt32(0);
                                knowledgesharing.title = reader.GetString(1);
                                knowledgesharing.content = reader.GetString(2);
                                knowledgesharing.timestamp = reader.GetDateTime(3);

                                knowledgesharingList.Add(knowledgesharing);
                            }
                        }
                    }
                    con.Close();
                }

            }
            catch (Exception ex)
            {

            }
        }
    }
}
