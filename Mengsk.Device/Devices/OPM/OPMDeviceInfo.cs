using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mengsk.Device.Devices.OPM
{
    public class OPMDeviceInfo : DeviceFunctions
    {
        public override string[] SugestBusFilters
        {
            get
            {
                return new string[] { "AN" };
            }
        }

        public int WaitStableTime { get; set; }

        public OPMDeviceInfo()
        {
            this.WaitStableTime = 1000;
        }
    }
}
