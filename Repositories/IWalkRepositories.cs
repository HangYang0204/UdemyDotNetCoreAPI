using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepositories
    {
        //Create
        Task<Walk> CreateAsync(Walk walk);

        //Get all
        Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true);

        //Get By Id
        Task<Walk?> GetByIdAsync(Guid id);

        //Update by Id
        Task<Walk?> UpdateAsync(Guid id, Walk walk);

        //Delete by ID
        Task<Walk?> DeleteAsync(Guid id);
    }
} 
