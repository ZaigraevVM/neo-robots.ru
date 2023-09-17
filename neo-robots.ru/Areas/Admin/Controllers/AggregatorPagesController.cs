using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMI.Areas.Admin.Models;
using SMI.Managers;
using SMI.Services;
using System;
using System.Threading.Tasks;

namespace SMI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class AggregatorPagesController : Controller
    {
        private readonly IAggregatorPagesManager _mgr;
        private readonly IViewRender _view;
        public AggregatorPagesController(IAggregatorPagesManager mgr, IViewRender view)
        {
            _mgr = mgr;
            _view = view;
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

        public async Task<IActionResult> Create(int aggregatorSourceId)
        {
            var m = new AggregatorPageEdit()
            {
                AggregatorSourceId = aggregatorSourceId,
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
        public async Task<IActionResult> SaveAsync(AggregatorPageEdit m)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _mgr.SaveAsync(m);
                    return Redirect($"/Admin/aggregatorsources/edit?id={m.AggregatorSourceId}");
                }
                catch (Exception ex)
                {
                    m.EditError = ex.ToString();
                }
            }

            m = await _mgr.EditorDataAsync(m);

            return View("Edit", m);
        }
    }
}
