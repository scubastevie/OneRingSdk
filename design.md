## Design Decisions

### .NET Standard 2.1
- Chosen to support both modern .NET Core and older frameworks like Xamarin or legacy ASP.NET Core 3.1 apps.

### Authentication
- Requires a Bearer token for every request, passed via constructor to enforce early validation.

### JSON Compatibility
- Uses `System.Text.Json` with `[JsonPropertyName]` to bind fields like `_id` correctly.
- Handles JSON parsing edge cases (e.g. `boxOfficeRevenueInMillions` as `double` instead of `int`).

### Extensibility
- The SDK is intentionally thin and focused.
- Future expansion could include:
  - Filtering or pagination support
  - Character and quote search
  - Retry policies or caching with `IHttpClientFactory`

## Room for Growth

- Implement logging or diagnostics
- Support filtering/sorting via query strings
- Add throttling or rate-limit awareness
- Include partial failure handling in aggregation methods
