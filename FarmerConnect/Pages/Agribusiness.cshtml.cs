using FarmerConnect.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace FarmerConnect.Pages
{
    public class AgribusinessModel : PageModel
    {
        private readonly string stgcon = "Data Source=SQL5112.site4now.net;Initial Catalog=db_aa2c0d_farmerconnect;User Id=db_aa2c0d_farmerconnect_admin;Password=Dreamon22";        public Agribusinesses Agribusinesses = new Agribusinesses();
        public string message = "";
        KnowledgeSharing knowledgesharing = new KnowledgeSharing();
        public List<KnowledgeSharing> knowledgesharingList = new List<KnowledgeSharing>();
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
                    Agribusinesses.userid = userId.Value;

                    using (SqlConnection con = new SqlConnection(stgcon))
                    {
                        string query = "SELECT * FROM Agribusinesses WHERE user_id=@user_id";
                        con.Open();

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@user_id", Agribusinesses.userid);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    message = "good";
                                }
                                else
                                {
                                    // Redirect to AgribusinessInfo page without specifying an area
                                    return RedirectToPage("/AgribusinessesInfo", new { area = "Pages" });
                                }
                            }
                        }

                        con.Close();
                    }
                }
                else
                {
                    message = "User ID is not available in the session.";
                    // Handle this case as needed
                    // For now, let's return a NotFound result
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
