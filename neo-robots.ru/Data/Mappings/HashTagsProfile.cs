using AutoMapper;
using SMI.Areas.Admin.Models;
using SMI.Data.Entities;

namespace SMI.Data.Mappings
{
    public class HashTagsProfile : Profile
    {
        public HashTagsProfile()
        {
            CreateMap<HashTag, HashTagEdit>();
            CreateMap<HashTagEdit, HashTag>();
        }
    }
}
