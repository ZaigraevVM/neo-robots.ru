/*
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
*/

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
//using System.Drawing;
using System.IO;
//using System.Drawing;

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
                //image.Mutate(x => x.Resize(image.Width / 2, image.Height / 2));
                //image.Save(m.WebRootPath + m.DestPath + fileName);

                /*
                Bitmap picture = new Bitmap(m.WebRootPath + m.DestPathOriginal + fileName);
				m.PictureWidth = picture.Width;
				m.PictureHeight = picture.Height;
				m.Calculate();
				SaveCropPicture(picture, m.Size(picture.Size), m.Rectangle(picture.Size), m.WebRootPath + m.DestPath + fileName, m.Watermark);
				return m.WebRootPath + m.DestPath + fileName;
				*/
            }
			return "";
		}

		public static string SaveCropPicture(EditPictureModel m)
		{
            /*
			string fileName = m.PictureId != null && m.PictureId != "" ? m.PictureId : m.IdGuid + m.Extension;

			if (fileName != null && fileName != "")
			{
				Bitmap picture = new Bitmap(m.WebRootPath + m.DestPathOriginal + fileName);
				SaveCropPicture(picture, m.Size(picture.Size), m.Rectangle(picture.Size), m.WebRootPath + m.DestPath + fileName, m.Watermark);
				WebRootPath = m.WebRootPath;

				return m.DestPath + fileName;
			}
			*/

            return "";			
		}

        public static void SaveCropPicture(Image image, Size size, Rectangle rectangle, string path, string watermark = "")
		{
            //image.Mutate(i => i.Resize(size.Width, size.Height).Crop(rectangle));
            image.Mutate(i => i.Crop(rectangle).Resize(size.Width, size.Height));
            image.Save(path);
        }

        /*
		public static void SaveCropPicture(Bitmap picture, Size size, Rectangle rectangle, string path, string watermark = "")
		{
			using (var newPicture = new Bitmap(size.Width, size.Height))
			{
				using (var gr = Graphics.FromImage(newPicture))
				{
					gr.SmoothingMode = SmoothingMode.HighQuality;
					gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
					gr.PixelOffsetMode = PixelOffsetMode.HighQuality;

					gr.DrawImage(picture, new Rectangle(new Point(0, 0), size), rectangle, GraphicsUnit.Pixel);

				}

				newPicture.MakeTransparent(Color.White);

				if (watermark != null && watermark != "")
				{
					using (Graphics g = Graphics.FromImage(newPicture))
					{
						Rectangle rect1 = new Rectangle(0, 0, size.Width, size.Height);

						StringFormat stringFormat = new StringFormat();
						stringFormat.Alignment = StringAlignment.Far;
						stringFormat.LineAlignment = StringAlignment.Far;

						Font font1 = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Point);

						g.DrawString("© " + watermark, font1, new SolidBrush(Color.FromArgb(128, 0, 0, 0)), new Rectangle(0, 0, size.Width - 9, size.Height - 9), stringFormat);
						g.DrawString("© " + watermark, font1, new SolidBrush(Color.FromArgb(128, 185, 184, 189)), new Rectangle(0, 0, size.Width - 10, size.Height - 10), stringFormat);
					}
				}

				newPicture.Save(path, ImageFormat.Png);
			}
		}
		*/
    }
}
