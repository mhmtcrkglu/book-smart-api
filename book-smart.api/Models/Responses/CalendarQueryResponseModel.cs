using System.Text.Json.Serialization;

namespace BookSmart.Api.Models.Responses;

public class CalendarQueryResponseModel
{
    [JsonPropertyName("available_count")] public int AvailableCount { get; set; }

    [JsonIgnore] public DateTime StartDate { get; set; }

    [JsonPropertyName("start_date")] public string StartDateString => StartDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
}