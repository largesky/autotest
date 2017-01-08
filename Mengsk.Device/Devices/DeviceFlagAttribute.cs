using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mengsk.Device.Devices
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    class DeviceFlagAttribute : Attribute
    {
        public DeviceType Type { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DeviceFlagAttribute(DeviceType type, string name, string description)
        {
            this.Type = type;
            this.Name = name;
            this.Description = description;
        }
    }
}
