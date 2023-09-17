using AutoMapper;
using SMI.Areas.Admin.Models;
using SMI.Data.Entities;

namespace SMI.Data.Mappings
{
    public class PhotosProfile : Profile
    {
        public PhotosProfile()
        {
            CreateMap<Photo, PhotoEdit>();
            CreateMap<PhotoEdit, Photo>();
        }
    }
}
