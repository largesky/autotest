using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Mengsk.Device.Devices.UI
{
    public class DeviceViewModel : DependencyObject
    {
        public static readonly DependencyProperty ReadableValueProperty = DependencyProperty.Register("ReadableValue", typeof(string), typeof(DeviceViewModel));

        public string ReadableValue { get { return (string)this.GetValue(ReadableValueProperty); } set { this.SetValue(ReadableValueProperty, value); } }

        public DeviceType Type { get; set; }
        
        public DeviceTypeInfo[] AvalibleTypeInfos { get; set; }

        public IDevice Device { get; set; }
    }
}
