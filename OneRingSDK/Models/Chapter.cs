using System.Text.Json.Serialization;

public class Chapter
{
    [JsonPropertyName("_id")]
    public string Id { get; set; }

    public string ChapterName { get; set; }
}
