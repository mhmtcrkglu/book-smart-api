using System.ComponentModel.DataAnnotations;

namespace BookSmart.Api.Models.Requests;

public class CalendarQueryRequestModel
{
    [Required] public DateTime Date { get; set; }

    [Required] public List<string> Products { get; set; }

    [Required] public string Language { get; set; }

    [Required] public string Rating { get; set; }
}