using FarmerConnect.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using System.Data.SqlClient;

namespace FarmerConnect.Pages
{
    public class PurchasingViewModel : PageModel
    {
        private readonly string stgcon = "Data Source=SQL5112.site4now.net;Initial Catalog=db_aa2c0d_farmerconnect;User Id=db_aa2c0d_farmerconnect_admin;Password=Dreamon22";
        public List<Resources> ResourcesList { get; set; }
        public List<Services> ServicesList { get; set; }

        public bool IsResourceView { get; set; }
        public void OnGet(string view)
        {
            if (view == "resources")
            {
                ResourcesList = GetResourcesData();
                IsResourceView = true;
            }
            else
            {
                ServicesList = GetServicesData();
                IsResourceView = false;
            }
        }

        private List<Resources> GetResourcesData()
        {
            List<Resources> resources = new List<Resources>();
            using (SqlConnection connection = new SqlConnection(stgcon))
            {
                connection.Open();

                string query = "SELECT resource_id, seller_id, name, quantity, price, description FROM Resources";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Resources resource = new Resources
                            {
                                resource_id = reader.GetInt32(0),
                                seller_id = reader.GetInt32(1),
                                name = reader.GetString(2),
                                quantity = reader.GetInt32(3),
                                price = reader.GetDecimal(4),
                                description = reader.GetString(5),
                                //timestamp = new DateAndTime(reader.GetDateTime(6))
                            };
                            resources.Add(resource);
                        }
                    }
                }
            }

            return resources;
        }

        private List<Services> GetServicesData()
        {
            List<Services> services = new List<Services>();
            using (SqlConnection connection = new SqlConnection(stgcon))
            {
                connection.Open();

                string query = "SELECT * FROM Services";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Services service = new Services
                            {
                                ServiceId = reader.GetInt32(0),
                                ProviderId = reader.GetInt32(1),
                                ServiceType = reader.GetString(2),
                                Description = reader.GetString(3),
                                Price = reader.GetDecimal(4),
                                Timestamp = reader.GetDateTime(5)
                            };
                            services.Add(service);
                        }
                    }
                }
            }

            return services;
        }

    }
}

