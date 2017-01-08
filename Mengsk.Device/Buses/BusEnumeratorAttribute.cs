using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mengsk.Device.Buses
{
    /// <summary>
    /// 总线枚举属性标记类
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class BusEnumeratorAttribute : Attribute
    {
    }
}
