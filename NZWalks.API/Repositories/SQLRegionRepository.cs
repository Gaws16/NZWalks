using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext _context;

        public SQLRegionRepository(NZWalksDbContext context)
        {
            _context = context;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await _context.Regions.AddAsync(region);
            await _context.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var expectedRegion = await _context.Regions.FindAsync(id);
            if (expectedRegion == null)
            {
                return null;
            }
            _context.Regions.Remove(expectedRegion);
            await _context.SaveChangesAsync();
            return expectedRegion;
            
        }

        public async Task<List<Region>> GetAllAsync()
        => await _context.Regions
            .AsNoTracking()
            .ToListAsync();

        public async Task<Region?> GetByIdAsync(Guid id)
       => await _context.Regions
            .FirstOrDefaultAsync(region => region.Id == id);

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var regionToUpdate = _context.Regions.Find(id);
            if (regionToUpdate == null)
            {
                return null;
            }
            regionToUpdate.Code = region.Code;
            regionToUpdate.Name = region.Name;
            regionToUpdate.RegionImageUrl = region.RegionImageUrl;
            await _context.SaveChangesAsync();
            return regionToUpdate;
        }
    }
}
