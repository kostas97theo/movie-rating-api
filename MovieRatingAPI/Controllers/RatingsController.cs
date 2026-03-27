using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MovieRatingAPI.CustomActionFilters;
using MovieRatingAPI.Data;
using MovieRatingAPI.Models.Domain;
using MovieRatingAPI.Models.DTO;
using MovieRatingAPI.Repositories;
using System.Formats.Asn1;

namespace MovieRatingAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RatingsController : ControllerBase
	{
		private readonly MovieRatingDbContext dbContext;
		private readonly IRatingRepository ratingRepository;
		private readonly IMapper mapper;

		public RatingsController(MovieRatingDbContext dbContext, IRatingRepository ratingRepository,
			IMapper mapper)
        {
			this.dbContext = dbContext;
			this.ratingRepository = ratingRepository;
			this.mapper = mapper;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
			[FromQuery] string? sortBy, [FromQuery] bool? isAscending,
			[FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
		{
			var ratingsDomain = await ratingRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

			var ratingsDto = mapper.Map<List<RatingDto>>(ratingsDomain);

			return Ok(ratingsDto);
		}

		[HttpGet]
		[Route("{id:Guid}")]
		public async Task<IActionResult> GetById(Guid id)
		{
			var ratingDomain = await ratingRepository.GetByIdAsync(id);

			if (ratingDomain == null)
			{
				return NotFound();
			}

			var ratingDto = mapper.Map<RatingDto>(ratingDomain);

			return Ok(ratingDto);
		}

		[HttpPost]
		[ValidateModel]
		public async Task<IActionResult> Create([FromBody] AddRatingRequestDto addRatingRequestDto)
		{
			var ratingDomain = mapper.Map<Rating>(addRatingRequestDto);

			ratingDomain = await ratingRepository.CreateAsync(ratingDomain);

			var ratingDto = mapper.Map<RatingDto>(ratingDomain);

			return CreatedAtAction(nameof(GetById), new { id = ratingDto.Id }, ratingDto);
		}

		[HttpPut]
		[Route("{id:Guid}")]
		[ValidateModel]
		public async Task<IActionResult> Update([FromRoute] Guid id, UpdateRatingRequestDto updateRatingRequestDto)
		{
			var ratingDomain = mapper.Map<Rating>(updateRatingRequestDto);

			ratingDomain = await ratingRepository.UpdateAsync(id, ratingDomain);

			if (ratingDomain == null)
			{
				return NotFound();
			}

			var ratingDto = mapper.Map<RatingDto>(ratingDomain);

			return Ok(ratingDto);
		}

		[HttpDelete]
		[Route("{id:Guid}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			var ratingDomain = await ratingRepository.DeleteAsync(id);

			if (ratingDomain == null)
			{
				return NotFound();
			}

			var ratingDto = mapper.Map<RatingDto>(ratingDomain);

			return Ok(ratingDto);
		}
	}
}
