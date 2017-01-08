using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Mengsk.Device.Buses
{
    /// <summary>
    /// 总线管理类
    /// </summary>
    public class BusManager
    {
        private static readonly BusManager instance = new BusManager();

        public static BusManager Instance { get { return instance; } }

        private Dictionary<BusInfo, IBus> busInstances = new Dictionary<BusInfo, IBus>();
        private List<Assembly> busEnumeratorAssemblies = new List<Assembly>();

        public event EventHandler<BusEventArgs> BusCreated;

        public event EventHandler<BusEventArgs> BusRemoved;

        public BusManager()
        {
            this.AddAssembly(typeof(BusManager).Assembly);
        }

        protected virtual void OnBusCreated(BusEventArgs e)
        {
            if (this.BusCreated != null)
            {
                this.BusCreated(this, e);
            }
        }

        protected virtual void OnBusRemoved(BusEventArgs e)
        {
            if (this.BusRemoved != null)
            {
                this.BusRemoved(this, e);
            }
        }

        public bool AddAssembly(Assembly assembly)
        {
            lock (this.busEnumeratorAssemblies)
            {
                if (this.busEnumeratorAssemblies.Contains(assembly) == false)
                {
                    this.busEnumeratorAssemblies.Add(assembly);
                    return true;
                }
                return false;
            }
        }

        public bool RemoveAssembly(Assembly assembly)
        {
            lock (this.busEnumeratorAssemblies)
            {
                if (this.busEnumeratorAssemblies.Contains(assembly))
                {
                    this.busEnumeratorAssemblies.Remove(assembly);
                    return true;
                }
                return false;
            }
        }

        public BusInfo[] EnumerateBusInfos()
        {
            List<IBusEnumerator> busEnumerators = new List<IBusEnumerator>();
            List<BusInfo> busInfos = new List<BusInfo>();

            foreach (Assembly assembly in this.busEnumeratorAssemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.GetCustomAttributes(typeof(BusEnumeratorAttribute), false).Length > 0 && type.GetInterface(typeof(IBusEnumerator).FullName, true) != null)
                    {
                        IBusEnumerator en = Activator.CreateInstance(type) as IBusEnumerator;
                        busEnumerators.Add(en);
                    }
                }
            }

            foreach (IBusEnumerator enumerator in busEnumerators)
            {
                busInfos.AddRange(enumerator.EnumerateBusInfos());
            }

            return busInfos.ToArray();
        }

        public IBus[] GetAllCreatedBus()
        {
            lock (this.busInstances)
            {
                return this.busInstances.Values.ToArray();
            }
        }

        public IBus TryGetCreatedBus(BusInfo busInfo)
        {
            lock (this.busInstances)
            {
                if (this.busInstances.ContainsKey(busInfo))
                {
                    return this.busInstances[busInfo];
                }
                return null;
            }
        }

        public IBus CreateBusFromBusInfo(BusInfo busInfo, bool callOpen = false)
        {
            lock (this.busInstances)
            {
                IBus bus = null;
                if (this.busInstances.ContainsKey(busInfo))
                {
                    bus = this.busInstances[busInfo];
                }
                else
                {
                    Assembly assembly = Assembly.Load(busInfo.AssemblyName);
                    Type t = assembly.GetType(busInfo.ClassFullName);
                    bus = Activator.CreateInstance(t) as IBus;
                    this.OnBusCreated(new BusEventArgs(bus));
                }
                if ((bus.State == BusState.Closed || bus.State == BusState.Created) && callOpen == true)
                {
                    bus.Open(busInfo);
                }
                this.busInstances[busInfo] = bus;
                return bus;
            }
        }

        public T CreateBusFromBusInfo<T>(BusInfo busInfo, bool callOpen = false) where T : IBus
        {
            return (T)CreateBusFromBusInfo(busInfo, callOpen);
        }

        private bool MatchBusWithParent(BusInfo matchListRoot, BusInfo busInfo)
        {
            if (busInfo == null)
            {
                throw new ArgumentNullException("busInfo");
            }

            while (matchListRoot != null)
            {
                if (matchListRoot.Equals(busInfo))
                {
                    return true;
                }
                matchListRoot = matchListRoot.Parent;
            }
            return false;
        }
        public bool CloseBus(BusInfo busInfo)
        {
            lock (this.busInstances)
            {
                IBus bus = this.TryGetCreatedBus(busInfo);
                if (bus == null)
                {
                    return false;
                }

                bus.Close();
                while (busInfo.Parent != null)
                {
                    bus = this.TryGetCreatedBus(busInfo.Parent);
                    if (bus == null)
                    {
                        break;
                    }

                    //该总线被其它设备所占用
                    if (this.busInstances.Keys.Any(obj => MatchBusWithParent(obj, busInfo)))
                    {
                        break;
                    }

                    bus.Close();
                    busInfo = busInfo.Parent;
                }
                return true;
            }
        }

        public void Write(BusInfo busInfo, byte[] writeBuf, int offset, int len)
        {
            IBus bus = this.CreateBusFromBusInfo(busInfo, true);
            try
            {
                bus.Write(writeBuf, offset, len);
            }
            finally
            {
                if (bus != null && bus.State != BusState.Closed && busInfo.CacheResource == false)
                {
                    bus.Close();
                }
            }
        }

        public int Read(BusInfo busInfo, byte[] readBuf, int maxReadLen = 0)
        {
            IBus bus = this.CreateBusFromBusInfo(busInfo, true);
            try
            {
                return bus.Read(readBuf, maxReadLen);
            }
            finally
            {
                if (bus != null && bus.State != BusState.Closed && busInfo.CacheResource == false)
                {
                    bus.Close();
                }
            }
        }

        public int WriteAndRead(BusInfo busInfo, byte[] writeBuf, int offset, int len, byte[] readBuf, int maxReadLen = 0, int waitTimeBeforeRead = 0)
        {
            IBus bus = this.CreateBusFromBusInfo(busInfo, true);
            try
            {
                bus.Write(writeBuf, offset, len);
                if (waitTimeBeforeRead > 0)
                {
                    Thread.Sleep(waitTimeBeforeRead);
                }
                return bus.Read(readBuf, maxReadLen);
            }
            finally
            {
                if (bus != null && bus.State != BusState.Closed && busInfo.CacheResource == false)
                {
                    bus.Close();
                }
            }
        }
    }
}
