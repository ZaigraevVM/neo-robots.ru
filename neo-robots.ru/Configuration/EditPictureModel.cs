using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;

namespace SMI.Configuration
{
	public class EditPictureModel
	{
		public EditPictureModel()
		{
			Watermark = "";
		}

		public string PictureId { get; set; }
		public int X1 { get; set; }
		public int Y1 { get; set; }
		public int X2 { get; set; }
		public int Y2 { get; set; }

		public int PictureWidth { get; set; }
		public int PictureHeight { get; set; }

		public int Width { get; set; }
		public int Height { get; set; }
		public int SourceId { get; set; }

		public string WebRootPath { get; set; }
		public string Watermark { get; set; }
		public string imageWatermark { get; set; }

		public Guid IdGuid { get; set; }
		public string Extension { get; set; }

		public void Calculate()
		{
			Size Crop = Size(new Size());
			if ((float)Crop.Width / (float)Crop.Height < (float)PictureWidth / (float)PictureHeight)
			{
				Height = PictureHeight;
				Width = (int)((float)Crop.Width * (float)PictureHeight / (float)Crop.Height);
				X1 = (int)(((float)PictureWidth - (float)Width) / 2);
				Y1 = 0;
				X2 = PictureWidth - (int)(((float)PictureWidth - (float)Width) / 2);
				Y2 = PictureHeight;
			}
			else
			{
				Height = (int)((float)PictureWidth * (float)Crop.Height / (float)Crop.Width);
				Width = PictureWidth;
				X1 = 0;
				Y1 = (int)(((float)PictureHeight - (float)Height) / 2);
				X2 = PictureWidth;
				Y2 = PictureHeight - (int)(((float)PictureHeight - (float)Height) / 2);
			}
		}

		public Rectangle Rectangle(Size size)
		{
			var ratioX = (float)size.Width / PictureWidth;
			var ratioY = (float)size.Height / PictureHeight;
			var x = (int)Math.Round(X1 * ratioX);
			var y = (int)Math.Round(Y1 * ratioY);
			var width = (int)Math.Round(Width * ratioX);
			var height = (int)Math.Round(Height * ratioY);

			return new Rectangle(x, y, width, height);
		}

		public string SizeType { get; set; }

		public Size Size(Size size)
		{
			switch (SizeType)
			{
				case "w1000x523":
					return PhotoSizes.W1000x523;
				case "w100x100":
					return PhotoSizes.W100x100;
				case "w200x150":
					return PhotoSizes.W200x150;
				case "w450x150":
					return PhotoSizes.W450x150;
				case "w500x300":
					return PhotoSizes.W500x300;
				case "w890x534":
					return PhotoSizes.W890x534;
				default:
					var ratio = (float)size.Width / PictureWidth;
					return new Size((int)Math.Round(Width * ratio), (int)Math.Round(Height * ratio));
			}
		}

		public string DestPath
		{
			get
			{
				switch (SizeType)
				{
					case "w1000x523":
						return ContentUrl.w1000x523;
					case "w100x100":
						return ContentUrl.w100x100;
					case "w200x150":
						return ContentUrl.w200x150;
					case "w450x150":
						return ContentUrl.w450x150;
					case "w500x300":
						return ContentUrl.w500x300;
					case "w890x534":
						return ContentUrl.w890x534;
					default:
						return ContentUrl.OriginalPhoto;
				}
			}
		}

		public string DestPathOriginal
		{
			get
			{
				return ContentUrl.OriginalPhoto;
			}
		}
	}
}
