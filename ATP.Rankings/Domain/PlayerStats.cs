namespace ATP.Rankings.Domain
{
    public class PlayerStats
    {
        public string Name { get; set; }
        public int Rank { get; set; }
        public string RankChange { get; set; }
        public int Age { get; set; }
        public string Country { get; set; }
        public string Points { get; set; }
        public string PointsChange { get; set; }
        public int NbTournaments { get; set; }
    }
}