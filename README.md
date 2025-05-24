# OneRingSdk

A C#/.NET SDK for [The One API](https://the-one-api.dev), which provides data for The Lord of the Rings and The Hobbit movie trilogies.

This SDK enables developers to easily retrieve data about movies, quotes, books, and chapters using strongly typed models and async-friendly methods.

---

## ✨ Features

- Retrieve all movies or a specific movie by ID
- Fetch quotes globally or for a specific movie
- Aggregate all quotes across all movies
- List all books and their chapters
- Designed for .NET Standard 2.1 for broad compatibility

---

## 📦 Installation
Run "dotnet build -c Release" on the sdk solution
This will create the sdk nuget package

then on the new project run 
"dotnet add package OneRingSdk --source C:\Users\Steve\source\repos\OneRingSDK\OneRingSDK\bin\Release"
This will add the nuget package.

## Usage
using MovieQuoteSdk.Clients;

var httpClient = new HttpClient();
var client = new MovieQuoteClient(httpClient, "YOUR_API_KEY");

// Get all movies
var movies = await client.GetMoviesAsync();

// Get quotes from all movies
var allQuotes = await client.GetAllMovieQuotesAsync();

// Get all chapters from all books
var chapters = await client.GetAllBookChaptersAsync();

