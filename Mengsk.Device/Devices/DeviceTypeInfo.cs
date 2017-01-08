using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace Mengsk.Device.Devices
{
    /// <summary>
    /// 设备类型信息
    /// </summary>
    public class DeviceTypeInfo
    {
        private readonly object typeLock = new object();

        private Assembly assemblyCache;
        private Type classTypeCache;

        /// <summary>
        /// 获取一个值，以指示设备的类型信息
        /// </summary>
        public DeviceType Type { get; private set; }

        /// <summary>
        /// 获取一个值，以指示该设备类型的名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 获取一个值，以指示该设备的描述
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// 获取一个值，以指示设备类所在的程序集
        /// </summary>
        public string Assembly { get; private set; }

        /// <summary>
        /// 获取一个值，以指示设备类的全名称
        /// </summary>
        public string ClassFullName { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">设备类型</param>
        /// <param name="name">名称</param>
        /// <param name="description">描述</param>
        /// <param name="assmbly">所在程序集</param>
        /// <param name="classType">类的全名称</param>
        public DeviceTypeInfo(DeviceType type, string name, string description, string assmbly, string classType)
        {
            this.Type = type;
            this.Name = name;
            this.Description = description;
            this.Assembly = assmbly;
            this.ClassFullName = classType;
        }

        /// <summary>
        /// 创建一个设备
        /// </summary>
        /// <returns></returns>
        public IDevice CreateDevice()
        {
            lock (this.typeLock)
            {
                if (assemblyCache == null)
                {
                    this.assemblyCache = System.Reflection.Assembly.Load(this.Assembly);
                }
                if (this.classTypeCache == null)
                {
                    this.classTypeCache = this.assemblyCache.GetType(this.ClassFullName);
                }
            }
            IDevice device = Activator.CreateInstance(this.classTypeCache) as IDevice;
            return device;
        }
    }
}
