using Mengsk.Device.Buses;
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
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace Mengsk.Device.Devices.UI
{
    /// <summary>
    /// Interaction logic for DeviceConfigOne.xaml
    /// </summary>
    public partial class DeviceConfigOne : Window
    {
        public DeviceViewModel DeviceViewModel { get; set; }

        public DeviceConfigOne()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.cbbBuses.ItemsSource = Buses.BusManager.Instance.EnumerateBusInfos();
            if (this.DeviceViewModel != null)
            {
                this.pgDeviceInfo.SelectedObject = this.DeviceViewModel.Device.ConfigInfo.Functions;
            }
        }

        private void cbbBuses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BusInfo busInfo = this.cbbBuses.SelectedItem as BusInfo;

            List<BusInfo> busInfos = new List<BusInfo>();

            while (busInfo != null)
            {
                busInfos.Add(busInfo.Clone() as BusInfo);
                busInfo = busInfo.Parent;
                this.DeviceViewModel.Device.SetBusDefault(busInfos[busInfos.Count - 1]);
            }
            this.lstBuses.ItemsSource = busInfos;
        }

        private void PropertyGrid_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PropertyGrid pg = sender as PropertyGrid;
            if (pg != null)
            {
                pg.Update();
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.cbbBuses.SelectedItem == null)
                {
                    if (MessageBox.Show("没有选择总线，是否继续?", "警告", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) != MessageBoxResult.Yes)
                    {
                        return;
                    }
                    this.DeviceViewModel.Device.ConfigInfo.BusInfo = null;
                }
                else
                {
                    this.DeviceViewModel.Device.ConfigInfo.BusInfo = this.lstBuses.Items[0] as BusInfo;
                }
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
