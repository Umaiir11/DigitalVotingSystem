using E_VotingSystem.Models;
using Microsoft.AspNetCore.Mvc;

using System.Data.SqlClient;

namespace E_VotingSystem.Controllers
{
    public class ProfileController : Controller
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
            l_SqlConnection.ConnectionString = "Data Source=MUHAMMAD-UMAIR\\AISONESQL;Initial Catalog=EVoting;Integrated Security=True";
        }

        [HttpPost]
       
        public IActionResult CastVote()
        {
            // Your logic for casting a vote goes here if needed.

            ModUser l_ModLoggedInUser = new ModUser();
            l_ModLoggedInUser = HttpContext.Session.Get<ModUser>("LoggedinUser")!;
            return View("category", l_ModLoggedInUser);
        }


        [HttpPost]
        public IActionResult Logout()
        {
            // Perform any necessary logout actions here, such as clearing user data or session.

            // Redirect to the "Index" action of the "Account" controller
            return RedirectToAction("Index", "Account");
        }



        [HttpGet]

        public IActionResult Executive()
        {
            ModUser l_ModLoggedInUser = HttpContext.Session.Get<ModUser>("LoggedinUser")!;
            List<ModCandidate> list_ModCandidate = new List<ModCandidate>();

            FncConnectionString();
            l_SqlConnection.Open();
            l_SqlCommand.Connection = l_SqlConnection;
            l_SqlCommand.CommandText = "SELECT * FROM TBU_Candidate WHERE Region = @Region AND NominateFor = 'Executive Council'";
            l_SqlCommand.Parameters.AddWithValue("@Region", l_ModLoggedInUser.Region); l_SqlDataReader = l_SqlCommand.ExecuteReader();

            while (l_SqlDataReader.Read())
            {
                ModCandidate l_ModCandidate = new ModCandidate
                {
                    PKGUID = l_SqlDataReader["PKGUID"] as string,
                    MemberShipNo = l_SqlDataReader["MemberShipNo"] as string,
                    CandidateName = l_SqlDataReader["CandidateName"] as string,
                    Address = l_SqlDataReader["Address"] as string,
                    MobileNo = l_SqlDataReader["MobileNo"] as string,
                    CompanyName = l_SqlDataReader["CompanyName"] as string,
                    Profile = l_SqlDataReader["Profile"] as string,
                    ProposedBy = l_SqlDataReader["ProposedBy"] as string,
                    SecondBy = l_SqlDataReader["SecondBy"] as string,
                    NominateFor = l_SqlDataReader["NominateFor"] as string,
                    Region = l_SqlDataReader["Region"] as string,
                    ImageLocation = l_SqlDataReader["ImageLocation"] as string
                };
                list_ModCandidate.Add(l_ModCandidate);
            }

            l_SqlConnection.Close();

            HttpContext.Session.Set<List<ModCandidate>>("Candidates", list_ModCandidate);
            return View("Executive", list_ModCandidate);
        }


        [HttpGet]
        public IActionResult Local()
        {
            ModUser l_ModLoggedInUser = HttpContext.Session.Get<ModUser>("LoggedinUser")!;
            List<ModCandidate> list_ModLoaclCandidate = new List<ModCandidate>();

            FncConnectionString();
            l_SqlConnection.Open();
            l_SqlCommand.Connection = l_SqlConnection;
            l_SqlCommand.CommandText = "SELECT * FROM TBU_Candidate WHERE Region = @Region AND NominateFor = 'Local Coucil'";
            l_SqlCommand.Parameters.AddWithValue("@Region", l_ModLoggedInUser.Region);
            l_SqlDataReader = l_SqlCommand.ExecuteReader();

            while (l_SqlDataReader.Read())
            {
                ModCandidate l_ModCandidate = new ModCandidate
                {
                    PKGUID = l_SqlDataReader["PKGUID"] as string,
                    MemberShipNo = l_SqlDataReader["MemberShipNo"] as string,
                    CandidateName = l_SqlDataReader["CandidateName"] as string,
                    Address = l_SqlDataReader["Address"] as string,
                    MobileNo = l_SqlDataReader["MobileNo"] as string,
                    CompanyName = l_SqlDataReader["CompanyName"] as string,
                    Profile = l_SqlDataReader["Profile"] as string,
                    ProposedBy = l_SqlDataReader["ProposedBy"] as string,
                    SecondBy = l_SqlDataReader["SecondBy"] as string,
                    NominateFor = l_SqlDataReader["NominateFor"] as string,
                    Region = l_SqlDataReader["Region"] as string,
                    ImageLocation = l_SqlDataReader["ImageLocation"] as string
                };
                list_ModLoaclCandidate.Add(l_ModCandidate);
            }

            l_SqlConnection.Close();

            HttpContext.Session.Set<List<ModCandidate>>("LocalCandidates", list_ModLoaclCandidate);
            return View("Local", list_ModLoaclCandidate);
        }

    }
}
