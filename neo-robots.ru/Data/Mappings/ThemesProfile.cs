using AutoMapper;
using SMI.Areas.Admin.Models;
using SMI.Data.Entities;

namespace SMI.Data.Mappings
{
    public class ThemesProfile : Profile
    {
        public ThemesProfile()
        {
            CreateMap<Theme, ThemeEdit>();
            CreateMap<ThemeEdit, Theme>();
        }
    }
}
