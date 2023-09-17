using SMI.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SMI.Models.Base;
using SMI.Web.Models.Base;
using SMI.Areas.Admin.Models.Base;
using System;

namespace SMI.Areas.Admin.Models
{
	public class NewsList : ItemList
	{
        public NewsList()
        {
            PagerUrl = Navigation.NewsUrl;
        }
        public List<News> Items { get; set; }

    }

	public class NewsEdit : ItemEdit
	{
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Требуется поле Заголовок.")]
        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        [Display(Name = "Path")]
        public string Path { get; set; }

        [Display(Name = "Автор")]
        public int? AuthorId { get; set; }

        [Display(Name = "Фотографии")]
        public int? PhotoId { get; set; }

        [Display(Name = "Дата")]
        public DateTime? Date { get; set; }

        [Required(ErrorMessage = "Требуется поле 'Краткое описание'")]
        [Display(Name = "Краткое описание")]
        public string Intro { get; set; }

        [Required(ErrorMessage = "Требуется поле Текст.")]
        [Display(Name = "Текст")]
        public string Text { get; set; }

        [Display(Name = "Издания")]
        public int? NewspapersId { get; set; }

        [Display(Name = "Опубликовать")]
        public bool IsPublish { get; set; }
        public string History { get; set; }

        [Display(Name = "Хэш-тэги")]
        public List<HashTag> HashTags { get; set; }

        [Display(Name = "Город")]
        public List<City> Cities { get; set; }

        [Display(Name = "Регион")]
        public List<Region> Regions { get; set; }

        [Display(Name = "Тема")]
        public List<Theme> Themes { get; set; }
        public List<Newspaper> Newspapers { get; set; }

        public List<Author> Authors { get; set; }
        public List<Photo> Photos { get; set; }

        public string PhotoFile { get; set; }
        public Photo Photo { get; set; }

        private int[] _hashTagsIds;
        [Display(Name = "Хэш-теги")]
        public int[] HashTagsIds
        {
            get { return _hashTagsIds ?? Array.Empty<int>(); }
            set { _hashTagsIds = value; }
        }

        private int[] _themesIds;
        [Display(Name = "Темы")]
        public int[] ThemesIds
        {
            get { return _themesIds ?? Array.Empty<int>(); }
            set { _themesIds = value; }
        }

        #region NewsRegions
        private int[] _regionsIds;

        [Display(Name = "Регионы")]
        public int[] RegionsIds
        {
            get { return _regionsIds ?? Array.Empty<int>(); }
            set { _regionsIds = value; }
        }
        #endregion

        #region NewsCities
        private int[] _citiesIds;

        [Display(Name = "Города")]
        public int[] CitiesIds
        {
            get { return _citiesIds ?? Array.Empty<int>(); }
            set { _citiesIds = value; }
        }
        #endregion
    }
}