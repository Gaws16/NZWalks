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

        public async Task<List<Region>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
                                                    string? sortBy = null, bool? isAscending = true,
                                                    int pageNumber = 1, int pageSize = 1000)
        {
            var regions = _context.Regions
            .AsNoTracking();

            //Filter data based on filterOn and filterQuery
            if ( filterOn!=null && filterQuery != null)
            {
                switch(filterOn.ToLower())
                {
                    case "name":
                        regions = regions.Where(region => region.Name.ToLower().Contains(filterQuery.ToLower()));
                        break;
                    case "code":
                        regions = regions.Where(region => region.Code.ToLower().Contains(filterQuery.ToLower()));
                        break;
                }
            }
            //Order data based on sortBy and isAscending
            if(string.IsNullOrWhiteSpace(sortBy) == false)
            {
                switch(sortBy.ToLower())
                {
                    case "name":
                        regions = isAscending == true ? regions.OrderBy(region => region.Name) : regions.OrderByDescending(region => region.Name);
                        break;
                    case "code":
                        regions = isAscending == true ? regions.OrderBy(region => region.Code) : regions.OrderByDescending(region => region.Code);
                        break;
                }
            }
            //Paginate the data
            var skip = (pageNumber - 1) * pageSize;

            return await regions.Skip(skip).Take(pageSize).ToListAsync();
        }
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
