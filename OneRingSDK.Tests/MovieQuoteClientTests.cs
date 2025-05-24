using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Moq;
using Moq.Protected;
using MovieQuoteSdk.Clients;

namespace MovieQuoteSdk.Tests
{
    public class MovieQuoteClientTests
    {
        private const string BaseUrl = "https://the-one-api.dev/v2/";
        private const string AuthToken = "test-token";

        private MovieQuoteClient CreateMockedClient<T>(string path, T responseObject)
        {
            var json = JsonSerializer.Serialize(responseObject);
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri!.AbsolutePath.EndsWith(path)),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() =>
                {
                    var response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(json)
                    };
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return response;
                });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri(BaseUrl)
            };

            return new MovieQuoteClient(httpClient, AuthToken);
        }

        private MovieQuoteClient CreateMockedClient(Dictionary<string, object> mockResponses)
        {
            var handlerMock = new Mock<HttpMessageHandler>();

            foreach (var entry in mockResponses)
            {
                var json = JsonSerializer.Serialize(entry.Value);
                handlerMock.Protected()
                    .Setup<Task<HttpResponseMessage>>("SendAsync",
                        ItExpr.Is<HttpRequestMessage>(req =>
                            req.RequestUri != null &&
                            req.RequestUri.AbsolutePath.Contains(entry.Key, StringComparison.OrdinalIgnoreCase)
                        ),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(() =>
                    {
                        var response = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new StringContent(json)
                        };
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        return response;
                    });
            }

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri(BaseUrl)
            };

            return new MovieQuoteClient(httpClient, AuthToken);
        }

        [Fact]
        public async Task GetMoviesAsync_ReturnsMovies()
        {
            var mockResponse = new { docs = new[] { new { _id = "1", name = "The Fellowship of the Ring" } } };
            var client = CreateMockedClient("/movie", mockResponse);
            var result = await client.GetMoviesAsync();
            Assert.Single(result);
        }

        [Fact]
        public async Task GetMovieByIdAsync_ReturnsMovie()
        {
            var mockResponse = new { _id = "1", name = "The Fellowship of the Ring" };
            var client = CreateMockedClient("/movie/1", mockResponse);
            var result = await client.GetMovieByIdAsync("1");
            Assert.Equal("The Fellowship of the Ring", result.Name);
        }

        [Fact]
        public async Task GetQuotesForMovieAsync_ReturnsQuotes()
        {
            var mockResponse = new { docs = new[] { new { _id = "1", dialog = "Even the smallest person can change the course of the future." } } };
            var client = CreateMockedClient("/movie/1/quote", mockResponse);
            var result = await client.GetQuotesForMovieAsync("1");
            Assert.Single(result);
        }

        [Fact]
        public async Task GetQuotesAsync_ReturnsQuotes()
        {
            var mockResponse = new { docs = new[] { new { _id = "1", dialog = "A wizard is never late." } } };
            var client = CreateMockedClient("/quote", mockResponse);
            var result = await client.GetQuotesAsync();
            Assert.Single(result);
        }

        [Fact]
        public async Task GetQuoteByIdAsync_ReturnsQuote()
        {
            var mockResponse = new { _id = "1", dialog = "Fly, you fools!" };
            var client = CreateMockedClient("/quote/1", mockResponse);
            var result = await client.GetQuoteByIdAsync("1");
            Assert.Equal("Fly, you fools!", result.Dialog);
        }

        [Fact]
        public async Task GetAllMovieQuotesAsync_ReturnsCombinedQuotes()
        {
            var mockResponses = new Dictionary<string, object>
            {
                ["/movie"] = new
                {
                    docs = new[]
                    {
                        new { _id = "1", name = "The Fellowship of the Ring" },
                        new { _id = "2", name = "The Two Towers" }
                    }
                },
                ["/movie/1/quote"] = new
                {
                    docs = new[]
                    {
                        new { _id = "q1", dialog = "Quote from Fellowship" }
                    }
                },
                ["/movie/2/quote"] = new
                {
                    docs = new[]
                    {
                        new { _id = "q2", dialog = "Quote from Two Towers" }
                    }
                }
            };

            var client = CreateMockedClient(mockResponses);
            var allQuotes = await client.GetAllMovieQuotesAsync();

            Assert.Equal(2, allQuotes.Count());
            Assert.Contains(allQuotes, q => q.Dialog.Contains("Fellowship"));
            Assert.Contains(allQuotes, q => q.Dialog.Contains("Two Towers"));
        }

        [Fact]
        public async Task GetAllBookChaptersAsync_ReturnsCombinedChapters()
        {
            var mockResponses = new Dictionary<string, object>
            {
                ["/v2/book"] = new
                {
                    docs = new[]
                    {
                        new { _id = "b1", name = "The Fellowship of the Ring" },
                        new { _id = "b2", name = "The Two Towers" }
                    }
                },
                ["/v2/book/b1/chapter"] = new
                {
                    docs = new[]
                    {
                        new { _id = "c1", chapterName = "A Long-expected Party" }
                    }
                },
                ["/v2/book/b2/chapter"] = new
                {
                    docs = new[]
                    {
                        new { _id = "c2", chapterName = "The Departure of Boromir" }
                    }
                }
            };

            var client = CreateMockedClient(mockResponses);
            var allChapters = await client.GetAllBookChaptersAsync();

            Assert.Equal(2, allChapters.Count());
            Assert.Contains(allChapters, c => c.ChapterName.Contains("Long-expected"));
            Assert.Contains(allChapters, c => c.ChapterName.Contains("Boromir"));
        }
    }
}
