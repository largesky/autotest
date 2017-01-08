using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Mengsk.Device.Devices
{
    public class DeviceFunctions
    {
        [Browsable(false)]
        public virtual string[] SugestBusFilters { get { return new string[] { }; } }
    }
}
