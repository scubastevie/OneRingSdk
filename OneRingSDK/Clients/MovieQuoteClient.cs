using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MovieQuoteSdk.Clients
{
    public class MovieQuoteClient
    {
        private readonly HttpClient _httpClient;

        public MovieQuoteClient(HttpClient httpClient, string authToken)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.BaseAddress ??= new Uri("https://the-one-api.dev/v2/");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }

        public async Task<IEnumerable<Movie>> GetMoviesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<Movie>>("movie");
            return response?.Docs ?? new List<Movie>();
        }

        public async Task<Movie> GetMovieByIdAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<Movie>($"movie/{id}");
        }

        public async Task<IEnumerable<Quote>> GetQuotesForMovieAsync(string movieId)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<Quote>>($"movie/{movieId}/quote");
            return response?.Docs ?? new List<Quote>();
        }

        public async Task<IEnumerable<Quote>> GetQuotesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<Quote>>("quote");
            return response?.Docs ?? new List<Quote>();
        }

        public async Task<Quote> GetQuoteByIdAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<Quote>($"quote/{id}");
        }

        public async Task<IEnumerable<Quote>> GetAllMovieQuotesAsync()
        {
            var allQuotes = new List<Quote>();
            var movies = await GetMoviesAsync();

            foreach (var movie in movies)
            {
                var quotes = await GetQuotesForMovieAsync(movie.Id);
                allQuotes.AddRange(quotes);
            }

            return allQuotes;
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<Book>>("book");
            return response?.Docs ?? new List<Book>();
        }

        public async Task<IEnumerable<Chapter>> GetChaptersForBookAsync(string bookId)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<Chapter>>($"book/{bookId}/chapter");
            return response?.Docs ?? new List<Chapter>();
        }

        public async Task<IEnumerable<Chapter>> GetAllBookChaptersAsync()
        {
            var allChapters = new List<Chapter>();
            var books = await GetBooksAsync();

            foreach (var book in books)
            {
                var chapters = await GetChaptersForBookAsync(book.Id);
                allChapters.AddRange(chapters);
            }

            return allChapters;
        }
    }
}
