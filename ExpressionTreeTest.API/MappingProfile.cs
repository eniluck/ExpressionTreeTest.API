using AutoMapper;

namespace ExpressionTreeTest.API
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Contracts.FilterParam, DataAccess.MSSQL.Models.FilterParam>().ReverseMap();
            CreateMap<Contracts.OrderParam, DataAccess.MSSQL.Models.OrderParam>().ReverseMap();
            CreateMap<Contracts.QueryParams, DataAccess.MSSQL.Models.QueryParams>().ReverseMap();
        }
    }
}