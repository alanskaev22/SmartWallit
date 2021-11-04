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
            CreateMap<AppUser, AuthenticateResponse>()
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.Id));

            CreateMap<RegisterRequest, AppUser>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.Email))
                .ForMember(d => d.DateOfBirth, o => o.MapFrom(s => s._DateOfBirth)); 

            CreateMap<Address, AddressModel>();
            CreateMap<AddressModel, Address>();

            CreateMap<CardEntity, Card>();

            CreateMap<CardRequest, CardEntity>();

            var walletMap = CreateMap<WalletEntity, Wallet>();
            walletMap.ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Balance, o => o.MapFrom(s => s.Balance))
                .ForAllOtherMembers(x => x.Ignore());
        }
    }
}
