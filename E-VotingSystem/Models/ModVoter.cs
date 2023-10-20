namespace E_VotingSystem.Models
{
    public class ModVoter
    {
        public string? PKGUID { get; set; }
        public string? UserDID { get; set; }
        public string? CandidateDID { get; set; }
        public DateTime VoteTimestamp { get; set; }
        public bool? isVote { get; set; }
    }
}


