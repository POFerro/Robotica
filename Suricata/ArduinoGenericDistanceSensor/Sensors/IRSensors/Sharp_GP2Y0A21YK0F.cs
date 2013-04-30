using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arduino.Firmata.Types.Proxy;
using Arduino.Proxy;
using Microsoft.Ccr.Core;

namespace ArduinoGenericDistanceSensor.Sensors.IRSensors
{
    public class Sharp_GP2Y0A21YK0F: ISensorBase
    {
        public sealed class Specification
        {
            public static int MinDistance = 10;

            public static int MaxDistance = 80;
        }

        private double mDistance = 0;

        public Sharp_GP2Y0A21YK0F()
        {
        }

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
            return mDistance;
        }

        public void SetAnalogData(int value)
        {
			if (value < 80)		//upper boundary: 36 cm (returning 37 means over the boundary)
				mDistance = GetDistanceMax();
			else
				mDistance = Math.Round(1 / (0.0002391473 * value - 0.0100251467));
				//mDistance = Math.Round(4800 / ((double)value - 20));

			/*
			The formula to translate SensorValue into Distance for Sharp 4-30cm analog sensor is: 
			Distance (cm) = 2076/(SensorValue - 11) This formula is only valid over the SensorValue range 80-530. 

			 The formula to translate SensorValue into Distance for Sharp 10-80cm analog sensors is: 
			Distance (cm) = 4800/(SensorValue - 20) This formula is only valid over the SensorValue range 80-500. 

			 The formula to translate SensorValue into Distance for Sharp 20-150cm analog sensors is: 
			Distance (cm) = 9462/(SensorValue - 16.92) This formula is only valid over the SensorValue range 80-490. 
			*/
		}

        public string GetName()
        {
            return "Sharp GP2Y0A21YK0F";
        }

		public void ConfigureArduino(ArduinoGenericDistanceSensorService arduinoGenericDistanceSensorService, ArduinoOperations _arduinoServicePort, ArduinoGenericDistanceSensorState _state)
		{
			if (_state.HardwareIdentifier > 0)
			{
				arduinoGenericDistanceSensorService.Activate(
					_arduinoServicePort.SetPinMode(new Arduino.Messages.Proxy.SetPinModeRequest() { Pin = (Pins)_state.HardwareIdentifier, Mode = Arduino.Firmata.Types.Proxy.PinMode.Analog }).Choice()
					);
			}
		}

		public bool IsSensorAnalogPin(ArduinoGenericDistanceSensorState _state, Pins pin)
		{
			return (int)pin == _state.HardwareIdentifier;
		}
	}
}
