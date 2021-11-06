using AutoMapper;
using SmartWallit.Core.Entities;
using SmartWallit.Core.Entities.Identity;
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
            CreateMap<AppUser, Account>();

            CreateMap<RegisterRequest, AppUser>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.Email))
                .ForMember(d => d.DateOfBirth, o => o.MapFrom(s => s._DateOfBirth));

            CreateMap<Address, AddressModel>();
            CreateMap<AddressModel, Address>();

            CreateMap<CardEntity, CardResponse>();

            CreateMap<CardRequest, CardEntity>();
            
            CreateMap<WalletEntity, Wallet>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Balance, o => o.MapFrom(s => s.Balance))
                .ForAllOtherMembers(x => x.Ignore());
        }
    }
}
