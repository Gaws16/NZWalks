using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalkController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;
        public WalkController(IMapper _mapper,IWalkRepository _walkRepository )
        {
            mapper = _mapper;
            walkRepository = _walkRepository;
        }
        [HttpGet]
        //GET: api/walk/
        //Get all walks
        public async Task<ActionResult> GetAll()
        {
            //get all walks from database
            var walks = await walkRepository.GetAllAsync();
            //map domain model to response data
            return Ok(mapper.Map<List<WalkDTO>>(walks));
        }

        //GET: api/walk/{id}
        //Get walk by id
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<ActionResult> GetById([FromRoute] Guid id)
        {
            //get walk by id from database
            var walk = await walkRepository.GetByIdAsync(id);
            if (walk == null)
            {
                return NotFound();
            }
            //map domain model to response data
            return Ok(mapper.Map<WalkDTO>(walk));
        }


        [HttpPost]
        //POST: api/walk/
        //Create a new walk
        public async Task<ActionResult> Create([FromBody]AddWalkRequestDTO addWalkRequestDTO ) 
        {
            //map reqest data to domain model
            var walk = mapper.Map<Walk>(addWalkRequestDTO);
            //add walk to database
            await walkRepository.CreateAsync(walk);
            //map domain model to response data
            return Ok(mapper.Map<WalkDTO>(walk));
            //TODO should be 201 created
        }

        //PUT: api/walk/{id}
        //Update walk by id
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDTO updateWalkRequestDTO)
        {
            //map request data to domain model
            var walk = mapper.Map<Walk>(updateWalkRequestDTO);

            //update walk in database
            var updatedWalk = await walkRepository.UpdateAsync(id,walk);
            if (updatedWalk == null)
            {
                return NotFound();
            }

            //map domain model to response data
            return Ok(mapper.Map<WalkDTO>(updatedWalk));
        }
        //Delete walk by id
        //DELETE: api/walk/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            //delete walk by id from database
            var deletedWalk = await walkRepository.DeleteAsync(id);
            if (deletedWalk == null)
            {
                return NotFound();
            }
            //map domain model to response data
            return Ok(mapper.Map<WalkDTO>(deletedWalk));
        }
    }
}
