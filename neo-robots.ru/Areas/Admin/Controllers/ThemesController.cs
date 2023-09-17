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
    public class ThemesController : Controller
    {
        private readonly IThemesManager _mgr;
        private readonly IViewRender _view;
        public ThemesController(IThemesManager mgr, IViewRender view)
        {
            _mgr = mgr;
            _view = view;
        }

        public async Task<IActionResult> IndexAsync(ThemesList m)
        {
            m = await _mgr.GetListAsync(m);

            if ("ajax" == m.Type)
                return Json(new
                {
                    m.Search,
                    Items = _view.Render("~/Areas/Admin/Views/Themes/ListItems.cshtml", m),
                    Pager = _view.Render("~/Areas/Admin/Views/Shared/_Pager.cshtml", m.GetPager())
                });

            return View("List", _mgr.ListData(m));
        }

        public async Task<IActionResult> RemoveAsync(int Id)
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

        public async Task<IActionResult> CreateAsync()
        {
            var m = new ThemeEdit()
            {
                ReturnTo = Request.Headers["Referer"]
            };

            return View("Edit", await _mgr.EditorDataAsync(m));
        }

        public async Task<IActionResult> EditAsync(int Id)
        {
            var m = await _mgr.GetAsync(Id);

            if (m == null) return NotFound("Error");

            m.ReturnTo = Request.Headers["Referer"];

            return View("Edit", await _mgr.EditorDataAsync(m));
        }

        [HttpPost]
        public async Task<IActionResult> SaveAsync(ThemeEdit m)
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
            IList<Theme> ThemeList = _mgr.GetCache();
            return Json(new
            {
                Theme = ThemeList.Select(s => new { s.Id, s.Name })
            });
        }
    }
}