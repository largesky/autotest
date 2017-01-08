using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Mengsk.Device.Buses.TCP
{
    public class TCPBusInfo : BusInfo
    {
        private string ip = "192.168.0.1";
        private int port = 0;

        [Category("网络信息")]
        [Description("设备IP地址")]
        [PropertyOrder(11)]
        public string IP
        {
            get { return ip; }
            set
            {
                this.ip = value;
                this.UiqueValue = string.Format("{0} {1}", this.ip, port);
                this.DriverValue = this.UiqueValue;
            }
        }

        [Category("网络信息")]
        [Description("设备IP端口")]
        [Editor(typeof(Xceed.Wpf.Toolkit.PropertyGrid.Editors.TextBoxEditor), typeof(UITypeEditor))]
        [PropertyOrder(12)]
        public int Port
        {
            get { return this.port; }
            set
            {
                this.port = value;
                this.UiqueValue = string.Format("{0} {1}", this.ip, port);
                this.DriverValue = this.UiqueValue;
            }
        }
    }
}
