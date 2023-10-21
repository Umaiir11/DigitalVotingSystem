using E_VotingSystem.Models;
using Microsoft.SqlServer.Server;
using NuGet.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IEnumerable
{
	public class IEnumerableVoter : List<ModVoter>, IEnumerable<SqlDataRecord>
	{
		public IEnumerator<SqlDataRecord> Pb_Fnc_GenerateCollection()
		{
			var sqlRow = new SqlDataRecord(
				new SqlMetaData("PKGUID", SqlDbType.NVarChar, 50),
				new SqlMetaData("UserDID", SqlDbType.NVarChar, 50),
				new SqlMetaData("CandidateDID", SqlDbType.NVarChar, 50),
				new SqlMetaData("VoteTimestamp", SqlDbType.DateTime));

			foreach (ModVoter modVoter in this)
			{
				sqlRow.SetString(0, modVoter.PKGUID);
				sqlRow.SetString(1, modVoter.UserDID);
				sqlRow.SetString(2, modVoter.CandidateDID);
				sqlRow.SetDateTime(3, modVoter.VoteTimestamp);
				yield return sqlRow;
			}

		}

		IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator() => Pb_Fnc_GenerateCollection();

	}
}
