using System.Text.Json.Serialization;

public class Book
{
    [JsonPropertyName("_id")]
    public string Id { get; set; }

    public string Name { get; set; }
}
