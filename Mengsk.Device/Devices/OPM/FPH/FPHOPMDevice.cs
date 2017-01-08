using Mengsk.Device.Buses;
using Mengsk.Device.Buses.COM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mengsk.Device.Devices.OPM.FPH
{
    [DeviceFlag(DeviceType.OPM, "FPH OPM", "FPH 光功率计，使用串口进行通信")]
    public class FPHOPMDevice : DeviceBase<OPMDeviceInfo, DeviceEmptyUserControl>, IOPM
    {
        protected override void OpenImplement()
        {
        }

        /// <summary>
        /// Closes the implement.
        /// </summary>
        protected override void CloseImplement()
        {
        }

        public int GetWaveLength()
        {
            byte[] writeBuf = new byte[] { 0x00, 0x01 };
            byte[] readBuf = new byte[10];
            int ret = BusManager.Instance.WriteAndRead(this.ConfigInfo.BusInfo, writeBuf, 0, writeBuf.Length, readBuf, 10, 200);

            return BitConverter.ToInt32(readBuf, 0);
        }

        public void SetWaveLength(int waveLength)
        {
            byte[] writeBuf = new byte[] { 0x00, 0x01 };
            byte[] readBuf = new byte[10];
            int ret = BusManager.Instance.WriteAndRead(this.ConfigInfo.BusInfo, writeBuf, 0, writeBuf.Length, readBuf, 10, 200);

            if (BitConverter.ToInt32(readBuf, 0) != 0)
            {
                throw new Exception("设置波长失败");
            }
        }

        public double GetPower(int waitStableTime = -1)
        {
            return 2.5;
        }

        public override string SelfTest()
        {
            return "OK";
        }

        public override void SetBusDefault(BusInfo busInfo)
        {
            SerialBusInfo serialBus = busInfo as SerialBusInfo;
            if (serialBus == null)
            {
                return;
            }

            serialBus.BaudRate = 9600;
            serialBus.DataBits = 8;
            serialBus.Parity = System.IO.Ports.Parity.Even;
            serialBus.StopBits = System.IO.Ports.StopBits.One;
        }
    }
}
