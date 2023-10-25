using E_VotingSystem.ConnectionString;
using E_VotingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;

namespace E_VotingSystem.Controllers
{
    public class ChangePassword : Controller
    {
        SqlConnection l_SqlConnection = new SqlConnection();
        SqlCommand l_SqlCommand = new SqlCommand();
        SqlDataReader? l_SqlDataReader;
      
        public IActionResult Index()
        {

            ModUser l_ModLoggedInUser = new ModUser();
            l_ModLoggedInUser = HttpContext.Session.Get<ModUser>("LoggedinUser")!;
            return View(l_ModLoggedInUser);
        }

        void FncConnectionString()
        {
            l_SqlConnection.ConnectionString = ConnectionHelper.FncGetConnectionString();
        }

        [HttpPost]
        public ActionResult UpdatePassword(string newPassword)
        {
            FncConnectionString();
            l_SqlConnection.Open();
            l_SqlCommand.Connection = l_SqlConnection;

            ModUser l_ModLoggedInUser = new ModUser();
            l_ModLoggedInUser = HttpContext.Session.Get<ModUser>("LoggedinUser")!;

            if (l_ModLoggedInUser != null)
            {
                // Update the password for the logged-in user
                l_SqlCommand.CommandText = "UPDATE Vw_TBU_Members SET Password = @NewPassword WHERE MembershipID = @MembershipID";
                l_SqlCommand.Parameters.AddWithValue("@MembershipID", l_ModLoggedInUser.MembershipID);
                l_SqlCommand.Parameters.AddWithValue("@NewPassword", newPassword);

                int rowsAffected = l_SqlCommand.ExecuteNonQuery();

                l_SqlConnection.Close();

                if (rowsAffected > 0)
                {
                    // Password updated successfullyy
                    return View("passchanged");
                }
                else
                {
                    // Failed to update the password
                    return View("failed");
                }
            }
            else
            {
                // User is not logged in
                l_SqlConnection.Close();
                return View("ErrorPassword");
            }
        }
    }
}
