using Mengsk.Device.Buses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Mengsk.Device.Devices
{
    public class DeviceInfo
    {
        [Browsable(false)]
        public string DeviceName { get; set; }

        /// <summary>
        /// 获取或者设置一个值，以指示设备类型信息
        /// </summary>
        public DeviceTypeInfo TypeInfo { get; set; }

        /// <summary>
        /// 获取或者设置一个值，以指示设备的功能
        /// </summary>
        public DeviceFunctions Functions { get; set; }

        /// <summary>
        /// 获取或者设置一个值，以指示总线信息
        /// </summary>
        public BusInfo BusInfo { get; set; }

        public DeviceInfo()
        {
        }
    }
}
