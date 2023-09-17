using AutoMapper;
using SMI.Areas.Admin.Models;
using SMI.Data.Entities;

namespace SMI.Data.Mappings
{
    public class AggregatorProfile : Profile
    {
        public AggregatorProfile()
        { 
            CreateMap<AggregatorSource, AggregatorSourceEdit>();
            CreateMap<AggregatorList, AggregatorListEdit>();
            CreateMap<AggregatorPage, AggregatorPageEdit>();
        }
    }
}
