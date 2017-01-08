using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mengsk.Device.Devices.OPM
{
    public interface IOPM : IDevice
    {
        int GetWaveLength();

        void SetWaveLength(int waveLength);

        double GetPower(int waitStableTime = -1);
    }
}
