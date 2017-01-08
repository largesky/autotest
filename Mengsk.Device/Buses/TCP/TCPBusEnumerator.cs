using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mengsk.Device.Buses.TCP
{
    [BusEnumerator()]
    public class TCPBusEnumerator : IBusEnumerator
    {
        public BusInfo[] EnumerateBusInfos()
        {
            TCPBusInfo info = new TCPBusInfo
            {
                DriverProvider = "Mengsk",
                AssemblyName = typeof(TCPBus).Assembly.GetName().Name,
                ClassFullName = typeof(TCPBus).FullName,
                DriverDescription = "Megnsk TCP Bus",
                CacheResource = true,
                DriverValue = "",
                ReadableValue = "Mengsk TCP",
                Type = BusType.TCP,
                UiqueValue = "",
                IP = "xxx.xxx.xxx.xxx",
                Port = 0,
                EnableLog = false,
                ReadTimeOut = 500,
                WriteTimeOut = 500,
                Parent = null,
            };

            return new BusInfo[] { info };
        }
    }
}
