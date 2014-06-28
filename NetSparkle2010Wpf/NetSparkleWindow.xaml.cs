using NetSparkle.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Linq;
using NetSparkle;

namespace NetSparkle2010Wpf
{
	/// <summary>
	/// The main form
	/// </summary>
	public partial class NetSparkleWpfWindow : Window, INetSparkleForm
	{
		System.Windows.Forms.DialogResult winFormsDialogResult = System.Windows.Forms.DialogResult.None;

		private static readonly HashSet<string> MarkDownExtension = new HashSet<string> { ".md", ".mkdn", ".mkd", ".markdown" };

		private readonly NetSparkleAppCastItem[] _updates;

		/// <summary>
		/// Event fired when the user has responded to the 
		/// skip, later, install question.
		/// </summary>
		public event EventHandler UserResponded;

		/// <summary>
		/// Template for HTML code drawig release notes separator. {0} used for version number, {1} for publication date
		/// </summary>
		public string SeparatorTemplate { get; set; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="items">List of updates to show</param>
		/// <param name="applicationIcon"></param>
		public NetSparkleWpfWindow(NetSparkleAppCastItem[] items, Icon applicationIcon)
		{
			_updates = items;

			SeparatorTemplate = "<div style=\"border: 1px black dashed; padding: 5px; margin-bottom: 5px; margin-top: 5px;\"><span style=\"float: right; display:float;\">{1}</span>{0}</div>";

			InitializeComponent();

			// init ui 
			try
			{
				netSparkleBrowser.AllowDrop = false;
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Error in browser init: " + ex.Message);
			}

			NetSparkleAppCastItem item = items[0];


			lblHeader.Content = ((string)lblHeader.Content).Replace("APP", item.AppName);
			lblInfoText.Text = lblInfoText.Text.Replace("APP", item.AppName + " " + item.Version);
			lblInfoText.Text = lblInfoText.Text.Replace("OLDVERSION", item.AppVersionInstalled);

			if (items.Length == 0)
			{
				RemoveReleaseNotesControls();
			}
			else
			{
				StringBuilder sb = new StringBuilder("<html><head><meta http-equiv='Content-Type' content='text/html;charset=UTF-8'></head><body>");
				foreach (NetSparkleAppCastItem castItem in items)
				{
					sb.Append(string.Format(SeparatorTemplate, castItem.Version,
											castItem.PublicationDate.ToString("dd MMM yyyy")));
					sb.Append(GetReleaseNotes(castItem));
				}
				sb.Append("</body>");

				string releaseNotes = sb.ToString();
				netSparkleBrowser.NavigateToString(releaseNotes);
			}

			imgAppIcon.Source = NetSparkleWpfHelper.IconToBitmap(applicationIcon);
			Icon = NetSparkleWpfHelper.IconToImageSource(applicationIcon);

			Topmost = true;
		}

		private string GetReleaseNotes(NetSparkleAppCastItem item)
		{
			if (string.IsNullOrEmpty(item.ReleaseNotesLink))
				return null;

			string notes = DownloadReleaseNotes(item.ReleaseNotesLink);
			if (string.IsNullOrEmpty(notes))
				return null;

			var extension = System.IO.Path.GetExtension(item.ReleaseNotesLink);
			if (extension != null && MarkDownExtension.Contains(extension.ToLower()))
			{
				try
				{
					var md = new MarkdownSharp.Markdown();
					notes = md.Transform(notes);
				}
				catch (Exception ex)
				{
					Debug.WriteLine("Error parsing MarkDown syntax: " + ex.Message);
				}
			}
			return notes;
		}

		private string DownloadReleaseNotes(string link)
		{
			try
			{
				using (var webClient = new WebClient())
				{
					webClient.Encoding = Encoding.UTF8;
					return webClient.DownloadString(link);
				}
			}
			catch (WebException ex)
			{
				Debug.WriteLine("Cannot download release notes from " + link + " because " + ex.Message);
				return "";
			}
		}

		/// <summary>
		/// The current item being installed
		/// </summary>
		NetSparkleAppCastItem INetSparkleForm.CurrentItem
		{
			get { return _updates[0]; }
		}

		/// <summary>
		/// The result of ShowDialog()
		/// </summary>
		DialogResult INetSparkleForm.Result
		{
			get { return winFormsDialogResult; }
		}

		/// <summary>
		/// Hides the release notes
		/// </summary>
		void INetSparkleForm.HideReleaseNotes()
		{
			RemoveReleaseNotesControls();
		}

		/// <summary>
		/// Shows the dialog
		/// </summary>
		void INetSparkleForm.Show()
		{
			ShowDialog();
			if (UserResponded != null)
			{
				UserResponded(this, new EventArgs());
			}
		}

		/// <summary>
		/// Removes the release notes control
		/// </summary>
		private void RemoveReleaseNotesControls()
		{
			if (label3.Parent == null)
				return;

			// calc new size
			double newHeight = this.Height - label3.Height - panel1.Height;

			// remove the no more needed controls      
			grid1.Children.Remove(label3);
			panel1.Children.Remove(netSparkleBrowser);
			grid1.Children.Remove(panel1);

			// resize the window
			/*this.MinimumSize = newSize;
			this.Size = this.MinimumSize;
			this.MaximumSize = this.MinimumSize;*/
			this.Height = newHeight;
		}

		/// <summary>
		/// Event called when the skip button is clicked
		/// </summary>
		/// <param name="sender">not used.</param>
		/// <param name="e">not used.</param>
		private void skipButton_Click(object sender, RoutedEventArgs e)
		{
			// set the dialog result to no
			winFormsDialogResult = System.Windows.Forms.DialogResult.No;

			// close the windows
			Close();
		}

		/// <summary>
		/// Event called when the "remind me later" button is clicked
		/// </summary>
		/// <param name="sender">not used.</param>
		/// <param name="e">not used.</param>
		private void buttonRemind_Click(object sender, RoutedEventArgs e)
		{
			// set the dialog result ot retry
			winFormsDialogResult = System.Windows.Forms.DialogResult.Retry;

			// close the window
			Close();
		}

		/// <summary>
		/// Called when the "Update button" is clicked
		/// </summary>
		/// <param name="sender">not used.</param>
		/// <param name="e">not used.</param>
		private void updateButton_Click(object sender, RoutedEventArgs e)
		{
			// set the result to yes
			winFormsDialogResult = System.Windows.Forms.DialogResult.Yes;

			// close the dialog
			Close();
		}


	}
}
