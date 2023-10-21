using E_VotingSystem.Models;
using System.Data.SqlClient;
using System.Data;
using DAL.IEnumerable;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace E_VotingSystem.Controllers
{
    public class DalInsertVoting
    {
        
        public void InsertVoterRecord(ModVoter voter,string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string insertQuery = "INSERT INTO TBU_Voter (PKGUID, UserDID, CandidateDID, VoteTimestamp) " +
                                     "VALUES (@PKGUID, @UserDID, @CandidateDID, @VoteTimestamp)";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.Add("@PKGUID", SqlDbType.NVarChar).Value = voter.PKGUID;
                    command.Parameters.Add("@UserDID", SqlDbType.NVarChar).Value = voter.UserDID;
                    command.Parameters.Add("@CandidateDID", SqlDbType.NVarChar).Value = voter.CandidateDID;
                    command.Parameters.Add("@VoteTimestamp", SqlDbType.DateTime).Value = voter.VoteTimestamp;

                    command.ExecuteNonQuery();
                }
            }
        }
        public int GetRecordCountForUser(string userDID, string connectionString)
        {
            int recordCount = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("GetRecordCountForUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@UserDID", SqlDbType.VarChar, 50));
                    command.Parameters["@UserDID"].Value = userDID;

                    connection.Open();
                    var result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        recordCount = Convert.ToInt32(result);
                    }
                }
            }

            return recordCount;
        }

		public void InsertModVotersList(List<ModVoter> modVoterList, string sqlConnection)
		{
			var modVoterEnumerable = new IEnumerableVoter(); // Create an instance of IEnumerableItem

			foreach (ModVoter modVoter in modVoterList)
			{
				modVoterEnumerable.Add(modVoter);
			}

            using (SqlConnection connection = new SqlConnection(sqlConnection))
            {
				connection.Open(); // Open the connection


				using (SqlCommand sqlCommand = new SqlCommand("Pr_TBU_Voter_CRUDm", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    sqlCommand.Parameters.Add("@mDTTBU_VoterType", SqlDbType.Structured);
                    sqlCommand.Parameters["@mDTTBU_VoterType"].Value = modVoterEnumerable;
                    sqlCommand.Parameters["@mDTTBU_VoterType"].Direction = ParameterDirection.Input;

                    sqlCommand.ExecuteNonQuery();
                }
            }
		}

	}

}


