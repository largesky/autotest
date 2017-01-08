using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Mengsk.Device.Devices
{
    public interface IDeviceUserControl
    {
        UserControl GetUserControl();

        void SetDevice(IDevice device);

        IDevice GetDevice();

        void UnSetDevice();
    }
}
