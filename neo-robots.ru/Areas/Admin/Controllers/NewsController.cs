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
		public NewsController(INewsManager mgr, IViewRender view, IHostingEnvironment hostingEnvironment, IPhotosManager mgrPhotos)
		{
			_mgr = mgr;
			_view = view;
			_hostingEnvironment = hostingEnvironment;
			_mgrPhotos = mgrPhotos;
			ViewBag.WebRootPath = _hostingEnvironment.WebRootPath;
		}

		public ActionResult Index(NewsList m)
		{
			m = _mgr.GetList(m);

			if ("ajax" == m.Type)
				return Json(new
				{
					m.Search,
					Items = _view.Render("~/Areas/Admin/Views/News/ListItems.cshtml", m),
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
			var m = new NewsEdit()
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
			ViewBag.WebRootPath = _hostingEnvironment.WebRootPath;
			return View("Edit", _mgr.EditorData(m));
		}

		[HttpPost]
		public async Task<IActionResult> Save(NewsEdit m, IFormFile file)
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

							var photo = _mgrPhotos.Save(new PhotoEdit() { Name = file.FileName, FileName = IdGuid + Extension });
							m.PhotoId = photo.Id;
						}
						else
						{
							m.EditMessage = "Поддерживаются только такие форматы файлов - .png, .jpg, .gif, .jpeg,";
							TempData["EditMessage"] = m.EditMessage;
						}
					}
					#endregion

					_mgr.Save(m);

					if (file != null)
						return RedirectToAction("edit", new { id = m.Id, returnto = m.GetReturnTo, selecttab = "tabphotos" });

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
			List<News> NewsList = _mgr.GetCache();
			return Json(new
			{
				News = NewsList.Select(s => new { s.Id, s.Title })
			});
		}
	}
}