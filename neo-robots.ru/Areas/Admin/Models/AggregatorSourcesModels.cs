using SMI.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SMI.Models.Base;
using SMI.Web.Models.Base;
using SMI.Areas.Admin.Models.Base;

namespace SMI.Areas.Admin.Models
{
    public class AggregatorSourcesList : ItemList
    {
        public AggregatorSourcesList()
        {
            PagerUrl = Navigation.AuthorUrl;
        }
        public List<AggregatorSource> Items { get; set; }
    }

    public class AggregatorSourceEdit : ItemEdit
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Требуется поле Название")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Требуется поле Url")]
        [Display(Name = "Url")]
        public string Url { get; set; }
        public string History { get; set; }
        public ICollection<AggregatorListEdit> AggregatorLists { get; set; }
        public ICollection<AggregatorPageEdit> AggregatorPages { get; set; }
    }
}