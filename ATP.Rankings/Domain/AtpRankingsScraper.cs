using NScrape;

namespace ATP.Rankings.Domain
{
    public class AtpRankingsScraper : Scraper
    {
        public AtpRankingsScraper(string html) : base(html)
        {
        }

        public Task<List<PlayerStats>> GetStats()

        {

            var nodes = HtmlDocument.DocumentNode.Descendants()

                .Where(n => n.Attributes.Contains("class") &&
                        (n.Attributes["class"].Value == "player-cell border-left-dash-1 border-right-dash-1" ||
                        n.Attributes["class"].Value == "rank-cell border-left-4 border-right-dash-1" ||
                        n.Attributes["class"].Value == "move-cell border-right-4 border-left-dash-1" ||
                        n.Attributes["class"].Value == "age-cell border-left-dash-1 border-right-4" ||
                        n.Attributes["class"].Value == "points-cell border-right-dash-1" ||
                        n.Attributes["class"].Value == "points-move-cell border-right-dash-1" ||
                        n.Attributes["class"].Value == "tourn-cell border-left-dash-1 border-right-dash-1"
                        ));

            List<PlayerStats> players = new List<PlayerStats>();
            int counter = 1;

            foreach (var node in nodes)
            {
                if ((counter == 1 || (counter % 7) == 1) && int.TryParse(node.InnerText.TrimStart().TrimEnd(), out int rank))
                {
                    players.Add(new PlayerStats { Rank = int.Parse(node.InnerText.TrimStart().TrimEnd()) });
                }
                else if ((counter % 7) == 2)
                {
                    players.Last().RankChange = node.InnerText.TrimEnd().TrimStart();
                }
                else if ((counter % 7) == 3)
                {
                    players.Last().Name = node.InnerText.TrimEnd().TrimStart();
                }
                else if ((counter % 7) == 4)
                {
                    players.Last().Age = int.Parse(node.InnerText.TrimEnd().TrimStart());
                }

                else if ((counter % 7) == 5)
                {
                    players.Last().Points = node.InnerText.TrimEnd().TrimStart();
                }
                else if ((counter % 7) == 6)
                {
                    players.Last().PointsChange = node.InnerText.TrimEnd().TrimStart();
                }
                else if ((counter % 7) == 0)
                {
                    players.Last().NbTournaments = int.Parse(node.InnerText.TrimEnd().TrimStart());
                }

                counter++;
            }

            if (nodes != null)
            {
                return Task.FromResult(players.ToList());
            }

            throw new ScrapeException("Could not scrape conditions.", Html);
        }

    }
}
