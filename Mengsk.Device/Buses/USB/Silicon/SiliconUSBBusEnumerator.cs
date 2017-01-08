using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mengsk.Device.Buses.USB.Silicon
{
    [BusEnumerator]
    class SiliconUSBBusEnumerator : IBusEnumerator
    {
        private BusInfo[] EnumerateQribSub(BusInfo busInfo)
        {
            List<BusInfo> busInfos = new List<BusInfo>();
            Type siliconUSBQribBusType = typeof(SiliconUSBQRIBBus);

            BusInfo usbIICBus = new BusInfo
            {
                AssemblyName = siliconUSBQribBusType.Assembly.GetName().Name,
                ClassFullName = siliconUSBQribBusType.FullName,
                DriverProvider = "NI",
                DriverDescription = "Silicon USB Qrib IIC",
                DriverValue = "IIC",
                UiqueValue = busInfo.UiqueValue + "IIC",
                ReadableValue = "Silicon USB::Qrib::IIC" + busInfo.DriverValue,
                Type = BusType.QRIB,
                CacheResource = true,
                EnableLog = false,
                Parent = busInfo,
                ReadTimeOut = 500,
                WriteTimeOut = 500,
            };

            busInfos.Add(usbIICBus);

            //枚举qbert

            return busInfos.ToArray();
        }

        public BusInfo[] EnumerateBusInfos()
        {
            List<BusInfo> busInfos = new List<BusInfo>();
            uint deviceNum = 0;
            int ret = SiliconUSBBusNative.SI_GetNumDevices(ref deviceNum);
            if (ret != SiliconUSBBusNative.SI_SUCCESS)
            {
                return busInfos.ToArray();
            }
            for (uint i = 0; i < deviceNum; i++)
            {
                StringBuilder sb = new StringBuilder(SiliconUSBBusNative.SI_MAX_DEVICE_STRLEN);
                ret = SiliconUSBBusNative.SI_GetProductString(i, sb, SiliconUSBBusNative.SI_RETURN_SERIAL_NUMBER);
                if (ret == SiliconUSBBusNative.SI_SUCCESS)
                {
                    string strSerialNumber = sb.ToString();
                    BusInfo busInfo = new BusInfo
                    {
                        AssemblyName = typeof(SiliconUSBBus).Assembly.FullName,
                        ClassFullName = typeof(SiliconUSBBus).FullName,
                        DriverDescription = "Silicon USB Device",
                        DriverValue = strSerialNumber,
                        Parent = null,
                        CacheResource = true,
                        DriverProvider = "NI",
                        EnableLog = false,
                        ReadableValue = "NI USB " + strSerialNumber,
                        ReadTimeOut = 500,
                        WriteTimeOut = 500,
                        Type = BusType.USB,
                        UiqueValue = "USB"
                    };
                    busInfos.Add(busInfo);

                    //如果是qrib则枚举级联的设备
                    if (strSerialNumber.IndexOf("qrib", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        try
                        {
                            busInfos.AddRange(EnumerateQribSub(busInfo));
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("枚举QRIB(" + strSerialNumber + ")级联设备出错:" + ex.Message + "\n" + ex.StackTrace);
                        }
                    }
                }
            }

            return busInfos.ToArray();
        }
    }
}
