using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    //api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepositories walkRepositories;

        public WalksController(IMapper mapper, IWalkRepositories walkRepositories)
        {
            this.mapper = mapper;
            this.walkRepositories = walkRepositories;
        }
        //Create new Walk
        //POST
        [HttpPost]
        [ValidationModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            //DTO to Domain Model with AutoMapper
            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

            //Create
            await walkRepositories.CreateAsync(walkDomainModel);

            //Map domain to dto
            return Ok(mapper.Map<WalkDTO>(walkDomainModel)); 
        }

        // GET WALKS
        // GET /api/walks
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var walks = await walkRepositories.GetAllAsync();

            return Ok(mapper.Map<List<WalkDTO>>(walks));
        }

        // GET A WALK
        // GET /api/walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //Get the Domain Model
            var walk = await walkRepositories.GetByIdAsync(id);

            //return DTO
            if (walk == null)
                return NotFound();

            return Ok(mapper.Map<WalkDTO>(walk));
        }

        // UPDATE A WALK
        // PUT /api/walks/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidationModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDTO updateWalkRequestDTO)
        {
            //Map DTO to Domain Model
            var walkDomailModel = mapper.Map<Walk>(updateWalkRequestDTO);

            //Update via Repo
            walkDomailModel = await walkRepositories.UpdateAsync(id, walkDomailModel);

            //return DTO
            if (walkDomailModel == null)
                return NotFound();

            return Ok(mapper.Map<WalkDTO>(walkDomailModel)); 
        }

        // DELETE A WALK
        // DELETE /api/walks/{id}
        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            //Delete via repo
            var walkDomainModel = await walkRepositories.DeleteAsync(id);

            //Check if found
            if (walkDomainModel == null)
                return NotFound();

            //Return ok and deleted walk, Domain Model to DTO
            return Ok(mapper.Map<WalkDTO>(walkDomainModel));
        }
    }
}
