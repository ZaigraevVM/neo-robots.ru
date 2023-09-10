using Microsoft.AspNetCore.Mvc;
using SMI.Data.Entities;
using SMI.Managers;
using SMI.Areas.Admin.Models;
using SMI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace SMI.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize]
	public class CitiesController : Controller
    {
		private readonly ICitiesManager _mgr;
		private readonly IViewRender _view;
		public CitiesController(ICitiesManager mgr, IViewRender view)
		{
			_mgr = mgr;
			_view = view;
		}

		public ActionResult Index(CitiesList m)
		{
			m = _mgr.GetList(m);

			if ("ajax" == m.Type)
				return Json(new
				{
					m.Search,
					Items = _view.Render("~/Areas/Admin/Views/Cities/ListItems.cshtml", m),
					Pager = _view.Render("~/Areas/Admin/Views/Shared/_Pager.cshtml", m.GetPager())
				});

			return View("List", _mgr.ListData(m));
		}

		public ActionResult Remove(int Id)
		{
			try
			{
				_mgr.Delete(Id);
				if (Request.Headers["Referer"] != "")
					return Redirect(Request.Headers["Referer"]);
			}
			catch { }
			return RedirectToAction("Index");
		}

		public ActionResult Create()
		{
			var m = new CityEdit()
			{
				ReturnTo = Request.Headers["Referer"]
			};

			return View("Edit", _mgr.EditorData(m));
		}

		public ActionResult Edit(int Id)
		{
			var m = _mgr.Get(Id);

			if (m == null) return NotFound("Error");

			m.ReturnTo = Request.Headers["Referer"];

			return View("Edit", _mgr.EditorData(m));
		}

		[HttpPost]
		public ActionResult Save(CityEdit m)
		{
			if (ModelState.IsValid)
			{
				try
				{
					_mgr.Save(m);
					return Redirect(m.GetReturnTo);
				}
				catch (Exception ex)
				{
					m.EditError = ex.ToString();
				}
			}

			m = _mgr.EditorData(m);

			return View("Edit", m);
		}

		public ActionResult GetList()
		{
			List<City> CityList = _mgr.GetCache();
			return Json(new
			{
				City = CityList.Select(s => new { s.Id, s.Name })
			});
		}
	}
}