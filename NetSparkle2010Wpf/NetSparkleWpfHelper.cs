using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NetSparkle2010Wpf
{
	public class NetSparkleWpfHelper
	{
		/// <summary>
		/// Converts instance of System.Drawing.Icon to System.Windows.Media.Imaging.BitmapImage
		/// </summary>
		/// <param name="applicationIcon">source</param>
		/// <returns></returns>
		public static BitmapImage IconToBitmap(Icon applicationIcon)
		{
			Bitmap bmp = applicationIcon.ToBitmap();

			MemoryStream ms = new MemoryStream();
			bmp.Save(ms, ImageFormat.Bmp);
			ms.Position = 0;
			BitmapImage bi = new BitmapImage();
			bi.BeginInit();
			bi.StreamSource = ms;
			bi.EndInit();

			return bi;
		}

		/// <summary>
		/// Converts instance of System.Drawing.Icon to System.Windows.Media.ImageSource
		/// </summary>
		/// <param name="applicationIcon">source icon</param>
		/// <returns></returns>
		public static ImageSource IconToImageSource(Icon applicationIcon)
		{
			Bitmap bmp = applicationIcon.ToBitmap();

			MemoryStream ms = new MemoryStream();
			bmp.Save(ms, ImageFormat.Bmp);

			return BitmapFrame.Create(ms);
		}

		/// <summary>
		/// Converts instance of System.Drawing.Image to System.Windows.Media.ImageSource
		/// </summary>
		/// <param name="image"></param>
		/// <returns></returns>
		public static ImageSource FromImage(Image image)
		{
			MemoryStream ms = new MemoryStream();
			image.Save(ms, ImageFormat.Bmp);

			return BitmapFrame.Create(ms);
		}


		//internal static System.Windows.Forms.DialogResult ConvertDialogResult(bool? DialogResult)
		//{
		//	System.Windows.Forms.DialogResult result = System.Windows.Forms.DialogResult.None;

		//	switch (DialogResult)
		//	{
		//		case null:
		//			result = System.Windows.Forms.DialogResult.None;
		//			break;
		//		case true:
		//			result = System.Windows.Forms.DialogResult.OK;
		//			break;
		//		case false:
		//			result = System.Windows.Forms.DialogResult.Cancel;
		//			break;
		//	}

		//	return result;

		//	//			// Summary:
		//	////     Nothing is returned from the dialog box. This means that the modal dialog
		//	////     continues running.
		//	//None = 0,
		//	////
		//	//// Summary:
		//	////     The dialog box return value is OK (usually sent from a button labeled OK).
		//	//OK = 1,
		//	////
		//	//// Summary:
		//	////     The dialog box return value is Cancel (usually sent from a button labeled
		//	////     Cancel).
		//	//Cancel = 2,
		//	////
		//	//// Summary:
		//	////     The dialog box return value is Abort (usually sent from a button labeled
		//	////     Abort).
		//	//Abort = 3,
		//	////
		//	//// Summary:
		//	////     The dialog box return value is Retry (usually sent from a button labeled
		//	////     Retry).
		//	//Retry = 4,
		//	////
		//	//// Summary:
		//	////     The dialog box return value is Ignore (usually sent from a button labeled
		//	////     Ignore).
		//	//Ignore = 5,
		//	////
		//	//// Summary:
		//	////     The dialog box return value is Yes (usually sent from a button labeled Yes).
		//	//Yes = 6,
		//	////
		//	//// Summary:
		//	////     The dialog box return value is No (usually sent from a button labeled No).
		//	//No = 7,

		//}
	}
}
