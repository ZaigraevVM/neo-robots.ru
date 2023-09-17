using AutoMapper;
using SMI.Areas.Admin.Models;
using SMI.Data.Entities;

namespace SMI.Data.Mappings
{
    public class CitiesProfile : Profile
    {
        public CitiesProfile()
        {
            CreateMap<City, CityEdit>();
            CreateMap<CityEdit, City>();
        }
    }
}
