using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Mengsk.Device.Buses
{
    /// <summary>
    /// 总线信息
    /// </summary>
    public class BusInfo : ICloneable
    {
        /// <summary>
        ///获取或者设置一个值，以指示总线的类型
        /// </summary>
        [Category("基础信息")]
        [Description("总线类型")]
        [Browsable(false)]
        [PropertyOrder(1)]
        public BusType Type { get; set; }

        /// <summary>
        /// 获取或者设置一个值，以指示底层驱动程序提供者
        /// </summary>
        [Category("基础信息")]
        [Description("总线驱动程序提供商")]
        [Browsable(false)]
        [PropertyOrder(2)]
        public string DriverProvider { get; set; }

        /// <summary>
        /// 获取或者设置一个值，以指示驱动程序对该总线的描述
        /// </summary>
        [Category("基础信息")]
        [Description("总线驱动程序对设备的描述")]
        [Browsable(false)]
        [PropertyOrder(3)]
        public string DriverDescription { get; set; }

        /// <summary>
        /// 获取或者设置一个值，以指示驱动程序类所在的DLL名称
        /// </summary>
        [Category("基础信息")]
        [Description("总线驱动程序所在的程序集")]
        [ReadOnly(true)]
        [PropertyOrder(4)]
        public string AssemblyName { get; set; }

        /// <summary>
        /// 获取或者设置一个值，以指示驱动程序类的全名称
        /// </summary>
        [Category("基础信息")]
        [Description("总线驱动程序全限定名称")]
        [ReadOnly(true)]
        [PropertyOrder(5)]
        public string ClassFullName { get; set; }

        /// <summary>
        /// 获取或者设置一个值，以指示在整个系统中唯一的值，对于同种类型的总线，如果表示相同的设置则其值，必须一样
        /// 请参见各种总线的表示格式
        /// </summary>
        [Category("基础信息")]
        [ReadOnly(true)]
        [PropertyOrder(8)]
        public string UiqueValue { get; set; }

        /// <summary>
        /// 获取或者设置一个值，以指示总线的值，该值只能由特定的总线驱动程序解释
        /// </summary>
        [Browsable(false)]
        [PropertyOrder(9)]
        public string DriverValue { get; set; }

        /// <summary>
        /// 获取或者设置一个值，以指示一个可以阅读的值，用来在配置时使用DriverProvider Type::SubBus xxxxx
        /// 通常使用级联方式表示如: NI COM::17或者Silicon USB::QRIB 1234567
        /// </summary>
        [Browsable(false)]
        [PropertyOrder(10)]
        public string ReadableValue { get; set; }

        /// <summary>
        /// 获取或者设置一个值，以指示该总线父总线
        /// </summary>
        [Browsable(false)]
        [PropertyOrder(11)]
        public BusInfo Parent { get; set; }

        /// <summary>
        /// 获取或者设置一个值，以指示在打开设备后，是否进行缓存
        /// </summary>
        [Category("资源行为")]
        [Description("True表示该总线只打开一次，直到程序运行结束时关闭，False表示每次读写都要打开并关闭总线")]
        [PropertyOrder(1)]
        public bool CacheResource { get; set; }

        /// <summary>
        /// 获取或者设置一个值，以指示是否记录设备的通信数据
        /// </summary>
        [Category("资源行为")]
        [Description("True表示将记录每一次通信数据及其时间等信息")]
        [PropertyOrder(2)]
        public bool EnableLog { get; set; }

        [Category("读写超时")]
        [Description("读取超时时间")]
        [PropertyOrder(1)]
        public int WriteTimeOut { get; set; }

        [Category("读写超时")]
        [Description("写入超时时间")]
        [PropertyOrder(1)]
        public int ReadTimeOut { get; set; }

        protected string GetSubXElementValue(XElement ele, string name)
        {
            XElement xe = ele.Ancestors("Item").FirstOrDefault(obj => obj.Attribute("Name") != null && obj.Attribute("Name").Value == name);
            if (xe == null)
            {
                throw new Exception("XElement:" + ele.Name + " 的下级结点不包含形如<Item Name=" + name + ">******</Item>的结点");
            }
            return xe.Value;
        }

        /// <summary>
        /// 已重写，比较Type,与UniqueValue是否相同
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            BusInfo other = (obj) as BusInfo;

            if (other == null)
            {
                return false;
            }

            if (this.Type != other.Type)
            {
                return false;
            }

            if (string.Compare(this.UiqueValue, other.UiqueValue) != 0)
            {
                return false;
            }

            if (string.Compare(this.DriverProvider, other.DriverProvider) != 0)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return (int)this.Type + this.DriverProvider.GetHashCode() + this.UiqueValue.GetHashCode();
        }

        public override string ToString()
        {
            return this.ReadableValue;
        }

        public object Clone()
        {
            return base.MemberwiseClone();
        }
    }
}
