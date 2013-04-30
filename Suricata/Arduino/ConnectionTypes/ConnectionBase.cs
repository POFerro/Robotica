using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arduino.ConnectionTypes
{
    public delegate void EventConnectionData(byte[] data,int length);
    public delegate void EventConnectionError(string error);

    public abstract class ConnectionBase : IDisposable
    {
        public event EventConnectionData OnData;
        public event EventConnectionError OnError;

        protected void CallEventOnData(byte[] data, int length)
        {
            if (OnData != null) OnData(data, length);
        }

        protected void CallEventOnError(string error)
        {
            if (OnError != null) OnError(error);
        }

        public abstract void Write(byte data);
        public abstract void Write(byte[] data);
		public virtual void Dispose()
		{
		}
	}
}
