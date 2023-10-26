namespace E_VotingSystem.ConnectionString
{
    public static class ConnectionHelper
    {
        public static string FncGetConnectionString()
        {
            //return "Data Source=MUHAMMAD-UMAIR\\AISONESQL;Initial Catalog=EVoting;Integrated Security=True";
            return "Data Source = (local)\\AisoneSQL; User ID = sa; Pwd = Smc786<>; Initial Catalog = EVoting; Connection Timeout = 3000";
        }
    }

}

