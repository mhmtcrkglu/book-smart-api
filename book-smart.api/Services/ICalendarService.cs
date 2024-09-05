using BookSmart.Api.Models.Requests;
using BookSmart.Api.Models.Responses;

namespace BookSmart.Api.Services;

public interface ICalendarService
{
    Task<List<CalendarQueryResponseModel>> GetAvailableSlotsAsync(CalendarQueryRequestModel request);
}