using Microsoft.AspNetCore.Mvc;
using SMI.Data.Entities;
using SMI.Managers;
using SMI.Areas.Admin.Models;
using SMI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;
using SMI.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SMI.Code.Extensions;
using SMI.Managers.Core;
using Microsoft.Extensions.Logging;
using SMI.Controllers;

namespace SMI.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "admin")]
	public class NewsController : Controller
    {
		private readonly INewsManager _mgr;
		private readonly IPhotosManager _mgrPhotos;
		private readonly IViewRender _view;
		private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<NewsController> _logger;
        public NewsController(INewsManager mgr, IViewRender view, IHostingEnvironment hostingEnvironment, IPhotosManager mgrPhotos, ILogger<NewsController> logger)
		{
			_mgr = mgr;
			_view = view;
			_hostingEnvironment = hostingEnvironment;
			_mgrPhotos = mgrPhotos;
			_logger = logger;
			ViewBag.WebRootPath = _hostingEnvironment.WebRootPath;
		}

		public async Task<IActionResult> IndexAsync(NewsList m)
		{
			m = await _mgr.GetListAsync(m);

			if ("ajax" == m.Type)
				return Json(new
				{
					m.Search,
					Items = _view.Render("~/Areas/Admin/Views/News/ListItems.cshtml", m),
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
			catch (Exception ex){
				_logger.LogError(ex, "Error when remove news");
			}
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> CreateAsync()
		{
			var m = new NewsEdit()
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
			ViewBag.WebRootPath = _hostingEnvironment.WebRootPath;
			return View("Edit", await _mgr.EditorDataAsync(m));
		}

		[HttpPost]
		public async Task<IActionResult> SaveAsync(NewsEdit m, IFormFile file)
		{
			if (ModelState.IsValid)
			{
				try
				{
					#region file
					if (file != null && file.Length > 0)
					{
						var filename = file.FileName;

						string Extension = Path.GetExtension(filename).ToLower();

						if (".png" == Extension || ".jpg" == Extension || ".gif" == Extension || ".jpeg" == Extension || ".png" == Extension)
						{
							Guid IdGuid = Guid.NewGuid();
							string fPath = _hostingEnvironment.WebRootPath + ContentUrl.OriginalPhoto + IdGuid + Extension;

							using (Stream fileStream = new FileStream(fPath, FileMode.Create))
							{
								await file.CopyToAsync(fileStream);
							}

							ImageProcessing.SaveCropPictureCentral(
								new EditPictureModel()
								{
									IdGuid = IdGuid,
									Extension = Extension,
									WebRootPath = _hostingEnvironment.WebRootPath,
									SizeType = "w1000x523"
								});

							ImageProcessing.SaveCropPictureCentral(
								new EditPictureModel()
								{
									IdGuid = IdGuid,
									Extension = Extension,
									WebRootPath = _hostingEnvironment.WebRootPath,
									SizeType = "w100x100"
								});

							ImageProcessing.SaveCropPictureCentral(
								new EditPictureModel()
								{
									IdGuid = IdGuid,
									Extension = Extension,
									WebRootPath = _hostingEnvironment.WebRootPath,
									SizeType = "w200x150"
								});

							ImageProcessing.SaveCropPictureCentral(
								new EditPictureModel()
								{
									IdGuid = IdGuid,
									Extension = Extension,
									WebRootPath = _hostingEnvironment.WebRootPath,
									SizeType = "w450x150"
								});

							ImageProcessing.SaveCropPictureCentral(
								new EditPictureModel()
								{
									IdGuid = IdGuid,
									Extension = Extension,
									WebRootPath = _hostingEnvironment.WebRootPath,
									SizeType = "w500x300"
								});
							ImageProcessing.SaveCropPictureCentral(
								new EditPictureModel()
								{
									IdGuid = IdGuid,
									Extension = Extension,
									WebRootPath = _hostingEnvironment.WebRootPath,
									SizeType = "w890x534"
								});

							var photo = await _mgrPhotos.SaveAsync(new PhotoEdit() { Name = file.FileName, FileName = IdGuid + Extension });
							m.PhotoId = photo.Id;
						}
						else
						{
							m.EditMessage = "Поддерживаются только такие форматы файлов - .png, .jpg, .gif, .jpeg,";
							TempData["EditMessage"] = m.EditMessage;
						}
					}
					#endregion

					await _mgr.SaveAsync(m);

					if (file != null)
						return RedirectToAction("edit", new { id = m.Id, returnto = m.GetReturnTo, selecttab = "tabphotos" });

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

		public async Task<IActionResult> GetListAsync()
		{
			IList<News> NewsList = _mgr.GetCache();
			return Json(new
			{
				News = NewsList.Select(s => new { s.Id, s.Title })
			});
		}
	}
}