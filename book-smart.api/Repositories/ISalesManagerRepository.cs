namespace BookSmart.Api.Repositories;

public interface ISalesManagerRepository
{
    Task<IEnumerable<int>> GetMatchingSalesManagerIdsAsync(string? language, List<string>? products, string? rating);
}