using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Arduino.Firmata.Types.Proxy;
using POFerro.Robotics.ArduinoSonarTurret.Proxy;
using POFerro.Robotics.Wanderer.Proxy;

namespace POFerro.Robotics.WandererDashboard
{
	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
	public partial class WandererDashboardWPF : Window, INotifyPropertyChanged
	{
		public ObservableCollection<RadarLine> RadarLines { get; set; }
		public ObservableCollection<PinInfo> PinsList { get; set; }
		public WandererState State { get; set; }

		public WandererDashboardWPF()
		{
			InitializeComponent();

			this.RadarLines = new ObservableCollection<RadarLine>();
			PinsList = new ObservableCollection<PinInfo>(Enum.GetValues(typeof(Pins)).Cast<Pins>().Where(p => p != Pins.None).Select(p => p >= Pins.A0 ? new AnalogPinInfo() { Pin = (int)p } : new PinInfo() { Pin = (int)p }));

			this.DataContext = this;
			((IRDistanceToBrushConverter)this.FindResource("irDistanceBrushConverter")).Window = this;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string propName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
		}

		int maxAngle = -1;
		public void UpdateState(WandererState state)
		{
			if (state.BestAngle > 0 && this.State != null && this.State.BestAngle != state.BestAngle)
			{
				foreach (var item in RadarLines.Where(l => l.IsBestAngle)) { item.IsBestAngle = false; }
				var bestLine = RadarLines.FirstOrDefault(l => l.Angle == state.BestAngle);
				if (bestLine != null)
					bestLine.IsBestAngle = true;

				if (this.picDepthInfoOriginal.Source != null && this.picDepthInfoOriginal.ActualWidth > 0 && this.picDepthInfoOriginal.ActualHeight > 0)
				{
					this.picDepthInfoOriginal.Source = DrawBestAngleElipse(this.picDepthInfoOriginal.Source, state.BestAngle, maxAngle, this.picDepthInfoOriginal.ActualWidth, this.picDepthInfoOriginal.ActualHeight);
				}
				if (this.picDepthInfo.Source != null && this.picDepthInfo.ActualWidth > 0 && this.picDepthInfo.ActualHeight > 0)
				{
					this.picDepthInfo.Source = DrawBestAngleElipse(this.picDepthInfo.Source, state.BestAngle, maxAngle, this.picDepthInfo.ActualWidth, this.picDepthInfo.ActualHeight);
				}
			}
			this.State = state;
			this.OnPropertyChanged("State");
		}

		public ImageSource DrawBestAngleElipse(ImageSource depthPicture, int bestAngle, int maxAngle, double projectedWidth, double projectedHeight)
		{
			DrawingVisual drawingVisual = new DrawingVisual();
			DrawingContext drawingContext = drawingVisual.RenderOpen();
			drawingContext.DrawImage(depthPicture, new Rect(new Size(projectedWidth, projectedHeight)));
			drawingContext.DrawEllipse(Brushes.Transparent, new Pen(Brushes.Green, 5), new Point(bestAngle * projectedWidth / maxAngle, projectedHeight / 2), projectedHeight / 2, projectedHeight);
			drawingContext.Close();

			RenderTargetBitmap depthBMP = new RenderTargetBitmap((int)projectedWidth, (int)projectedHeight, 96, 96, PixelFormats.Default);
			depthBMP.Render(drawingVisual);

			return depthBMP;
		}

		public static double DegreeToRadian(double degree)
		{
			return degree * (Math.PI / 180.0);
		}

		public static double RadianToDegree(double radian)
		{
			return radian * (180.0 / Math.PI);
		}

		protected BitmapSource CreateDepthProfile(double [] depth, double maxDistance, double angleStep)
		{
			int width = (int)depth.Length;
			int height = (int)28;
			int stride = width * 3;

			byte[] data = new byte[height * stride];
			byte[] lineData = new byte[stride];
			for (int i = 0; i < lineData.Length - 3; i += 3)
			{
				double distance = depth[(int)((double)i * (double)depth.Length / lineData.Length)];
				lineData[i] = 
				lineData[i + 1] = 
				lineData[i + 2] = (byte)((distance * 255) / maxDistance);
					
			}
			for (int i = 0; i < height; i++)
			{
				lineData.CopyTo(data, i * stride);
			}

			return BitmapSource.Create(
				width,
				height, 96, 96,
				PixelFormats.Rgb24, null, data,
				stride);
		}

		public void RangeSweepCompleted(ArduinoSonarTurretState arduinoSonarTurretState)
		{
			double[] normalizedDepth = new double[arduinoSonarTurretState.DistanceMeasurements.Length];
			int numSamples = /* Sonar lateral resolution */ 20 / (int)arduinoSonarTurretState.AngularResolution;
			for (int i = 0; i < arduinoSonarTurretState.DistanceMeasurements.Length; i++)
			{
				normalizedDepth[i] = 0;
				for (int numAvg = Math.Max(0, i - (numSamples / 2)); numAvg < Math.Min(i + (numSamples / 2), arduinoSonarTurretState.DistanceMeasurements.Length); numAvg++)
				{
					normalizedDepth[i] += arduinoSonarTurretState.DistanceMeasurements[numAvg];
				}
				normalizedDepth[i] /= numSamples;
			}

			this.picDepthInfoOriginal.Source = CreateDepthProfile(arduinoSonarTurretState.DistanceMeasurements, arduinoSonarTurretState.MaxDistance, /*RadianToDegree(*/arduinoSonarTurretState.AngularResolution/*)*/);
			this.picDepthInfo.Source = CreateDepthProfile(normalizedDepth, arduinoSonarTurretState.MaxDistance, /*RadianToDegree(*/arduinoSonarTurretState.AngularResolution/*)*/);

			maxAngle = (int)this.RadarLines.Max(l => l.Angle);
		}

		public void UpdateRadar(RangePositionRead rangePositionRead)
		{
			foreach (var radarLine in RadarLines)
			{
				radarLine.Alpha = (byte)Math.Max((int)radarLine.Alpha - rangePositionRead.SweepAngularStep, 0);
			}
			var line = RadarLines.FirstOrDefault(l => l.Angle == rangePositionRead.CurrentAngle);
			if (line == null)
				RadarLines.Add(new RadarLine(rangePositionRead.DistanceMeasurement * 100, rangePositionRead.CurrentAngle));
			else
			{
				line.ResetAlfa();
				line.Distance = rangePositionRead.DistanceMeasurement * 100;
			}
		}

		public void UpdateDigitalPin(Arduino.Messages.Proxy.DigitalOutputUpdateRequest digitalOutputUpdateRequest)
		{
			PinInfo pin = PinsList[(int)digitalOutputUpdateRequest.CurrentPin - 1];
			if (digitalOutputUpdateRequest.CurrentPinMode == PinMode.Servo)
				if (pin is ServoPinInfo)
					pin.Value = digitalOutputUpdateRequest.Value;
				else
					PinsList[(int)digitalOutputUpdateRequest.CurrentPin - 1] = new ServoPinInfo() { Pin = (int)digitalOutputUpdateRequest.CurrentPin, Value = digitalOutputUpdateRequest.Value };
			else if (digitalOutputUpdateRequest.CurrentPinMode == PinMode.PWM)
				if (pin is PWMPinInfo)
					pin.Value = digitalOutputUpdateRequest.Value;
				else
					PinsList[(int)digitalOutputUpdateRequest.CurrentPin - 1] = new PWMPinInfo() { Pin = (int)digitalOutputUpdateRequest.CurrentPin, Value = digitalOutputUpdateRequest.Value };
			else if (pin is DigitalPinInfo)
			{
				pin.Mode = digitalOutputUpdateRequest.CurrentPinMode;
				pin.Value = digitalOutputUpdateRequest.Value;
			}
			else
				PinsList[(int)digitalOutputUpdateRequest.CurrentPin - 1] = new DigitalPinInfo(digitalOutputUpdateRequest.CurrentPinMode) { Pin = (int)digitalOutputUpdateRequest.CurrentPin, Value = digitalOutputUpdateRequest.Value };
		}

		public void UpdateAnalogPin(Arduino.Messages.Proxy.AnalogOutputUpdateRequest analogOutputUpdateRequest)
		{
			PinInfo pin = PinsList[(int)analogOutputUpdateRequest.CurrentPin - 1];
			if (pin is AnalogPinInfo)
				pin.Value = analogOutputUpdateRequest.Value;
			else
				PinsList[(int)analogOutputUpdateRequest.CurrentPin - 1] = new AnalogPinInfo() { Pin = (int)analogOutputUpdateRequest.CurrentPin, Value = analogOutputUpdateRequest.Value };
		}
	}

	public class PinInfo : INotifyPropertyChanged
	{
		public int Pin { get; set; }
		private PinMode mode;
		public PinMode Mode
		{
			get { return mode; }
			set
			{
				if (this.mode != value)
				{
					this.mode = value;
					this.OnPropertyChanged("Mode");
				}
			}
		}
		private int value;
		public int Value
		{
			get { return value; }
			set
			{
				if (this.value != value)
				{
					this.value = value;
					this.OnPropertyChanged("Value");
				}
			}
		}

		protected void OnPropertyChanged(string propName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
		}
		public event PropertyChangedEventHandler PropertyChanged;

		public PinInfo()
		{
			this.value = -1;
		}
	}

	public class AnalogPinInfo : PinInfo
	{
		public AnalogPinInfo()
		{
			this.Mode = PinMode.Analog;
		}
	}

	public class PWMPinInfo : PinInfo
	{
		public PWMPinInfo()
		{
			this.Mode = PinMode.PWM;
		}
	}

	public class ServoPinInfo : PinInfo
	{
		public ServoPinInfo()
		{
			this.Mode = PinMode.Servo;
		}
	}

	public class DigitalPinInfo : PinInfo
	{
		public DigitalPinInfo(PinMode mode)
		{
			this.Mode = mode;
		}
	}

	public class RadarLine : INotifyPropertyChanged
	{
		const int lineSize = 4;

		private double angle;
		public double Angle
		{
			get { return angle; }
			set
			{
				if (this.angle != value)
				{
					angle = value;
					this.OnPropertyChanged("Angle");
				}
			}
		}

		private double distance;
		public double Distance
		{
			get { return distance; }
			set
			{
				if (this.distance != value)
				{
					distance = value;
					this.OnPropertyChanged("Distance");
				}
			}
		}

		private int alpha;
		public int Alpha
		{
			get { return alpha; }
			set
			{
				if (this.alpha != value)
				{
					alpha = value;
					this.OnPropertyChanged("Alpha");
				}
			}
		}

		private bool isBestAngle;
		public bool IsBestAngle
		{
			get { return isBestAngle; }
			set
			{
				if (this.isBestAngle != value)
				{
					isBestAngle = value;
					this.OnPropertyChanged("IsBestAngle");
				}
			}
		}

		public double BestLineDistance { get { return 100; } }

		public void ResetAlfa()
		{
			this.Alpha = 180;
		}

		public RadarLine(double distance, double angle)
		{
			this.Angle = angle;
			this.Distance = distance;

			this.ResetAlfa();
			this.IsBestAngle = false;
		}

		protected void OnPropertyChanged(string propName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
		}
		public event PropertyChangedEventHandler PropertyChanged;
	}

	public class IRDistanceToBrushConverter : DependencyObject, IValueConverter
	{
		public WandererDashboardWPF Window
		{
			get { return (WandererDashboardWPF)GetValue(WindowProperty); }
			set { SetValue(WindowProperty, value); }
		}
		// Using a DependencyProperty as the backing store for Window.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty WindowProperty =
			DependencyProperty.Register("Window", typeof(WandererDashboardWPF), typeof(IRDistanceToBrushConverter), new PropertyMetadata());

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (this.Window == null)
				return null;

			WandererState state = this.Window.State;
			double distance = (double)value;

			if (distance < state.IRSafeDistance)
				return Brushes.Red;
			else if (distance < state.IRSafeDistance + state.IRDistanceDiferenceToAdjust)
				return Brushes.DarkGoldenrod;
			else
				return Brushes.Black;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class AlphaDistanceToStrokeConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return new SolidColorBrush(Color.FromArgb(System.Convert.ToByte(values[0]), 255, (byte)Math.Min(255, System.Convert.ToInt32(values[1])), 0));
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class AngleDistanceToX2Converter : IMultiValueConverter
	{
		const int lineSize = 4;
		public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			int objectWidth = parameter == null ? 0 : System.Convert.ToInt32(parameter);
			return Math.Cos(WandererDashboard.WandererDashboardWPF.DegreeToRadian((double)values[0])) * ((double)values[1] * lineSize) - (objectWidth / 2);
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class AngleDistanceToY2Converter : IMultiValueConverter
	{
		const int lineSize = 4;
		public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			int objectHeight = parameter == null ? 0 : System.Convert.ToInt32(parameter);
			return Math.Sin(WandererDashboard.WandererDashboardWPF.DegreeToRadian((double)values[0])) * ((double)values[1] * lineSize) - (objectHeight / 2);
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class DivideBy2Converter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return (double)value / 2;
		}

		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
