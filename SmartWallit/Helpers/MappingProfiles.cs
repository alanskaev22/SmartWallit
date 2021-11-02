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
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.FirstName, o => o.MapFrom(s => s.FirstName))
                .ForMember(d => d.LastName, o => o.MapFrom(s => s.LastName))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Email))
                .ForMember(d => d._DateOfBirth, o => o.MapFrom(s => s.DateOfBirth));

            CreateMap<UserRequest, UserEntity>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.FirstName, o => o.MapFrom(s => s.FirstName))
                .ForMember(d => d.LastName, o => o.MapFrom(s => s.LastName))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Email))
                .ForMember(d => d.DateOfBirth, o => o.MapFrom(s => s._DateOfBirth))
                .ForMember(d => d.Password, o => o.MapFrom(s => s.Password));



            CreateMap<CardEntity, Card>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.CardNickname, o => o.MapFrom(s => s.CardNickname))
                .ForMember(d => d.CardNumber, o => o.MapFrom(s => s.CardNumber))
                .ForMember(d => d.ExpirationYear, o => o.MapFrom(s => s.ExpirationYear))
                .ForMember(d => d.ExpirationMonth, o => o.MapFrom(s => s.ExpirationMonth));

            CreateMap<CardRequest, CardEntity>()
                   .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                   .ForMember(d => d.CardNickname, o => o.MapFrom(s => s.CardNickname))
                   .ForMember(d => d.CardNumber, o => o.MapFrom(s => s.CardNumber))
                   .ForMember(d => d.ExpirationYear, o => o.MapFrom(s => s.ExpirationYear))
                   .ForMember(d => d.ExpirationMonth, o => o.MapFrom(s => s.ExpirationMonth))
                   .ForMember(d => d.Cvv, o => o.MapFrom(s => s.Cvv));



            var walletMap = CreateMap<WalletEntity, Wallet>();
            walletMap.ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Balance, o => o.MapFrom(s => s.Balance))
                .ForAllOtherMembers(x => x.Ignore());
        }
    }
}
