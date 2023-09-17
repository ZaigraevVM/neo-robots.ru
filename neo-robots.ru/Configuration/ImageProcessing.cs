using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IO;

namespace SMI.Configuration
{
    public static class ImageProcessing
    {
        public static string WebRootPath { get; set; }
        public static string SaveCropPictureCentral(EditPictureModel m)
        {
            string fileName = m.PictureId != null && m.PictureId != "" ? m.PictureId : m.IdGuid + m.Extension;

            if (fileName != null && fileName != "")
            {
                using var image = Image.Load(m.WebRootPath + m.DestPathOriginal + fileName);
                m.PictureWidth = image.Width;
                m.PictureHeight = image.Height;
                m.Calculate();
                SaveCropPicture(image, m.Size(image.Size), m.Rectangle(image.Size), m.WebRootPath + m.DestPath + fileName, m.Watermark);
            }
            return "";
        }

        public static string SaveCropPicture(EditPictureModel m)
        {
            string fileName = m.PictureId != null && m.PictureId != "" ? m.PictureId : m.IdGuid + m.Extension;
            if (fileName != null && fileName != "")
            {
                using var image = Image.Load(m.WebRootPath + m.DestPathOriginal + fileName);
                SaveCropPicture(image, m.Size(image.Size), m.Rectangle(image.Size), m.WebRootPath + m.DestPath + fileName, m.Watermark);
                WebRootPath = m.WebRootPath;

                return m.DestPath + fileName;
            }
            return "";
        }

        public static void SaveCropPicture(Image image, Size size, Rectangle rectangle, string path, string watermark = "")
        {
            image.Mutate(i => i.Crop(rectangle).Resize(size.Width, size.Height));
            image.Save(path);
        }
    }
}