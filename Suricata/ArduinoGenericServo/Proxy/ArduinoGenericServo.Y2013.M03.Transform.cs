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
[assembly: global::System.Reflection.AssemblyProductAttribute("ArduinoGenericServo")]
[assembly: global::System.Reflection.AssemblyTitleAttribute("ArduinoGenericServo")]
[assembly: global::Microsoft.Dss.Core.Attributes.ServiceDeclarationAttribute(global::Microsoft.Dss.Core.Attributes.DssServiceDeclaration.Transform, SourceAssemblyKey="ArduinoGenericServo.Y2013.M03, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e" +
    "3a6d1d8ea7297f8")]
[assembly: global::System.Security.SecurityTransparentAttribute()]
[assembly: global::System.Security.SecurityRulesAttribute(global::System.Security.SecurityRuleSet.Level1)]

namespace Dss.Transforms.TransformArduinoGenericServo {
    
    
    public class Transforms : global::Microsoft.Dss.Core.Transforms.TransformBase {
        
        static Transforms() {
            Register();
        }
        
        public static void Register() {
            global::Microsoft.Dss.Core.Transforms.TransformBase.AddProxyTransform(typeof(global::POFerro.Robotics.ArduinoGenericServo.Proxy.ArduinoGenericServoState), new global::Microsoft.Dss.Core.Attributes.Transform(POFerro_Robotics_ArduinoGenericServo_Proxy_ArduinoGenericServoState_TO_POFerro_Robotics_ArduinoGenericServo_ArduinoGenericServoState));
            global::Microsoft.Dss.Core.Transforms.TransformBase.AddSourceTransform(typeof(global::POFerro.Robotics.ArduinoGenericServo.ArduinoGenericServoState), new global::Microsoft.Dss.Core.Attributes.Transform(POFerro_Robotics_ArduinoGenericServo_ArduinoGenericServoState_TO_POFerro_Robotics_ArduinoGenericServo_Proxy_ArduinoGenericServoState));
        }
        
        public static object POFerro_Robotics_ArduinoGenericServo_Proxy_ArduinoGenericServoState_TO_POFerro_Robotics_ArduinoGenericServo_ArduinoGenericServoState(object transformFrom) {
            global::POFerro.Robotics.ArduinoGenericServo.ArduinoGenericServoState target = new global::POFerro.Robotics.ArduinoGenericServo.ArduinoGenericServoState();
            global::POFerro.Robotics.ArduinoGenericServo.Proxy.ArduinoGenericServoState from = ((global::POFerro.Robotics.ArduinoGenericServo.Proxy.ArduinoGenericServoState)(transformFrom));
            target.HardwareIdentifier = from.HardwareIdentifier;
            target.MinPulse = from.MinPulse;
            target.MaxPulse = from.MaxPulse;
            target.StartAngle = from.StartAngle;
            target.CurrentAngle = from.CurrentAngle;
            return target;
        }
        
        public static object POFerro_Robotics_ArduinoGenericServo_ArduinoGenericServoState_TO_POFerro_Robotics_ArduinoGenericServo_Proxy_ArduinoGenericServoState(object transformFrom) {
            global::POFerro.Robotics.ArduinoGenericServo.Proxy.ArduinoGenericServoState target = new global::POFerro.Robotics.ArduinoGenericServo.Proxy.ArduinoGenericServoState();
            global::POFerro.Robotics.ArduinoGenericServo.ArduinoGenericServoState from = ((global::POFerro.Robotics.ArduinoGenericServo.ArduinoGenericServoState)(transformFrom));
            target.HardwareIdentifier = from.HardwareIdentifier;
            target.MinPulse = from.MinPulse;
            target.MaxPulse = from.MaxPulse;
            target.StartAngle = from.StartAngle;
            target.CurrentAngle = from.CurrentAngle;
            return target;
        }
    }
}
