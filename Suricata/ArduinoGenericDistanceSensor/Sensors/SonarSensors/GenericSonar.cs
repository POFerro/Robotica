using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arduino.Firmata.Types.Proxy;
using Arduino.Messages.Proxy;
using Arduino.Proxy;
using Microsoft.Ccr.Core;

namespace ArduinoGenericDistanceSensor.Sensors.SonarSensors
{
	public class GenericSonar : ISensorBase
	{
		public sealed class Specification
		{
			public static int MinDistance = 10;

			public static int MaxDistance = 500;
		}

		private double mDistanceCM = 0;
		public double GetDistanceMax()
		{
			return Specification.MaxDistance;
		}

		public double GetDistanceMin()
		{
			return Specification.MinDistance;
		}

		public double GetDistanceCurrent()
		{
			return mDistanceCM;
		}

		public void SetAnalogData(int value)
		{
			mDistanceCM = value;
		}

		public string GetName()
		{
			return "HC_SR";
		}

		public void ConfigureArduino(ArduinoGenericDistanceSensorService arduinoGenericDistanceSensorService, ArduinoOperations _arduinoServicePort, ArduinoGenericDistanceSensorState _state)
		{
			if (_state.HardwareIdentifier > 0)
			{
				arduinoGenericDistanceSensorService.Activate(
					_arduinoServicePort.SetSonarMode(
						new SetSonarModeRequest()
						{
							TriggerPin = _state.TriggerPin == Pins.None ? (Pins)_state.HardwareIdentifier : _state.TriggerPin,
							EchoPin = (Pins)_state.HardwareIdentifier,
							MaxDistance = Specification.MaxDistance
						})
					.Choice());
			}
		}

		public bool IsSensorAnalogPin(ArduinoGenericDistanceSensorState _state, Pins pin)
		{
			return (int)pin == _state.HardwareIdentifier;
		}
	}
}
