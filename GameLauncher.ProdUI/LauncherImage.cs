using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace GameLauncher.ProdUI
{
	public class LauncherImage
	{
		private float mDpi;

		public LauncherImage(float mDpi)
		{
			this.mDpi = mDpi;
		}

		public ImageList ResizeImageList(ImageList sourceImageList, float baseDpi)
		{
			ImageList imageList = new ImageList();
			foreach (Image image in sourceImageList.Images)
			{
				imageList.Images.Add(ResizeImage(image, baseDpi));
			}
			return imageList;
		}

		public Image ResizeImage(Image sourceImage, float baseDpi)
		{
			if (mDpi > baseDpi)
			{
				float num = mDpi / baseDpi;
				Image image = new Bitmap((int)((float)sourceImage.Width * num), (int)((float)sourceImage.Height * num));
				using (Graphics graphics = Graphics.FromImage(image))
				{
					graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
					graphics.DrawImage(sourceImage, 0f, 0f, (float)sourceImage.Width * num, (float)sourceImage.Height * num);
					return image;
				}
			}
			return sourceImage;
		}

		public Image ResizeImage(Image sourceImage)
		{
			return ResizeImage(sourceImage, 96f);
		}

		public static Image DownloadImage(string link)
		{
			using (WebClient webClient = new WebClient())
			{
				using (MemoryStream stream = new MemoryStream(webClient.DownloadData(link)))
				{
					return Image.FromStream(stream);
				}
			}
		}

		public static Image SetImgOpacity(Image imgPic, float imgOpac)
		{
			Bitmap bitmap = new Bitmap(imgPic.Width, imgPic.Height);
			Graphics graphics = Graphics.FromImage(bitmap);
			ColorMatrix colorMatrix = new ColorMatrix();
			colorMatrix.Matrix33 = imgOpac;
			ImageAttributes imageAttributes = new ImageAttributes();
			imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
			graphics.DrawImage(imgPic, new Rectangle(0, 0, bitmap.Width, bitmap.Height), 0, 0, imgPic.Width, imgPic.Height, GraphicsUnit.Pixel, imageAttributes);
			graphics.Dispose();
			return bitmap;
		}
	}
}
