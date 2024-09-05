using System.Data;
using BookSmart.Api.Models.Responses;
using Dapper;

namespace BookSmart.Api.Repositories;

public class SlotRepository : ISlotRepository
{
    private readonly IDbConnection _dbConnection;

    public SlotRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<List<CalendarQueryResponseModel>> GetAvailableSlotsAsync(IEnumerable<int> salesManagerIds,
        DateTime date)
    {
        const string query = @"
            SELECT s.start_date AS StartDate, COUNT(*) AS AvailableCount
            FROM slots s
            LEFT JOIN slots s2 
                ON s.sales_manager_id = s2.sales_manager_id
                AND s2.booked = TRUE
                AND s.start_date < s2.end_date
                AND s2.start_date < s.end_date
            WHERE s.sales_manager_id = ANY(@SalesManagerIds)
              AND s.booked = FALSE
              AND s.start_date::date = @Date
              AND s2.sales_manager_id IS NULL
            GROUP BY s.start_date
            ORDER BY s.start_date;";

        return (List<CalendarQueryResponseModel>)await _dbConnection.QueryAsync<CalendarQueryResponseModel>(query, new
        {
            SalesManagerIds = salesManagerIds,
            Date = date
        });
    }
}