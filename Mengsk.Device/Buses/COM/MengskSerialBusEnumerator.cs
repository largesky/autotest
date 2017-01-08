using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mengsk.Device.Buses.COM
{
    [BusEnumerator()]
    public class MengskSerialBusEnumerator : IBusEnumerator
    {
        public BusInfo[] EnumerateBusInfos()
        {
            string[] portNames = System.IO.Ports.SerialPort.GetPortNames();
            List<BusInfo> busInfos = new List<BusInfo>();

            foreach (string s in portNames)
            {
                SerialBusInfo info = new SerialBusInfo
                {
                    AssemblyName = typeof(MengskSerialBus).Assembly.GetName().Name,
                    ClassFullName = typeof(MengskSerialBus).FullName,
                    DriverDescription = "串口设备，使用System.IO.Port.SerialPort实现",
                    DriverProvider = "System",
                    DriverValue = s,
                    UiqueValue = s,
                    ReadableValue = "System " + s,
                    CacheResource = true,
                    EnableLog = false,
                    Parent = null,
                    ReadTimeOut = 500,
                    WriteTimeOut = 500,
                    Type = BusType.COM,
                    StopBits = System.IO.Ports.StopBits.One,
                    Parity = System.IO.Ports.Parity.Even,
                    DataBits = 8,
                    BaudRate = 9600,
                };

                busInfos.Add(info);
            }

            return busInfos.ToArray();
        }
    }
}
