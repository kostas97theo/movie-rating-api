using Xunit;
using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieRatingAPI.Controllers;
using MovieRatingAPI.Models.Domain;
using MovieRatingAPI.Models.DTO;
using MovieRatingAPI.Repositories;
using MovieRatingAPI.Data;

namespace MyApi.Tests.Controllers
{
	public class MoviesControllerTests
	{
		private readonly Mock<IMovieRepository> _movieRepoMock;
		private readonly Mock<IMapper> _mapperMock;
		private readonly Mock<ILogger<MoviesController>> _loggerMock;
		private readonly MoviesController _controller;

		public MoviesControllerTests()
		{
			// Δημιουργία mock αντικειμένων για όλα τα dependencies
			_movieRepoMock = new Mock<IMovieRepository>();
			_mapperMock = new Mock<IMapper>();
			_loggerMock = new Mock<ILogger<MoviesController>>();

			// Το DbContext δεν χρειάζεται πραγματική βάση - μπορούμε να περάσουμε null
			MovieRatingDbContext? dbContext = null;

			// Δημιουργούμε τον controller όπως ακριβώς τον έχεις ορίσει
			_controller = new MoviesController(
				dbContext,
				_movieRepoMock.Object,
				_mapperMock.Object,
				_loggerMock.Object
			);
		}

		

		// ====================== TEST GetAll() ======================
		[Fact]
		public async Task GetAll_ShouldReturnOk_WithListOfMovies()
		{
			// Arrange
			var moviesDomain = new List<Movie>
			{
				new Movie { Id = Guid.NewGuid(), Title = "Inception", Description = "Dream movie" },
				new Movie { Id = Guid.NewGuid(), Title = "Interstellar", Description = "Space movie" }
			};

			var moviesDto = new List<MovieDto>
			{
				new MovieDto { Id = moviesDomain[0].Id, Title = "Inception", Description = "Dream movie" },
				new MovieDto { Id = moviesDomain[1].Id, Title = "Interstellar", Description = "Space movie" }
			};

			_movieRepoMock
				.Setup(r => r.GetAllAsync(It.IsAny<string>(), It.IsAny<string>(),
										  It.IsAny<string>(), It.IsAny<bool>(),
										  It.IsAny<int>(), It.IsAny<int>()))
				.ReturnsAsync(moviesDomain);

			_mapperMock
				.Setup(m => m.Map<List<MovieDto>>(moviesDomain))
				.Returns(moviesDto);

			// Act
			var result = await _controller.GetAll(null, null, null, null, 1, 10);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			var returnedMovies = Assert.IsType<List<MovieDto>>(okResult.Value);
			Assert.Equal(2, returnedMovies.Count);
			Assert.Contains(returnedMovies, m => m.Title == "Inception");
		}

		
	}
}
