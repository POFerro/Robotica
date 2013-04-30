using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Arduino.Firmata.Types.Proxy;
using POFerro.Robotics.Suricata.Proxy;

namespace POFerro.Robotics.SuricataDashboard
{
	public class PinInfo : INotifyPropertyChanged
	{
		public Pins Pin { get; set; }
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

	public class SonarLateralDistanceToBrushConverter : DependencyObject, IValueConverter
	{
		public SuricataDashboardWPF Window
		{
			get { return (SuricataDashboardWPF)GetValue(WindowProperty); }
			set { SetValue(WindowProperty, value); }
		}
		// Using a DependencyProperty as the backing store for Window.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty WindowProperty =
			DependencyProperty.Register("Window", typeof(SuricataDashboardWPF), typeof(SonarLateralDistanceToBrushConverter), new PropertyMetadata());

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (this.Window == null)
				return null;

			double distance = (double)value;

			if (distance < this.Window.IRLateralSafeDistance)
				return Brushes.Red;
			else if (distance < this.Window.IRLateralSafeDistance + this.Window.SonarDistanceDiferenceToAdjust)
				return Brushes.DarkGoldenrod;
			else
				return Brushes.Black;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class IRLateralDistanceToBrushConverter : DependencyObject, IValueConverter
	{
		public SuricataDashboardWPF Window
		{
			get { return (SuricataDashboardWPF)GetValue(WindowProperty); }
			set { SetValue(WindowProperty, value); }
		}
		// Using a DependencyProperty as the backing store for Window.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty WindowProperty =
			DependencyProperty.Register("Window", typeof(SuricataDashboardWPF), typeof(IRLateralDistanceToBrushConverter), new PropertyMetadata());

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (this.Window == null)
				return null;

			double distance = (double)value;

			if (distance < this.Window.IRLateralSafeDistance)
				return Brushes.Red;
			else if (distance < this.Window.IRLateralSafeDistance + this.Window.IRDistanceDiferenceToAdjust)
				return Brushes.DarkGoldenrod;
			else
				return Brushes.Black;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class IRDistanceToBrushConverter : DependencyObject, IValueConverter
	{
		public SuricataDashboardWPF Window
		{
			get { return (SuricataDashboardWPF)GetValue(WindowProperty); }
			set { SetValue(WindowProperty, value); }
		}
		// Using a DependencyProperty as the backing store for Window.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty WindowProperty =
			DependencyProperty.Register("Window", typeof(SuricataDashboardWPF), typeof(IRDistanceToBrushConverter), new PropertyMetadata());

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (this.Window == null)
				return null;

			double distance = (double)value;

			if (distance < this.Window.IRSafeDistance)
				return Brushes.Red;
			else if (distance < this.Window.IRSafeDistance + this.Window.IRDistanceDiferenceToAdjust)
				return Brushes.DarkGoldenrod;
			else
				return Brushes.Black;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
