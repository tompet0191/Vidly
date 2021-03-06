﻿using AutoMapper;
using Vidly.Dtos;
using Vidly.Models;

namespace Vidly
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<Customer, CustomerDto>().ReverseMap();
            Mapper.CreateMap<Movie, MovieDto>().ReverseMap();
            Mapper.CreateMap<MembershipType, MembershipTypeDto>().ReverseMap();
            Mapper.CreateMap<Rental, RentalDto>().ReverseMap();
        }
    }
}
