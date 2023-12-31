﻿
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
            l_SqlConnection.ConnectionString = new CmConnectionHelper().FncGetConnectionString();

        }

        [HttpPost]
        public ActionResult Verify(ModUser lModUser)
        {
            try
            {

                DalInsertVoting l_DalInsertVoting = new DalInsertVoting();
                FncConnectionString();
                l_SqlConnection.Open();
                l_SqlCommand.Connection = l_SqlConnection;
                l_SqlCommand.CommandText = "SELECT * FROM Vw_TBU_Members WHERE MembershipID = @MembershipID AND Password = @Password";
                l_SqlCommand.Parameters.AddWithValue("@MembershipID", lModUser.MembershipID);
                l_SqlCommand.Parameters.AddWithValue("@Password", lModUser.Password);
                l_SqlDataReader = l_SqlCommand.ExecuteReader();

                if (l_SqlDataReader.Read())
                {
                    ModUser l_ModloggedInUser = new ModUser
                    {
                        ImageLocation = l_SqlDataReader["ImageLocation"] as string,
                        ExRegionSeats = l_SqlDataReader["ExCouncilSeats"] as string,
                        LcRegionSeats = l_SqlDataReader["LcCouncilSeats"] as string,
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

                    int? lUserCount = l_DalInsertVoting.FncGetRecordCountForUser(l_ModloggedInUser.PKGUID!, l_SqlConnection.ConnectionString);

                    int l_ExSeatsCount = int.Parse(l_ModloggedInUser.ExRegionSeats ?? "0");
                    int l_LcSeatsCount = int.Parse(l_ModloggedInUser.LcRegionSeats ?? "0");
                    int? TotalVotes = l_ExSeatsCount + l_LcSeatsCount;


                    if (lUserCount > TotalVotes)
                    {

                        TempData["ErrorMessage"] = l_ModloggedInUser.MemberName;
                        return View("Index");

                    }

                    HttpContext.Session.Set<ModUser>("LoggedinUser", l_ModloggedInUser);

                    //return RedirectToAction("Index", "OTP", l_ModloggedInUser);
                    return RedirectToAction("Index", "Profile");
                }

                l_SqlConnection.Close();
                return View("ErrorPassword"); // Password is incorrect
            }
            catch (Exception ex)
            {
                new CmConnectionHelper().WriteToFile(ex.Message);
                TempData["ErrorMessage"] = ex.Message;
                return View("Index");
                
            }
        }

      
    }
}
