using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mengsk.Device.Buses
{
    public class BusStateChangedEventArgs : EventArgs
    {
        public IBus Bus { get; private set; }

        public BusState PreState { get; private set; }

        public BusState CurrentState { get; private set; }

        public BusStateChangedEventArgs()
            : this(null, BusState.Error, BusState.Error)
        { }

        public BusStateChangedEventArgs(IBus bus, BusState preState, BusState currentState)
        {
            this.Bus = bus;
            this.PreState = preState;
            this.CurrentState = currentState;
        }
    }
}
