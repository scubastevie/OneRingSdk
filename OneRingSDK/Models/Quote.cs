using System.Text.Json.Serialization;

public class Quote
{
    [JsonPropertyName("_id")]
    public string Id { get; set; }
    public string Dialog { get; set; }
    public string Movie { get; set; }
    public string Character { get; set; }
}
