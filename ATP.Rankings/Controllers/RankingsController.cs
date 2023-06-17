using ATP.Rankings.Domain;
using ATP.Rankings.Parameters;
using Microsoft.AspNetCore.Mvc;
using NScrape;

namespace ATP.Rankings.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class RankingsController : ControllerBase
    {
        private readonly ILogger<RankingsController> _logger;

        public RankingsController(ILogger<RankingsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("rankings")]
        [ProducesResponseType(typeof(List<PlayerStats>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRankings()
        {
            var webClient = new WebClient();
            var response = webClient.SendRequest(new Uri("https://www.atptour.com/en/rankings/singles"));

            if (response.ResponseType == WebResponseType.Html)
            {
                var scraper = new AtpRankingsScraper(((HtmlWebResponse)response).Html);
                var res = await scraper.GetStats();

                return Ok(res);

            }

            return NotFound();

        }



        [HttpGet]
        [Route("rankings/paged")]
        [ProducesResponseType(typeof(List<PlayerStats>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPagedRankings([FromBody] GetRankedPlayersParameters parameters)
        {
            var webClient = new WebClient();
            var response = webClient.SendRequest(new Uri($"https://www.atptour.com/en/rankings/singles?rankRange={parameters.RankStart}-{parameters.RankEnd}"));

            if (response.ResponseType == WebResponseType.Html)
            {
                var scraper = new AtpRankingsScraper(((HtmlWebResponse)response).Html);
                var res = await scraper.GetStats();

                return Ok(res);

            }

            return NotFound();

        }
    }
}