﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;


        public RegionsController(IRegionRepository regionRepository, IMapper mapper, ILogger<RegionsController> logger)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        //GET ALL REGIONS
        //GET: https://localhost:portnumber/api/regions
        [HttpGet]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("GetAll Action Method was invoked");

            //Get Data from DataBase -- Domain Models
            var regions = await regionRepository.GetAllAsync();

            //Map Domain Models to DTOs
            var regionDto = mapper.Map<List<RegionDTO>>(regions);

            //Return DTOs to client.
            logger.LogInformation($"Finished GetAllRegions Regions: {JsonSerializer.Serialize(regions)}");
            return Ok(regionDto);
        }

        //GET SINGLE RETION
        //GET: https://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")] //type safe format id:id_type
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var region = await regionRepository.GetByIdAsync(id);

            if (region == null)
                return NotFound();

            //Map domain to DTO
            var regionDto = mapper.Map<RegionDTO>(region);

            return Ok(regionDto);
        }

        //POST To Create New Region
        //POST: https://localhost:portnumber/api/regions
        [HttpPost]
        [Authorize(Roles = "Writer")]
        [ValidationModel]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
            //Map or Convert DTO to domain model
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDTO);

            //use DOmain Model to create Region
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            //Map Domain MOdel back to DTO
            var regionDto = mapper.Map<RegionDTO>(regionDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);  
        }

        //PUT Update region
        //PUT: https://localhost:portnumber/api/regions/{id}

        [HttpPut]
        [Authorize(Roles = "Writer")]
        [Route("{id:Guid}")]
        [ValidationModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {
            //Map DTO to domain model
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDTO);

            //Check if exists
            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
                return NotFound();

            //Convert Domain to DTO
            var regionDto = mapper.Map<RegionDTO>(regionDomainModel);

            //return dto to client
            return Ok(regionDto); 
        }

        //DELETE Region
        //DELETE: https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Authorize(Roles = "Writer")]
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
