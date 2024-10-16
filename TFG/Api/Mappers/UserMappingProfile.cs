using AutoMapper;
using Shared.DTOs.Users;
using TFG.Model.Entities;

namespace TFG.Api.Mappers
{
	public class UserMappingProfile : Profile
	{
		public UserMappingProfile()
		{
			CreateMap<User, FilteredUserDto>();
			CreateMap<User, UserDto>();
		}
	}
}
