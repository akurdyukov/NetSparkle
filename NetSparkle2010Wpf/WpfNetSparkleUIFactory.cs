using System;
using System.Drawing;
using NetSparkle.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSparkle;
using System.Windows;
using NetSparkle.Properties;
using System.IO;
using System.Windows.Media.Imaging;

namespace NetSparkle2010Wpf
{
	/// <summary>
	/// UI factory for default interface
	/// </summary>
	public class WpfNetSparkleUIFactory : INetSparkleUIFactory
	{
		/// <summary>
		/// Create sparkle form implementation
		/// </summary>
		/// <param name="updates">Sorted array of updates from latest to previous</param>
		/// <param name="applicationIcon">Icon</param>
		/// <returns></returns>
		public virtual INetSparkleForm CreateSparkleForm(NetSparkleAppCastItem[] updates, Icon applicationIcon)
		{
			return new NetSparkleWpfWindow(updates, applicationIcon);
		}

		/// <summary>
		/// Create download progress window
		/// </summary>
		/// <param name="item">Appcast item to download</param>
		/// <param name="applicationIcon">Application icon to use</param>
		/// <returns></returns>
		public virtual INetSparkleDownloadProgress CreateProgressWindow(NetSparkleAppCastItem item, Icon applicationIcon)
		{
			return new NetSparkleDownloadProgress(item, applicationIcon);
		}

		/// <summary>
		/// Initialize UI. Called when Sparkle is constructed.
		/// </summary>
		public virtual void Init()
		{
		}

		/// <summary>
		/// Show user a message saying downloaded update format is unknown
		/// </summary>
		/// <param name="downloadFileName"></param>
		public virtual void ShowUnknownInstallerFormatMessage(string downloadFileName)
		{
			MessageBox.Show(
				string.Format(Resources.DefaultNetSparkleUIFactory_ShowUnknownInstallerFormatMessageText, downloadFileName),
				Resources.DefaultNetSparkleUIFactory_ErrorTitle,
				MessageBoxButton.OK,
				MessageBoxImage.Error);
		}

		/// <summary>
		/// Show user that current installed version is up-to-date
		/// </summary>
		public virtual void ShowVersionIsUpToDate()
		{
			MessageBox.Show(
				Resources.DefaultNetSparkleUIFactory_ShowVersionIsUpToDateMessage,
				Resources.DefaultNetSparkleUIFactory_MessageTitle);
		}

		/// <summary>
		/// Show message that latest update was skipped by user
		/// </summary>
		public virtual void ShowVersionIsSkippedByUserRequest()
		{
			MessageBox.Show(
				Resources.DefaultNetSparkleUIFactory_ShowVersionIsSkippedByUserRequestMessage,
				Resources.DefaultNetSparkleUIFactory_MessageTitle);//review: I'm not crystal clear on this one
		}

		/// <summary>
		/// Show message that appcast is not available
		/// </summary>
		/// <param name="appcastUrl"></param>
		public virtual void ShowCannotDownloadAppcast(string appcastUrl)
		{
			MessageBox.Show(
				Resources.DefaultNetSparkleUIFactory_ShowCannotDownloadAppcastMessage,
				Resources.DefaultNetSparkleUIFactory_ErrorTitle);
		}

		/// <summary>
		/// Show 'toast' window to notify new version is available
		/// </summary>
		/// <param name="updates">Appcast updates</param>
		/// <param name="applicationIcon">Icon to use in window</param>
		/// <param name="clickHandler">handler for click</param>
		public virtual void ShowToast(NetSparkleAppCastItem[] updates, Icon applicationIcon, Action<NetSparkleAppCastItem[]> clickHandler)
		{
			BitmapImage bi = NetSparkleWpfHelper.IconToBitmap(applicationIcon);

			var toast = new ToastNotifier
				{
					Image =
						{
							Source = bi
						}
				};
			toast.ToastClicked += (sender, args) => clickHandler(updates); // TODO: this is leak
			toast.Show(Resources.DefaultNetSparkleUIFactory_ToastMessage, Resources.DefaultNetSparkleUIFactory_ToastCallToAction, 5);
		}

		/// <summary>
		/// Show message on download error
		/// </summary>
		/// <param name="message">Error message from exception</param>
		/// <param name="appCastUrl"></param>
		public virtual void ShowDownloadErrorMessage(string message, string appCastUrl)
		{
			MessageBox.Show(
				string.Format(Resources.DefaultNetSparkleUIFactory_ShowDownloadErrorMessage, message),
				Resources.DefaultNetSparkleUIFactory_ErrorTitle);
		}
	}
}
