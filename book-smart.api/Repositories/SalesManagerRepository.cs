using System.Data;
using Dapper;

namespace BookSmart.Api.Repositories;

public class SalesManagerRepository : ISalesManagerRepository
{
    private readonly IDbConnection _dbConnection;

    public SalesManagerRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<int>> GetMatchingSalesManagerIdsAsync(string? language, List<string>? products,
        string? rating)
    {
        const string query = @"
            SELECT id
            FROM sales_managers
            WHERE @Language = ANY(languages)
              AND @Rating = ANY(customer_ratings)
              AND EXISTS (
                  SELECT 1
                  FROM unnest(products) AS product
                  WHERE product = ANY(@Products)
                  GROUP BY id
                  HAVING COUNT(DISTINCT product) = array_length(@Products, 1)
              )";

        var parameters = new
        {
            Language = language,
            Products = products?.ToArray(),
            Rating = rating
        };

        var salesManagerIds = await _dbConnection.QueryAsync<int>(query, parameters);

        return salesManagerIds.ToList();
    }
}