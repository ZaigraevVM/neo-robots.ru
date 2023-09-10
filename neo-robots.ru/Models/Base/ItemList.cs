using SMI.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace SMI.Web.Models.Base
{
	[NotMapped]
	public class ItemList
	{
		public ItemList()
		{
			PagerUrl = "/Admin/";
		}

		public string SortField { get; set; }
		public string SortOrder { get; set; }

		public string SortFieldForLink(string Name = "")
		{
			return SortField == Name && SortOrder == "asc" ? "desc" : "asc";
		}

		public string SortFieldForLinkForClass(string Name = "")
		{
			return (SortField == Name ? (SortOrder == "asc" ? "bi-sort-up" : "bi-sort-down") : "");
		}

		public string SortFieldForClass(string Name = "")
		{
			return SortField == Name ? (SortOrder == "asc" ? "desc" : "asc") : "";
		}

		public string Type { get; set; }

		private string search { get; set; }
		public string Search
		{
			get
			{
				if (search == null) search = "";
				return search;
			}
			set
			{
				search = value;
			}
		}

		public string[] SearchWords
		{
			get
			{
				string[] ReturnTo = new string[0];
				if (Search != null && Search != "")
					ReturnTo = Regex.Replace(Search.Trim(), @"\s+", " ").Split(' ');
				return ReturnTo;
			}
		}

		public int SearchID { get; set; }

		public int Select { get; set; }

		private int pageIndex { get; set; }
		public int PageIndex
		{
			get
			{
				if (pageIndex < 1) pageIndex = 1;
				return pageIndex;
			}
			set
			{
				pageIndex = value;
			}
		}

		int pagesize { get; set; }
		public int PageSize
		{
			get
			{
				if (pagesize < 5 || pagesize > 500) pagesize = 10;
				return pagesize;
			}
			set
			{
				pagesize = value;
			}
		}

		/// <summary>
		/// Количество всех записей с учетом фильтра
		/// </summary>
		public int Count { get; set; }
		public string PagerUrl { get; set; }
		public string ListMessage { get; set; }
		public string ListError { get; set; }

		public Pager GetPager()
		{
			return new Pager()
			{
				URL = PagerUrl,
				URLGet = "?search=" + Search + "&select=" + Select.ToString() + "&Count=" + Count,
				Index = PageIndex,
				PageCount = (int)Count / PageSize + ((Count % PageSize == 0) ? 0 : 1),
				PageSize = PageSize,
				SortOrder = SortOrder,
				SortField = SortField
			};
		}
	}
}