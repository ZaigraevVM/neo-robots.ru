using AutoMapper;
using SMI.Areas.Admin.Models;
using SMI.Data.Entities;
using System.Linq;

namespace SMI.Data.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {

            #region Cities
            CreateMap<City, CityEdit>();
            CreateMap<CityEdit, City>();
            #endregion

            #region Authors
            CreateMap<Author, AuthorEdit>();
            CreateMap<AuthorEdit, Author>();
            #endregion

            #region HashTags
            CreateMap<HashTag, HashTagEdit>();
            CreateMap<HashTagEdit, HashTag>();
            #endregion

            #region News
            CreateMap<News, NewsEdit>()
                .ForMember(des => des.HashTagsIds, src => src.MapFrom(x => x.HashTagsNews.Select(h => h.HashTagId)))
                .ForMember(des => des.ThemesIds, src => src.MapFrom(x => x.NewsThemes.Select(h => h.ThemeId)));
            CreateMap<NewsEdit, News>();
            #endregion

            #region Newspapers
            CreateMap<Newspaper, NewspaperEdit>();
            CreateMap<NewspaperEdit, Newspaper>();
            #endregion

            #region Photos
            CreateMap<Photo, PhotoEdit>();
            CreateMap<PhotoEdit, Photo>();
            #endregion

            #region Regions
            CreateMap<Region, RegionEdit>();
            CreateMap<RegionEdit, Region>();
            #endregion

            #region Themes
            CreateMap<Theme, ThemeEdit>();
            CreateMap<ThemeEdit, Theme>();
            #endregion
        }
    }
}