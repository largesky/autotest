using Mengsk.Device.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mengsk.AutoTest
{
    public interface IAutoTest
    {
        event EventHandler<AutoTestMessageEventArgs> TestingMessage;

        void InitializeDevices(DeviceManager deviceManager);

        AutoTestValueCollection ExecuteTest(AutoTestParameters parameters);
    }
}
