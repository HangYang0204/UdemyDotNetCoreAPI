using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepositories
    {
        //Create
        Task<Walk> CreateAsync(Walk walk);

        //Get all
        Task<List<Walk>> GetAllAsync();
    }
} 
