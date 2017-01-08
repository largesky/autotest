using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mengsk.Device.Buses
{
    /// <summary>
    /// 总线接口
    /// </summary>
    public interface IBus
    {
        BusInfo Info { get; }

        BusState State { get; }

        event EventHandler<BusStateChangedEventArgs> BusStateChanged;

        event EventHandler<BusOperationEventArgs> BusOperationStarting;

        event EventHandler<BusOperationEventArgs> BusOperationCompleted;

        void Open(BusInfo busInfo);

        void Close();

        void Write(byte[] writeBuf, int offset, int len);

        int Read(byte[] readBuf, int maxReadLen = 0);
    }
}
