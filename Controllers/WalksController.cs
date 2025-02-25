using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}
