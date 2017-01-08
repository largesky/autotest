using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mengsk.Device.Buses
{
    /// <summary>
    /// 总线状态
    /// </summary>
    public enum BusState
    {
        Created,
        Opening,
        Opened,
        Writing,
        Writed,
        Reading,
        Readed,
        Closing,
        Closed,
        Error,
    }
}
