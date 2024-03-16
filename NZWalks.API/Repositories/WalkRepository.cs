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

        public async Task<List<Walk>> GetAllAsync()
        => await context.Walks
            .Include("Region")
            .Include("Difficulty")
            .ToListAsync();

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
