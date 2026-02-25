using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieRatingAPI.CustomActionFilters;
using MovieRatingAPI.Data;
using MovieRatingAPI.Models.Domain;
using MovieRatingAPI.Models.DTO;
using MovieRatingAPI.Repositories;
using System.Text.Json;

namespace MovieRatingAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MoviesController : ControllerBase
	{
		private readonly MovieRatingDbContext dbContext;
		private readonly IMovieRepository movieRepository;
		private readonly IMapper mapper;
		private readonly ILogger<MoviesController> logger;

		public MoviesController(MovieRatingDbContext dbContext, IMovieRepository movieRepository
			,IMapper mapper,
			ILogger<MoviesController> logger)
		{
			this.dbContext = dbContext;
			this.movieRepository = movieRepository;
			this.mapper = mapper;
			this.logger = logger;
		}

		[HttpGet]
		[Authorize(Roles = "Reader")]
		public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
			[FromQuery] string? sortBy, [FromQuery] bool? isAscending,
			[FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
		{
			var moviesDomain = await movieRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

			//var moviesDto = new List<MovieDto>();
			//foreach (var movieDomain in moviesDomain)
			//{
			//	moviesDto.Add(new MovieDto()
			//	{
			//		Id = movieDomain.Id,
			//		Title = movieDomain.Title,
			//		Description = movieDomain.Description,
			//		CreatedAt = movieDomain.CreatedAt,
			//		CreatedBy = movieDomain.CreatedBy
			//	});
			//}
			var moviesDto =  mapper.Map<List<MovieDto>>(moviesDomain);

			return Ok(moviesDto);
		}

		[HttpGet]
		[Route("{id:Guid}")]
		//[Authorize(Roles = "Reader,Writer")]
		public async Task<IActionResult> GetById(Guid id)
		{
			var movieDomain = await movieRepository.GetByIdAsync(id);

			if (movieDomain == null)
			{
				return NotFound();
			}

			//var movieDto = new MovieDto
			//{
			//	Id=movieDomain.Id,
			//	Title = movieDomain.Title,
			//	Description = movieDomain.Description,
			//	CreatedAt = movieDomain.CreatedAt,
			//	CreatedBy = movieDomain.CreatedBy
			//};

			var movieDto = mapper.Map<MovieDto>(movieDomain);

			return Ok(movieDto);
		}

		[HttpPost]
		[ValidateModel]
		[Authorize(Roles = "Writer")]
		public async Task<IActionResult> Create([FromBody] AddMovieRequestDto addMovieRequestDto)
		{
				//var movieDomain = new Movie
				//{
				//	Title = addMovieRequestDto.Title,
				//	Description = addMovieRequestDto.Description,
				//	CreatedAt = addMovieRequestDto.CreatedAt,
				//	CreatedBy = addMovieRequestDto.CreatedBy
				//};

				var movieDomain = mapper.Map<Movie>(addMovieRequestDto);

				movieDomain = await movieRepository.CreateAsync(movieDomain);

				//var movieDto = new MovieDto
				//{
				//	Id = movieDomain.Id,
				//	Title = movieDomain.Title,
				//	Description = movieDomain.Description,
				//	CreatedAt = movieDomain.CreatedAt,
				//	CreatedBy = movieDomain.CreatedBy
				//};

				var movieDto = mapper.Map<MovieDto>(movieDomain);

				return CreatedAtAction(nameof(GetById), new { id = movieDto.Id }, movieDto);
		}

		[HttpPut]
		[Route("{id:Guid}")]
		[ValidateModel]
		[Authorize(Roles = "Writer")]
		public async Task<IActionResult> Update([FromRoute]Guid id, [FromBody]UpdateMovieRequestDto updateMovieRequestDto)
		{
				var movieDomain = mapper.Map<Movie>(updateMovieRequestDto);

				movieDomain = await movieRepository.UpdateAsync(id, movieDomain);

				if (movieDomain == null)
				{
					return NotFound();
				}

				var movieDto = mapper.Map<MovieDto>(movieDomain);

				return Ok(movieDto);
		}

		[HttpDelete]
		[Route("{id:Guid}")]
		[Authorize(Roles = "Writer")]
		public async Task<IActionResult> Delete([FromRoute] Guid id)
		{
			var movieDomain = await movieRepository.DeleteAsync(id);

			if (movieDomain == null)
			{
				return NotFound();
			}

			var movieDto = mapper.Map<MovieDto>(movieDomain);

			return Ok(movieDto);
		}
	}
}
