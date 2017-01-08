using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mengsk.Device.Buses
{
    /// <summary>
    /// 总线枚举接口
    /// </summary>
    public interface IBusEnumerator
    {
        BusInfo[] EnumerateBusInfos();
    }
}
