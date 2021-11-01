using AutoMapper;
using SmartWallit.Core.Entities;
using SmartWallit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartWallit.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<UserEntity, User>()
                .ForMember(d => d.FirstName, o => o.MapFrom(s => s.FirstName))
                .ForMember(d => d.LastName, o => o.MapFrom(s => s.LastName))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Email))
                .ForMember(d => d._DateOfBirth, o => o.MapFrom(s => s.DateOfBirth));

            CreateMap<NewUserRequest, UserEntity>()
                .ForMember(d => d.FirstName, o => o.MapFrom(s => s.FirstName))
                .ForMember(d => d.LastName, o => o.MapFrom(s => s.LastName))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Email))
                .ForMember(d => d.DateOfBirth, o => o.MapFrom(s => s._DateOfBirth))
                .ForMember(d => d.Password, o => o.MapFrom(s => s.Password));
        }
    }
}
