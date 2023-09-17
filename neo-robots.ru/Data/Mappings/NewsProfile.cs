using AutoMapper;
using SMI.Areas.Admin.Models;
using SMI.Data.Entities;
using System.Linq;

namespace SMI.Data.Mappings
{
    public class NewsProfile : Profile
    {
        public NewsProfile()
        {
            CreateMap<News, NewsEdit>()
                .ForMember(des => des.HashTagsIds, src => src.MapFrom(x => x.HashTagsNews.Select(h => h.HashTagId)))
                .ForMember(des => des.ThemesIds, src => src.MapFrom(x => x.NewsThemes.Select(h => h.ThemeId)))
                .ForMember(des => des.RegionsIds, src => src.MapFrom(x => x.NewsRegions.Select(h => h.RegionId)))
                .ForMember(des => des.CitiesIds, src => src.MapFrom(x => x.NewsCities.Select(h => h.CityId)));
            CreateMap<NewsEdit, News>();
        }
    }
}
