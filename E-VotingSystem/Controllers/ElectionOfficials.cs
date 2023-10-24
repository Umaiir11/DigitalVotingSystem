using E_VotingSystem.ConnectionString;
using E_VotingSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace E_VotingSystem.Controllers
{
	public class ElectionOfficials : Controller
	{

		SqlConnection l_SqlConnection = new SqlConnection();
		SqlCommand l_SqlCommand = new SqlCommand();
		SqlDataReader? l_SqlDataReader;
		public IActionResult Index()
		{
			return View();
		}

		void FncConnectionString()
		{
            l_SqlConnection.ConnectionString = ConnectionHelper.FncGetConnectionString();
        }

		[HttpPost]
		public ActionResult Verify(ModElectionOfficials lModElectionOfficial)
		{
			DalInsertVoting l_DalInsertVoting = new DalInsertVoting();
			FncConnectionString();

			l_SqlConnection.Open();
			l_SqlCommand.Connection = l_SqlConnection;
			l_SqlCommand.CommandText = "SELECT * FROM TBU_ElectionOfficials WHERE ID = @ID AND Password = @Password";
			l_SqlCommand.Parameters.AddWithValue("@ID", lModElectionOfficial.ID);
			l_SqlCommand.Parameters.AddWithValue("@Password", lModElectionOfficial.Password);
			l_SqlDataReader = l_SqlCommand.ExecuteReader();

			if (l_SqlDataReader.Read())
			{
				l_SqlConnection.Close();

				// This means the user is a valid election official.
				return RedirectToAction("CandidateVoteInfo");
			}

			l_SqlConnection.Close();
			return View("ErrorPassword"); // Password is incorrect or the user is not an election official
		}

		[HttpGet]
		public ActionResult CandidateVoteInfo()
		{
			//db call

			FncConnectionString();
			DalInsertVoting l_dalInsertVoting = new DalInsertVoting();

			List<ModCandidateVoteInfo> l_ListModCandidateVoteInfo = new List<ModCandidateVoteInfo>();
			l_ListModCandidateVoteInfo = l_dalInsertVoting.FncGetResultOfCandidates(l_SqlConnection.ConnectionString);
			return View("CandidateVoteInfo", l_ListModCandidateVoteInfo);

		}

	}
}
