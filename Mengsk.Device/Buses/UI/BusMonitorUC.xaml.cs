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

namespace Mengsk.Device.Buses.UI
{
    /// <summary>
    /// Interaction logic for BusMonitorUC.xaml
    /// </summary>
    public partial class BusMonitorUC : UserControl
    {
        private ObservableCollection<BusViewModel> buses = new ObservableCollection<BusViewModel>();

        public BusMonitorUC()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BusManager.Instance.BusCreated += Instance_BusCreated;
            BusManager.Instance.BusRemoved += Instance_BusRemoved;
            this.lstBuses.ItemsSource = this.buses;
            IBus[] buses = BusManager.Instance.GetAllCreatedBus();
            foreach (var bus in buses)
            {
                this.Instance_BusCreated(BusManager.Instance, new BusEventArgs(bus));
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            BusManager.Instance.BusCreated -= Instance_BusCreated;
            BusManager.Instance.BusRemoved -= Instance_BusRemoved;
        }

        void Instance_BusRemoved(object sender, BusEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() => this.AddBus(e.Bus)));
        }

        void Instance_BusCreated(object sender, BusEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() => this.RemoveBus(e.Bus)));
        }

        void bus_BusOperationCompleted(object sender, BusOperationEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() => this.BusOperationCompleted(e)));
        }

        void bus_BusOperationStarting(object sender, BusOperationEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() => this.BusOperationStarting(e)));
        }

        void bus_BusStateChanged(object sender, BusStateChangedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() => this.BusStateChanged(e)));
        }

        private BusViewModel FindFirtOrDefault(IBus bus)
        {
            return this.buses.FirstOrDefault(obj => obj.Bus == bus);
        }

        void AddBus(IBus bus)
        {
            BusViewModel m = FindFirtOrDefault(bus);
            if (m != null)
            {
                return;
            }
            bus.BusStateChanged += bus_BusStateChanged;
            bus.BusOperationStarting += bus_BusOperationStarting;
            bus.BusOperationCompleted += bus_BusOperationCompleted;
            this.buses.Add(new BusViewModel(bus));
        }

        void RemoveBus(IBus bus)
        {
            BusViewModel m = FindFirtOrDefault(bus);
            if (m == null)
            {
                return;
            }

            bus.BusStateChanged -= bus_BusStateChanged;
            bus.BusOperationStarting -= bus_BusOperationStarting;
            bus.BusOperationCompleted -= bus_BusOperationCompleted;

            this.buses.Remove(m);
        }

        void BusOperationCompleted(BusOperationEventArgs e)
        {
            BusViewModel m = FindFirtOrDefault(e.Bus);
            if (m == null)
            {
                return;
            }
            m.Message = string.Format("成功{0}数据， Offset:{1}, Len:{2}", e.Operation == BusOperation.Reading ? "读取" : "写入", e.Offset, e.Length);
        }

        void BusOperationStarting(BusOperationEventArgs e)
        {
            BusViewModel m = FindFirtOrDefault(e.Bus);
            if (m == null)
            {
                return;
            }
            m.Message = string.Format("正在{0}数据， Offset:{1}, Len:{2}......", e.Operation == BusOperation.Reading ? "读取" : "写入", e.Offset, e.Length);
        }

        void BusStateChanged(BusStateChangedEventArgs e)
        {
            BusViewModel m = FindFirtOrDefault(e.Bus);
            if (m == null)
            {
                return;
            }
            m.State = e.CurrentState;
        }
    }
}
