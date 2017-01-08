using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mengsk.Device.Buses
{
    public class BusOperationEventArgs : EventArgs
    {
        public IBus Bus { get; set; }

        public BusOperation Operation { get; set; }

        public byte[] Data { get; set; }

        public int Offset { get; set; }

        public int Length { get; set; }

        public BusOperationEventArgs()
            :this(null, BusOperation.Writing,null,0,0)
        {

        }

        public BusOperationEventArgs(IBus bus, BusOperation operation, byte[] data, int offset, int len)
        {
            this.Bus = bus;
            this.Operation = operation;
            this.Data = data;
            this.Offset = offset;
            this.Length = len;
        }
    }
}
