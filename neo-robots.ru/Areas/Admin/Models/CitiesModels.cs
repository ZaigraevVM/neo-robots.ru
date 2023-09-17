using SMI.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SMI.Models.Base;
using SMI.Web.Models.Base;
using SMI.Areas.Admin.Models.Base;

namespace SMI.Areas.Admin.Models
{
	public class CitiesList : ItemList
	{
        public CitiesList()
        {
            PagerUrl = Navigation.CityUrl;
        }

        public List<City> Items { get; set; }
		public List<Region> RegionsList { get; set; }
	}

	public class CityEdit : ItemEdit
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "Требуется поле Название.")]
		[Display(Name = "Название")]
		public string Name { get; set; }

		
		[Display(Name = "Регион")]
		[Required(ErrorMessage = "Требуется поле Регион")]
		[Range(1, 1100, ErrorMessage = "Требуется поле Регион")]
		public int RegionId { get; set; }
        public string History { get; set; }

        public List<Region> RegionsList { get; set; }

	}
}