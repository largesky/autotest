using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mengsk.Device.Buses.USB.Silicon
{

    /// <summary>
    /// silicon usb bus
    /// </summary>
    public class SiliconUSBBus : BusBase<BusInfo>
    {
        private IntPtr hDevice = IntPtr.Zero;

        protected override void OpenImplement(BusInfo busInfo)
        {
            string[] usbSerialNumbers = SiliconUSBBusNative.SI_GetAllDeviceProductString(SiliconUSBBusNative.SI_RETURN_SERIAL_NUMBER);
            uint deviceNum = 0;
            for (; deviceNum < usbSerialNumbers.Length; deviceNum++)
            {
                if (usbSerialNumbers[(int)deviceNum].Equals(busInfo.DriverValue, StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
            }
            if (deviceNum == usbSerialNumbers.Length)
            {
                throw new Exception(SiliconUSBBusNative.FormartErrorCode(SiliconUSBBusNative.SI_DEVICE_NOT_FOUND));
            }

            int er = SiliconUSBBusNative.SI_Open(deviceNum, ref hDevice);
            if (er != SiliconUSBBusNative.SI_SUCCESS)
            {
                throw new Exception(SiliconUSBBusNative.FormartErrorCode(er));
            }
            er = SiliconUSBBusNative.SI_SetTimeouts((uint)busInfo.ReadTimeOut, (uint)busInfo.WriteTimeOut);
            if (er != SiliconUSBBusNative.SI_SUCCESS)
            {
                throw new Exception(SiliconUSBBusNative.FormartErrorCode(er));
            }
        }

        protected override void CloseImplement()
        {
            int er = SiliconUSBBusNative.SI_Close(hDevice);
            if (er != SiliconUSBBusNative.SI_SUCCESS)
            {
                throw new Exception(SiliconUSBBusNative.FormartErrorCode(er));
            }
            hDevice = IntPtr.Zero;
        }

        protected override void WriteImplement(byte[] buf, int offset, int len)
        {
            int writed = 0;
            uint writedBySI = 0;

            while (writed <= len)
            {
                byte[] data = new byte[len - writed > SiliconUSBBusNative.SI_MAX_WRITE_SIZE ? SiliconUSBBusNative.SI_MAX_WRITE_SIZE : len - writed];
                Array.Copy(buf, offset + writed, data, 0, data.Length);
                int er = SiliconUSBBusNative.SI_Write(hDevice, data, (uint)data.Length, ref writedBySI, IntPtr.Zero);
                if (er != SiliconUSBBusNative.SI_SUCCESS)
                {
                    throw new Exception(SiliconUSBBusNative.FormartErrorCode(er));
                }
                if (writedBySI != data.Length)
                {
                    throw new Exception(string.Format("Silicon SI_Write 失败，待写入数据:{0},实际写入数据:{1}", data.Length, writedBySI));
                }
                writed += data.Length;
            }
        }

        protected override int ReadImplement(byte[] buf, int readMaxLen)
        {
            uint dataReaded = 0;
            int er = SiliconUSBBusNative.SI_Read(hDevice, buf, (uint)readMaxLen, ref dataReaded, IntPtr.Zero);
            if (er != SiliconUSBBusNative.SI_SUCCESS)
            {
                throw new Exception(SiliconUSBBusNative.FormartErrorCode(er));
            }
            return (int)dataReaded;
        }
    }
}
