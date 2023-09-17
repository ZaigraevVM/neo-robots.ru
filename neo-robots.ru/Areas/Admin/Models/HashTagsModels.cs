using SMI.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SMI.Models.Base;
using SMI.Web.Models.Base;
using SMI.Areas.Admin.Models.Base;

namespace SMI.Areas.Admin.Models
{
	public class HashTagsList : ItemList
	{
        public HashTagsList()
        {
            PagerUrl = Navigation.HashTagUrl;
        }

        public List<HashTag> Items { get; set; }
	}

	public class HashTagEdit : ItemEdit
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "Требуется поле Название.")]
		[Display(Name = "Название")]
		public string Name { get; set; }
        public string History { get; set; }

        public List<HashTagsNews> HashTagsNews { get; set; }
		public List<HashTagsTheme> HashTagsTheme { get; set; }
	}
}