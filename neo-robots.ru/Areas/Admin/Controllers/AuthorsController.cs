using Microsoft.AspNetCore.Mvc;
using SMI.Data.Entities;
using SMI.Managers;
using SMI.Areas.Admin.Models;
using SMI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace SMI.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "admin")]
	public class AuthorsController : Controller
    {
		private readonly IAuthorsManager _mgr;
		private readonly IViewRender _view;
		public AuthorsController(IAuthorsManager mgr, IViewRender view)
		{
			_mgr = mgr;
			_view = view;
		}

		public async Task<IActionResult> Index(AuthorsList m)
		{
			m = await _mgr.GetListAsync(m);

			if ("ajax" == m.Type)
				return Json(new
				{
					m.Search,
					Items = _view.Render("~/Areas/Admin/Views/Authors/ListItems.cshtml", m),
					Pager = _view.Render("~/Areas/Admin/Views/Shared/_Pager.cshtml", m.GetPager())
				});

			return View("List", _mgr.ListData(m));
		}

		public async Task<IActionResult> Remove(int Id)
		{
			try
			{
                await _mgr.DeleteAsync(Id);
				if (Request.Headers["Referer"] != "")
					return Redirect(Request.Headers["Referer"]);
			}
			catch { }
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Create()
		{
			var m = new AuthorEdit()
			{
				ReturnTo = Request.Headers["Referer"]
			};

			return View("Edit", await _mgr.EditorDataAsync(m));
		}

		public async Task<IActionResult> Edit(int Id)
		{
			var m = await _mgr.GetAsync(Id);

			if (m == null) return NotFound("Error");

			m.ReturnTo = Request.Headers["Referer"];

			return View("Edit", await _mgr.EditorDataAsync(m));
		}

		[HttpPost]
		public async Task<IActionResult> Save(AuthorEdit m)
		{
			if (ModelState.IsValid)
			{
				try
				{
                    await _mgr.SaveAsync(m);
					return Redirect(m.GetReturnTo);
				}
				catch (Exception ex)
				{
					m.EditError = ex.ToString();
				}
			}

			m = await _mgr.EditorDataAsync(m);

			return View("Edit", m);
		}

		public async Task<IActionResult> GetList()
		{
			IList<Author> AuthorList = _mgr.GetCache();
			return Json(new
			{
				Author = AuthorList.Select(s => new { s.Id, s.FirstName })
			});
		}
	}
}