using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arduino.Firmata.Types.Proxy;
using Arduino.Proxy;

namespace ArduinoGenericDistanceSensor.Sensors
{
    public interface ISensorBase
    {
		void ConfigureArduino(ArduinoGenericDistanceSensorService arduinoGenericDistanceSensorService, ArduinoOperations _arduinoServicePort, ArduinoGenericDistanceSensorState _state);
		
        double GetDistanceMax();

        double GetDistanceMin();

        double GetDistanceCurrent();

        string GetName();

        void SetAnalogData(int value);

		bool IsSensorAnalogPin(ArduinoGenericDistanceSensorState _state, Pins pins);
	}
}
