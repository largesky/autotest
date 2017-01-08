using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mengsk.Device.Buses
{
    public class BusEventArgs : EventArgs
    {
        public IBus Bus { get; set; }

        public BusEventArgs(IBus bus)
        {
            this.Bus = bus;
        }
    }
}
