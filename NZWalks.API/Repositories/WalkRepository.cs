using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext context;
        public WalkRepository(NZWalksDbContext _context)
        {
            context= _context;
        }


        public async Task<Walk> CreateAsync(Walk walk)
        {
            await context.Walks.AddAsync(walk);
            await context.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var walkToDelete = await context.Walks
                .FirstOrDefaultAsync(w => w.Id == id);
            if (walkToDelete == null)
            {
                return null;
            }
             context.Walks.Remove(walkToDelete);
            await context.SaveChangesAsync();
            return walkToDelete;
        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
                                                  string? sortBy = null, bool? isAscending = true,
                                                  int pageNumber = 1, int pageSize = 1000)
        {
            var walks =  context.Walks
             .Include("Region")
             .Include("Difficulty");
            //Filter data based on filterOn and filterQuery
            if (filterOn != null && filterQuery != null)
            {
                switch(filterOn.ToLower())
                {
                    case "name":
                        walks = walks.Where(w => w.Name.ToLower().Contains(filterQuery.ToLower()));
                        break;
                    case "region":
                        walks = walks.Where(w => w.Region.Name.ToLower().Contains(filterQuery.ToLower()));
                        break;
                    case "difficulty":
                        walks = walks.Where(w => w.Difficulty.Name.ToLower().Contains(filterQuery.ToLower()));
                        break;
                }
            }
            //Order data based on sortBy and isAscending
            if( string.IsNullOrEmpty(sortBy) == false )
            {
                switch(sortBy.ToLower())
                {
                    case "name":
                        walks = isAscending == true ? walks.OrderBy(w => w.Name) : walks.OrderByDescending(w => w.Name);
                        break;
                    case "region":
                        walks = isAscending == true ? walks.OrderBy(w => w.Region.Name) : walks.OrderByDescending(w => w.Region.Name);
                        break;
                    case "difficulty":
                        walks = isAscending == true ? walks.OrderBy(w => w.Difficulty.Name) : walks.OrderByDescending(w => w.Difficulty.Name);
                        break;
                    case "lengthinkm":
                        walks = isAscending == true ? walks.OrderBy(w => w.LengthInKm) : walks.OrderByDescending(w => w.LengthInKm);
                        break;
                }
            }
            //Paginate data
            var skip = (pageNumber-1) * pageSize;


           return await walks.Skip(skip).Take(pageSize).ToListAsync();
        }
        public async Task<Walk?> GetByIdAsync(Guid id)
        => await context.Walks
            .Include("Region")
            .Include("Difficulty")
            .FirstOrDefaultAsync(w => w.Id == id);

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var walkToUpdate = await context.Walks
                .FirstOrDefaultAsync(w => w.Id == id);

            if (walkToUpdate == null)
            {
                return null;
            }

            walkToUpdate.Name = walk.Name;
            walkToUpdate.Description = walk.Description;
            walkToUpdate.LengthInKm = walk.LengthInKm;
            walkToUpdate.WalkImageUrl = walk.WalkImageUrl;
            walkToUpdate.RegionId = walk.RegionId;
            walkToUpdate.DifficultyId = walk.DifficultyId;

            await context.SaveChangesAsync();

            return walkToUpdate;
        }
    }
}
