using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Dss.Core.Attributes;

namespace ArduinoGenericDistanceSensor.Sensors
{
    [DataContract(Namespace = "ArduinoGenericDistanceSensor")]
    public enum SensorsModels
    {
        None,
        IR_Sharp_GP2Y0A21YK0F,
		Sonar_HC_SR
    }
}
