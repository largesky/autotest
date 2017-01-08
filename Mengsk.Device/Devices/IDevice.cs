using Mengsk.Device.Buses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mengsk.Device.Devices
{
    /// <summary>
    /// 设备接口
    /// </summary>
    public interface IDevice
    {
        /// <summary>
        /// 获取或者设置一个值，以指示设备的基础信息
        /// </summary>
        DeviceInfo ConfigInfo { get; set; }

        /// <summary>
        /// 获取设备实现的自定义的控件
        /// </summary>
        /// <returns></returns>
        IDeviceUserControl UserControlInterface { get; }

        /// <summary>
        /// 获取一个值，以指示设备是否已经打开
        /// </summary>
        bool DeviceOpened { get; }

        /// <summary>
        /// 打开设备
        /// </summary>
        void Open();

        /// <summary>
        /// 设备自测，
        /// </summary>
        /// <returns>成功返回OK,否则返回其它值</returns>
        string SelfTest();

        /// <summary>
        /// 设置总线到默认的状态
        /// </summary>
        /// <param name="busInfo"></param>
        void SetBusDefault(BusInfo busInfo);

        /// <summary>
        /// 关闭设备
        /// </summary>
        void Close();
    }
}
