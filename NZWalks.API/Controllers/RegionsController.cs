using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
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
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository _regionRepository, IMapper _mapper)
        {
            regionRepository = _regionRepository;
            mapper = _mapper;
        }
        // GET: api/Regions/filterOn=Name&filterQuery=Abel&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10
        // Get all regions
        [HttpGet]
        [Authorize(Roles = "Reader, Writer")]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
                                                    [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
                                                    [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            //Get data from Database and map it to DTO
            var regions = await regionRepository
                .GetAllAsync(filterOn,filterQuery,
                                sortBy,isAscending ?? true,
                                pageNumber,pageSize);
                           
            //Return the mapped to DTO data
            return Ok(mapper.Map<List<RegionsDTO>>(regions));
        }

        //Get specific region by id
        // GET: api/Regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader, Writer")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            //Get specific Region by id
            var region = await regionRepository.GetByIdAsync(id);
            if (region == null)
            {
                return NotFound();
            }
            
            //Return mapped to DTO data
            return Ok(mapper.Map<RegionsDTO>(region));
        }

        //Create new region
        // POST: api/Regions
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateRegion([FromBody] CreateRegionRequestDTO region)
        {
            //Map the data to Domain
            var newRegion = mapper.Map<Region>(region);

            //Add the data to Database
           var addedRegion = await regionRepository.CreateAsync(newRegion);

            //map the data to return DTO
            var regionDTO = mapper.Map<RegionsDTO>(addedRegion);
            //Return the created object following the RESTful convention
            return CreatedAtAction(nameof(GetRegionById), new { id = addedRegion.Id }, regionDTO);

        }
        //Update specific region by id
        // PUT: api/Regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegion)
        {
            //Map the data to Domain model
            var regionToUpdate =mapper.Map<Region>(updateRegion); 
            //Update the data in Database
            var regionDomainModel = await regionRepository.UpdateAsync(id,regionToUpdate);
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            //Map the data to return DTO
            //Return the updated object following the RESTful convention
            return Ok(mapper.Map<RegionsDTO>(regionDomainModel));
        }

        //Delete specific region by id
        // DELETE: api/Regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles="Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var region = await regionRepository.DeleteAsync(id);
            if (region == null)
            {
                return NotFound();
            }
            //Return the deleted object following the RESTful convention
            return Ok(mapper.Map<RegionsDTO>(region));
        }
    }
}
