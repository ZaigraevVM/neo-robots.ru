using AutoMapper;
using SMI.Areas.Admin.Models;
using SMI.Data.Entities;

namespace SMI.Data.Mappings
{
    public class RegionsProfile : Profile
    {
        public RegionsProfile()
        {
            CreateMap<Region, RegionEdit>();
            CreateMap<RegionEdit, Region>();
        }
    }
}
