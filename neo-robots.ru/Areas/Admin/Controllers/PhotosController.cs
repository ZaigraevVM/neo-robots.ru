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
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using SMI.Configuration;

namespace SMI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class PhotosController : Controller
    {
        private readonly IPhotosManager _mgr;
        private readonly IViewRender _view;
        private readonly ILogger<PhotosController> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        public PhotosController(IPhotosManager mgr, IViewRender view, ILogger<PhotosController> logger, IHostingEnvironment hostingEnvironment)
        {
            _mgr = mgr;
            _view = view;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> IndexAsync(PhotosList m)
        {
            m = await _mgr.GetListAsync(m);

            if ("ajax" == m.Type)
                return Json(new
                {
                    m.Search,
                    Items = _view.Render("~/Areas/Admin/Views/Photos/ListItems.cshtml", m),
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when remove news");
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CreateAsync()
        {
            var m = new PhotoEdit()
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
        public async Task<IActionResult> SaveAsync(PhotoEdit m, IFormFile file)
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

                            m.FileName= IdGuid + Extension;
                        }
                        else
                        {
                            m.EditMessage = "Поддерживаются только такие форматы файлов - .png, .jpg, .gif, .jpeg,";
                            TempData["EditMessage"] = m.EditMessage;
                        }
                    }
                    #endregion

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

        public async Task<IActionResult> GetListAsync()
        {
            IList<Photo> PhotoList = _mgr.GetCache();
            return Json(new
            {
                Photo = PhotoList.Select(s => new { s.Id, s.Name })
            });
        }

        [HttpPost]
        public ActionResult CropPicture(EditPictureModel m)
        {
            m.WebRootPath = _hostingEnvironment.WebRootPath;
            return Json(new { path = ImageProcessing.SaveCropPicture(m) + "?v=" + DateTime.Now.ToString(), type = m.SizeType });
        }
    }
}