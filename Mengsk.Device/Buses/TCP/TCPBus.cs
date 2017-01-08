using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Mengsk.Device.Buses.TCP
{

    /// <summary>
    /// TCP 通信总线
    /// </summary>
    public class TCPBus : BusBase<TCPBusInfo>, IBus
    {
        private TcpClient tcpClient = null;

        protected override void OpenImplement(BusInfo busInfo)
        {
            try
            {
                this.tcpClient = new TcpClient();
                this.tcpClient.Connect(this.info.IP, this.info.Port);
            }
            catch
            {
                this.tcpClient = null;
                throw;
            }
        }

        protected override void CloseImplement()
        {
            this.tcpClient.Close();
            this.tcpClient = null;
        }

        protected override void WriteImplement(byte[] writeBuf, int offset, int len)
        {
            this.tcpClient.GetStream().Write(writeBuf, offset, len);
        }

        protected override int ReadImplement(byte[] readBuf, int maxReadLen)
        {
            return this.tcpClient.GetStream().Read(readBuf, 0, maxReadLen);
        }
    }
}
