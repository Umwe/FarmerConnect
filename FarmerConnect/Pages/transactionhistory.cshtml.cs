using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FarmerConnect.Model;
using System.Data.SqlClient;
using System.Resources;


namespace FarmerConnect.Pages
{
    public class transactionhistoryModel : PageModel
    {
        private readonly string stgcon = "Data Source=SQL5112.site4now.net;Initial Catalog=db_aa2c0d_farmerconnect;User Id=db_aa2c0d_farmerconnect_admin;Password=Dreamon22";
        public List<Transaction> Transactions { get; set; }
        public void OnGet()
        {
            Transactions = GetTransactionsHistory();
        }
        private List<Transaction> GetTransactionsHistory()
        {
            List<Transaction> transactions = new List<Transaction>();
            int sellerId = (int)HttpContext.Session.GetInt32("UserId");
            using (SqlConnection connection = new SqlConnection(stgcon))
            {
                connection.Open();

                string query = "SELECT * FROM Transactions WHERE [seller_id]=@seller_id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@seller_id", sellerId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Transaction transactionItem = new Transaction
                            {
                                buyer_id = reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1),
                                resource_id = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                                service_id = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4),
                                quantity = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5),
                                total_amount = reader.IsDBNull(6) ? (decimal?)null : reader.GetDecimal(6),
                                transaction_date = reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7),
                            };

                            transactions.Add(transactionItem);
                        }
                    }
                }
            }
            return transactions;
        }
    }

}
