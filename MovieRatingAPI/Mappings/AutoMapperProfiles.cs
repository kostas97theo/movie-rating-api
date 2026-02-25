using AutoMapper;
using MovieRatingAPI.Models.Domain;
using MovieRatingAPI.Models.DTO;

namespace MovieRatingAPI.Mappings
{
	public class AutoMapperProfiles: Profile
	{
        public AutoMapperProfiles()
        {
            CreateMap<Movie, MovieDto>().ReverseMap();
			CreateMap<Movie, AddMovieRequestDto>().ReverseMap();
			CreateMap<Movie, UpdateMovieRequestDto>().ReverseMap();
			CreateMap<Rating, RatingDto>().ReverseMap();
			CreateMap<Rating, AddRatingRequestDto>().ReverseMap();
			CreateMap<Rating, UpdateRatingRequestDto>().ReverseMap();
		}
    }
}
