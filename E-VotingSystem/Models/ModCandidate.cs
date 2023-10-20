namespace E_VotingSystem.Models
{
    public class ModCandidate
    {
        public string? PKGUID { get; set; }
        public string? MemberShipNo { get; set; }
        public string? CandidateName { get; set; }
        public string? Address { get; set; }
        public string? MobileNo { get; set; }
        public string? CompanyName { get; set; }
        public string? Profile { get; set; }
        public string? ProposedBy { get; set; }
        public string? SecondBy { get; set; }
        public string? NominateFor { get; set; }
        public string? Region { get; set; }
        public string? ImageLocation { get; set; }
        public bool IsVote { get; set; }

    }
}
