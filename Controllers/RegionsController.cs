using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IMapper mapper;

        public IRegionRepository RegionRepository { get; }

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            RegionRepository = regionRepository;
            this.mapper = mapper;
        }
        //GET ALL REGIONS
        //GET: https://localhost:portnumber/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get Data from DataBase -- Domain Models
            var regions = await RegionRepository.GetAllAsync();

            //Map Domain Models to DTOs
            var regionDto = mapper.Map<List<RegionDTO>>(regions);

            //Return DTOs to client.
            return Ok(regionDto);
        }

        //GET SINGLE RETION
        //GET: https://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")] //type safe format id:id_type
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var region = await RegionRepository.GetByIdAsync(id);

            if (region == null)
                return NotFound();

            //Map domain to DTO
            var regionDto = mapper.Map<RegionDTO>(region);

            return Ok(regionDto);
        }

        //POST To Create New Region
        //POST: https://localhost:portnumber/api/regions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
            if(ModelState.IsValid)
            { 
                //Map or Convert DTO to domain model
                var regionDomainModel = mapper.Map<Region>(addRegionRequestDTO);

                //use DOmain Model to create Region
                regionDomainModel = await RegionRepository.CreateAsync(regionDomainModel);

                //Map Domain MOdel back to DTO
                var regionDto = mapper.Map<RegionDTO>(regionDomainModel);

                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
            }

            return BadRequest(ModelState);
           
        }

        //PUT Update region
        //PUT: https://localhost:portnumber/api/regions/{id}

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {
            if (ModelState.IsValid)
            {
                //Map DTO to domain model
                var regionDomainModel = mapper.Map<Region>(updateRegionRequestDTO);

                //Check if exists
                regionDomainModel = await RegionRepository.UpdateAsync(id, regionDomainModel);

                if (regionDomainModel == null)
                    return NotFound();

                //Convert Domain to DTO
                var regionDto = mapper.Map<RegionDTO>(regionDomainModel);

                //return dto to client
                return Ok(regionDto); 
            }
            return BadRequest(ModelState);
        }

        //DELETE Region
        //DELETE: https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await RegionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
                return NotFound();

            return Ok();

        }

    }
}
