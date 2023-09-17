using SMI.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SMI.Models.Base;
using SMI.Web.Models.Base;
using SMI.Areas.Admin.Models.Base;

namespace SMI.Areas.Admin.Models
{
	public class ThemesList : ItemList
	{
        public ThemesList()
        {
            PagerUrl = Navigation.ThemeUrl;
        }

		public List<HashTagsTheme> HashTagsThemes { get; set; }
		public List<NewsTheme> NewsThemes { get; set; }
		public List<Theme> Items { get; set; }
	}

	public class ThemeEdit : ItemEdit
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "Требуется поле Название.")]
		[Display(Name = "Название")]
		public string Name { get; set; }

		[Display(Name = "Сортировка")]
		public int Sorting { get; set; }
        public string History { get; set; }
        public List<HashTagsTheme> HashTagsThemes { get; set; }
		public List<NewsTheme> NewsThemes { get; set; }
	}
}