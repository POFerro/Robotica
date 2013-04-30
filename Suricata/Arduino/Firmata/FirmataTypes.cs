using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arduino.Firmata.Types
{
    public enum PinMode: int
    {
        None = -1,

		Input = 0,
		Output = 1,
        Analog = 2,
        PWM = 3,
        Servo = 4,
        Shift = 5,
        I2C = 6,
        Sonar = 7,
        IrReceiver = 8
    }

    public enum PinDigitalValue: int
    {
        Low = 0,
        High = 1
    }

	[Flags]
	public enum Messages : int
	{
		// send data for a digital port 
		DIGITAL_MESSAGE = 0x90,
		// send data for an analog _pin (or PWM) 
		ANALOG_MESSAGE = 0xE0,
		// enable analog input by _pin
		REPORT_ANALOG = 0xC0,
		// enable digital input by port 
		REPORT_DIGITAL = 0xD0,
		// set a _pin to INPUT/OUTPUT/PWM/etc 
		SET_PIN_MODE = 0xF4,
		// report firmware version 
		REPORT_VERSION = 0xF9,
		// reset from MIDI 
		SYSTEM_RESET = 0xFF,
		// start a MIDI SysEx message 
		START_SYSEX = 0xF0,
		// end a MIDI SysEx message 
		END_SYSEX = 0xF7,
		// report name and version of the firmware
		REPORT_FIRMWARE = 0x79,
		 // set the poll rate of the main loop
		SAMPLING_INTERVAL = 0x7A,

		// extended commands
		SERVO_CONFIG = 0x70,
		// receive a string
		STRING_DATA = 0x71,

		FIRMATA_STRING = 0x71,
		
		SONAR_CONFIG = 0x72,

		PIN_STATE_QUERY = 0x6D,
		PIN_STATE_RESPONSE = 0x6E,
		EXTENDED_ANALOG = 0x6F,
		CAPABILITY_QUERY = 0x6B,
		CAPABILITY_RESPONSE = 0x6C,
		ANALOG_MAPPING_QUERY = 0x69,
		ANALOG_MAPPING_RESPONSE = 0x6A,
	}

    [Flags]
    public enum Other : int
    {
        MODE_INPUT = 0x00,
        MODE_OUTPUT = 0x01,
        MODE_ANALOG = 0x02,
        MODE_PWM = 0x03,
        MODE_SERVO = 0x04,
        MODE_SHIFT = 0x05,
        MODE_I2C = 0x06,
		MODE_SONAR = 0x07,
	}

    public enum Pins : int
    {
        None = 0, 

        D1 = 1,
        D2 = 2,
        D3 = 3,
        D4 = 4,
        D5 = 5,
        D6 = 6,
        D7 = 7,
        D8 = 8,
        D9 = 9,
        D10 = 10,
        D11 = 11,
        D12 = 12,
        D13 = 13,

        A0 = 14,
        A1 = 15,
        A2 = 16,
        A3 = 17,
        A4 = 18,
        A5 = 19
    }
}
