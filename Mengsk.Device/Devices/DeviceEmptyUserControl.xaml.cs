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

namespace Mengsk.Device.Devices
{
    /// <summary>
    /// Interaction logic for DeviceEmptyUserControl.xaml
    /// </summary>
    public partial class DeviceEmptyUserControl : UserControl, IDeviceUserControl
    {
        private IDevice device = null;

        public DeviceEmptyUserControl()
        {
            InitializeComponent();
        }

        public UserControl GetUserControl()
        {
            return this;
        }

        public void SetDevice(IDevice device)
        {
            this.device = null;
        }

        public IDevice GetDevice()
        {
            return this.device;
        }

        public void UnSetDevice()
        {
            this.device = null;
        }
    }
}
