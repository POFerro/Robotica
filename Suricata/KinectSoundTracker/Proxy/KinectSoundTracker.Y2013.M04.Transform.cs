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
[assembly: global::System.Reflection.AssemblyProductAttribute("KinectSoundTracker")]
[assembly: global::System.Reflection.AssemblyTitleAttribute("KinectSoundTracker")]
[assembly: global::Microsoft.Dss.Core.Attributes.ServiceDeclarationAttribute(global::Microsoft.Dss.Core.Attributes.DssServiceDeclaration.Transform, SourceAssemblyKey="KinectSoundTracker.Y2013.M04, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e3" +
    "a6d1d8ea7297f8")]
[assembly: global::System.Security.SecurityTransparentAttribute()]
[assembly: global::System.Security.SecurityRulesAttribute(global::System.Security.SecurityRuleSet.Level1)]

namespace Dss.Transforms.TransformKinectSoundTracker {
    
    
    public class Transforms : global::Microsoft.Dss.Core.Transforms.TransformBase {
        
        static Transforms() {
            Register();
        }
        
        public static void Register() {
            global::Microsoft.Dss.Core.Transforms.TransformBase.AddProxyTransform(typeof(global::POFerro.Robotics.KinectSoundTracker.Proxy.KinectSoundTrackerState), new global::Microsoft.Dss.Core.Attributes.Transform(POFerro_Robotics_KinectSoundTracker_Proxy_KinectSoundTrackerState_TO_POFerro_Robotics_KinectSoundTracker_KinectSoundTrackerState));
            global::Microsoft.Dss.Core.Transforms.TransformBase.AddSourceTransform(typeof(global::POFerro.Robotics.KinectSoundTracker.KinectSoundTrackerState), new global::Microsoft.Dss.Core.Attributes.Transform(POFerro_Robotics_KinectSoundTracker_KinectSoundTrackerState_TO_POFerro_Robotics_KinectSoundTracker_Proxy_KinectSoundTrackerState));
        }
        
        public static object POFerro_Robotics_KinectSoundTracker_Proxy_KinectSoundTrackerState_TO_POFerro_Robotics_KinectSoundTracker_KinectSoundTrackerState(object transformFrom) {
            global::POFerro.Robotics.KinectSoundTracker.KinectSoundTrackerState target = new global::POFerro.Robotics.KinectSoundTracker.KinectSoundTrackerState();
            global::POFerro.Robotics.KinectSoundTracker.Proxy.KinectSoundTrackerState from = ((global::POFerro.Robotics.KinectSoundTracker.Proxy.KinectSoundTrackerState)(transformFrom));
            target.CurrentAngle = from.CurrentAngle;
            target.CurrentConfidenceLevel = from.CurrentConfidenceLevel;
            return target;
        }
        
        public static object POFerro_Robotics_KinectSoundTracker_KinectSoundTrackerState_TO_POFerro_Robotics_KinectSoundTracker_Proxy_KinectSoundTrackerState(object transformFrom) {
            global::POFerro.Robotics.KinectSoundTracker.Proxy.KinectSoundTrackerState target = new global::POFerro.Robotics.KinectSoundTracker.Proxy.KinectSoundTrackerState();
            global::POFerro.Robotics.KinectSoundTracker.KinectSoundTrackerState from = ((global::POFerro.Robotics.KinectSoundTracker.KinectSoundTrackerState)(transformFrom));
            target.CurrentAngle = from.CurrentAngle;
            target.CurrentConfidenceLevel = from.CurrentConfidenceLevel;
            return target;
        }
    }
}
