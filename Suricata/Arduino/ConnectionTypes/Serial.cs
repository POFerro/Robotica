using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace Arduino.ConnectionTypes.SerialPort
{
    public class Serial: ConnectionBase
    {
        private System.IO.Ports.SerialPort mPort = null;

        public Serial()
        {
        }

		public override void Dispose()
		{
			this.Close();
		}

        #region Data
        private void serialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
			System.IO.Ports.SerialPort port = (System.IO.Ports.SerialPort)sender;

			while (e.EventType != SerialData.Eof && port.IsOpen && port.BytesToRead > 0)
            {
                int ToRead = port.BytesToRead;
				byte[] data = new byte[ToRead];
				int bytesRead = port.Read(data, 0, ToRead);

				if (bytesRead > 0) this.CallEventOnData(data, bytesRead);
            }
        }

        private void serialPort_ErrorReceived(object sender, System.IO.Ports.SerialErrorReceivedEventArgs e)
        {
            this.CallEventOnError(e.EventType.ToString());
        }
        #endregion

        #region Other
        public void Open(int port, int rate)
        {
            Close();

            Console.WriteLine("Port num = {0}", port);

            mPort = new System.IO.Ports.SerialPort(String.Format("COM{0}", port), rate);
            mPort.ErrorReceived += new SerialErrorReceivedEventHandler(serialPort_ErrorReceived);
			mPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);

			mPort.Open();
        }

        public void Close()
        {
            if (mPort != null)
            {
                Console.Write("Port opened. Closing port...");
                try
                {
					mPort.ErrorReceived -= new SerialErrorReceivedEventHandler(serialPort_ErrorReceived);
					mPort.DataReceived -= new SerialDataReceivedEventHandler(serialPort_DataReceived);
					mPort.Close();
					mPort.Dispose();
                    Console.WriteLine("closed successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("cannot close - {0}",ex.Message);
                }
                mPort = null;
            }
        }
        #endregion

        #region Write
		public override void Write(byte ptr)
		{
			mPort.Write(new byte[] { ptr }, 0, 1);
		}
		public override void Write(byte[] ptr)
        {
			mPort.Write(ptr, 0, ptr.Length);
        }
        #endregion
	}
}
