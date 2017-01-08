using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mengsk.Device.Buses
{
    public abstract class BusBase<T> : IBus where T : BusInfo
    {
        protected T info = null;
        private BusState state = BusState.Created;
        protected object busLock = new object();

        public BusInfo Info { get { return this.info; } set { this.info = (T)value; } }

        public BusState State
        {
            get { return this.state; }
            private set
            {
                if (this.state == value)
                {
                    return;
                }
                BusState preState = this.state;
                this.state = value;
                this.OnBusStateChanged(new BusStateChangedEventArgs(this, preState, this.State));
            }
        }

        public event EventHandler<BusStateChangedEventArgs> BusStateChanged;

        public event EventHandler<BusOperationEventArgs> BusOperationStarting;

        public event EventHandler<BusOperationEventArgs> BusOperationCompleted;

        public BusBase()
        {
            this.busLock = new object();
            this.State = BusState.Created;
        }
        protected virtual void OnBusStateChanged(BusStateChangedEventArgs e)
        {
            if (this.BusStateChanged != null)
            {
                this.BusStateChanged(this, e);
            }
        }

        protected virtual void OnBusOperationStarting(BusOperationEventArgs e)
        {
            if (this.BusOperationStarting != null)
            {
                this.BusOperationStarting(this, e);
            }
        }

        protected virtual void OnBusOperationCompleted(BusOperationEventArgs e)
        {
            if (this.BusOperationCompleted != null)
            {
                this.BusOperationCompleted(this, e);
            }
        }

        public void Open(BusInfo busInfo)
        {
            lock (this.busLock)
            {
                try
                {
                    this.Info = busInfo;
                    if (this.State != BusState.Closed && this.State != BusState.Error && this.State != BusState.Created)
                    {
                        throw new Exception(string.Format("Bus:{0} is {1} could not open", busInfo.ReadableValue, this.State));
                    }
                    this.State = BusState.Opening;
                    this.OpenImplement(busInfo);
                    this.State = BusState.Opened;
                }
                catch
                {
                    this.State = BusState.Error;
                    this.Info = null;
                    throw;
                }
            }
        }

        public void Close()
        {
            lock (this.busLock)
            {
                try
                {
                    if (this.State == BusState.Closed)
                    {
                        return;
                    }
                    this.State = BusState.Closing;
                    this.CloseImplement();
                    this.Info = null;
                    this.State = BusState.Closed;
                }
                catch
                {
                    this.State = BusState.Error;
                    throw;
                }
            }
        }

        public void Write(byte[] writeBuf, int offset, int len)
        {
            lock (this.busLock)
            {
                try
                {
                    if (this.Info == null || this.State == BusState.Error || this.State == BusState.Closed)
                    {
                        throw new Exception(string.Format("Bus:{0} is at {1} state,could not do write", this.Info.ReadableValue, this.State));
                    }

                    this.State = BusState.Writing;
                    this.OnBusOperationStarting(new BusOperationEventArgs(this, BusOperation.Writing, writeBuf, offset, len));

                    this.WriteImplement(writeBuf, offset, len);

                    this.State = BusState.Writed;
                    this.OnBusOperationCompleted(new BusOperationEventArgs(this, BusOperation.Writing, writeBuf, offset, len));
                }
                catch
                {
                    this.State = BusState.Error;
                    throw;
                }
            }
        }

        public int Read(byte[] readBuf, int maxReadLen = 0)
        {
            lock (this.busLock)
            {
                try
                {
                    if (this.Info == null || this.State == BusState.Error || this.State == BusState.Closed)
                    {
                        throw new Exception(string.Format("Bus:{0} is at {1} state,could not do write", this.Info.ReadableValue, this.State));
                    }

                    this.State = BusState.Reading;
                    this.OnBusOperationStarting(new BusOperationEventArgs(this, BusOperation.Reading, readBuf, 0, maxReadLen == 0 ? readBuf.Length : maxReadLen));

                    int ret = this.ReadImplement(readBuf, maxReadLen == 0 ? readBuf.Length : maxReadLen);

                    this.State = BusState.Readed;
                    this.OnBusOperationCompleted(new BusOperationEventArgs(this, BusOperation.Reading, readBuf, 0, ret));

                    return ret;
                }
                catch
                {
                    this.State = BusState.Error;
                    throw;
                }
            }
        }

        protected abstract void OpenImplement(BusInfo busInfo);

        protected abstract void CloseImplement();

        protected abstract void WriteImplement(byte[] writeBuf, int offset, int len);

        protected abstract int ReadImplement(byte[] readBuf, int maxReadLen);

    }
}
