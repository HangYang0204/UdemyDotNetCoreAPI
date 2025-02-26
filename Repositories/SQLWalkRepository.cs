using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepositories
    {
        private readonly NZWalksDbContext dbContext;

        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            //Get by ID
            var myWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            //Verify
            if (myWalk == null)
                return null;

            //Delete
            dbContext.Walks.Remove(myWalk);

            //save changes to db
            await dbContext.SaveChangesAsync();

            //return removed walk
            return myWalk;
        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null)
        {
            //Using Queryable
            var walks = dbContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .AsQueryable();

            //Filtering
            if(!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                //check which column to filter
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
            }
            return await walks.ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            //Get by Id
            var mywalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            //verify
            if (mywalk == null)
                return null;

            //Update
            mywalk.Name = walk.Name;
            mywalk.Description = walk.Description;
            mywalk.LengthInKm = walk.LengthInKm;
            mywalk.WalkImageUrl = walk.WalkImageUrl;
            mywalk.DifficultyId = walk.DifficultyId;
            mywalk.RegionId = walk.RegionId;

            //Save to DB
            await dbContext.SaveChangesAsync();

            return (mywalk);
        }
    }
}
