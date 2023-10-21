using E_VotingSystem.Models;
using System.Data.SqlClient;
using System.Data;
using DAL.IEnumerable;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace E_VotingSystem.Controllers
{
    public class DalInsertVoting
    {

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

        public List<ModCandidateVoteInfo> GetResultOfCandidates(string connectionString)
        {
            List<ModCandidateVoteInfo> candidateResults = new List<ModCandidateVoteInfo>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("ResultOfCandidates", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ModCandidateVoteInfo candidate = new ModCandidateVoteInfo
                        {
                            AutoID = reader["AutoID"] as int?,
                            CandidateName = reader["CandidateName"] as string,
                            VoteCount = reader["VoteCount"] as int?,
                            ImageLocation = reader["ImageLocation"] as string,
                            MemberShipNo = reader["MemberShipNo"] as string
                        };

                        candidateResults.Add(candidate);
                    }
                }
            }

            return candidateResults;
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


