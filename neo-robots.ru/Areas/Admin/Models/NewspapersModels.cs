using SMI.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SMI.Models.Base;
using SMI.Web.Models.Base;
using SMI.Areas.Admin.Models.Base;

namespace SMI.Areas.Admin.Models
{
	public class NewspapersList : ItemList
	{
        public NewspapersList()
        {
            PagerUrl = Navigation.NewspaperUrl;
        }

		public List<News> News { get; set; }
		public List<Newspaper> Items { get; set; }
	}

	public class NewspaperEdit : ItemEdit
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "Требуется поле Название.")]
		[Display(Name = "Название")]
		public string Name { get; set; }
        public string History { get; set; }
        public List<News> News { get; set; }
	}
}