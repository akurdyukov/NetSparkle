using NetSparkle;
using NetSparkle.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NetSparkle2010Wpf
{
	/// <summary>
	/// A progress bar
	/// </summary>
	public partial class NetSparkleDownloadProgress : Window, INetSparkleDownloadProgress
	{
		/// <summary>
		/// event to fire when the form asks the application to be relaunched
		/// </summary>
		public event EventHandler InstallAndRelaunch;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="item"></param>
		/// <param name="applicationIcon">Your application Icon</param>
		public NetSparkleDownloadProgress(NetSparkleAppCastItem item, Icon applicationIcon)
		{
			InitializeComponent();

			imgAppIcon.Source = NetSparkleWpfHelper.IconToBitmap(applicationIcon);
			Icon = NetSparkleWpfHelper.IconToImageSource(applicationIcon);

			// init ui
			btnInstallAndReLaunch.Visibility = Visibility.Hidden;
			lblHeader.Content = ((string)lblHeader.Content).Replace("APP", item.AppName + " " + item.Version);
			progressDownload.Maximum = 100;
			progressDownload.Minimum = 0;

			// show the right 
			//this.Height = 107;
			lblSecurityHint.Visibility = Visibility.Collapsed;
		}

		/// <summary>
		/// Show the UI and waits
		/// </summary>
		void INetSparkleDownloadProgress.ShowDialog()
		{
			base.ShowDialog();
		}

		/// <summary>
		/// Update UI to show file is downloaded and signature check result
		/// </summary>
		/// <param name="signatureValid"></param>
		public void ChangeDownloadState(bool signatureValid)
		{
			progressDownload.Visibility = Visibility.Hidden;
			btnInstallAndReLaunch.Visibility = Visibility.Visible;

			UpdateDownloadValid(signatureValid);
		}

		/// <summary>
		/// Force window close
		/// </summary>
		public void ForceClose()
		{
			DialogResult = false;
			Close();
		}

		/// <summary>
		/// Updates the UI to indicate if the download is valid
		/// </summary>
		private void UpdateDownloadValid(bool signatureValid)
		{
			if (!signatureValid)
			{
				this.Height = 137;
				lblSecurityHint.Visibility = Visibility.Visible;
				Background = System.Windows.Media.Brushes.Tomato;
			}
		}

		/// <summary>
		/// Event called when the client download progress changes
		/// </summary>
		/// <param name="sender">not used.</param>
		/// <param name="e">not used.</param>
		public void OnClientDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			progressDownload.Value = e.ProgressPercentage;
		}

		/// <summary>
		/// Event called when the "Install and relaunch" button is clicked
		/// </summary>
		/// <param name="sender">not used.</param>
		/// <param name="e">not used.</param>
		private void btnInstallAndReLaunch_Click(object sender, RoutedEventArgs e)
		{
			if (InstallAndRelaunch != null)
			{
				InstallAndRelaunch(this, new EventArgs());
			}
		}

	}
}
