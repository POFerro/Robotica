//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::System.Reflection.AssemblyVersionAttribute("1.0.0.0")]
[assembly: global::System.Reflection.AssemblyProductAttribute("ArduinoGenericDistanceSensor")]
[assembly: global::System.Reflection.AssemblyTitleAttribute("ArduinoGenericDistanceSensor")]
[assembly: global::Microsoft.Dss.Core.Attributes.ServiceDeclarationAttribute(global::Microsoft.Dss.Core.Attributes.DssServiceDeclaration.Transform, SourceAssemblyKey="ArduinoGenericDistanceSensor.Y2012.M01, Version=1.0.0.0, Culture=neutral, PublicK" +
    "eyToken=e3a6d1d8ea7297f8")]
[assembly: global::System.Security.SecurityTransparentAttribute()]
[assembly: global::System.Security.SecurityRulesAttribute(global::System.Security.SecurityRuleSet.Level1)]

namespace Dss.Transforms.TransformArduinoGenericDistanceSensor {
    
    
    public class Transforms : global::Microsoft.Dss.Core.Transforms.TransformBase {
        
        static Transforms() {
            Register();
        }
        
        public static void Register() {
            global::Microsoft.Dss.Core.Transforms.TransformBase.AddProxyTransform(typeof(global::ArduinoGenericDistanceSensor.Proxy.ArduinoGenericDistanceSensorState), new global::Microsoft.Dss.Core.Attributes.Transform(ArduinoGenericDistanceSensor_Proxy_ArduinoGenericDistanceSensorState_TO_ArduinoGenericDistanceSensor_ArduinoGenericDistanceSensorState));
            global::Microsoft.Dss.Core.Transforms.TransformBase.AddSourceTransform(typeof(global::ArduinoGenericDistanceSensor.ArduinoGenericDistanceSensorState), new global::Microsoft.Dss.Core.Attributes.Transform(ArduinoGenericDistanceSensor_ArduinoGenericDistanceSensorState_TO_ArduinoGenericDistanceSensor_Proxy_ArduinoGenericDistanceSensorState));
        }
        
        public static object ArduinoGenericDistanceSensor_Proxy_ArduinoGenericDistanceSensorState_TO_ArduinoGenericDistanceSensor_ArduinoGenericDistanceSensorState(object transformFrom) {
            global::ArduinoGenericDistanceSensor.ArduinoGenericDistanceSensorState target = new global::ArduinoGenericDistanceSensor.ArduinoGenericDistanceSensorState();
            global::ArduinoGenericDistanceSensor.Proxy.ArduinoGenericDistanceSensorState from = ((global::ArduinoGenericDistanceSensor.Proxy.ArduinoGenericDistanceSensorState)(transformFrom));
            target.SensorModel = ((global::ArduinoGenericDistanceSensor.Sensors.SensorsModels)(((int)(from.SensorModel))));
            target.HardwareIdentifier = from.HardwareIdentifier;
            target.Pose = from.Pose;
            target.TriggerPin = from.TriggerPin;
            return target;
        }
        
        public static object ArduinoGenericDistanceSensor_ArduinoGenericDistanceSensorState_TO_ArduinoGenericDistanceSensor_Proxy_ArduinoGenericDistanceSensorState(object transformFrom) {
            global::ArduinoGenericDistanceSensor.Proxy.ArduinoGenericDistanceSensorState target = new global::ArduinoGenericDistanceSensor.Proxy.ArduinoGenericDistanceSensorState();
            global::ArduinoGenericDistanceSensor.ArduinoGenericDistanceSensorState from = ((global::ArduinoGenericDistanceSensor.ArduinoGenericDistanceSensorState)(transformFrom));
            target.SensorModel = ((global::ArduinoGenericDistanceSensor.Sensors.Proxy.SensorsModels)(((int)(from.SensorModel))));
            target.HardwareIdentifier = from.HardwareIdentifier;
            target.Pose = from.Pose;
            target.TriggerPin = from.TriggerPin;
            return target;
        }
    }
}