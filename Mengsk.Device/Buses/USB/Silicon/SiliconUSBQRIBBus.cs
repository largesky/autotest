﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mengsk.Device.Buses.USB.Silicon
{
    public class SiliconUSBQRIBBus : BusBase<BusInfo>
    {
        protected override void OpenImplement(BusInfo busInfo)
        {
            throw new NotImplementedException();
        }

        protected override void CloseImplement()
        {
            throw new NotImplementedException();
        }

        protected override void WriteImplement(byte[] writeBuf, int offset, int len)
        {
            throw new NotImplementedException();
        }

        protected override int ReadImplement(byte[] readBuf, int maxReadLen)
        {
            throw new NotImplementedException();
        }
    }
}
