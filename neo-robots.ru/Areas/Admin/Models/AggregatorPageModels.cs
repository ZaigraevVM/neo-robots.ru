using SMI.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SMI.Models.Base;
using SMI.Web.Models.Base;
using SMI.Areas.Admin.Models.Base;
using SMI.Data.Entities.Enums;

namespace SMI.Areas.Admin.Models
{
    public class AggregatorPagesList : ItemList
    {
        public AggregatorPagesList()
        {
            PagerUrl = Navigation.AuthorUrl;
        }
        public List<AggregatorPage> Items { get; set; }
    }

    public class AggregatorPageEdit : ItemEdit
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Требуется поле Название")]
        [Display(Name = "Название")]
        public string Title { get; set; }
        public int AggregatorSourceId { get; set; }
        public string UrlRegex { get; set; }
        public string TitleHtmlPath { get; set; }
        public string TextHtmlPath { get; set; }
        public string ImageHtmlPath { get; set; }
        public string DateHtmlPath { get; set; }
        public string SouceUrlHtmlPath { get; set; }
        public string SouceTitleHtmlPath { get; set; }
        public AggregatorPageType Type { get; set; }
        public string History { get; set; }
    }
}