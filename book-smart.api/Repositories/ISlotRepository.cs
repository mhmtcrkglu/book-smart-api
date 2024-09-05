using BookSmart.Api.Models.Responses;

namespace BookSmart.Api.Repositories;

public interface ISlotRepository
{
    Task<List<CalendarQueryResponseModel>> GetAvailableSlotsAsync(IEnumerable<int> salesManagerIds, DateTime date);
}