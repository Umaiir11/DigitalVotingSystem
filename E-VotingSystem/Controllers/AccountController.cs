using E_VotingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace E_VotingSystem.Controllers
{
    public class AccountController : Controller
    {
        SqlConnection l_SqlConnection = new SqlConnection();
        SqlCommand l_SqlCommand = new SqlCommand();
        SqlDataReader? l_SqlDataReader;

        // GET: Account
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        void FncConnectionString()
        {
            l_SqlConnection.ConnectionString = "Data Source=MUHAMMAD-UMAIR\\AISONESQL;Initial Catalog=EVoting;Integrated Security=True";
        }

        [HttpPost]
        public ActionResult Verify(ModUser lModUser)
        {
            DalInsertVoting l_DalInsertVoting = new DalInsertVoting();
            FncConnectionString();
            l_SqlConnection.Open();
            l_SqlCommand.Connection = l_SqlConnection;
            l_SqlCommand.CommandText = "SELECT * FROM TBU_Member WHERE MembershipID = @MembershipID AND Password = @Password";
            l_SqlCommand.Parameters.AddWithValue("@MembershipID", lModUser.MembershipID);
            l_SqlCommand.Parameters.AddWithValue("@Password", lModUser.Password);
            l_SqlDataReader = l_SqlCommand.ExecuteReader();

            if (l_SqlDataReader.Read())
            {
                ModUser l_ModloggedInUser = new ModUser
                {
                    ImageLocation = l_SqlDataReader["ImageLocation"] as string,
                    PKGUID = l_SqlDataReader["PKGUID"] as string,
                    MembershipID = l_SqlDataReader["MembershipID"] as string,
                    MemberName = l_SqlDataReader["MemberName"] as string,
                    Address1 = l_SqlDataReader["Address1"] as string,
                    Address2 = l_SqlDataReader["Address2"] as string,
                    Email = l_SqlDataReader["Email"] as string,
                    FounderMembers = l_SqlDataReader["FounderMembers"] as string,
                    LifeAndAnnualMembers = l_SqlDataReader["LifeAndAnnualMembers"] as string,
                    CompanysName = l_SqlDataReader["CompanysName"] as string,
                    Region = l_SqlDataReader["Region"] as string,
                    Password = lModUser.Password, // Set the password from the user input
                    Mobile = l_SqlDataReader["Mobile"] as string,
                    ContactNo = l_SqlDataReader["ContactNo"] as int?

                    
                };
                l_SqlConnection.Close();


                if (String.IsNullOrEmpty(l_ModloggedInUser.Mobile))
                {
                    return View("ErrorMobile");
                }

                int? lUserCount = l_DalInsertVoting.GetRecordCountForUser(l_ModloggedInUser.PKGUID!, l_SqlConnection.ConnectionString);

                if (lUserCount == 5)
                {

                    TempData["ErrorMessage"] = l_ModloggedInUser.MemberName;
                    return View("Index");

                }

                HttpContext.Session.Set<ModUser>("LoggedinUser", l_ModloggedInUser);

                return RedirectToAction("Index", "OTP", l_ModloggedInUser);
            }

            l_SqlConnection.Close();
            return View("ErrorPassword"); // Password is incorrect
        }

        [HttpGet]

        public ActionResult CandidateVoteInfo()
        {
            //db call

            FncConnectionString();
            DalInsertVoting l_dalInsertVoting = new DalInsertVoting();

            List<ModCandidateVoteInfo> l_ListModCandidateVoteInfo = new List<ModCandidateVoteInfo>();
            l_ListModCandidateVoteInfo = l_dalInsertVoting.GetResultOfCandidates(l_SqlConnection.ConnectionString);
			return View("CandidateVoteInfo", l_ListModCandidateVoteInfo);

		}


	}
}
