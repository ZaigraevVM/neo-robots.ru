using SMI.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SMI.Models.Base;
using SMI.Web.Models.Base;
using SMI.Areas.Admin.Models.Base;
using SMI.Data.Entities.Enums;

namespace SMI.Areas.Admin.Models
{
    public class AggregatorListsList : ItemList
    {
        public AggregatorListsList()
        {
            PagerUrl = Navigation.AuthorUrl;
        }
        public List<AggregatorList> Items { get; set; }
    }

    public class AggregatorListEdit : ItemEdit
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Требуется поле Название")]
        [Display(Name = "Название")]
        public string Title { get; set; }

        [Required(ErrorMessage = "URL не заполнен")]
        [Display(Name = "URL списка новостей")]
        public string Url { get; set; }

        [Required(ErrorMessage = "Путь к ссылкам в Html пуст!")]
        [Display(Name = "Path в HTML")]
        public string LinkHtmlPath { get; set; }

        public int AggregatorSourceId { get; set; }
        [Display(Name = "Активен")]
        public bool IsActive { get; set; }
        public AggregatorListType Type { get; set; }
        public string History { get; set; }
    }
}