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
using Microsoft.Kinect;

using kinect = Microsoft.Robotics.Services.Sensors.Kinect.Proxy;

namespace POFerro.Robotics.SkeletonFollower
{
	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
	public partial class SkeletonFollowerDashboardWPF : Window, INotifyPropertyChanged
	{
		public SkeletonFollowerState State { get; set; }
		public SkeletonFollowerDashboardWPF()
		{
			InitializeComponent();

			this.DataContext = this;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string propName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
		}

		public void UpdateState(SkeletonFollowerState state)
		{
			this.State = state;
			this.OnPropertyChanged("State");
		}

		public static double DegreeToRadian(double degree)
		{
			return degree * (Math.PI / 180.0);
		}

		public static double RadianToDegree(double radian)
		{
			return radian * (180.0 / Math.PI);
		}
	}
}
