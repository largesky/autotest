using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace Mengsk.Device.Devices
{
    public class DeviceManager
    {
        private static readonly DeviceManager instance = new DeviceManager();

        public static DeviceManager Instance { get { return instance; } }

        private List<Assembly> deviceAssemblies = new List<Assembly>();
        private Dictionary<string, IDevice> deviceCreated = new Dictionary<string, IDevice>();
        private Dictionary<string, DeviceInfo> deviceInfos = new Dictionary<string, DeviceInfo>();

        public DeviceManager()
        {
            this.AddAssembly(typeof(DeviceManager).Assembly);
        }

        public bool AddAssembly(Assembly assembly)
        {
            lock (this.deviceAssemblies)
            {
                if (this.deviceAssemblies.Contains(assembly) == false)
                {
                    this.deviceAssemblies.Add(assembly);
                    return true;
                }
                return false;
            }
        }

        public bool RemoveAssembly(Assembly assembly)
        {
            lock (this.deviceAssemblies)
            {
                if (this.deviceAssemblies.Contains(assembly))
                {
                    this.deviceAssemblies.Remove(assembly);
                    return true;
                }
                return false;
            }
        }

        public DeviceTypeInfo[] GetDeviceTypes()
        {
            List<DeviceTypeInfo> classInfos = new List<DeviceTypeInfo>();

            foreach (Assembly assembly in this.deviceAssemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    object[] attributes = type.GetCustomAttributes(typeof(DeviceFlagAttribute), false);
                    if (attributes == null || attributes.Length < 1)
                    {
                        continue;
                    }
                    if (type.GetInterface(typeof(IDevice).FullName, false) == null)
                    {
                        continue;
                    }
                    DeviceFlagAttribute dt = attributes[0] as DeviceFlagAttribute;
                    classInfos.Add(new DeviceTypeInfo(dt.Type, dt.Name, dt.Description, assembly.GetName().Name, type.FullName));
                }
            }

            return classInfos.ToArray();
        }

        public DeviceTypeInfo[] GetDeviceTypes(DeviceType type)
        {
            return this.GetDeviceTypes().Where(t => t.Type == type).ToArray();
        }

        public string[] GetDeviceKeys()
        {
            return this.deviceInfos.Keys.ToArray();
        }

        public T GetDevice<T>(string name) where T : IDevice
        {
            lock (this.deviceCreated)
            {
                if (this.deviceInfos.ContainsKey(name) == false)
                {
                    throw new KeyNotFoundException(name);
                }

                if (this.deviceCreated.ContainsKey(name) == true)
                {
                    return (T)this.deviceCreated[name];
                }

                this.deviceCreated[name] = this.deviceInfos[name].TypeInfo.CreateDevice();
                this.deviceCreated[name].ConfigInfo = this.deviceInfos[name];
                return (T)this.deviceCreated[name];
            }
        }

        public bool ContainsDevice(string name)
        {
            lock (this.deviceInfos)
            {
                return this.deviceInfos.ContainsKey(name);
            }
        }

        public void LoadDevice(string filePath, string groupName)
        {
            lock (this.deviceCreated)
            {
                if (this.deviceCreated.Count > 0)
                {
                    throw new Exception(string.Format("DeviceManager caches {0} devices,call UnloadDevice first", this.deviceCreated.Count));
                }
                XDocument xDoc = XDocument.Load(filePath);
                try
                {
                    XElement xeGroup = xDoc.Document.Root.Elements().FirstOrDefault(xe => xe.Attribute("Group").Value == groupName);
                    if (xeGroup == null)
                    {
                        throw new Exception("无法在设备配置文件中找到关于设备组:" + groupName);
                    }

                    foreach (XElement xe in xeGroup.Elements())
                    {

                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("DeviceManager load device configuration fail:\n{0}", ex.Message), ex);
                }
            }
        }

        public void UnloadDevice(string groupName)
        {

        }
    }
}
