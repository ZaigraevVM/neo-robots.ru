using SMI.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SMI.Models.Base;
using SMI.Web.Models.Base;
using SMI.Areas.Admin.Models.Base;

namespace SMI.Areas.Admin.Models
{
	public class AuthorsList : ItemList
	{
        public AuthorsList()
        {
            PagerUrl = Navigation.AuthorUrl;
        }
		public List<Author> Items { get; set; }
	}

	public class AuthorEdit : ItemEdit
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "Требуется поле Имя.")]
		[Display(Name = "Имя")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Требуется поле Фамилия.")]
		[Display(Name = "Фамилия")]
		public string LastName { get; set; }



		public List<News> News { get; set; }
		public List<Author> Items { get; set; }
	}
}