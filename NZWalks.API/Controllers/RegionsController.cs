using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository regionRepository;

        public RegionsController(IRegionRepository _regionRepository)
        {
           regionRepository = _regionRepository;
        }
        // GET: api/Regions
        // Get all regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get data from Database and map it to DTO
            var regions = await regionRepository
                .GetAllAsync();


            var regionsDTO = regions.Select(region => new RegionsDTO
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            });
                
               
            //Return the data
            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        //Get specific region by id
        // GET: api/Regions/{id}
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            //Get specific Region by id
            var region = await regionRepository.GetByIdAsync(id);
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
           var addedRegion = await regionRepository.CreateAsync(newRegion);

            //map the data to return DTO
            var regionDTO = new RegionsDTO
            {
                Id = addedRegion.Id,
                Code = addedRegion.Code,
                Name = addedRegion.Name,
                RegionImageUrl = addedRegion.RegionImageUrl
            };
            //Return the created object following the RESTful convention
            return CreatedAtAction(nameof(GetRegionById), new { id = addedRegion.Id }, regionDTO);

        }
        //Update specific region by id
        // PUT: api/Regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegion)
        {
            //Get specific Region by id
            var regionDomainModel = await regionRepository.UpdateAsync(id, new Region
            {
                Code = updateRegion.Code,
                Name = updateRegion.Name,
                RegionImageUrl = updateRegion.RegionImageUrl

            });
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            
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
            var region = await regionRepository.DeleteAsync(id);
            if (region == null)
            {
                return NotFound();
            }
            
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
