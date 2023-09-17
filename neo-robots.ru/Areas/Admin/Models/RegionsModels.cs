using SMI.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SMI.Models.Base;
using SMI.Web.Models.Base;
using SMI.Areas.Admin.Models.Base;

namespace SMI.Areas.Admin.Models
{
	public class RegionsList : ItemList
	{
        public RegionsList()
        {
            PagerUrl = Navigation.RegionUrl;
        }

		public List<NewsRegion> NewsRegions { get; set; }
		public List<City> Cities { get; set; }
		public List<Region> Items { get; set; }
	}

	public class RegionEdit : ItemEdit
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "Требуется поле Название.")]
		[Display(Name = "Название")]
		public string Name { get; set; }
        public string History { get; set; }
        public List<NewsRegion> NewsRegions { get; set; }
		public List<City> Cities { get; set; }
	}
}