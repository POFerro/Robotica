using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Arduino.Firmata.Types;

namespace Arduino.Firmata
{
    public class Const
    {
        public const int INPUT_SIZE = 16;
        public const int TOTAL_PINS = 24;
        public const int MAX_DATA_BYTES = 32;
    }

    public class PinConfig
    {
        public PinConfig(Types.Pins pin)
        {
            this.Pin = pin;
        }

        public Types.Pins Pin
        {
            get;
            private set;
        }

        public Types.PinMode Mode
        {
            get;
            private set;
        }
    }

	public delegate void FirmataStringReceivedDelegate(string Message);
	public delegate void AnalogOutputUpdatedDelegate(Pins pin, int value);
	public delegate void DigitalOutputUpdatedDelegate(Pins pin, PinDigitalValue value);

    public class Firmata : IDisposable
    {
        #region Variables
        private ConnectionTypes.ConnectionBase mConnection = null;

        private Dictionary<Types.Pins, Types.PinMode> pinsConfig = new Dictionary<Types.Pins, Types.PinMode>();

        // Firmata version
        int majorVersion;
        int minorVersion;

        int[] digitalOutputData = new int[Const.INPUT_SIZE];
        int[] digitalInputData = new int[Const.INPUT_SIZE];
		int[] analogInputData = new int[Const.TOTAL_PINS];

        int[] reportModeData = new int[Const.TOTAL_PINS];

        int parse_count;
        int parse_command_len;
        byte[] parse_buf = new byte[4096];

		public event AnalogOutputUpdatedDelegate AnalogOutputUpdated;
		public event DigitalOutputUpdatedDelegate DigitalOutputUpdated;
		public event FirmataStringReceivedDelegate FirmataStringReceived;

		protected void OnFirmataStringReceived(string message)
		{
			if (this.FirmataStringReceived != null)
				this.FirmataStringReceived(message);
		}

		protected void OnDigitalOutputUpdated(Pins pin, PinDigitalValue value)
		{
			if (this.DigitalOutputUpdated != null)
				this.DigitalOutputUpdated(pin, value);
		}

		protected void OnAnalogOutputUpdated(Pins pin, int value)
		{
			if (this.AnalogOutputUpdated != null)
				this.AnalogOutputUpdated(pin, value);
		}

		public DateTime LastAnalogUpdateTime
		{
			get;
			private set;
		}
		public DateTime LastDigitalUpdateTime
		{
			get;
			private set;
		}

        #endregion

		ManualResetEvent startedSignal = new ManualResetEvent(false);
		bool started = false;
		public void WaitToStart(int miliseconds)
		{
			startedSignal.WaitOne(miliseconds);
			if (!started)
			{
				this.Dispose();
				throw new TimeoutException("Timeout à espera da inicialização do Arduino");
			}
		}

        public Firmata(ConnectionTypes.ConnectionBase connection, int samplingInterval)
        {
			this.LastAnalogUpdateTime = DateTime.MinValue;
			this.LastDigitalUpdateTime = DateTime.MinValue;

            connection.OnData += new ConnectionTypes.EventConnectionData(this.parse);
            mConnection = connection;

			sendSamplingInterval(samplingInterval);
			sendVersionRequest();
//			sendFirmwareRequest();
		}

		public void Dispose()
		{
			mConnection.OnData -= new ConnectionTypes.EventConnectionData(this.parse);
			this.mConnection.Dispose();
			//terminateEvent.Set();
		}

        //
        // Arduino functions 
        //
        public int digitalRead(Types.Pins pin)
        {
            int _pin = (int)pin;
            if (_pin >= 0 && _pin < Const.INPUT_SIZE)
				return digitalInputData[_pin];
            return -1;
        }

        public int analogRead(Types.Pins pin)
        {
			int _pin = (int)pin -14;
			return analogInputData[_pin];
        }

        public void setPinMode(Types.Pins pin, Types.PinMode mode)
        {
            int _pin = (int)pin;
			lock (this.pinsConfig)
			{
				this.pinsConfig[pin] = mode;
			}

            mConnection.Write(new byte [] {
				(byte)Types.Messages.SET_PIN_MODE,
				(byte)_pin,
				(byte)mode
			});
			if (mode == PinMode.Analog || mode == PinMode.IrReceiver)
				reportModeData[_pin] = 1;
		}

        public Types.PinMode getPinMode(Types.Pins pin)
        {
			lock (this.pinsConfig)
			{
				return pinsConfig.ContainsKey(pin) ? this.pinsConfig[pin] : Types.PinMode.None;
			}
        }

        public List<Types.Pins> getPinList(Types.PinMode mode)
        {
			lock (this.pinsConfig)
			{
				return pinsConfig.Where(p => p.Value == mode).Select(p => p.Key).ToList();
			}
        }

		public void digitalWrite(Types.Pins pin, Types.PinDigitalValue value)
		{
			int _pin = (int)pin;
			if (_pin > Const.INPUT_SIZE)
			{
				Console.WriteLine("[!][ArduinoFirmata] Error: too big _pin number: {0}", _pin);
				return;
			}

			int portNumber = (_pin >> 3) & 0x0F;

			if (value == Types.PinDigitalValue.Low)
			{
				digitalOutputData[portNumber] &= ~(1 << (_pin & 0x07));
			}
			else if (value == Types.PinDigitalValue.High)
			{
				digitalOutputData[portNumber] |= (1 << (_pin & 0x07));
			}

			mConnection.Write(new byte[] {
				(byte)((int)Types.Messages.DIGITAL_MESSAGE | portNumber),
				(byte)(digitalOutputData[portNumber] & 0x7F),
				(byte)(digitalOutputData[portNumber] >> 7),
			});
		}

        public void analogWrite(Types.Pins pin, int value)
        {
            int _pin = (int)pin;
			mConnection.Write(new byte[] {
				(byte)((int)Types.Messages.ANALOG_MESSAGE | (_pin & 0x0F)),
				(byte)(value & 0x7F),
				(byte)((value >> 7) & 0x7F),
			});
        }

        public void sendSamplingInterval(int ms)
        {
			this.sendSysEx((byte)Types.Messages.SAMPLING_INTERVAL, new byte[] {
				(byte)(ms & 0x7F), 
				(byte)((ms >> 7) & 0x7F), 
			}, 2);
        }

		public void setSonarMode(Types.Pins triggerPin, Types.Pins echoPin, int maxDistance)
		{
			lock (this.pinsConfig)
			{
				this.pinsConfig[triggerPin] = Types.PinMode.Output;
				this.pinsConfig[echoPin] = Types.PinMode.Sonar;
			}
			reportModeData[(int)echoPin] = 1;
			sendSysEx(Types.Messages.SONAR_CONFIG, new byte[] { 
				(byte)triggerPin, 
				(byte)echoPin, 
				(byte)(maxDistance & 0x7F), 
				(byte)((maxDistance >> 7) & 0x7F), 
			}, 4);
		}

		public void setServoMode(Types.Pins pin, int minPulse, int maxPulse, int startAngle)
		{
			lock (this.pinsConfig)
			{
				this.pinsConfig[pin] = Types.PinMode.Servo;
			}
			sendSysEx(Types.Messages.SERVO_CONFIG, new byte[] { 
				(byte)pin, 
				(byte)(minPulse & 0x7F), 
				(byte)((minPulse >> 7) & 0x7F), 
				(byte)(maxPulse & 0x7F), 
				(byte)((maxPulse >> 7) & 0x7F), 
				(byte)(startAngle & 0x7F), 
				(byte)((startAngle >> 7) & 0x7F), 
			}, 7);
		}
		
        public void sendSysEx(Types.Messages command, byte[] data, int length)
        {
            sendSysEx((int)command, data, length);
        }

        public void sendSysEx(Types.Other command, byte[] data, int length)
        {
            sendSysEx((int)command, data, length);
        }

        public void sendSysEx(int command, byte[] data, int length)
        {
			var bytes = new List<byte>
                        {
                            (byte)Types.Messages.START_SYSEX,
                            (byte)command,
                        };
			bytes.AddRange(data);
			bytes.Add((byte)Types.Messages.END_SYSEX);
			mConnection.Write(bytes.ToArray());
        }

        public void sendSysExBegin()
        {
            mConnection.Write((int)Types.Messages.START_SYSEX);
        }

        public void sendSysExEnd()
        {
            mConnection.Write((int)Types.Messages.END_SYSEX);
        }

        public void sendString(char[] str, int length)
        {
			List<byte> msg = new List<byte>();
			msg.Add((byte)Types.Messages.START_SYSEX);
			msg.Add((byte)Types.Messages.FIRMATA_STRING);
            for (int i = 0; i < length; i++)
            {
                int value = (int)str[i];
                msg.Add((byte)(value & 0x7F));
                msg.Add((byte)(value >> 7));
            }
			msg.Add((byte)Types.Messages.END_SYSEX);
			mConnection.Write(msg.ToArray());
        }

        public void sendVersionRequest()
        {
            mConnection.Write((int)Types.Messages.REPORT_VERSION);
        }

        public void sendFirmwareRequest()
        {
			this.sendSysEx((int)Types.Messages.REPORT_FIRMWARE, new byte[0], 0);
        }

        public void sendReset()
        {
            mConnection.Write((int)Types.Messages.SYSTEM_RESET);
        }

        public void sendRequestPinState(Types.Pins pin)
        {
			byte[] s = new byte[1];
			s[0] = (byte)(int)pin;
            this.sendSysEx(Types.Messages.PIN_STATE_QUERY, s, 1);
        }

        public void sendSetPinReporting(Types.Pins pin, bool state)
        {
            int _pin = (int)pin;
            if (_pin >= 0 && pin < Pins.A0)
            {
				mConnection.Write(new byte[] { (byte)((int)Types.Messages.REPORT_DIGITAL | ((_pin <= 7) ? 0 : 1)), (byte)(state ? 1 : 0) });
            }
			else if (pin >= Pins.A0 && _pin <= Enum.GetValues(typeof(Pins)).Length)
            {
				mConnection.Write(new byte[] { (byte)((int)Types.Messages.REPORT_ANALOG | ((_pin - (int)Pins.A0) & 0x0F)), (byte)(state ? 1 : 0) });
            }
			reportModeData[_pin] = state ? 1 : 0;
        }

        public bool getPinReportMode(Types.Pins pin)
        {
            int _pin = (int)pin;
            if (_pin >= 0 && _pin < Const.TOTAL_PINS)
            {
                if (reportModeData[_pin] == 1) return true;
                else return false;
            }
            return false;
        }
        
        public void setVersion(int majorVersion, int minorVersion)
        {
            Console.WriteLine("[i][ArduinoFirmata] set version: {0}.{1}", majorVersion, minorVersion);
            this.majorVersion = majorVersion;
            this.minorVersion = minorVersion;
        }

        #region Parse incoming data

        private void parse(byte[] buf, int len)
        {
            for (int i = 0; i < len; i++)
            {
				byte msn = (byte)(buf[i] & 0xF0);
                if (msn == (byte)Types.Messages.ANALOG_MESSAGE || msn == (byte)Types.Messages.DIGITAL_MESSAGE || buf[i] == (byte)Types.Messages.REPORT_VERSION)
                {
                    parse_command_len = 3;
                    parse_count = 0;
                }
                else if (msn == (byte)Types.Messages.REPORT_ANALOG || msn == (byte)Types.Messages.REPORT_DIGITAL)
                {
                    parse_command_len = 2;
                    parse_count = 0;
                }
                else if (buf[i] == (byte)Types.Messages.START_SYSEX)
                {
                    parse_count = 0;
                    parse_command_len = parse_buf.Length;
                }
                else if (buf[i] == (byte)Types.Messages.END_SYSEX)
                {
                    parse_command_len = parse_count + 1;
                }
                else if ((buf[i] & 0x80) == 0x80)
                {
                    parse_command_len = 1;
                    parse_count = 0;
                }
                if (parse_count < parse_buf.Length)
                {
                    parse_buf[parse_count++] = buf[i];
                }
                if (parse_count == parse_command_len)
                {
                    this.doMessage();
                    parse_count = parse_command_len = 0;
                }
            }
        }

		///<summary>
		///  We use this internally to set the value of the pins when we get a port report from the Arduino.
		///  If we get a report from a pin that's not in input mode then we log the incident but don't change
		///  the value internally.
		///</summary>
		///<param name = "portNumber">The port number.</param>
		///<param name = "value">The value of the port.</param>
		///<exception cref = "ArgumentOutOfRangeException">When the value specified for the port number is not valid.</exception>
		internal void SetPortValue(int portNumber, int value)
		{
			int mask=1;

			int lastPin = portNumber * 8 + 8;
			if (lastPin > digitalInputData.Length) lastPin = digitalInputData.Length;
			for (int pin = portNumber * 8; pin < lastPin; pin++)
			{
				PinMode mode = getPinMode((Pins)pin);
				if (mode == PinMode.Input || mode == PinMode.Output)
				{
					int oldValue = digitalInputData[pin];
					digitalInputData[pin] = ((byte)value & mask) != 0 ? 1 : 0;
					if (oldValue != digitalInputData[pin])
						OnDigitalOutputUpdated((Pins)pin, (PinDigitalValue)digitalInputData[pin]);
				}
				mask = mask << 1;
			}
			LastDigitalUpdateTime = DateTime.Now;
		}
		
		private void doMessage()
        {
            int cmd = (parse_buf[0]);

			if (((parse_buf[0] & 0xF0) == (byte)Types.Messages.ANALOG_MESSAGE || cmd == (byte)Types.Messages.ANALOG_MESSAGE) && parse_count == 3)
            {
                int analog_ch = (parse_buf[0] & 0x0F);
                int analog_val = parse_buf[1] | (parse_buf[2] << 7);
				if (analog_val != analogInputData[analog_ch])
				{
					analogInputData[analog_ch] = analog_val;
					LastAnalogUpdateTime = DateTime.Now;
					OnAnalogOutputUpdated(Pins.A0 + analog_ch, analogInputData[analog_ch]);
				}
                return;
            }
			if (((parse_buf[0] & 0xF0) == (byte)Types.Messages.DIGITAL_MESSAGE || cmd == (byte)Types.Messages.DIGITAL_MESSAGE) && parse_count == 3)
            {
                int port_num = (parse_buf[0] & 0x0F);
                int port_val = parse_buf[1] | (parse_buf[2] << 7);
				SetPortValue(port_num, port_val);
				return;
            }
			if (cmd == (byte)Types.Messages.REPORT_VERSION && parse_count == 3)
            {
                this.setVersion(parse_buf[1], parse_buf[2]);
				this.started = true;
				this.startedSignal.Set();
			}

			if (parse_buf[0] == (byte)Types.Messages.START_SYSEX && parse_buf[parse_count - 1] == (byte)Types.Messages.END_SYSEX)
            {
                // Sysex message
				if (parse_buf[1] == (byte)Types.Messages.REPORT_FIRMWARE)
                {
					this.setVersion(parse_buf[2], parse_buf[3]);
					StringBuilder sb = new StringBuilder();
					for (int i = 4; i < parse_count - 2; i += 2)
                    {
                        int t = (parse_buf[i] & 0x7F)
                          | ((parse_buf[i + 1] & 0x7F) << 7);

						sb.Append((char)t);
                    }
                    sb.Append('-');
                    sb.Append((char)(parse_buf[2] + (byte)'0'));
                    sb.Append('.');
                    sb.Append((char)(parse_buf[3] + (byte)'0'));
                    Console.WriteLine("[i][ArduinoFirmata] firmata name: {0}", sb.ToString());

					// query the board's capabilities only after hearing the
					// REPORT_FIRMWARE message.  For boards that reset when
					// the port open (eg, Arduino with reset=DTR), they are
					// not ready to communicate for some time, so the only
					// way to reliably query their capabilities is to wait
					// until the REPORT_FIRMWARE message is heard.
					//this.started = true;
					//this.startedSignal.Set();
					//mConnection.Write(new byte[]
					//	{
					//		(byte)Types.Messages.START_SYSEX,
					//		(byte)Types.Messages.ANALOG_MAPPING_QUERY, // read analog to _pin # info
					//		(byte)Types.Messages.END_SYSEX
					//	});
					//buf[len++] = (byte)Types.Messages.START_SYSEX;
					//buf[len++] = (byte)Types.Messages.CAPABILITY_QUERY; // read capabilities
					//buf[len++] = (byte)Types.Messages.END_SYSEX;
				}
				else if (parse_buf[1] == (byte)Types.Messages.EXTENDED_ANALOG)
				{
					int pin = parse_buf[2];
					if (pin < digitalInputData.Length - 1)
					{
						//int lVal = BitHelper.BytesToInt(parse_buf[4], parse_buf[5]);
						//int hVal = BitHelper.BytesToInt(parse_buf[6], parse_buf[7]);
						int val = BitHelper.BytesToInt(parse_buf[3], parse_buf[4]); //lVal | hVal << 7;

						digitalInputData[pin] = val;
						LastDigitalUpdateTime = DateTime.Now;
						if (getPinReportMode((Pins)pin))
							OnAnalogOutputUpdated((Pins)pin, digitalInputData[pin]);
					}
					return;
				}
				else if (parse_buf[1] == (byte)Types.Messages.CAPABILITY_RESPONSE)
				{
					Console.WriteLine("[i][ArduinoFirmata] CAPABILITY_RESPONSE");
					this.started = true;
					this.startedSignal.Set();
				}
				else if (parse_buf[1] == (byte)Types.Messages.ANALOG_MAPPING_RESPONSE)
				{
					Console.WriteLine("[i][ArduinoFirmata] ANALOG_MAPPING_RESPONSE");
					this.started = true;
					this.startedSignal.Set();
				}
				else if (parse_buf[1] == (byte)Types.Messages.PIN_STATE_RESPONSE && parse_count >= 6)
				{
					Arduino.Firmata.Types.Pins pin = (Arduino.Firmata.Types.Pins)parse_buf[2];
					Arduino.Firmata.Types.PinMode mode = (Arduino.Firmata.Types.PinMode)parse_buf[3];
					int _value = parse_buf[4];
					if (parse_count > 6) _value |= (parse_buf[5] << 7);
					if (parse_count > 7) _value |= (parse_buf[6] << 14);

					lock (this.pinsConfig)
					{
						pinsConfig[pin] = mode;
					}
					if (mode == Types.PinMode.Input || mode == Types.PinMode.Output)
					{
						Arduino.Firmata.Types.PinDigitalValue value = (Types.PinDigitalValue)_value;
						//Console.WriteLine("[i][ArduinoFirmata] PIN_STATE_RESPONSE: pin = {0}, pin state = {1}, pin value = {2}", pin, mode, value);
					}
					else if (mode == Types.PinMode.PWM)
					{
						int value = _value;
						//Console.WriteLine("[i][ArduinoFirmata] PIN_STATE_RESPONSE: pin = {0}, pin state = {1}, pin value = {2}", pin, mode, value);
					}
					else
					{
						//Console.WriteLine("[i][ArduinoFirmata] PIN_STATE_RESPONSE: pin = {0}, pin state = {1}, pin value = {2}", pin, mode, _value);
					}
				}
				else if (parse_buf[1] == (byte)Types.Messages.STRING_DATA)
				{
					int len = (parse_count - 3) / 2;
					char[] s = new char[len];
					for (int i = 0; i < len; i++)
					{
						int char_val = parse_buf[2 + i * 2] | (parse_buf[2 + i * 2 + 1] << 7);
						s[i] = (char)char_val;
					}

					this.OnFirmataStringReceived(new string(s));
					Console.WriteLine("[i][ArduinoFirmata] STRING_DATA: {0}", new string(s));
				}
                return;
            }
        }
        #endregion
	}

}
