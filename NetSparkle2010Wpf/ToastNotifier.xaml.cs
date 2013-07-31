using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Threading;

namespace NetSparkle2010Wpf
{
	/// <summary>
	/// Like a notification ballon, but more reliable "toast" because it slowly goes up, then down.
	/// Subscribe to the Click even to know if the user clicked on it.
	/// </summary>
	public partial class ToastNotifier : Window
	{
		private DispatcherTimer _goUpTimer;
		private DispatcherTimer _goDownTimer;
		private DispatcherTimer _pauseTimer;
		private double startPosX;
		private double startPosY;

		/// <summary>
		/// The user clicked on the toast popup
		/// </summary>
		public event EventHandler ToastClicked;



		/// <summary>
		/// constructor
		/// </summary>
		public ToastNotifier()
		{
			InitializeComponent();
			// We want our window to be the top most
			Topmost = true;
			// Pop doesn't need to be shown in task bar
			ShowInTaskbar = false;
			// Create and run timer for animation
			_goUpTimer = new DispatcherTimer();
			_goUpTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);
			_goUpTimer.Tick += GoUpTimerTick;
			_goDownTimer = new DispatcherTimer();
			_goDownTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);
			_goDownTimer.Tick += GoDownTimerTick;
			_pauseTimer = new DispatcherTimer();
			_pauseTimer.Interval = new TimeSpan(0, 0, 0, 0, 15000);
			_pauseTimer.Tick += PauseTimerTick;
		}

		private void PauseTimerTick(object sender, EventArgs e)
		{
			_pauseTimer.Stop();
			_goDownTimer.Start();
		}

		/// <summary>
		/// 
		/// </summary>
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			// Move window out of screen
			startPosX = SystemParameters.WorkArea.Width - Width;
			startPosY = SystemParameters.WorkArea.Height;
			SetDesktopLocation(startPosX, startPosY);
			// Begin animation
			_goUpTimer.Start();
		}

		private void SetDesktopLocation(double posX, double posY)
		{
			Left = posX;
			Top = posY;
		}

		void GoUpTimerTick(object sender, EventArgs e)
		{
			//Lift window by 5 pixels
			startPosY -= 5;
			//If window is fully visible stop the timer
			if (startPosY < SystemParameters.WorkArea.Height - Height)
			{
				_goUpTimer.Stop();
				_pauseTimer.Start();
			}
			else
				SetDesktopLocation(startPosX, startPosY);
		}

		private void GoDownTimerTick(object sender, EventArgs e)
		{
			//Lower window by 5 pixels
			startPosY += 5;
			//If window is fully visible stop the timer
			if (startPosY > SystemParameters.WorkArea.Height)
			{
				_goDownTimer.Stop();
				Close();
			}
			else
				SetDesktopLocation(startPosX, startPosY);
		}

		private void Window_MouseUp(object sender, MouseButtonEventArgs e)
		{
			//DialogResult = true; // this line causes exception
			Close();
			EventHandler handler = ToastClicked;
			if (handler != null) handler(this, e);
		}

		/// <summary>
		/// Show the toast
		/// </summary>
		/// <param name="message"></param>
		/// <param name="callToAction">Text of the hyperlink </param>
		/// <param name="seconds">How long to show before it goes back down</param>
		public void Show(string message, string callToAction, int seconds)
		{
			_message.Content = message;
			_callToAction.Inlines.Clear();
			_callToAction.Inlines.Add(callToAction);
			_pauseTimer.Interval = new TimeSpan(0, 0, 0, 1000 * seconds);
			Show();
		}

		private void Hyperlink_Click(object sender, RoutedEventArgs e)
		{
			this.Window_MouseUp(null, null);
		}

	}
}
