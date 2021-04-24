using AutoMapper.Configuration;
using EthereumTransactions.Extensions;
using Nethereum.Util;
using Nethereum.Web3;
using System;

namespace EthereumTransactions.Utils
{
	public class MappingProfile : MapperConfigurationExpression
    {
        public MappingProfile()
        {
            CreateMap<Models.TransactionSearchRequest, Services.ThirdPartyServices.Dtos.Infura.GetBlockByNumberRequest>()
                .ForMember(dest => dest.Jsonrpc, m => m.MapFrom( src => Constants.JsonrpcVersion))
                .ForMember(dest => dest.Method, m => m.MapFrom(src => Constants.EthGetBlockByNumber))
                .ForMember(dest => dest.Params, m => m.MapFrom(src => new object[] { $"0X{src.BlockNumber:X2}", true}))
                .ForMember(dest => dest.Id, m => m.MapFrom(src => Constants.Id));

            CreateMap<Services.ThirdPartyServices.Dtos.Infura.Transaction, Models.Transaction>()
                .ForMember(dest => dest.BlockHash, m => m.MapFrom(src => src.BlockHash))
                .ForMember(dest => dest.BlockNumber, m => m.MapFrom(src => Convert.ToInt64(src.BlockNumber, 16)))
                .ForMember(dest => dest.Gas, m => m.MapFrom(src => Web3.Convert.FromWei(src.Gas.HexToBigInteger(), UnitConversion.EthUnit.Ether)))
                .ForMember(dest => dest.Hash, m => m.MapFrom(src => src.Hash))
                .ForMember(dest => dest.From, m => m.MapFrom(src => src.From))
                .ForMember(dest => dest.To, m => m.MapFrom(src => src.To))
                .ForMember(dest => dest.Value, m => m.MapFrom(src => Web3.Convert.FromWei(src.Value.HexToBigInteger(), UnitConversion.EthUnit.Ether)));
        }
	}
}
