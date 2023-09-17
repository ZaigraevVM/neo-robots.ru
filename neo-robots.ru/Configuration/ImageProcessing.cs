using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp;
using System.IO;
using SixLabors.ImageSharp.Processing;

namespace SMI.Configuration
{
    public static class ImageProcessing
    {
		public static string WebRootPath { get; set; }
		public static string SaveCropPictureCentral(EditPictureModel m)
		{
			string fileName = m.PictureId != null && m.PictureId != "" ? m.PictureId : m.IdGuid + m.Extension;
			/*
			if (fileName != null && fileName != "")
			{
				using (Bitmap picture = new Bitmap(m.WebRootPath + m.DestPathOriginal + fileName))
				{
					m.PictureWidth = picture.Width;
					m.PictureHeight = picture.Height;
					m.Calculate();
					SaveCropPicture(picture, m.Size(picture.Size), m.Rectangle(picture.Size), m.WebRootPath + m.DestPath + fileName, m.Watermark);
				}
				return m.WebRootPath + m.DestPath + fileName;
			}
			*/
			return "";
		}

		public static string SaveCropPicture(EditPictureModel m)
		{
            string fileName = m.PictureId != null && m.PictureId != "" ? m.PictureId : m.IdGuid + m.Extension;
            using var imageOrg = Image.Load(m.WebRootPath + m.DestPathOriginal + fileName);
            var tt = m.Size(imageOrg.Size);
			var ww = m.Rectangle(imageOrg.Size);


            using var image = Image.Load("original.jpg");
            image.Mutate(
                x => x
                    .Resize(image.Width / 2, image.Height / 2)
                    .Crop(new Rectangle(m.X1, m.Y1, m.Width, m.Height)));
            //.Crop(new Rectangle(x, y, cropWidth, cropHeight))
            image.Save("result.jpg");

            /*
			if (fileName != null && fileName != "")
			{
				using (Bitmap picture = new Bitmap(m.WebRootPath + m.DestPathOriginal + fileName))
				{
					SaveCropPicture(picture, m.Size(picture.Size), m.Rectangle(picture.Size), m.WebRootPath + m.DestPath + fileName, m.Watermark);
					WebRootPath = m.WebRootPath;
				}
				return m.DestPath + fileName;
			}
			*/
            return "";
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

        public static void SaveCropPictureNew(Size size, Rectangle rectangle, string path, string watermark = "")
        {
            using var image = Image.Load("original.jpg");
            image.Mutate(
				x => x
					.Resize(image.Width / 2, image.Height / 2)
					.Crop(new Rectangle(x, y, cropWidth, cropHeight)));
            //.Crop(new Rectangle(x, y, cropWidth, cropHeight))
            image.Save("result.jpg");

			/*
            using (Image image2 = Image.Load(path))
			using (var inStream = new MemoryStream())
			{
                //image2.Save(inStream, );
				using (var outStream = new MemoryStream())
				using (var image = Image.Load(inStream, out IImageFormat format))
				{
					var clone = image.Clone(
									i => i.Resize(width, height)
										  .Crop(new Rectangle(x, y, cropWidth, cropHeight)));

					clone.Save(outStream, format);
				}
			}
			*/
        }
    }
}
