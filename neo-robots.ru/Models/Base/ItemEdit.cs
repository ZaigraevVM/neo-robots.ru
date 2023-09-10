using SMI.Web.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMI.Models.Base
{
	public class ItemEdit
	{
		public ItemEditType EditType { get; set; }

		public string EditError { get; set; }

		public string EditMessage { get; set; }

		public string LoadType { get; set; }

		public string ReturnTo { get; set; }

		public ItemList List { get; set; }

		public string GetReturnTo
		{
			get
			{
				return ReturnTo != null && ReturnTo != "" ? ReturnTo : (List != null ? List.PagerUrl : "Index");
			}
		}
	}
}