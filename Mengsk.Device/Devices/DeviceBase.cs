using Mengsk.Device.Buses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mengsk.Device.Devices
{
    /// <summary>
    /// 设备基类
    /// </summary>
    /// <typeparam name="TFunctions">配置类型</typeparam>
    /// <typeparam name="TUserControl">自定义控制类型</typeparam>
    public abstract class DeviceBase<TFunctions, TUserControl> : IDevice
        where TFunctions : DeviceFunctions, new()
        where TUserControl : IDeviceUserControl, new()
    {
        protected DeviceInfo info = null;

        /// <summary>
        /// 获取一个值，以指示设备是否已经打开
        /// </summary>
        public bool DeviceOpened { get; private set; }

        /// <summary>
        /// 获取或者设置一个值，以指示设备的基础信息
        /// </summary>
        public virtual DeviceInfo ConfigInfo
        {
            get { return info; }
            set
            {
                if (this.info == value)
                {
                    return;
                }
                if (this.DeviceOpened)
                    this.Close();
                this.info = value;
            }
        }

        /// <summary>
        /// 获取一个值，以指示设备实现的自定义控件，每次都会创建新的实例
        /// </summary>
        public virtual IDeviceUserControl UserControlInterface
        {
            get { return new TUserControl(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceBase{TConfig}"/> class.
        /// </summary>
        public DeviceBase()
        {
            this.ConfigInfo = new DeviceInfo { Functions = new TFunctions() };
        }

        /// <summary>
        /// 打开设备
        /// </summary>
        /// <exception cref="System.Exception"></exception>
        public void Open()
        {
            if (this.DeviceOpened)
            {
                throw new Exception(string.Format("Device:{0} is already opened", this.ConfigInfo.DeviceName));
            }
            this.OpenImplement();
            this.DeviceOpened = true;
        }

        /// <summary>
        /// 关闭设备
        /// </summary>
        /// <exception cref="System.Exception"></exception>
        public void Close()
        {
            if (this.DeviceOpened == false)
            {
                throw new Exception(string.Format("Device:{0} is not opened,could not close", this.ConfigInfo.DeviceName));
            }
            this.CloseImplement();
            BusManager.Instance.CloseBus(this.ConfigInfo.BusInfo);
            this.DeviceOpened = false;
        }

        /// <summary>
        /// 子类必须重写该方法，以实现打开
        /// </summary>
        protected abstract void OpenImplement();

        /// <summary>
        /// 子类必须重写该方法，以实现关闭
        /// </summary>
        protected abstract void CloseImplement();

        /// <summary>
        /// 子类必须重写该方法，以实现自检
        /// </summary>
        public abstract string SelfTest();

        public abstract void SetBusDefault(BusInfo busInfo);
    }
}
