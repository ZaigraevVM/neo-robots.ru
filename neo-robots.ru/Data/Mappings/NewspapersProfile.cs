using AutoMapper;
using SMI.Areas.Admin.Models;
using SMI.Data.Entities;

namespace SMI.Data.Mappings
{
    public class NewspapersProfile : Profile
    {
        public NewspapersProfile()
        {
            CreateMap<Newspaper, NewspaperEdit>();
            CreateMap<NewspaperEdit, Newspaper>();
        }
    }
}
