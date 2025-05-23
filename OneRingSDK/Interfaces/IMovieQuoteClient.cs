using System.Collections.Generic;
using System.Threading.Tasks;

public interface IMovieQuoteClient
{
    Task<IEnumerable<Movie>> GetMoviesAsync();
    Task<Movie> GetMovieAsync(string id);
    Task<IEnumerable<Quote>> GetMovieQuotesAsync(string movieId);
    Task<IEnumerable<Quote>> GetQuotesAsync();
    Task<Quote> GetQuoteAsync(string id);
}
