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
            l_SqlConnection.ConnectionString = new  CmConnectionHelper().FncGetConnectionString();
        }

        [HttpPost]

        public IActionResult Category()
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


        [HttpPost]
  
        public IActionResult CastVote(List<ModCandidate> l_ListModCandidates)
        {
            DalInsertVoting l_DalInsertVoting = new DalInsertVoting();
            List<ModVoter> l_ListVoterMode = new List<ModVoter>();
            ModUser l_ModLoggedInUser = HttpContext.Session.Get<ModUser>("LoggedinUser")!;

            // Validate l_ModLoggedInUser.LcRegionSeats before parsing
            if (!int.TryParse(l_ModLoggedInUser.LcRegionSeats, out int l_LocalRegionSeats))
            {
                // Handle the case where l_ModLoggedInUser.LcRegionSeats is not a valid integer
                // You can set an error message in TempData and return to the view
                TempData["ErrorMessage"] = "Invalid Region Seats";
                return RedirectToAction("Local");
            }

            l_ListModCandidates = l_ListModCandidates.Where(x => x.IsVote).ToList();

            for (int i = 0; i < l_ListModCandidates.Count; i++)
            {
                ModVoter lModVoter = new ModVoter
                {
                    CandidateDID = l_ListModCandidates[i].PKGUID,
                    UserDID = l_ModLoggedInUser.PKGUID,
                    isVote = l_ListModCandidates[i].IsVote,
                    VoteTimestamp = DateTime.Now,
                    PKGUID = Guid.NewGuid().ToString()
                };
                l_ListVoterMode.Add(lModVoter);
            }

            if (l_ListVoterMode.Count == l_LocalRegionSeats)
            {
                FncConnectionString();
                int? lUserCount = l_DalInsertVoting.FncGetRecordCountForUser(l_ModLoggedInUser.PKGUID!, l_SqlConnection.ConnectionString);

                if (lUserCount == l_LocalRegionSeats)
                {
                    TempData["ErrorMessage"] = $"{l_ModLoggedInUser.MemberName} have to select at least {l_LocalRegionSeats} candidates.";
                    TempData["SeatsCount"] = l_LocalRegionSeats;

                    return View("Local");
                }

                FncConnectionString();

                l_DalInsertVoting.FncInsertModVotersList(l_ListVoterMode, l_SqlConnection.ConnectionString);

                return View("votescasted");
            }
            else
            {
                TempData["ErrorMessage"] = $"{l_ModLoggedInUser.MemberName} have to select at least {l_LocalRegionSeats} candidates.";
                return RedirectToAction("Local");
            }
        }


        [HttpPost]
        public IActionResult CastVoteEx(List<ModCandidate> l_ListModCandidates)
        {
            List<ModVoter> l_ListVoterMode = new List<ModVoter>();
            ModUser l_ModLoggedInUser = HttpContext.Session.Get<ModUser>("LoggedinUser")!;
            l_ListModCandidates = l_ListModCandidates.Where(x => x.IsVote).ToList();

            DalInsertVoting l_DalInsertVoting = new DalInsertVoting();

            // Validate l_ModLoggedInUser.ExRegionSeats before parsing
            if (!int.TryParse(l_ModLoggedInUser.ExRegionSeats, out int l_ExecutiveRegionSeats))
            {
                // Handle the case where l_ModLoggedInUser.ExRegionSeats is not a valid integer
                // You can set an error message in TempData and return to the view
                TempData["ErrorMessage"] = "Invalid Executive Region Seats";
                return RedirectToAction("Executive");
            }

            for (int i = 0; i < l_ListModCandidates.Count; i++)
            {
                ModVoter lModVoter = new ModVoter
                {
                    CandidateDID = l_ListModCandidates[i].PKGUID,
                    UserDID = l_ModLoggedInUser.PKGUID,
                    isVote = l_ListModCandidates[i].IsVote,
                    VoteTimestamp = DateTime.Now,
                    PKGUID = Guid.NewGuid().ToString()
                };
                l_ListVoterMode.Add(lModVoter);
            }

            if (l_ListVoterMode.Count == l_ExecutiveRegionSeats)
            {
                FncConnectionString();
                l_SqlConnection.Open();
                l_SqlCommand.Connection = l_SqlConnection;

                int? lUserCount = l_DalInsertVoting.FncGetRecordCountForUser(l_ModLoggedInUser.PKGUID!, l_SqlConnection.ConnectionString);

                if (lUserCount == l_ExecutiveRegionSeats)
                {
                    TempData["ErrorMessage"] = $"{l_ModLoggedInUser.MemberName} have to select at least {l_ExecutiveRegionSeats} candidates.";
                    TempData["SeatsCount"] = l_ExecutiveRegionSeats;
                    return View("Executive");
                }

                FncConnectionString();

                l_DalInsertVoting.FncInsertModVotersList(l_ListVoterMode, l_SqlConnection.ConnectionString);
                return View("votescasted");
            }
            else
            {
                TempData["ErrorMessage"] = $"{l_ModLoggedInUser.MemberName} have to select at least {l_ExecutiveRegionSeats} candidates.";
                return RedirectToAction("Executive");
            }
        }
        [HttpGet]

        public IActionResult Executive()
        {
            ModUser l_ModLoggedInUser = HttpContext.Session.Get<ModUser>("LoggedinUser")!;
            List<ModCandidate> list_ModCandidate = new List<ModCandidate>();
            FncConnectionString();
            DalInsertVoting l_DalInsertVoting = new DalInsertVoting();

            //if (l_ModLoggedInUser== null)
            //{
            //    return RedirectToAction("Index","Account");

            //}

            int? lUserCount = l_DalInsertVoting.FncGetRecordCountForUser(l_ModLoggedInUser.PKGUID!, l_SqlConnection.ConnectionString);

            if (lUserCount == 1)
            {

                TempData["ErrorMessage"] = l_ModLoggedInUser.MemberName;
                return View("Executive");

            }


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
            //if (l_ModLoggedInUser== null)
            //{
            //    return RedirectToAction("Index","Account");

            //}
            List<ModCandidate> list_ModLoaclCandidate = new List<ModCandidate>();
            DalInsertVoting l_DalInsertVoting = new DalInsertVoting();
            FncConnectionString();
            int? lUserCount = l_DalInsertVoting.FncGetRecordCountForUser(l_ModLoggedInUser.PKGUID!, l_SqlConnection.ConnectionString);

            if (lUserCount == 5)
            {

                TempData["ErrorMessage"] = l_ModLoggedInUser.MemberName;
                return View("Local");

            }

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
