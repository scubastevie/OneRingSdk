using Microsoft.AspNetCore.Mvc;
using MovieQuoteSdk.Clients;

namespace OneRing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieQuotesController : ControllerBase
    {
        private readonly MovieQuoteClient _client;

        public MovieQuotesController(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            var httpClient = httpClientFactory.CreateClient("OneRingApi");
            var token = config["OneRingApiKey"] ?? throw new ArgumentNullException("API key not configured");
            _client = new MovieQuoteClient(httpClient, token);
        }

        [HttpGet("movies")]
        public async Task<IActionResult> GetMovies()
        {
            var movies = await _client.GetMoviesAsync();
            return Ok(movies);
        }

        [HttpGet("moviequotes")]
        public async Task<IActionResult> GetAllMovieQuotes()
        {
            var quotes = await _client.GetAllMovieQuotesAsync();
            return Ok(quotes);
        }

        [HttpGet("books")]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _client.GetBooksAsync();
            return Ok(books);
        }

        [HttpGet("bookchapters")]
        public async Task<IActionResult> GetAllBookChapters()
        {
            var chapters = await _client.GetAllBookChaptersAsync();
            return Ok(chapters);
        }
    }
}
