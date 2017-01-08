using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Mengsk.Device.Buses.COM
{
    public class SerialBusInfo : BusInfo
    {
        [Category("串口设置")]
        [Description("通信速率")]
        public int BaudRate { get; set; }

        [Category("串口设置")]
        [Description("奇偶校验 None：无校验，Odd：奇校验,Even：偶校验")]
        public Parity Parity { get; set; }

        [Category("串口设置")]
        [Description("通信bit数，通常为8")]
        public int DataBits { get; set; }

        [Category("串口设置")]
        [Description("停止位")]
        public StopBits StopBits { get; set; }
    }
}
