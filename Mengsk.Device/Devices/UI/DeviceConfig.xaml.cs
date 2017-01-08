using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Mengsk.Device.Devices.UI
{
    /// <summary>
    /// Interaction logic for DeviceConfig.xaml
    /// </summary>
    public partial class DeviceConfig : UserControl
    {
        private ObservableCollection<DeviceViewModel> devices = new ObservableCollection<DeviceViewModel>();

        public DeviceConfig()
        {
            InitializeComponent();
        }

        public void Save()
        {

        }
        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void cbbDeviceTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DeviceViewModel device = ((ComboBox)sender).DataContext as DeviceViewModel;
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
            new DeviceConfigOne { DeviceViewModel = device }.ShowDialog();
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnATest_Click(object sender, RoutedEventArgs e)
        {
            DeviceViewModel device = ((Button)sender).DataContext as DeviceViewModel;
            new DeviceMethodCaller { DeviceViewModel = device }.Show();
        }
    }
}
