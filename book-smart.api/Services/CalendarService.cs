using BookSmart.Api.Models.Requests;
using BookSmart.Api.Models.Responses;
using BookSmart.Api.Repositories;

namespace BookSmart.Api.Services;

public class CalendarService : ICalendarService
{
    private readonly ISalesManagerRepository _salesManagerRepository;
    private readonly ISlotRepository _slotRepository;

    public CalendarService(ISalesManagerRepository salesManagerRepository, ISlotRepository slotRepository)
    {
        _salesManagerRepository = salesManagerRepository;
        _slotRepository = slotRepository;
    }

    public async Task<List<CalendarQueryResponseModel>> GetAvailableSlotsAsync(CalendarQueryRequestModel request)
    {
        var matchingManagerIds =
            await _salesManagerRepository.GetMatchingSalesManagerIdsAsync(request.Language, request.Products,
                request.Rating);

        var salesManagerIds = matchingManagerIds as int[] ?? matchingManagerIds.ToArray();

        if (salesManagerIds.Length == 0)
            return [];

        return await _slotRepository.GetAvailableSlotsAsync(salesManagerIds, request.Date);
    }
}