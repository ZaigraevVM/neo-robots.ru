using SMI.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SMI.Models.Base;
using SMI.Web.Models.Base;
using SMI.Areas.Admin.Models.Base;

namespace SMI.Areas.Admin.Models
{
	public class PhotosList : ItemList
	{
        public PhotosList()
        {
            PagerUrl = Navigation.PhotoUrl;
        }
		public List<News> News { get; set; }
		public List<Photo> Items { get; set; }
	}

	public class PhotoEdit : ItemEdit
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "Требуется поле Название.")]
		[Display(Name = "Название")]
		public string Name { get; set; }
		public string FileName { get; set; }

        [Display(Name = "Url источника")]
        public string SourceUrl { get; set; }
        public string History { get; set; }
        public List<News> News { get; set; }
	}

	public class CropModel
	{
		public string OriginalUrl { get; set; }
        public string Type { get; set; }
        public string FileName { get; set; }
    }
}