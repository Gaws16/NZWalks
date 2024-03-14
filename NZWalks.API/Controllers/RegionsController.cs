using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _context;

        public RegionsController(NZWalksDbContext context)
        {
           _context= context;
        }
        // GET: api/Regions
        // Get all regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get data from Database and map it to DTO
            var regions = await _context.Regions
                .Select(region => new RegionsDTO
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    RegionImageUrl = region.RegionImageUrl
                })
                .ToListAsync();
               
            //Return the data
            return Ok(regions);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        //Get specific region by id
        // GET: api/Regions/{id}
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            //Get specific Region by id
            var region = await _context.Regions.FindAsync(id);
            if (region == null)
            {
                return NotFound();
            }
            //Map the data to DTO
            var regionDTO = new RegionsDTO
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            };
            //Return the data
            return Ok(regionDTO);
        }

        [HttpPost]
        //Create new region
        // POST: api/Regions
        public async Task<IActionResult> CreateRegion([FromBody] CreateRegionRequestDTO region)
        {
            //Map the data to Domain
            var newRegion = new Region
            {
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            };

            //Add the data to Database
            await _context.Regions.AddAsync(newRegion);
            await _context.SaveChangesAsync();

            //map the data to return DTO
            var regionDTO = new RegionsDTO
            {
                Id = newRegion.Id,
                Code = newRegion.Code,
                Name = newRegion.Name,
                RegionImageUrl = newRegion.RegionImageUrl
            };
            //Return the created object following the RESTful convention
            return CreatedAtAction(nameof(GetRegionById), new { id = newRegion.Id }, regionDTO);

        }
        //Update specific region by id
        // PUT: api/Regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegion)
        {
            //Get specific Region by id
            var regionDomainModel = _context.Regions.Find(id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //Update the specific region
            regionDomainModel.Code = updateRegion.Code;
            regionDomainModel.Name = updateRegion.Name;
            regionDomainModel.RegionImageUrl = updateRegion.RegionImageUrl;

           await _context.SaveChangesAsync();
            //Map the data to return DTO
            var regionDTO = new RegionsDTO
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            //Return the updated object following the RESTful convention
            return Ok(regionDTO);
        }

        //Delete specific region by id
        // DELETE: api/Regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            //Get specific Region by id
            var region = await _context.Regions.FindAsync(id);
            if (region == null)
            {
                return NotFound();
            }
            //Remove the specific region
            _context.Regions.Remove(region);
            await _context.SaveChangesAsync();
            //Return the deleted object following the RESTful convention
            return Ok(new RegionsDTO
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            });
        }
    }
}
