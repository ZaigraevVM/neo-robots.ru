using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SMI.Models.Base
{
	public class Pager
	{
		public Pager()
		{
			Index = 1;
			PageCount = 1;
			PageSize = 10;
			URL = "";
			URLGet = "";
		}

		/// <summary>
		/// Индекс активной страницы.
		/// </summary>
		public int Index { get; set; }

		/// <summary>
		/// Количество страниц.
		/// </summary>
		public int PageCount { get; set; }

		/// <summary>
		/// Количество элементов на странице.
		/// </summary>
		public int PageSize { get; set; }

		/// <summary>
		/// Количество элементов на странице.
		/// </summary>
		public string URL { get; set; }

		/// <summary>
		/// Количество элементов на странице.
		/// </summary>
		public string URLGet { get; set; }

		public string SortField { get; set; }

		public string SortOrder { get; set; }
	}
}