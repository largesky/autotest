using System;
using System.Collections.Generic;
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
using System.Collections.ObjectModel;
using Mengsk.Device.Devices.UI;
using Mengsk.Device.Devices;

namespace Mengsk.Device.Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<DeviceViewModel> devices = new ObservableCollection<DeviceViewModel>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.lstDeviceTypes.ItemsSource = Enum.GetValues(typeof(DeviceType));
            this.lstDevices.ItemsSource = this.devices;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void lstDeviceTypes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.lstDeviceTypes.SelectedItem == null)
            {
                return;
            }

            DeviceType type = (DeviceType)this.lstDeviceTypes.SelectedItem;

            DeviceViewModel m = new DeviceViewModel
            {
                AvalibleTypeInfos = DeviceManager.Instance.GetDeviceTypes(type),
                ReadableValue = "",
                Device = null,
                Type = type,
            };

            this.devices.Add(m);
        }

        private void cbbDeviceTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DeviceViewModel device = ((ComboBox)sender).DataContext as DeviceViewModel;

            if (device == null)
            {
                return;
            }

            if (device.Device != null)
            {
                device.Device.Close();
                device.Device = null;
            }
            device.Device = (((ComboBox)sender).SelectedItem as DeviceTypeInfo).CreateDevice();
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            DeviceViewModel device = ((Button)sender).DataContext as DeviceViewModel;
            if (new DeviceConfigOne { DeviceViewModel = device }.ShowDialog().Value == true)
            {
                if (device.Device.ConfigInfo.BusInfo == null)
                {
                    device.ReadableValue = "";
                }
                else
                {
                    device.ReadableValue = device.Device.ConfigInfo.BusInfo.ReadableValue;
                }
            }
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnATest_Click(object sender, RoutedEventArgs e)
        {
            DeviceViewModel device = ((Button)sender).DataContext as DeviceViewModel;
            new DeviceMethodCaller { DeviceViewModel = device }.Show();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            DeviceViewModel m = btn.DataContext as DeviceViewModel;

            if (m == null)
            {
                return;
            }
            try
            {
                if (m.Device != null && m.Device.DeviceOpened)
                {
                    m.Device.Close();
                }
                this.devices.Remove(m);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
